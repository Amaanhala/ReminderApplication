using ReminderApplication.Model;
using ReminderApplication.View;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ReminderApplication.ViewModel
{
    public class LoginViewModel : BaseViewModel
    {
        private string _email;
        private string _password;

        public ICommand LoginCommand { get; }
        public ICommand RegisterCommand { get; }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await LoginAsync());
            RegisterCommand = new Command(async () => await RegisterAsync());
        }

        private async Task LoginAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (Email == "admin@gmail.com" && Password == "admin123")
                {
                    await Shell.Current.GoToAsync($"///{nameof(AdminPage)}");
                }
                else
                {
                    var user = await App.DatabaseHelper.GetUserByEmailAsync(Email);

                    if (user != null && BCrypt.Net.BCrypt.Verify(Password, user.Password))
                    {
                        Debug.WriteLine($"User ID: {user.ID}, PatientId: {user.PatientId}, CaregiverId: {user.CaregiverId}");

                        if (user.Role == UserType.Patient)
                        {
                            await Shell.Current.Navigation.PushAsync(new PatientTasksPage(Shell.Current.Navigation, user.ID, user.PatientId, user.CaregiverId));
                        }
                        else if (user.Role == UserType.Caretaker)
                        {
                            user.CaregiverId = user.ID;
                            await App.DatabaseHelper.UpdateUserAsync(user);
                            await Shell.Current.Navigation.PushAsync(new CaregiverTasksPage(Shell.Current.Navigation, user.ID, user.PatientId, user.CaregiverId));
                        }
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Login Failed", "Invalid email or password", "OK");
                    }
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred while logging in: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task RegisterAsync()
        {
            await Shell.Current.GoToAsync($"//{nameof(RegisterPage)}");
        }
    }
}

