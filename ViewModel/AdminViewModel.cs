using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using ReminderApplication.Model;
using ReminderApplication.View;

namespace ReminderApplication.ViewModel
{
    public class AdminViewModel : BaseViewModel
    {
        private ObservableCollection<User> _users;
        public ObservableCollection<User> Users
        {
            get { return _users; }
            set
            {
                SetProperty(ref _users, value);
            }
        }

        public Command LoadUsersCommand { get; }
        public Command DeleteUserCommand { get; }
        public Command ResetDatabaseCommand { get; }
        public Command LogoutCommand { get; }

        public AdminViewModel()
        {
            Users = new ObservableCollection<User>();
            LoadUsersCommand = new Command(async () => await LoadUsersAsync());
            DeleteUserCommand = new Command<int>(async (userId) => await DeleteUserAsync(userId));
            ResetDatabaseCommand = new Command(async () => await ResetDatabaseAsync());
            LogoutCommand = new Command(async () => await LogoutAsync());
        }

        public async Task LoadUsersAsync()
        {
            Debug.WriteLine("LoadUsersAsync called");
            var userList = await App.DatabaseHelper.GetAllUsersAsync();
            Users = new ObservableCollection<User>(userList);

            Debug.WriteLine($"Loaded {Users.Count} users");
            foreach (var user in Users)
            {
                Debug.WriteLine($"User: {user.Email}, {user.Role}, {user.Name}");
            }
        }

        private async Task DeleteUserAsync(int userId)
        {
            var user = await App.DatabaseHelper.GetUserByIdAsync(userId);
            if (user != null)
            {
                await App.DatabaseHelper.DeleteUserAsync(user);
                await LoadUsersAsync();
            }
        }

        private async Task ResetDatabaseAsync()
        {
            await App.DatabaseHelper.ResetDatabaseAsync();
            await LoadUsersAsync();
        }

        private async Task LogoutAsync()
        {
            await Shell.Current.GoToAsync("//LoginPage", true);
        }
    }
}
