using SQLite;

namespace ReminderApplication.Model
{
    public class PatientTasks
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int CaregiverId { get; set; }
        public string Title { get; set; }

        public string Breakfast { get; set; }
        public string Lunch { get; set; }
        public string Dinner { get; set; }
        public string FoodDetails { get; set; }
        public string WalkDetails { get; set; }
        public string Activity { get; set; }
        public string Appointments { get; set; }

        public string WaterReminderMorning { get; set; }
        public string WaterReminderAfternoon { get; set; }
        public string WaterReminderEvening{ get; set; }
        public string WaterReminderNight { get; set; }

        public string MorningMedicines { get; set; }
        public string AfternoonMedicines { get; set; }
        public string EveningMedicines { get; set; }
        public string NightMedicines { get; set; }

        public string MoreTasks1 { get; set; }
        public string MoreTasks2 { get; set; }
        public string MoreTasks3 { get; set; }
        public string MoreTasks4 { get; set; }
        public string MoreTasks5 { get; set; }

        public string TaskName { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime DueDate { get; set; }
        public int AssignedToPatientId { get; set; }

        public string PhotoDetails { get; set; }
        public byte[] ImageBytes { get; set; }

        public TimeSpan BreakfastTime { get; set; }
        public TimeSpan LunchTime { get; set; }
        public TimeSpan DinnerTime { get; set; }


        public TimeSpan WaterReminderMorningTime { get; set; }
        public TimeSpan WaterReminderAfternoonTime { get; set; }
        public TimeSpan WaterRemindereEveningTime { get; set; }
        public TimeSpan WaterReminderNightTime { get; set; }


        public TimeSpan MorningMedicinesTime { get; set; }
        public TimeSpan AfternoonMedicinesTime { get; set; }
        public TimeSpan EveningMedicinesTime { get; set; }
        public TimeSpan NightMedicinesTime { get; set; }


        public TimeSpan MoreTasksTime1 { get; set; }
        public TimeSpan MoreTasksTime2 { get; set; }
        public TimeSpan MoreTasksTime3 { get; set; }
        public TimeSpan MoreTasksTime4 { get; set; }
        public TimeSpan MoreTasksTime5 { get; set; }


        public TimeSpan FoodTime { get; set; }
        public TimeSpan WalkTime { get; set; }
        public TimeSpan ActivityTime { get; set; }
        public TimeSpan AppointmentsTime { get; set; }

        public PatientTasks()
        {
            Breakfast = "";
            Lunch = "";
            Dinner = "";

            WaterReminderMorning = "";
            WaterReminderAfternoon = "";
            WaterReminderEvening = "";
            WaterReminderNight = "";

            MorningMedicines = "";
            AfternoonMedicines = "";
            EveningMedicines = "";
            NightMedicines = "";

            MoreTasks1 = "";
            MoreTasks2 = "";
            MoreTasks3 = "";
            MoreTasks4 = "";
            MoreTasks5 = "";

            FoodDetails = "";
            WalkDetails = "";
            Activity = "";
            Appointments = "";

            PhotoDetails = "";
            TaskName = "";
            Description = "";
            IsCompleted = false;
        }
    }
}
