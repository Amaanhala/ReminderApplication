using System.Collections.Generic;
using System.Threading.Tasks;
using System.Diagnostics;
using SQLite;
using ReminderApplication.Model;
using ReminderApplication.Interfaces;
using Microsoft.Maui.Storage;

namespace ReminderApplication.Helpers
{
    public class DatabaseHelper : IDatabaseHelper
    {
        private readonly SQLiteAsyncConnection _database;

        public DatabaseHelper(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);
            _database.CreateTableAsync<User>().Wait();
            _database.CreateTableAsync<PatientTasks>().Wait();
            _database.CreateTableAsync<FamilyPhoto>().Wait();
            _database.CreateTableAsync<PatientMusic>().Wait();
        }

        public Task<List<User>> GetAllUsersAsync()
        {
            var users = _database.Table<User>().ToListAsync();
            Debug.WriteLine($"Fetched {users.Result.Count} users from database");
            return users;
        }

        public Task<User> GetUserByEmailAsync(string email)
        {
            return _database.Table<User>().Where(u => u.Email == email).FirstOrDefaultAsync();
        }

        public Task<int> SaveUserAsync(User user)
        {
            return _database.InsertAsync(user);
        }

        public Task<int> DeleteUserAsync(User user)
        {
            Debug.WriteLine($"Deleting user with ID: {user.ID}, Email: {user.Email}");
            return _database.DeleteAsync(user);
        }

        public async Task<bool> ValidateUserAsync(string email, string password)
        {
            var user = await _database.Table<User>().Where(u => u.Email == email && u.Password == password).FirstOrDefaultAsync();
            return user != null;
        }

        public async Task<bool> AddUserAsync(User user)
        {
            var existingUser = await GetUserByEmailAsync(user.Email);
            if (existingUser != null)
            {
                return false;
            }

            int result = await _database.InsertAsync(user);

            if (result > 0 && user.Role == UserType.Patient)
            {
                user.PatientId = user.ID;
                await _database.UpdateAsync(user);
            }

            return result > 0;
        }

        public Task<List<PatientTasks>> GetAllTasksAsync()
        {
            return _database.Table<PatientTasks>().ToListAsync();
        }

        public Task<int> UpdateTaskAsync(PatientTasks task)
        {
            return _database.UpdateAsync(task);
        }

        public async Task<int> DeleteTaskAsync(PatientTasks task)
        {
            return await _database.DeleteAsync(task);
        }


        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _database.Table<User>().FirstOrDefaultAsync(u => u.ID == id);
        }

        public async Task<User> GetUserByEmailAndPasswordAsync(string email, string password)
        {
            var user = await _database.Table<User>()
                                      .Where(u => u.Email == email && u.Password == password)
                                      .FirstOrDefaultAsync();

            if (user != null)
            {
                Debug.WriteLine($"User ID: {user.ID}, Email: {user.Email}, Role: {user.Role}, PatientId: {user.PatientId}, CaregiverId: {user.CaregiverId}");

                if (user.Role == UserType.Caretaker && user.CaregiverId != null)
                {
                    var caregiver = await GetUserByIdAsync((int)user.CaregiverId);
                    if (caregiver != null)
                    {
                        user.CaregiverId = caregiver.ID;
                    }
                }
            }
            else
            {
                Debug.WriteLine("User not found.");
            }

            return user;
        }

        public async Task ResetDatabaseAsync()
        {
            await _database.DropTableAsync<User>();
            await _database.DropTableAsync<PatientTasks>();
            await _database.DropTableAsync<FamilyPhoto>();
            await _database.DropTableAsync<PatientMusic>();

            await _database.CreateTableAsync<User>();
            await _database.CreateTableAsync<PatientTasks>();
            await _database.CreateTableAsync<FamilyPhoto>();
            await _database.CreateTableAsync<PatientMusic>();
        }

        public async Task<int> AddTaskAsync(PatientTasks task)
        {
            try
            {
                var result = await _database.InsertAsync(task);
                Debug.WriteLine($"Task saved: PatientId = {task.PatientId}, FoodDetails = {task.FoodDetails}, WalkDetails = {task.WalkDetails}, DueDate = {task.DueDate}, FamilyMemberName = {task.PhotoDetails}");
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error saving task: {ex.Message}");
                return 0;
            }
        }

        public async Task<List<PatientTasks>> GetPatientTasksAsync(int userId)
        {
            return await _database.Table<PatientTasks>().Where(pt => pt.CaregiverId == userId).ToListAsync();
        }

        public Task<int> UpdateUserAsync(User user)
        {
            return _database.UpdateAsync(user);
        }

        public async Task<List<PatientTasks>> GetTasksForPatientAsync(int patientId, int caregiverId)
        {
            var tasks = await _database.Table<PatientTasks>().Where(t => t.PatientId == patientId && t.CaregiverId == caregiverId).ToListAsync();
            Debug.WriteLine($"Fetched {tasks.Count} tasks for patient with ID {patientId} and caregiver with ID {caregiverId}");
            return tasks;
        }

        public async Task<List<FamilyPhoto>> GetFamilyPhotosAsync(int caregiverId)
        {
            var photos = await _database.Table<FamilyPhoto>().Where(p => p.CaregiverId == caregiverId).ToListAsync();
            Debug.WriteLine($"Retrieved {photos.Count} family photos for caregiver with ID {caregiverId}");
            return photos;
        }

        public async Task<int> AddFamilyPhotoAsync(FamilyPhoto photo)
        {
            return await _database.InsertAsync(photo);
        }

        public async Task<int> DeleteAsync(FamilyPhoto familyPhoto)
        {
            return await _database.DeleteAsync(familyPhoto);
        }





        //public Task<int> DeleteAsync(PatientMusic music)
        //{
        //    return _database.DeleteAsync(music);
        //}

        //public async Task<int> AddMusicFileAsync(PatientMusic music)
        //{
        //    int result = await _database.InsertAsync(music);
        //    Console.WriteLine($"Added music file with ID: {music.Id}, CaregiverId: {music.CaregiverId}, FilePath: {music.FilePath}");
        //    return result;
        //}


        //public Task<List<PatientMusic>> GetPatientMusicAsync(int caregiverId)
        //{
        //    Console.WriteLine($"Getting music files for CaregiverId: {caregiverId}");
        //    return _database.Table<PatientMusic>().Where(x => x.CaregiverId == caregiverId).ToListAsync();
        //}

        public async Task<int> DeleteMusicAsync(PatientMusic music)
        {
            return await _database.DeleteAsync(music);
        }


        public async Task<int> AddPatientMusicAsync(PatientMusic music)
        {
            Console.WriteLine($"Adding music file with FilePath: {music.FilePath}");
            int result = await _database.InsertAsync(music);
            Console.WriteLine($"Added music file with ID: {music.Id}, CaregiverId: {music.CaregiverId}, FilePath: {music.FilePath}");
            return result;
        }

        public async Task<List<string>> GetMusicFilesForCaregiverAsync(int caregiverId)
        {
            Console.WriteLine($"Getting music files for CaregiverId: {caregiverId}");
            List<PatientMusic> audioFiles = await _database.Table<PatientMusic>().Where(m => m.CaregiverId == caregiverId).ToListAsync();

            Console.WriteLine($"Retrieved {audioFiles.Count} music files for CaregiverId: {caregiverId}");

            foreach (var file in audioFiles)
            {
                Console.WriteLine($"Music file - ID: {file.Id}, CaregiverId: {file.CaregiverId}, FilePath: {file.FilePath}");
            }

            List<string> filePaths = audioFiles.Select(m => m.FilePath).ToList();
            Console.WriteLine($"File paths retrieved for CaregiverId: {caregiverId}: {string.Join(", ", filePaths)}");

            return filePaths;
        }




    }
}