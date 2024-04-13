using Android.App;
using Android.Content.PM;
using Android.OS;
using Java.Lang.Reflect;
using ReminderApplication.Helpers;
using Microsoft.Maui;
using System.IO;

namespace ReminderApplication;


[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{

}
