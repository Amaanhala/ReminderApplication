using System.Collections.Generic;
using System.Threading.Tasks;
using ReminderApplication.Model;

namespace ReminderApplication.Interfaces
{
    public interface IDatabaseHelper
    {


        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByEmailAsync(string email);
        Task<int> SaveUserAsync(User user);
        Task<int> DeleteUserAsync(User user);

        Task<bool> ValidateUserAsync(string email, string password);
        Task<bool> AddUserAsync(User user);
    }
}
