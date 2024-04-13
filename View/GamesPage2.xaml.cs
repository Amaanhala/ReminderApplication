using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Xaml;
using ReminderApplication.ViewModel;


namespace ReminderApplication.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamesPage2 : ContentPage
    {
        public GamesPage2()
        {
            InitializeComponent();
            this.Title = "Memory Game";
            BindingContext = new MemoryGameViewModel2();
        }
    }
}
