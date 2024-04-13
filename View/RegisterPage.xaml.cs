using Microsoft.Maui.Controls;
using ReminderApplication.ViewModel;
using System;

namespace ReminderApplication.View
{
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            BindingContext = new RegisterViewModel();
        }

        private void OnRolePickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            string selectedRole = (string)picker.SelectedItem;

            if (selectedRole == "Caretaker")
            {
                CaretakerDetails.IsVisible = true;
                PatientDetails.IsVisible = false;
            }
            else if (selectedRole == "Patient")
            {
                PatientDetails.IsVisible = true;
                CaretakerDetails.IsVisible = false;
            }
        }

        private async void OnBackToLoginButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
