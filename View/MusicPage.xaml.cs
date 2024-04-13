using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Maui.Controls;

using CommunityToolkit.Maui.Views;
using System.Reflection;
//using Microsoft.Maui.Controls.Media;


namespace ReminderApplication.View;

public partial class MusicPage : ContentPage
{
    private readonly List<string> _audioFiles = new List<string> { "song1.mp3", "song2.mp3", "song3.mp3", "audio4.mp4" };
    private int _currentSongIndex = 0;

    public MusicPage()
    {
        InitializeComponent();
        BindingContext = this;
        LoadAudioFile(_audioFiles[_currentSongIndex]);
        System.Diagnostics.Debug.WriteLine($"Loaded {_audioFiles.Count} audio files.");

    }

    public List<string> AudioFiles => _audioFiles;

    private async Task LoadAudioFile(string fileName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var resourceName = $"ReminderApplication.Resources.Audio.{fileName}";

        string appDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var filePath = Path.Combine(appDataDirectory, fileName);

        if (!File.Exists(filePath))
        {
            using Stream stream = assembly.GetManifestResourceStream(resourceName);
            if (stream != null)
            {
                using var fileStream = File.Create(filePath);
                await stream.CopyToAsync(fileStream);
            }
        }

        mediaElement.Source = MediaSource.FromFile(filePath);
    }


    private void OnPlayButtonClicked(object sender, EventArgs e)
    {
        mediaElement.Play();
    }

    private void OnPauseButtonClicked(object sender, EventArgs e)
    {
        mediaElement.Pause();
    }

    private void OnStopButtonClicked(object sender, EventArgs e)
    {
        mediaElement.Stop();
    }

    private async void OnNextButtonClicked(object sender, EventArgs e)
    {
        NextSong();
        if (Path.GetExtension(_audioFiles[_currentSongIndex]).ToLower() == ".mp3")
        {
            mediaElement.Play();
        }
        else if (Path.GetExtension(_audioFiles[_currentSongIndex]).ToLower() == ".mp4")
        {
            mediaElement.Play();
        }
    }

    private async void NextSong()
    {
        _currentSongIndex++;
        if (_currentSongIndex >= _audioFiles.Count)
        {
            _currentSongIndex = 0;
        }
        await LoadAudioFile(_audioFiles[_currentSongIndex]);
    }

    private async void OnSongListSelectionChanged(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is string selectedSong)
        {
            mediaElement.Stop();
            _currentSongIndex = _audioFiles.IndexOf(selectedSong);
            await LoadAudioFile(selectedSong);
            if (Path.GetExtension(selectedSong).ToLower() == ".mp3")
            {
                mediaElement.Play();
            }
            else if (Path.GetExtension(selectedSong).ToLower() == ".mp4")
            {
                mediaElement.Play();
            }
        }
    }
}