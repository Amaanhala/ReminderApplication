using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Maui.Controls;
using ReminderApplication.Model;
using ReminderApplication.ViewModel;

namespace ReminderApplication.View
{
    public partial class AddTaskPage : ContentPage
    {
        private TaskListViewModel _viewModel;

        private byte[] _imageBytes;

        private bool _foodTimeChanged;
        private bool _walkTimeChanged;
        private bool _brekfastTimeChanged;
        private bool _lunchTimeChanged;
        private bool _dinnerTimeChanged;
        private bool _morningMedicinesTimeChanged;
        private bool _afternoonMedicinesTimeChanged;
        private bool _eveningMedicinesTimeChanged;
        private bool _nightMedicinesTimeChanged;
        private bool _moreTasks1TimeChanged;
        private bool _moreTasks2TimeChanged;
        private bool _moreTasks3TimeChanged;
        private bool _moreTasks4TimeChanged;
        private bool _moreTasks5TimeChanged;
        private bool _waterReminderMorningTimeChanged;
        private bool _waterReminderAfternoonTimeChanged;
        private bool _waterReminderEveningTimeChanged;
        private bool _waterReminderNightTimeChanged;
        private bool _appointmentsChanged;
        private bool _activityTimeChanged;


        public AddTaskPage(TaskListViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;

            FoodTimePicker.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
                    _foodTimeChanged = true;
            };

            WalkTimePicker.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
                    _walkTimeChanged = true;
            };

            BreakfastPicker.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
                    _brekfastTimeChanged = true;
            };

            LunchPicker.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
                    _lunchTimeChanged = true;
            };

            DinnerPicker.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
                    _dinnerTimeChanged = true;
            };

            MorningMedicinesPicker.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
                    _morningMedicinesTimeChanged = true;
            };

            AfternoonMedicinesPicker.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
                    _afternoonMedicinesTimeChanged = true;
            };

            EveningMedicinesPicker.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
                    _eveningMedicinesTimeChanged = true;
            };

            NightMedicinesPicker.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
                    _nightMedicinesTimeChanged = true;
            };

            MoreTasks1Picker.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
                    _moreTasks1TimeChanged = true;
            };

            MoreTasks2Picker.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
                    _moreTasks2TimeChanged = true;
            };

            MoreTasks3Picker.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
                    _moreTasks3TimeChanged = true;
            };

            MoreTasks4Picker.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
                    _moreTasks4TimeChanged = true;
            };

            MoreTasks5Picker.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
                    _moreTasks5TimeChanged = true;
            };

            WaterReminderMorningPicker.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
                    _waterReminderMorningTimeChanged = true;
            };

            WaterReminderAfternoonPicker.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
                    _waterReminderAfternoonTimeChanged = true;
            };

            WaterReminderEveningPicker.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
                    _waterReminderEveningTimeChanged = true;
            };

            WaterReminderNightPicker.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
                    _waterReminderNightTimeChanged = true;
            };

            AppointmentsPicker.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
                    _appointmentsChanged = true;
            };

            ActivityPicker.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
                    _activityTimeChanged = true;
            };
        }


        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {

            if (_imageBytes == null && !string.IsNullOrWhiteSpace(PhotoDetails.Text))
            {
                await DisplayAlert("Missing Image", "Please upload a photo or delete the photo details.", "OK");
                return;
            }

            if (!string.IsNullOrWhiteSpace(PatientFoodsEntry.Text) && !_foodTimeChanged)
            {
                await DisplayAlert("Missing Time", "Please select a time for the food details.", "OK");
                return;
            }

            if (!string.IsNullOrWhiteSpace(PatientWalkEntry.Text) && ! _walkTimeChanged)
            {
                await DisplayAlert("Missing Time", "Please select a time for the breakfast.", "OK");
                return;
            }

            if (!string.IsNullOrWhiteSpace(BreakfastEntry.Text) && !_brekfastTimeChanged)
            {
                await DisplayAlert("Missing Time", "Please select a time for the breakfast.", "OK");
                return;
            }

            if (!string.IsNullOrWhiteSpace(LunchEntry.Text) && !_lunchTimeChanged)
            {
                await DisplayAlert("Missing Time", "Please select a time for the lunch.", "OK");
                return;
            }

            if (!string.IsNullOrWhiteSpace(DinnerEntry.Text) && !_dinnerTimeChanged)
            {
                await DisplayAlert("Missing Time", "Please select a time for the dinner.", "OK");
                return;

            }

            if (!string.IsNullOrWhiteSpace(MorningMedicinesEntry.Text) && !_morningMedicinesTimeChanged)
            {
                await DisplayAlert("Missing Time", "Please select a time for the morning medicines.", "OK");
                return;
            }

            if (!string.IsNullOrWhiteSpace(AfternoonMedicinesEntry.Text) && !_afternoonMedicinesTimeChanged)
            {
                await DisplayAlert("Missing Time", "Please select a time for the afternoon medicines.", "OK");
                return;
            }

            if (!string.IsNullOrWhiteSpace(EveningMedicinesEntry.Text) && !_eveningMedicinesTimeChanged)
            {
                await DisplayAlert("Missing Time", "Please select a time for the evening medicines.", "OK");
                return;
            }

            if (!string.IsNullOrWhiteSpace(NightMedicinesEntry.Text) && !_nightMedicinesTimeChanged)
            {
                await DisplayAlert("Missing Time", "Please select a time for the night medicines.", "OK");
                return;
            }

            if (!string.IsNullOrWhiteSpace(MoreTasks1Entry.Text) && !_moreTasks1TimeChanged)
            {
                await DisplayAlert("Missing Time", "Please select a time for the first entered task.", "OK");
                return;
            }

            if (!string.IsNullOrWhiteSpace(MoreTasks2Entry.Text) && !_moreTasks2TimeChanged)
            {
                await DisplayAlert("Missing Time", "Please select a time for the second entered task.", "OK");
                return;
            }

            if (!string.IsNullOrWhiteSpace(MoreTasks3Entry.Text) && !_moreTasks3TimeChanged)
            {
                await DisplayAlert("Missing Time", "Please select a time for the third entered task.", "OK");
                return;
            }

            if (!string.IsNullOrWhiteSpace(MoreTasks4Entry.Text) && !_moreTasks4TimeChanged)
            {
                await DisplayAlert("Missing Time", "Please select a time for the fourth entered task.", "OK");
                return;
            }

            if (!string.IsNullOrWhiteSpace(MoreTasks5Entry.Text) && !_moreTasks5TimeChanged)
            {
                await DisplayAlert("Missing Time", "Please select a time for the fifth entered task.", "OK");
                return;
            }

            if (!string.IsNullOrWhiteSpace(WaterReminderMorningEntry.Text) && !_waterReminderMorningTimeChanged)
            {
                await DisplayAlert("Missing Time", "Please select a time for the morning water.", "OK");
                return;
            }

            if (!string.IsNullOrWhiteSpace(WaterReminderAfternoonEntry.Text) && !_waterReminderAfternoonTimeChanged)
            {
                await DisplayAlert("Missing Time", "Please select a time for the afternoon water.", "OK");
                return;
            }

            if (!string.IsNullOrWhiteSpace(WaterReminderEveningEntry.Text) && !_waterReminderEveningTimeChanged)
            {
                await DisplayAlert("Missing Time", "Please select a time for the evening water.", "OK");
                return;
            }

            if (!string.IsNullOrWhiteSpace(WaterReminderNightEntry.Text) && !_waterReminderNightTimeChanged)
            {
                await DisplayAlert("Missing Time", "Please select a time for the night water.", "OK");
                return;
            }

            if (!string.IsNullOrWhiteSpace(AppointmentsEntry.Text) && !_appointmentsChanged)
            {
                await DisplayAlert("Missing Time", "Please select a time for the appointment details.", "OK");
                return;
            }

            if (!string.IsNullOrWhiteSpace(ActivityEntry.Text) && !_activityTimeChanged)
            {
                await DisplayAlert("Missing Time", "Please select a time for the activity details.", "OK");
                return;
            }

            if (_viewModel.TaskExistsForDate(DueDatePicker.Date))
            {
                await DisplayAlert("Duplicate Task", "A task already exists for the selected date. Please choose a different date.", "OK");
                return;
            }

            var task = new PatientTasks
            {
                PatientId = _viewModel.PatientId,
                CaregiverId = _viewModel.CaregiverId,
                FoodDetails = PatientFoodsEntry.Text,
                WalkDetails = PatientWalkEntry.Text,
                Breakfast = BreakfastEntry.Text,
                Lunch = LunchEntry.Text,
                Dinner = DinnerEntry.Text,
                MorningMedicines = MorningMedicinesEntry.Text,
                AfternoonMedicines = AfternoonMedicinesEntry.Text,
                EveningMedicines = EveningMedicinesEntry.Text,
                NightMedicines = NightMedicinesEntry.Text,
                MoreTasks1 = MoreTasks1Entry.Text,
                MoreTasks2 = MoreTasks2Entry.Text,
                MoreTasks3 = MoreTasks3Entry.Text,
                MoreTasks4 = MoreTasks4Entry.Text,
                MoreTasks5 = MoreTasks5Entry.Text,
                WaterReminderMorning = WaterReminderMorningEntry.Text,
                WaterReminderAfternoon = WaterReminderAfternoonEntry.Text,
                WaterReminderEvening = WaterReminderEveningEntry.Text,
                WaterReminderNight = WaterReminderNightEntry.Text,
                Appointments = AppointmentsEntry.Text,
                Activity = ActivityEntry.Text,

                DueDate = DueDatePicker.Date,
                ImageBytes = _imageBytes,
                PhotoDetails = PhotoDetails.Text,

                FoodTime = FoodTimePicker.Time,
                WalkTime = WalkTimePicker.Time,
                BreakfastTime = BreakfastPicker.Time,
                LunchTime = LunchPicker.Time,
                DinnerTime = DinnerPicker.Time,
                MorningMedicinesTime = MorningMedicinesPicker.Time,
                AfternoonMedicinesTime = AfternoonMedicinesPicker.Time,
                EveningMedicinesTime = EveningMedicinesPicker.Time,
                NightMedicinesTime = NightMedicinesPicker.Time,
                MoreTasksTime1 = MoreTasks1Picker.Time,
                MoreTasksTime2 = MoreTasks2Picker.Time,
                MoreTasksTime3 = MoreTasks3Picker.Time,
                MoreTasksTime4 = MoreTasks4Picker.Time,
                MoreTasksTime5 = MoreTasks5Picker.Time,
                WaterReminderMorningTime = WaterReminderMorningPicker.Time,
                WaterReminderAfternoonTime = WaterReminderAfternoonPicker.Time,
                WaterRemindereEveningTime = WaterReminderEveningPicker.Time,
                WaterReminderNightTime = WaterReminderNightPicker.Time,
                AppointmentsTime = AppointmentsPicker.Time,
                ActivityTime = ActivityPicker.Time,
            };

            Debug.WriteLine($"Image bytes: {_imageBytes}");

            _viewModel.SaveTaskCommand.Execute(task);

            await DisplayAlert("Task Saved", "The task has been saved successfully.", "OK");

            await Shell.Current.Navigation.PopAsync();
        }



        private async void OnPickImageClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Pick an image"
            });

            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                SelectedImage.Source = ImageSource.FromStream(() => stream);

                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    _imageBytes = ms.ToArray();
                }
            }
        }


    }
}
