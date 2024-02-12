using System;
using System.Net;
using ScottPlot;
using System.IO;
using System.Xml.Serialization;
using System.Runtime.CompilerServices;

namespace FitnessTracker{

  class Program{

    public static DateTime currentDate = DateTime.Today;
    public static Weekday today = (Weekday)currentDate.DayOfWeek;
    public static void Main(){
      Console.WriteLine("**** Welcome to the Fitness App **** \n\n");

      Trainee newTrainee = new();
      UserLog(ref newTrainee);
      //Plot2D();

    }

    public static void UserLog(ref Trainee newTrainee){

      Console.WriteLine("Are you a returning user (y/n):");
      
      bool Flag = true;

      do{
        string? Input = Console.ReadLine()?.ToLower();

        switch(Input){

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

      }while(Flag);




    }

    public static Trainee RegisterUser(){

      Console.WriteLine("Please enter your name:");
      string? name = Console.ReadLine()?.ToLower();

      Console.WriteLine("Please enter your weight:");

      double weight;
      while(!double.TryParse(Console.ReadLine(), out weight) || weight <= 0)
      {
        Console.WriteLine("Invalied weight. Please enter a valid positive number:");
      }

      Console.WriteLine("Please enter your height:");

      double height;
      while(!double.TryParse(Console.ReadLine(), out height) || height <= 0)
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

    public static Trainee RetrieveUser(){

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

    public static void TrainingData(ref Trainee newTrainee){

      bool Exit = false;

      while(Exit){
        Console.WriteLine("Please choose an action:");
        Console.WriteLine("1. Record a workout.");
        Console.WriteLine("2. View your health curve.");
        Console.WriteLine("3. Exit.");

        bool success = int.TryParse(Console.ReadLine(), out int opt);

        switch(opt){
          case 1:
            RecordWorkOut(ref Trainee newTrainee);
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

    public static void RecordWorkOut(ref Trainee newTrainee){
      
    }

    public static void Plot2D(){
      double[] dataX = { 1, 2, 3, 4, 5 };
      double[] dataY = { 1, 4, 9, 16, 25 };

      ScottPlot.Plot myPlot = new();
      myPlot.Add.Scatter(dataX, dataY);

      myPlot.SavePng("CaloryCurve.png", 400, 300);
    }

  }
}
