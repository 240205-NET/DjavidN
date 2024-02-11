
namespace FitnessTracker{

  abstract class Training{

    public List<Weekday> day = new List<Weekday>();
    
    public abstract double CalculateBurntCalories();
  }

  class Running : Training{
    override public double CalculateBurntCalories(){

      return 0.0;
    }
  }

  class Pushups : Training{
    override public double CalculateBurntCalories()
    {
      return 0.0;
    }
  }

  class Pullups : Training{
    override public double CalculateBurntCalories()
    {
      return 0.0;
    }
  } 

  class BenchPress : Training{
    override public double CalculateBurntCalories()
    {
      return 0.0;
    }
  }

}