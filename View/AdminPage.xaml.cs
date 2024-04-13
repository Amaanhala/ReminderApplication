using ReminderApplication.ViewModel;
using Microsoft.Maui.Controls;

namespace ReminderApplication.View
{
    public partial class AdminPage : ContentPage
    {
        private readonly AdminViewModel _viewModel;

        public AdminPage()
        {
            InitializeComponent();
            _viewModel = (AdminViewModel)BindingContext;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadUsersAsync();
        }
    }
}
