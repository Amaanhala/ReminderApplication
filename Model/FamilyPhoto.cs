using SQLite;

namespace ReminderApplication.Model
{
    public class FamilyPhoto
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public int PatientId { get; set; }
        public int CaregiverId { get; set; }
        public byte[] ImageBytes { get; set; }
        public string FamilyMemberName { get; set; }
    }
}
