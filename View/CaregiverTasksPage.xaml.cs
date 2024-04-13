using Microsoft.Maui.Controls;
using ReminderApplication.Model;
using ReminderApplication.ViewModel;
using System;
using System.Linq;

namespace ReminderApplication.View
{
    public partial class CaregiverTasksPage : ContentPage
    {
        public TaskListViewModel ViewModel { get; set; }

        public CaregiverTasksPage()
        {
            InitializeComponent();
        }

        public CaregiverTasksPage(INavigation navigation, int userId, int patientId, int caregiverId)
        {
            InitializeComponent();
            ViewModel = new TaskListViewModel(navigation, userId, patientId, caregiverId);
            BindingContext = ViewModel;

        }
        private void OnDateSelected(object sender, DateChangedEventArgs e)
        {
            if (BindingContext is TaskListViewModel viewModel)
            {
                viewModel.FilterTasksByDate(e.NewDate);
            }
        }


        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await ViewModel.OnAppearing();
        }
    }
}
