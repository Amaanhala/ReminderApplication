using SQLite;

namespace ReminderApplication.Model
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserType Role { get; set; }
        public int PatientId { get; set; }
        public int CaregiverId { get; set; }
    }

    public enum UserType
    {
        Patient,
        Caretaker
    }
}
