namespace DesignPatterns.Decorator.CarRentalExample
{
    /// <summary>
    /// Decorator ConcreteDecorator
    /// </summary>
    public class Insurance : CarRentalDecorator
    {
        private readonly float rate;

        public Insurance(IRental rental, float insuranceRate)
        :base(rental)
        {
            this.rate = insuranceRate;
        }
        public override float CalcPrice()
        {
            return rental.CalcPrice() + insuranceAmount();
        }

        private float insuranceAmount()
        {
            return rate * GetDaysRented();
        }
    }
}
