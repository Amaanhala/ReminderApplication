using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using ReminderApplication.ViewModel;


namespace ReminderApplication.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamesPage : ContentPage
    {
        public GamesPage()
        {
            InitializeComponent();
            this.Title = "Memory Game";
            BindingContext = new MemoryGameViewModel();
        }
    }
}
