using SQLite;

namespace ReminderApplication.Model
{
    public class PatientMusic
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PatientId { get; set; }
        public int CaregiverId { get; set; }
        public string FileName { get; set; }
        public byte[] FileContent { get; set; }
        public string FilePath { get; set; }

    }
}
