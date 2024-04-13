using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;

namespace ReminderApplication.View
{
    public partial class PatientMusicPage : ContentPage
    {
        private readonly List<string> _audioFiles;
        private int _currentSongIndex = 0;
        private int _caregiverId;

        public PatientMusicPage(List<string> audioFiles, int caregiverId)
        {
            try
            {
                InitializeComponent();
                BindingContext = this;
                Console.WriteLine($"Data passed to the PatientMusicPage constructor: {string.Join(", ", audioFiles)}");

                if (audioFiles == null || !audioFiles.Any())
                {
                    Console.WriteLine("No audio files found for the patient.");
                    return;
                }

                System.Diagnostics.Debug.WriteLine($"Audio files passed to the constructor: {string.Join(", ", audioFiles)}");

                _audioFiles = audioFiles;
                _caregiverId = caregiverId;
                mediaElement.MediaOpened += (sender, e) => Console.WriteLine("MediaOpened event triggered");

                Console.WriteLine($"Loaded audio files for CaregiverId: {_caregiverId}:");
                foreach (var audioFile in _audioFiles)
                {
                    Console.WriteLine($"- {audioFile}");
                }

                Console.WriteLine($"Caregiver ID passed to the PatientMusicPage constructor: {_caregiverId}");

                LoadAudioFile(_audioFiles[_currentSongIndex]);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in PatientMusicPage constructor: {ex}");
            }
        }


        public List<string> AudioFiles => _audioFiles;

        private void LoadAudioFile(string filePath)
        {
            System.Diagnostics.Debug.WriteLine($"FilePath: {filePath}");

            if (string.IsNullOrEmpty(filePath))
            {
                Console.WriteLine("Cannot load audio file: filePath is null or empty");
                return;
            }

            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File does not exist: {filePath}");
                return;
            }

            try
            {
                var mediaSource = MediaSource.FromFile(filePath);
                if (mediaSource != null)
                {
                    Console.WriteLine($"Setting mediaElement.Source to {mediaSource}");
                    mediaElement.Source = mediaSource;
                    Console.WriteLine($"Loaded audio file: {mediaElement.Source}");
                }
                else
                {
                    Console.WriteLine($"MediaSource.FromFile returned null for file path: {filePath}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading audio file: {ex.Message}");
            }
        }


        private void OnPlayButtonClicked(object sender, EventArgs e)
        {
            Console.WriteLine("OnPlayButtonClicked - Before mediaElement.Play()");
            mediaElement.Play();
            Console.WriteLine("OnPlayButtonClicked - After mediaElement.Play()");
        }

        private void OnPauseButtonClicked(object sender, EventArgs e)
        {
            Console.WriteLine("OnPauseButtonClicked - Before mediaElement.Pause()");
            mediaElement.Pause();
            Console.WriteLine("OnPauseButtonClicked - After mediaElement.Pause()");
        }

        private void OnNextButtonClicked(object sender, EventArgs e)
        {
            Console.WriteLine("OnNextButtonClicked - Before NextSong()");
            NextSong();
            Console.WriteLine("OnNextButtonClicked - After NextSong()");
            mediaElement.Play();
        }

        private void NextSong()
        {
            _currentSongIndex++;
            if (_currentSongIndex >= _audioFiles.Count)
            {
                _currentSongIndex = 0;
            }
            LoadAudioFile(_audioFiles[_currentSongIndex]);
        }

        private void OnSongListSelectionChanged(object sender, SelectedItemChangedEventArgs e)
        {
            Console.WriteLine("OnSongListSelectionChanged - Before checking e.SelectedItem");
            if (e.SelectedItem is string selectedSong)
            {
                mediaElement.Stop();
                _currentSongIndex = _audioFiles.IndexOf(selectedSong);
                LoadAudioFile(selectedSong);
                mediaElement.Play();
            }
        }

    }
}
