using System;
using System.IO;
using Microsoft.Maui.Controls;
using CommunityToolkit.Maui.Views;
using ReminderApplication.Model;
using ReminderApplication.ViewModel;

namespace ReminderApplication.View
{
    public partial class AddMusicPage : ContentPage
    {
        private TaskListViewModel _viewModel;
        private string _selectedFilePath;
        private int _caregiverId;

        public AddMusicPage(TaskListViewModel viewModel, int caregiverId)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
            _caregiverId = caregiverId;
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_selectedFilePath))
            {
                await DisplayAlert("Missing File", "Please select a music file before saving.", "OK");
                return;
            }

            var audioFiles = new PatientMusic
            {
                CaregiverId = _caregiverId,
                FilePath = _selectedFilePath
            };

            Console.WriteLine($"Saving music file with CaregiverId: {_caregiverId}, FilePath: {_selectedFilePath}");

            await _viewModel.AddPatientMusicAsync(audioFiles);
            await DisplayAlert("Music Saved", "The music file has been saved successfully.", "OK");
            await Shell.Current.Navigation.PopAsync();
        }


        private async void OnSelectMusicClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Pick a music file",
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                {
                    { DevicePlatform.Android, new[] { "audio/*" } },
                })

            });

            if (result != null)
            {
                _selectedFilePath = result.FullPath;
                SelectedMusicLabel.Text = $"Selected music file: {Path.GetFileName(_selectedFilePath)}";
            }
        }

    }
}
