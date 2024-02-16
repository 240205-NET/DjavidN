using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms.DataVisualization.Charting;

namespace FitnessTracker
{
    public class Program
    {
        static void Main()
        {
            Console.WriteLine("**** Welcome to the Fitness App ****\n");

            Trainee newTrainee = new();
            UserLog(ref newTrainee);
            TrainingData(ref newTrainee);
            SaveCaloriesPerDay(ref newTrainee);
        }

        static void UserLog(ref Trainee newTrainee)
        {
            Console.WriteLine("Are you a returning user (y/n):");

            string? input;
            do
            {
                input = Console.ReadLine()?.ToLower();

                switch (input)
                {
                    case "yes":
                    case "y":
                        newTrainee = RetrieveUser();
                        break;

                    case "no":
                    case "n":
                        newTrainee = RegisterUser();
                        break;

                    default:
                        Console.WriteLine("Invalid Input");
                        break;
                }

            } while (input != "yes" && input != "y" && input != "no" && input != "n");
        }

        public static Trainee RegisterUser()
        {
            Console.WriteLine("Please enter your name:");
            string? name = Console.ReadLine()?.ToLower();

            double weight = GetUserInput("weight");
            double height = GetUserInput("height");

            Trainee newTrainee = new(name)
            {
                Height = height,
                Weight = weight
            };

            SerializeTrainee(newTrainee, $"{name}.xml");

            Console.WriteLine($"User {name} registered successfully!");

            return newTrainee;
        }

        static double GetUserInput(string prompt)
        {
            double value;
            do
            {
                Console.WriteLine($"Please enter your {prompt}:");
            } while (!double.TryParse(Console.ReadLine(), out value) || value <= 0);

            return value;
        }

        static Trainee RetrieveUser()
        {
            Console.WriteLine("Please enter your name:");
            string? name = Console.ReadLine()?.ToLower();
            string fileName = $"{name}.xml";
            Trainee returningUser = new();

            if (File.Exists(fileName))
            {
                returningUser = DeserializeTrainee(fileName);
                Console.WriteLine($"Welcome back, {returningUser.Name}!");
            }
            else
            {
                Console.WriteLine($"User with name {name} not found. Please register as a new user.");
                returningUser = RegisterUser();
            }

            return returningUser;
        }

        static void TrainingData(ref Trainee newTrainee)
        {
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("Please choose an action:");
                Console.WriteLine("1. Record a workout.");
                Console.WriteLine("2. Exit.");

                int opt = GetMenuOption(2);

                switch (opt)
                {
                    case 1:
                        RecordWorkout(ref newTrainee);
                        break;
                    case 2:
                        exit = true;
                        break;
                }
            }
        }

        public static void RecordWorkout(ref Trainee newTrainee)
        {
            Console.WriteLine("**** Please select your workout ****");
            Console.WriteLine("1. Running ");
            Console.WriteLine("2. PushUps ");
            Console.WriteLine("3. PullUps ");
            Console.WriteLine("4. BenchPress ");

            int opt = GetMenuOption(4);

            Training newTraining = opt switch
            {
                1 => new Running(),
                2 => new PushUps(),
                3 => new PullUps(),
                4 => new BenchPress(),
                _ => throw new NotImplementedException()
            };

            Weekday currentWeekday = (Weekday)DateTime.Today.DayOfWeek;

            double calories = GetUserInput("burnt calories");

            newTrainee.Training_Record.Add(new TrainingRecord
            {
                Training = newTraining,
                Day = currentWeekday,
                Calories = calories
            });
        }

        static int GetMenuOption(int maxOption)
        {
            bool success;
            int opt;
            do
            {
                success = int.TryParse(Console.ReadLine(), out opt);

                if (!success || opt < 1 || opt > maxOption)
                {
                    Console.WriteLine("Invalid option, try again!");
                }

            } while (!success || opt < 1 || opt > maxOption);

            return opt;
        }

        public static void SaveCaloriesPerDay(ref Trainee newTrainee)
        {
            string userName = newTrainee.Name ?? "unknown";
            string filename = $"{userName}_training.xml";

            List<DailyTrainingData> trainingData = LoadTrainingData(filename);

            UpdateTrainingData(ref newTrainee, ref trainingData);

            SerializeTrainingData(trainingData, filename);

            Dictionary<string, double> data = new Dictionary<string, double>
            {
                { "Monday", GetBurntCalories(trainingData, Weekday.Monday) },
                { "Tuesday", GetBurntCalories(trainingData, Weekday.Tuesday) },
                { "Wednesday", GetBurntCalories(trainingData, Weekday.Wednesday) },
                { "Thursday", GetBurntCalories(trainingData, Weekday.Thursday) },
                { "Friday", GetBurntCalories(trainingData, Weekday.Friday) },
                { "Saturday", GetBurntCalories(trainingData, Weekday.Saturday) },
                { "Sunday", GetBurntCalories(trainingData, Weekday.Sunday) }
            };

            CreateAndSaveChart(data);
        }

        static List<DailyTrainingData> LoadTrainingData(string filename)
        {
            List<DailyTrainingData> trainingData = new();

            if (File.Exists(filename))
            {
                trainingData = DeserializeTrainingData(filename);
            }

            return trainingData;
        }

        static void UpdateTrainingData(ref Trainee newTrainee, ref List<DailyTrainingData> trainingData)
        {
            foreach (TrainingRecord record in newTrainee.Training_Record)
            {
                DailyTrainingData? existingData = trainingData.Find(d => d.Day == record.Day);
                if (existingData != null)
                {
                    existingData.BurntCalories += record.Calories;
                }
                else
                {
                    trainingData.Add(new DailyTrainingData { Day = record.Day, BurntCalories = record.Calories });
                }
            }
        }

        static void SerializeTrainingData(List<DailyTrainingData> trainingData, string filename)
        {
            XmlSerializer dataSerializer = new XmlSerializer(typeof(List<DailyTrainingData>));
            using (TextWriter writer = new StreamWriter(filename))
            {
                dataSerializer.Serialize(writer, trainingData);
            }

            Console.WriteLine($"Total calories per day saved in {filename}");
        }

        static double GetBurntCalories(List<DailyTrainingData> trainingData, Weekday day)
        {
            DailyTrainingData? data = trainingData.Find(d => d.Day == day);
            return data?.BurntCalories ?? 0;
        }

        static void CreateAndSaveChart(Dictionary<string, double> data)
        {
            Chart chart = new Chart();
            chart.Size = new System.Drawing.Size(800, 600);

            ChartArea chartArea = new ChartArea();
            chart.ChartAreas.Add(chartArea);

            Series series = new Series();
            series.ChartType = SeriesChartType.Column;
            chart.Series.Add(series);

            chart.Series[0].Points.DataBindXY(data.Keys, data.Values);

            chart.SaveImage("CaloryCurve.png", ChartImageFormat.Png);

            Console.WriteLine("CaloryCurve.png saved successfully");
        }

        static Trainee DeserializeTrainee(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Trainee));
            using FileStream fileStream = new(fileName, FileMode.Open);
            return (Trainee)serializer.Deserialize(fileStream)!;
        }

        static List<DailyTrainingData> DeserializeTrainingData(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<DailyTrainingData>));
            using FileStream fileStream = new(fileName, FileMode.Open);
            return (List<DailyTrainingData>)serializer.Deserialize(fileStream)!;
        }

        static void SerializeTrainee(Trainee trainee, string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Trainee));
            using TextWriter writer = new StreamWriter(fileName);
            serializer.Serialize(writer, trainee);
        }
    }
}
