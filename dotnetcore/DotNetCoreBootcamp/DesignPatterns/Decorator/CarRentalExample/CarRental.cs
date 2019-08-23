using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Decorator.CarRentalExample
{
    /// <summary>
    /// Decorator ConcreteComponent
    /// </summary>
    public class CarRental : IRental
    {
        protected float fuelConsumed;
        protected int days;
        protected Model model;
        public CarRental(Model m, int rentalDays)
        {
            model = m;
            days = rentalDays;
        }
        public float CalcPrice()
        {
            float price = (model.price * days);
            return price;
        }
        public int GetDaysRented()
        {
            return days;
        }
        public Model GetModel()
        {
            return model;
        }
        public float GetFuelConsumed()
        {
            return fuelConsumed;
        }
        public void SetFuelConsumed(float amount)
        {
            fuelConsumed = amount;
        }
    }
}
