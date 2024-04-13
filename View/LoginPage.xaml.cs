using ReminderApplication.ViewModel;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;

namespace ReminderApplication.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        public LoginPage() : this(new LoginViewModel())
        {
        }

        public LoginPage(LoginViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private async void OnRegisterButtonClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }
    }
}
