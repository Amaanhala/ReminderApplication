using Microsoft.Maui.Controls;
using ReminderApplication.ViewModel;

namespace ReminderApplication.View
{
    public partial class FamilyPhotosPage : ContentPage
    {
        private TaskListViewModel _viewModel;

        public FamilyPhotosPage(TaskListViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

    }
}
