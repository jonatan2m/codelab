namespace DesignPatterns.Decorator.CarRentalExample
{
    /// <summary>
    /// Decorator component
    /// </summary>
    public interface IRental
    {
        float CalcPrice();
        int GetDaysRented();
        Model GetModel();
        float GetFuelConsumed();
        void SetFuelConsumed(float amount);
    }
}