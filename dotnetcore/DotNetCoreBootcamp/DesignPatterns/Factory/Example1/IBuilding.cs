namespace DesignPatterns.Factory.Example1
{
    public interface IBuilding
    {
        string BuildingType();
    }

    public class House : IBuilding
    {
        public string BuildingType()
        {
            return "house";
        }
    }

    public class Edifice : IBuilding
    {
        public string BuildingType()
        {
            return "edifice";
        }
    }
}