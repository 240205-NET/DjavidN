using System.Collections.Generic;

namespace FitnessTracker
{
    public class Trainee
    {
        public string? Name { get; set; } = "";
        public double Weight { get; set; } = 0.0;
        public double Height { get; set; } = 0.0;

        public List<TrainingRecord> Training_Record = new();

        public Trainee() { }

        public Trainee(string? _name)
        {
            Name = _name;
        }
    }

    public class TrainingRecord
    {
        public Training? Training { get; set; }
        public Weekday Day { get; set; }
        public double Calories { get; set; }
    }

    public class DailyTrainingData
    {
        public Weekday Day { get; set; }
        public double BurntCalories { get; set; }
    }
}
