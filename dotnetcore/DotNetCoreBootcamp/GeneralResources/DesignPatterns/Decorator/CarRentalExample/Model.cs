using System;

namespace DesignPatterns.Decorator.CarRentalExample
{
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
