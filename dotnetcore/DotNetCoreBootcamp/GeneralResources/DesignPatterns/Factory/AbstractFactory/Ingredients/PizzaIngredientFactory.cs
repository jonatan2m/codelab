using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Factory.AbstractFactory.Ingredients
{
    public interface IPizzaIngredientFactory
    {
        Dough CreateDough();
        Sauce CreateSauce();
        Cheese CreateCheese();
        Veggies[] CreateVeggies();
        Pepperoni CreatePepperoni();
        Clams CreateClam();
    }

    internal class SlicedPepperoni : Pepperoni
    {
    }

    internal class Onion : Veggies
    {
    }

    internal class Garlic : Veggies
    {
    }

    internal class ReggianoCheese : Cheese
    {
    }

    internal class MarinaraSauce : Sauce
    {
    }

    internal class ThinCrustDough : Dough
    {
    }

    public class Clams
    {
    }

    public class Pepperoni
    {
    }

    public class Veggies
    {
    }

    public class Cheese
    {
    }

    public class Sauce
    {
    }

    public class Dough
    {
    }
}
