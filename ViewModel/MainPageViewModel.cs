using ReminderApplication.Helpers;
using ReminderApplication.View;

namespace ReminderApplication.ViewModel;


public class MainPageViewModel : BaseViewModel
{
    private readonly DatabaseHelper _databaseHelper;

    public MainPageViewModel(DatabaseHelper databaseHelper)
    {
        _databaseHelper = databaseHelper;
    }

}