using System;
using System.Globalization;
using Microsoft.Maui.Controls;
using Plugin.LocalNotification;
using ReminderApplication.ViewModel;


namespace ReminderApplication.View
{
    public partial class PatientTasksPage : ContentPage
    {
        public TaskListViewModel ViewModel { get; set; }


        public PatientTasksPage()
        {
            InitializeComponent();
        }

        public PatientTasksPage(INavigation navigation, int userId, int patientId, int caregiverId)
        {
            InitializeComponent();
            ViewModel = new TaskListViewModel(navigation, userId, patientId, caregiverId);
            BindingContext = ViewModel;

        }

        public async void Speak() =>
           await TextToSpeech.Default.SpeakAsync("Hello World");

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            var viewModel = BindingContext as TaskListViewModel;
            viewModel.LoadTasksCommand.Execute(null);
            viewModel.ScheduleNotifications();
        }

    }
}
