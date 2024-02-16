namespace FitnessTracker.Tests;
using FitnessTracker;

public class UnitTests
{
    [Fact]
    public void Test1()  /// Evaluating user registration
    {
        string name = "John";
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
        Trainee newTrainee = Program.RegisterUser();

        // Reset Console input and output
        Console.SetIn(originalInput);
        Console.SetOut(originalOutput);


        Assert.Equal(name, newTrainee.Name);
        Assert.Equal(weight, newTrainee.Weight);
        Assert.Equal(height, newTrainee.Height);
    }

    [Theory]
    [InlineData(Weekday.Monday, 100)]
    [InlineData(Weekday.Tuesday, 150)]
    [InlineData(Weekday.Wednesday, 80)]
    public void RecordWorkout_Validation(Weekday day, double BurntCalories){

        Trainee newTrainee = new Trainee("Nima");
        TrainingRecord newRecord = new TrainingRecord
        {
            Training = new Running(),
            Day = Weekday.Tuesday,
            Calories = 150
        };

        Program.RecordWorkout(ref newTrainee, day, BurntCalories);

        Assert.Equal(2, newTrainee.Training_Record.Count);
        Assert.Contains(newTrainee.Training_Record, record => record.day == day && record.Calories == BurntCalories);
    }


}