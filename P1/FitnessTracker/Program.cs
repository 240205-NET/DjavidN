using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms.DataVisualization.Charting;

namespace FitnessTracker
{
    class Program
    {
        public static void Main()
        {
            Console.WriteLine("**** Welcome to the Fitness App ****\n");

            Trainee newTrainee = new();
            UserLog(ref newTrainee);
            TrainingData(ref newTrainee);
            SaveCaloriesPerDay(ref newTrainee);
        }

        public static void UserLog(ref Trainee newTrainee)
        {
            Console.WriteLine("Are you a returning user (y/n):");

            bool Flag = true;

            do
            {
                string? Input = Console.ReadLine()?.ToLower();

                switch (Input)
                {
                    case "yes":
                    case "y":
                        newTrainee = RetrieveUser();
                        Flag = false;
                        break;

                    case "no":
                    case "n":
                        newTrainee = RegisterUser();
                        Flag = false;
                        break;

                    default:
                        Console.WriteLine("Invalid Input");
                        break;
                }

            } while (Flag);
        }

        public static Trainee RegisterUser()
        {
            Console.WriteLine("Please enter your name:");
            string? name = Console.ReadLine()?.ToLower();

            Console.WriteLine("Please enter your weight:");
            double weight;
            while (!double.TryParse(Console.ReadLine(), out weight) || weight <= 0)
            {
                Console.WriteLine("Invalid weight. Please enter a valid positive number:");
            }

            Console.WriteLine("Please enter your height:");
            double height;
            while (!double.TryParse(Console.ReadLine(), out height) || height <= 0)
            {
                Console.WriteLine("Invalid height. Please enter a valid positive number:");
            }

            Trainee newTrainee = new(name)
            {
                Height = height,
                Weight = weight
            };

            // Serialize then Trainee object to XML
            XmlSerializer serializer = new(typeof(Trainee));
            using TextWriter writer = new StreamWriter($"{name}.xml");
            serializer.Serialize(writer, newTrainee);

            Console.WriteLine($"User {name} registered successfully!");

            return newTrainee;
        }

        public static Trainee RetrieveUser()
        {
            Console.WriteLine("Please enter your name:");
            string? name = Console.ReadLine()?.ToLower();
            string fileName = $"{name}.xml";
            Trainee returningUser = new();

            if (File.Exists(fileName))
            {
                XmlSerializer serializer = new(typeof(Trainee));
                using FileStream fileStream = new(fileName, FileMode.Open);
                Trainee? deserializedObject = (Trainee?)serializer.Deserialize(fileStream);
                if (deserializedObject != null)
                {
                    returningUser = deserializedObject;
                    Console.WriteLine($"Welcome back, {returningUser.Name}!");
                }
                else
                {
                    Console.WriteLine($"Failed to retrieve user data for {name}. The data may be corrupted.");
                }
            }
            else
            {
                Console.WriteLine($"User with name {name} not found. Please register as a new user.");
                RegisterUser();
            }

            return returningUser;
        }

        public static void TrainingData(ref Trainee newTrainee)
        {
            bool Exit = false;

            while (!Exit)
            {
                Console.WriteLine("Please choose an action:");
                Console.WriteLine("1. Record a workout.");
                Console.WriteLine("2. View your health curve.");
                Console.WriteLine("3. Exit.");

                bool success;
                int opt = 0;
                string input;
                bool LoopFlag = true;
                while (LoopFlag)
                {
                    input = Console.ReadLine();
                    success = int.TryParse(input, out opt);

                    if (success && (opt == 1 || opt == 2 || opt == 3))
                    {
                        LoopFlag = false;
                    }
                    else
                    {
                        Console.WriteLine("Invalid option, try again!");
                    }
                }

                switch (opt)
                {
                    case 1:
                        RecordWorkOut(ref newTrainee);
                        break;
                    case 2:
                        break;
                    case 3:
                        Exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid option, try again!");
                        break;
                }
            }
        }

        public static void RecordWorkOut(ref Trainee newTrainee)
        {
            Console.WriteLine("**** Please select your workout ****");
            Console.WriteLine("1. Running ");
            Console.WriteLine("2. PushUps ");
            Console.WriteLine("3. PullUps ");
            Console.WriteLine("4. BenchPress ");

            bool success = int.TryParse(Console.ReadLine(), out int opt);

            Training newTraining;
            switch (opt)
            {
                case 1:
                    newTraining = new Running();
                    break;
                case 2:
                    newTraining = new PushUps();
                    break;
                case 3:
                    newTraining = new PullUps();
                    break;
                case 4:
                    newTraining = new BenchPress();
                    break;
                default:
                    Console.WriteLine("Invalid choice!");
                    return;
            }

            Weekday currentWeekday = (Weekday)DateTime.Today.DayOfWeek;

            Console.WriteLine($"Today is {currentWeekday}. Enter the burnt calories for this workout:");
            double calories;
            while (!double.TryParse(Console.ReadLine(), out calories) || calories <= 0)
            {
                Console.WriteLine("Invalid input. Please enter a valid positive number:");
            }

            newTrainee.Training_Record.Add(new TrainingRecord
            {
                Training = newTraining,
                Day = currentWeekday,
                Calories = calories
            });
        }

        public static void SaveCaloriesPerDay(ref Trainee newTrainee)
        {
          string userName = newTrainee.Name ?? "unkown";
          string filename = $"{userName}_training.xml";

          List<DailyTrainingData> trainingData = new();

          //Load existing data if the file exists
          if (File.Exists(filename))
          {
            XmlSerializer serializer = new(typeof(List<DailyTrainingData>));
            using FileStream fileStream = new(filename, FileMode.Open);
            trainingData = (List<DailyTrainingData>)serializer.Deserialize(fileStream)!;

          }

          // Update totalCaloriesPerDay with the new data
          foreach(TrainingRecord record in newTrainee.Training_Record)
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

          // Serialize data to XML
          XmlSerializer dataSerializer = new XmlSerializer(typeof(List<DailyTrainingData>));
          using (TextWriter writer = new StreamWriter(filename))
          {
            dataSerializer.Serialize(writer, trainingData);
          }

          Console.WriteLine($"Total calories per day saved in {filename}");

          double[] dataY = { 0, 0, 0, 0, 0, 0, 0 };

          foreach (DailyTrainingData? dataxx in trainingData)
           {
            dataY[(int)dataxx.Day] = dataxx.BurntCalories;
           }

          var data = new Dictionary<string, double>
            {
                { "Monday", dataY[1] },
                { "Tuesday", dataY[2] },
                { "Wednesday", dataY[3] },
                { "Thursday", dataY[4] },
                { "Friday", dataY[5] },
                { "Saturday", dataY[6] },
                { "Sunday", dataY[0] }
            };

            // Create a chart control
            Chart chart = new Chart();
            chart.Size = new System.Drawing.Size(800, 600);

            // Add a chart area and series
            ChartArea chartArea = new ChartArea();
            chart.ChartAreas.Add(chartArea);

            Series series = new Series();
            series.ChartType = SeriesChartType.Column;
            chart.Series.Add(series);

            // Bind the data to the chart
            chart.Series[0].Points.DataBindXY(data.Keys, data.Values);

            // Save the chart as an image
            chart.SaveImage("CaloryCurve.png", ChartImageFormat.Png);

            // Display a message
            Console.WriteLine("CaloryCurve.png saved successfully");
        }
    }
}
