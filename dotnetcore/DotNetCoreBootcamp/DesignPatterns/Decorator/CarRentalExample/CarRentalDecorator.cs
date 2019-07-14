namespace DesignPatterns.Decorator.CarRentalExample
{
    public abstract class CarRentalDecorator : IRental
    {
        protected readonly IRental rental;
        public CarRentalDecorator(IRental rental)
        {
            this.rental = rental;
        }
        public int GetDaysRented()
        {
            return rental.GetDaysRented();
        }

        public Model GetModel()
        {
            return rental.GetModel();
        }

        public float GetFuelConsumed()
        {
            return rental.GetFuelConsumed();
        }

        public void SetFuelConsumed(float amount)
        {
            rental.SetFuelConsumed(amount);
        }

        public virtual float CalcPrice()
        {
            return rental.CalcPrice();
        }
    }
}
