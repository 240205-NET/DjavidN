using System;
using System.Net;
using ScottPlot;

namespace FitnessTracker{

  class Program{

    public static void Main(){
      Console.WriteLine("**** Welcome to the Fitness App **** \n\n");

      Plot2D();
      List<Trainee> trainees = new List<Trainee>();

    }

    public static void Menu(){

      Console.WriteLine("Are you a returning user (y/n):");
      
      bool Flag = true;

      do{
        string? Input = Console.ReadLine()?.ToLower();

        switch(Input){

          case "yes":
          case "y":
            RegisterUser();
            Flag = false;
            break;

          case "no":
          case "n":
            RetrieveUser();
            Flag = false;
            break;

          default:
            Console.WriteLine("Invalid Input");
            break;

        }

      }while(Flag);


    }

    public static Trainee RegisterUser(){

      return null;
    }

    public static Trainee RetrieveUser(){

      return null;

    }

    public static void Plot2D(){
      double[] dataX = { 1, 2, 3, 4, 5 };
      double[] dataY = { 1, 4, 9, 16, 25 };

      ScottPlot.Plot myPlot = new();
      myPlot.Add.Scatter(dataX, dataY);

      myPlot.SavePng("quickstart.png", 400, 300);
    }

  }
}
