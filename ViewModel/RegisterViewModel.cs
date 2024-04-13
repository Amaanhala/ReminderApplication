using ReminderApplication.Helpers;
using ReminderApplication.Model;
using Microsoft.Maui.Controls;
using System.Threading.Tasks;
using System;
using System.Windows.Input;
using ReminderApplication.View;
using System.Diagnostics;
using BCrypt.Net;

namespace ReminderApplication.ViewModel
{
    public class RegisterViewModel : BaseViewModel
    {
        private string _patientName;
        private string _patientEmail;
        private string _patientPassword;
        private string _name;
        private string _email;
        private string _password;
        private string _role;
        private int _patientId;
        private int _caregiverId;
        private bool _isCaretaker;
        public string CaregiverEmail { get; set; }


        public ICommand RegisterCommand { get; }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        public string Role
        {
            get => _role;
            set => SetProperty(ref _role, value);
        }

        public int PatientId
        {
            get => _patientId;
            set => SetProperty(ref _patientId, value);
        }

        public int CaregiverId
        {
            get => _caregiverId;
            set => SetProperty(ref _caregiverId, value);
        }

        public string PatientName
        {
            get => _patientName;
            set => SetProperty(ref _patientName, value);
        }

        public string PatientEmail
        {
            get => _patientEmail;
            set => SetProperty(ref _patientEmail, value);
        }

        public string PatientPassword
        {
            get => _patientPassword;
            set => SetProperty(ref _patientPassword, value);
        }

        public bool IsCaretaker
        {
            get => _isCaretaker;
            set => SetProperty(ref _isCaretaker, value);
        }

        public int SelectedRoleIndex
        {
            get => _selectedRoleIndex;
            set
            {
                if (SetProperty(ref _selectedRoleIndex, value))
                {
                    Role = value == 0 ? "patient" : "caretaker";
                }
            }
        }
        private int _selectedRoleIndex;



        public RegisterViewModel()
        {
            RegisterCommand = new Command(async () => await RegisterAsync());
        }


        private async Task RegisterAsync()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                if (string.IsNullOrEmpty(Role))
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Role cannot be empty. Please enter 'Patient' or 'Caretaker'", "OK");
                    IsBusy = false;
                    return;
                }

                UserType userType;
                if (Role.ToLower() == "patient")
                    userType = UserType.Patient;
                else if (Role.ToLower() == "caretaker")
                    userType = UserType.Caretaker;
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Invalid role. Please enter 'Patient' or 'Caretaker'", "OK");
                    IsBusy = false;
                    return;
                }


                var user = new User
                {
                    Name = this.Name,
                    Email = this.Email,
                    Password = BCrypt.Net.BCrypt.HashPassword(this.Password),
                    Role = userType,
                };

                Debug.WriteLine($"User: {user.Email}, Password: {this.Password}");

                // Hash the password using BCrypt
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(this.Password);

                Debug.WriteLine($"Hashed password: {hashedPassword}");

                user.Password = hashedPassword;


                // Check if a user with this email already exists
                var existingUser = await App.DatabaseHelper.GetUserByEmailAsync(user.Email);
                if (existingUser != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "An account with this email already exists. Please try another email.", "OK");
                    IsBusy = false;
                    return;
                }

                await App.DatabaseHelper.AddUserAsync(user);
                var savedUser = await App.DatabaseHelper.GetUserByEmailAsync(user.Email);

                if (userType == UserType.Caretaker)
                {
                    if (string.IsNullOrEmpty(PatientName) || string.IsNullOrEmpty(PatientEmail) || string.IsNullOrEmpty(PatientPassword))
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Please provide the patient's name, email, and password.", "OK");
                        IsBusy = false;
                        return;
                    }

                    var patient = new User
                    {
                        Name = PatientName,
                        Email = PatientEmail,
                        Password = BCrypt.Net.BCrypt.HashPassword(PatientPassword),
                        Role = UserType.Patient,
                        CaregiverId = savedUser.ID,
                    };

                    // Check if a patient with this email already exists
                    var existingPatient = await App.DatabaseHelper.GetUserByEmailAsync(patient.Email);
                    if (existingPatient != null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "A patient with this email already exists. Please try another email.", "OK");
                        IsBusy = false;
                        return;
                    }

                    await App.DatabaseHelper.AddUserAsync(patient);
                    var savedPatient = await App.DatabaseHelper.GetUserByEmailAsync(patient.Email);

                    savedUser.PatientId = savedPatient.ID;
                    await App.DatabaseHelper.UpdateUserAsync(savedUser);
                    savedUser = await App.DatabaseHelper.GetUserByEmailAsync(savedUser.Email);

                    Debug.WriteLine($"Caretaker account created. Caregiver ID: {savedUser.ID}, Patient ID: {savedPatient.ID}");
                }
            
                else if (userType == UserType.Patient)
                {
                    var caregiver = await App.DatabaseHelper.GetUserByEmailAsync(CaregiverEmail);

                    if (caregiver != null && caregiver.Role == UserType.Caretaker)
                    {
                        savedUser.CaregiverId = caregiver.ID;
                        await App.DatabaseHelper.UpdateUserAsync(savedUser);

                        caregiver.PatientId = savedUser.ID;
                        await App.DatabaseHelper.UpdateUserAsync(caregiver);

                        Debug.WriteLine($"Patient account created. Caregiver ID: {caregiver.ID}, Patient ID: {savedUser.ID}");
                    }

                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "The provided email does not belong to a registered caretaker. Please provide a valid caretaker email.", "OK");
                        IsBusy = false;
                        return;
                    }
                }

                await Application.Current.MainPage.DisplayAlert("Success", "User registered successfully", "OK");

                await Shell.Current.Navigation.PopAsync();
            }
            catch (NullReferenceException ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", $"An error occurred while registering. Details: {ex.Message} Stack trace: {ex.StackTrace}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

    }
}
