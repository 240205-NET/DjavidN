namespace FitnessTracker.Tests;

using System.Windows.Forms;
using FitnessTracker;
using Microsoft.VisualStudio.TestPlatform.TestHost;

public class UnitTests
{
    [Fact]
    public void UserTest()  /// Evaluating user registration
    {
        string name = "John".ToLower();
        double weight = 70.5;
        double height = 175;

        // Save the current console input and output
        TextReader originalInput = Console.In;
        TextWriter originalOutput = Console.Out;

        // Set up a string reader with the input values
        StringReader stringReader = new StringReader($"{name}\n{weight}\n{height}\n");
        Console.SetIn(stringReader);

        // Redirect Console.WriteLine to StringWriter
        using StringWriter stringWriter = new StringWriter();
        Console.SetOut(stringWriter);

        // Act
        Trainee newTrainee = FitnessTracker.Program.RegisterUser();

        // Reset Console input and output
        Console.SetIn(originalInput);
        Console.SetOut(originalOutput);


        Assert.Equal(name, newTrainee.Name);
        Assert.Equal(weight, newTrainee.Weight);
        Assert.Equal(height, newTrainee.Height);
    }

    [Fact]
    public void TestSavedCalories()
    {
        Trainee newTrainee = new Trainee();
        newTrainee.Training_Record.Add(new TrainingRecord
            {
                Training = new Running(),
                Day = Weekday.Monday,
                Calories = 303
            });

        newTrainee.Training_Record.Add(new TrainingRecord
            {
                Training = new PushUps(),
                Day = Weekday.Wednesday,
                Calories = 404
            });

        FitnessTracker.Program.SaveCaloriesPerDay(ref newTrainee);

        // Assert
        string userName = newTrainee.Name ?? "unknown";
        string filename = $"{userName}_training.xml";

        Assert.True(File.Exists(filename)); // Check that the file was created

    }

    


}