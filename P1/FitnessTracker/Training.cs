using System;
using System.Collections.Generic;

namespace FitnessTracker
{
    public abstract class Training
    {
        public abstract double CalculateBurntCalories();
    }

    public class Running : Training
    {
        public override double CalculateBurntCalories()
        {
            
            return 0;
        }
    }

    public class PushUps : Training
    {
        public override double CalculateBurntCalories()
        {
            
            return 0;
        }
    }

    public class PullUps : Training
    {
        public override double CalculateBurntCalories()
        {
            
            return 0;
        }
    }

    public class BenchPress : Training
    {
        public override double CalculateBurntCalories()
        {

            return 0;
        }
    }
}
