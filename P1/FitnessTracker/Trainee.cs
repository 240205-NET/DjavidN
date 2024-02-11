
namespace FitnessTracker{

  class Trainee{

    public string Name{ get;set; } = "";
    public double Weight{ get;set; } = 0.0;
    public double Hight{ get;set; } = 0.0;

    public List<Training> Training_Record = new List<Training>();

    public Trainee(){
    }
    public Trainee(string _name){
      Name = _name;
    }

  }
}