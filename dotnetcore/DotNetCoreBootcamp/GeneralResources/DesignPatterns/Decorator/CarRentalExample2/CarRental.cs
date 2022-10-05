using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Decorator.CarRentalExample2
{
    public interface IRental
    {
        float calcPrice();
        int getDaysRented();
        Model getModel();
        float getFuelConsumed();
        void setFuelConsumed(float amount);        
    }

    public class RefuelOnReturn : CarRentalDecorator
    {  
        private readonly float pricePerGallon;

        public RefuelOnReturn(IRental rental, float pricePerGallon)
            :base(rental)
        {            
            this.pricePerGallon = pricePerGallon;
        }
        public override float calcPrice()
        {
            return rental.calcPrice() + refuelPrice();
        }
     
        private float refuelPrice()
        {
            return (getModel().fuelCapacity - getFuelConsumed()) * pricePerGallon;
        }
    }

    public class Insurance : CarRentalDecorator
    {   
        private readonly float rate;

        public Insurance(IRental rental, float rate)
            :base(rental)
        {   
            this.rate = rate;
        }

        public override float calcPrice()
        {
            return rental.calcPrice() + insuranceAmount();
        }

        private float insuranceAmount()
        {
            return rate * rental.getDaysRented();
        }
    }

    public abstract class CarRentalDecorator : IRental
    {
        protected readonly IRental rental;

        public CarRentalDecorator(IRental rental)
        {
            this.rental = rental;
        }

        public virtual float calcPrice()
        {
            return rental.calcPrice();
        }

        public int getDaysRented()
        {
            return rental.getDaysRented();
        }

        public float getFuelConsumed()
        {
            return rental.getFuelConsumed();
        }

        public Model getModel()
        {
            return rental.getModel();
        }

        public void setFuelConsumed(float amount)
        {
            rental.setFuelConsumed(amount);
        }
    }

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
        public float calcPrice()
        {
            float price = (model.price * days);
            return price;
        }
        public int getDaysRented()
        {
            return days;
        }
        public Model getModel()
        {
            return model;
        }
        public float getFuelConsumed()
        {
            return fuelConsumed;
        }
        public void setFuelConsumed(float amount)
        {
            fuelConsumed = amount;
        }
    }

    public class Model
    {
        public float fuelCapacity;
        public float price;
        public String name;
        public Model(float fuelCapacity, float price, String name)
        {
            this.fuelCapacity = fuelCapacity;
            this.price = price;
            this.name = name;
        }
    }
}
