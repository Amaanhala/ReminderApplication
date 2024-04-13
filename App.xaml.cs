using ReminderApplication.View;
using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui.Controls.PlatformConfiguration.WindowsSpecific;
using Application = Microsoft.Maui.Controls.Application;
using ReminderApplication.Helpers;
using ReminderApplication.ViewModel;
using System.IO;
using Microsoft.Maui.Controls.Xaml;

namespace ReminderApplication
{
    public partial class App : Application
    {
        private static DatabaseHelper _databaseHelper;

        public static DatabaseHelper DatabaseHelper
        {
            get
            {
                if (_databaseHelper == null)
                {
                    string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ReminderApp.db3");
                    _databaseHelper = new DatabaseHelper(dbPath);
                }
                return _databaseHelper;
            }
        }

        public App()
        {
            InitializeComponent();

            MainPage = new AppShell();

        }
    }
}
