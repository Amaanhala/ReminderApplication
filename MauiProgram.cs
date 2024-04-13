using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Maui;
using Microsoft.Maui.Controls.Hosting;
using ReminderApplication.Helpers;
using Microsoft.Maui.Hosting;
using CommunityToolkit.Maui;

namespace ReminderApplication
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                //.using CommunityToolkit.Maui.Extensions
                .UseMauiCommunityToolkitMediaElement()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddSingleton<DatabaseHelper>(provider =>
                new DatabaseHelper(Path.Combine(provider.GetService<IFileSystem>().AppDataDirectory, "reminderApp.db3")));

            return builder.Build();
        }
    }
}
