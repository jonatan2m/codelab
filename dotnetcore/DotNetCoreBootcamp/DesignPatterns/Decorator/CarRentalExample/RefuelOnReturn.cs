namespace DesignPatterns.Decorator.CarRentalExample
{
    public class RefuelOnReturn : CarRentalDecorator
    {
        private readonly float _refuelPrice;

        public RefuelOnReturn(IRental rental, float refuelPrice)
        :base(rental)
        {
            _refuelPrice = refuelPrice;
        }
        public override float CalcPrice()
        {
            return rental.CalcPrice() + RefuelPrice();
        }

        private float RefuelPrice()
        {
            return (GetModel().fuelCapacity - GetFuelConsumed()) * _refuelPrice;
        }
    }
}
