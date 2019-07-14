namespace DesignPatterns.Decorator.CarRentalExample
{
    public interface IRental
    {
        float CalcPrice();
        int GetDaysRented();
        Model GetModel();
        float GetFuelConsumed();
        void SetFuelConsumed(float amount);
    }
}