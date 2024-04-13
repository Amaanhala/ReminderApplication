using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Maui.Controls;
using ReminderApplication.Model;
using ReminderApplication.ViewModel;

namespace ReminderApplication.View
{
    public partial class AddImagePage : ContentPage
    {
        private TaskListViewModel _viewModel;
        private byte[] _imageBytes;

        public AddImagePage(TaskListViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = _viewModel = viewModel;
        }

        private async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            if (_imageBytes == null || string.IsNullOrWhiteSpace(FamilyMemberName.Text))
            {
                await DisplayAlert("Missing Fields", "Please add an image and family member's name before saving.", "OK");
                return;
            }

            var image = new FamilyPhoto
            {
                PatientId = _viewModel.PatientId,
                CaregiverId = _viewModel.CaregiverId,
                ImageBytes = _imageBytes,
                FamilyMemberName = FamilyMemberName.Text,
            };

            Debug.WriteLine($"CaregiverId: {image.CaregiverId}"); // Debug statement to display CaregiverId
            Debug.WriteLine($"PatientId: {image.PatientId}");
            Debug.WriteLine($"Image size: {_imageBytes.Length} bytes"); // Debug statement to display image size

            await _viewModel.AddFamilyPhotoAsync(image);
            await DisplayAlert("Image Saved", "The image has been saved successfully.", "OK");
            await Shell.Current.Navigation.PopAsync();
        }


        private async void OnPickImageClicked(object sender, EventArgs e)
        {
            var result = await FilePicker.PickAsync(new PickOptions
            {
                PickerTitle = "Pick an image"
            });

            if (result != null)
            {
                var stream = await result.OpenReadAsync();
                SelectedImage.Source = ImageSource.FromStream(() => stream);

                using (MemoryStream ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    _imageBytes = ms.ToArray();
                }

                await DisplayAlert("1 Photo Selected", "1 photo has been selected. Click on 'Save' to save the photo.", "OK");
            }
        }

        private async void OnImageTapped(object sender, EventArgs e)
        {
            var image = ((Image)sender).BindingContext as FamilyPhoto;

            bool answer = await DisplayAlert("Delete Photo", "Are you sure you want to delete this photo?", "Yes", "No");

            if (answer)
            {
                await _viewModel.DeleteFamilyPhotoAsync(image);
                _viewModel.FamilyPhotos.Remove(image);
            }
        }

        private async void LoadFamilyPhotos()
        {
            await _viewModel.GetFamilyPhotosAsync();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadFamilyPhotosAsync();
        }


    }
}
