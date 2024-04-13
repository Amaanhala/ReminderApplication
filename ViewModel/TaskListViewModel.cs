using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using ReminderApplication.Helpers;
using ReminderApplication.Model;
using Microsoft.Maui.Controls;
using System.Diagnostics;
using System.Linq;
using ReminderApplication.View;
using Microsoft.VisualBasic;
using Plugin.LocalNotification;

namespace ReminderApplication.ViewModel
{
    public class TaskListViewModel : BaseViewModel
    {
        private readonly DatabaseHelper _databaseHelper;
        private ObservableCollection<PatientTasks> _patientTasks;
        public int UserId { get; set; }
        public int PatientId { get; set; }
        public int CaregiverId { get; set; }
        public INavigation Navigation { get; set; }
        public PatientTasks SelectedTask { get; set; }


        private ObservableCollection<FamilyPhoto> _familyPhotos;
        public ObservableCollection<FamilyPhoto> FamilyPhotos
        {
            get => _familyPhotos;
            set => SetProperty(ref _familyPhotos, value);

        }
        private ObservableCollection<PatientMusic> _patientMusic;
        public ObservableCollection<PatientMusic> PatientMusic
        {
            get => _patientMusic;
            set => SetProperty(ref _patientMusic, value);

        }
        private ObservableCollection<string> _audioFiles;
        public ObservableCollection<string> AudioFiles
        {
            get { return _audioFiles; }
            set
            {
                _audioFiles = value;
                OnPropertyChanged();
            }
        }

        public TaskListViewModel() : this(Shell.Current.Navigation, 0, 0, 0)
        {
            
        }

        public TaskListViewModel(INavigation navigation, int userId, int patientId, int caregiverId)
        {
            Navigation = navigation;
            UserId = userId;
            PatientId = patientId;
            CaregiverId = caregiverId;
            _databaseHelper = App.DatabaseHelper;
            Tasks = new ObservableCollection<PatientTasks>();
            NavigateToAddTaskCommand = new Command(async () => await ExecuteAddTaskCommand());
            AddTaskCommand = new Command<PatientTasks>(async (task) => await AddTaskAsync(task));
            SaveTaskCommand = new Command<PatientTasks>(async (task) => await OnAddTask(task));
            DeleteTaskCommand = new Command<PatientTasks>(async task => await DeleteTaskAsync(task));
            LoadTasksCommand = new Command(async () => await ExecuteLoadTasksCommand());
            NavigateToAddImageCommand = new Command(async () => await ExecuteAddImageCommand());
            NavigateToFamilyCommand = new Command(async () => await ExecuteNavigateToFamilyCommand());
            FamilyPhotos = new ObservableCollection<FamilyPhoto>();
            SpeakTextCommand = new Command<string>(async (text) => await SpeakText(text));
            NavigateToGamesCommand = new Command(async () => await ExecuteNavigateToGamesCommand());
            NavigateToMusicCommand = new Command(async () => await ExecuteNavigateToMusicCommand());
            NavigateToAddMusicCommand = new Command(async () => await ExecuteAddMusicCommand());
            NavigateToPatientMusicCommand = new Command(async () => await ExecutePatientMusicCommand());
            DeleteMusicCommand = new Command<PatientMusic>(async (music) => await DeleteMusicAsync(music));

            _isLoaded = false;
        }


        public ICommand DeleteMusicCommand { get; private set; }

        public ICommand NavigateToAddMusicCommand { get; private set; }
        public ICommand NavigateToPatientMusicCommand { get; private set; }


        public ICommand NavigateToMusicCommand { get; private set; }

        public ICommand NavigateToGamesCommand { get; private set; }

        public ICommand NavigateToFamilyCommand { get; private set; }

        public ICommand LoadTasksCommand { get; set; }

        public ICommand DeleteTaskCommand { get; set; }

        public ICommand SaveTaskCommand { get; private set; }
        public ICommand NavigateToAddTaskCommand { get; private set; }
        public ICommand AddTaskCommand { get; }

        public ICommand NavigateToAddImageCommand { get; private set; }


        public ObservableCollection<PatientTasks> Tasks
        {
            get => _patientTasks;
            set => SetProperty(ref _patientTasks, value);
        }

        private async Task AddTaskAsync(PatientTasks task)
        {
            task.PatientId = PatientId;

            var user = await App.DatabaseHelper.GetUserByIdAsync(UserId);
            if (user != null && user.Role == UserType.Caretaker)
            {
                task.CaregiverId = user.ID;
            }
            else if (user != null && user.Role == UserType.Patient)
            {
                task.CaregiverId = user.CaregiverId;
            }

            await _databaseHelper.AddTaskAsync(task);
            await ExecuteLoadTasksCommand();
        }


        public async Task OnAppearing()
        {
            _isLoaded = false;
            IsBusy = true;
            await ExecuteLoadTasksCommand();
            ScheduleNotifications();
            IsBusy = false;
        }


        private bool _isLoaded = false;

        private async Task ExecuteLoadTasksCommand()
        {
            if (_isLoaded)
            {
                return;
            }

            _isLoaded = true;
            IsBusy = true;

            try
            {
                var user = await App.DatabaseHelper.GetUserByIdAsync(UserId);
                DateTime today = DateTime.Today;

                if (user != null && user.Role == UserType.Caretaker)
                {
                    var tasks = await _databaseHelper.GetPatientTasksAsync(CaregiverId);
                    _allTasks = new ObservableCollection<PatientTasks>(tasks);
                    Debug.WriteLine($"Loaded {tasks.Count} tasks for caregiver with ID {CaregiverId}");
                }
                else if (user != null && user.Role == UserType.Patient)
                {
                    var tasks = await _databaseHelper.GetTasksForPatientAsync(PatientId, CaregiverId);
                    _allTasks = new ObservableCollection<PatientTasks>(tasks);
                    Debug.WriteLine($"Loaded {tasks.Count} tasks for patient with ID {PatientId} and caregiver with ID {CaregiverId}");
                }

                FilterTasksByDate(DateTime.Today);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task DeleteTaskAsync(PatientTasks task)
        {
            await _databaseHelper.DeleteTaskAsync(task);
            Tasks.Remove(task);
        }



        public async Task ExecuteAddTaskCommand()
        {
            await Navigation.PushAsync(new AddTaskPage(this));
        }

        private async Task OnAddTask(PatientTasks task)
        {
            // Save the task to the database
            task.CaregiverId = CaregiverId;
            task.PatientId = PatientId;
            await _databaseHelper.AddTaskAsync(task);
           
            Tasks.Clear();
            await ExecuteLoadTasksCommand();
        }

        private void ScheduleNotification(int notificationId, string title, string description, DateTime dateTime, string notificationTitle)
        {
            if (dateTime > DateTime.Now)
            {
                var request = new NotificationRequest
                {
                    NotificationId = notificationId,
                    Title = notificationTitle,
                    Description = $"{title} - {description}",
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = dateTime,
                        NotifyRepeatInterval = TimeSpan.FromDays(1)
                    }
                };

                Debug.WriteLine($"Scheduling notification {notificationId} for {dateTime}");
                LocalNotificationCenter.Current.Show(request);
            }
        }


        public void ScheduleNotifications()
        {
            foreach (var task in Tasks)
            {
                ScheduleNotification(task.Id, task.Title, task.FoodDetails, task.DueDate + task.FoodTime, "Food Reminder");

                ScheduleNotification(task.Id + 2, task.Title, task.WalkDetails, task.DueDate + task.WalkTime, "Walk Reminder");

                ScheduleNotification(task.Id + 3, task.Title, task.Breakfast, task.DueDate + task.BreakfastTime, "Breakfast Time");

                ScheduleNotification(task.Id + 4, task.Title, task.Lunch, task.DueDate + task.LunchTime, "Lunch Time");

                ScheduleNotification(task.Id + 5, task.Title, task.Dinner, task.DueDate + task.DinnerTime, "Dinner Time");

                ScheduleNotification(task.Id + 6, task.Title, task.MorningMedicines, task.DueDate + task.WaterReminderMorningTime, "Water Reminder Morning");

                ScheduleNotification(task.Id + 7, task.Title, task.AfternoonMedicines, task.DueDate + task.WaterReminderAfternoonTime, "Water Reminder Afternoon");

                ScheduleNotification(task.Id + 8, task.Title, task.EveningMedicines, task.DueDate + task.WaterRemindereEveningTime, "Water Remindere Evening");

                ScheduleNotification(task.Id + 9, task.Title, task.NightMedicines, task.DueDate + task.WaterReminderNightTime, "Water Reminder Night");

                ScheduleNotification(task.Id + 10, task.Title, task.MoreTasks1, task.DueDate + task.MorningMedicinesTime, "Morning Medicines Time");

                ScheduleNotification(task.Id + 11, task.Title, task.MoreTasks2, task.DueDate + task.AfternoonMedicinesTime, "Afternoon Medicines Time");

                ScheduleNotification(task.Id + 12, task.Title, task.MoreTasks3, task.DueDate + task.EveningMedicinesTime, "Evening Medicines Time");

                ScheduleNotification(task.Id + 13, task.Title, task.MoreTasks4, task.DueDate + task.NightMedicinesTime, "Night Medicines Time");

                ScheduleNotification(task.Id + 14, task.Title, task.MoreTasks5, task.DueDate + task.MoreTasksTime1, "Task Reminder");

                ScheduleNotification(task.Id + 15, task.Title, task.WaterReminderMorning, task.DueDate + task.MoreTasksTime2, "Task Reminder");

                ScheduleNotification(task.Id + 16, task.Title, task.WaterReminderAfternoon, task.DueDate + task.MoreTasksTime3, "Task Reminder");

                ScheduleNotification(task.Id + 17, task.Title, task.WaterReminderEvening, task.DueDate + task.MoreTasksTime4, "Task Reminder");

                ScheduleNotification(task.Id + 18, task.Title, task.WaterReminderNight, task.DueDate + task.MoreTasksTime5, "Task Reminder");

                ScheduleNotification(task.Id + 19, task.Title, task.Appointments, task.DueDate + task.ActivityTime, "Activity Time");

                ScheduleNotification(task.Id + 20, task.Title, task.Activity, task.DueDate + task.AppointmentsTime, "Appointment Time");
            }
        }

        public async Task<ObservableCollection<FamilyPhoto>> GetFamilyPhotosAsync()
        {
            var photos = await _databaseHelper.GetFamilyPhotosAsync(CaregiverId);
            FamilyPhotos = new ObservableCollection<FamilyPhoto>(photos);
            return FamilyPhotos;
        }

        private async Task ExecuteNavigateToFamilyCommand()
        {
            await LoadFamilyPhotosAsync();
            await Navigation.PushAsync(new FamilyPhotosPage(this));
        }

        private async Task ExecuteAddImageCommand()
        {
            await LoadFamilyPhotosAsync();
            await Navigation.PushAsync(new AddImagePage(this));
        }


        public async Task<int> AddFamilyPhotoAsync(FamilyPhoto photo)
        {
            return await _databaseHelper.AddFamilyPhotoAsync(photo);
        }
        public async Task LoadFamilyPhotosAsync()
        {
            FamilyPhotos = await GetFamilyPhotosAsync();
        }

        public async Task DeleteFamilyPhotoAsync(FamilyPhoto photo)
        {
            if (photo == null)
                return;

            try
            {
                await _databaseHelper.DeleteAsync(photo);
                FamilyPhotos.Remove(photo);
                OnPropertyChanged(nameof(FamilyPhotos));
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting FamilyPhoto: {ex.Message}");
            }
        }

        public bool TaskExistsForDate(DateTime date)
        {
            return Tasks.Any(t => t.DueDate.Date == date.Date);
        }

        private ObservableCollection<PatientTasks> _allTasks;

        public void FilterTasksByDate(DateTime date)
        {
            Tasks = new ObservableCollection<PatientTasks>(_allTasks.Where(task => task.DueDate.Date == date.Date));
        }

        public ICommand SpeakTextCommand { get; }

        private async Task SpeakText(string text)
        {
            if (!string.IsNullOrWhiteSpace(text))
            {
                await TextToSpeech.SpeakAsync(text);
            }
        }
        private async Task ExecuteNavigateToGamesCommand()
        {
            await Navigation.PushAsync(new GamesPage());
        }

        private async Task ExecuteNavigateToMusicCommand()
        {
            await Navigation.PushAsync(new MusicPage());
        }



        public async Task<ObservableCollection<string>> GetMusicFilesForCaregiverAsync()
        {
            var music = await _databaseHelper.GetMusicFilesForCaregiverAsync(CaregiverId);
            AudioFiles = new ObservableCollection<string>(music);
            return AudioFiles;
        }

        public async Task<int> AddPatientMusicAsync(PatientMusic music)
        {
            return await _databaseHelper.AddPatientMusicAsync(music);
        }

        public async Task LoadMusicFilesForCaregiverAsync()
        {
            AudioFiles = await GetMusicFilesForCaregiverAsync();
        }


        public async Task DeleteMusicAsync(PatientMusic music)
        {
            await _databaseHelper.DeleteMusicAsync(music);
            AudioFiles.Remove(music.FilePath);
        }



        private async Task ExecuteAddMusicCommand()
        {
            await LoadMusicFilesForCaregiverAsync();
            await Navigation.PushAsync(new AddMusicPage(this, CaregiverId));
        }


        private async Task ExecutePatientMusicCommand()
        {
            Debug.WriteLine("ExecutePatientMusicCommand method called");

            await LoadMusicFilesForCaregiverAsync();
            var audioFiles = AudioFiles.ToList();


            Debug.WriteLine($"CaregiverId: {CaregiverId}");
            Debug.WriteLine($"Data passed to the PatientMusicPage constructor: {string.Join(", ", audioFiles)}");

            if (audioFiles.Count == 0)
            {
                Debug.WriteLine("No audio files found for the patient.");
                await Application.Current.MainPage.DisplayAlert("No Music Found", "There are no music files for the patient.", "OK");
                return;
            }

            await Navigation.PushAsync(new PatientMusicPage(audioFiles, CaregiverId));
        }

    }
}

