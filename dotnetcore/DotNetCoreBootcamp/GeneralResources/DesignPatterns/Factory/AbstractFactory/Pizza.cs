using DesignPatterns.Factory.AbstractFactory.Ingredients;

namespace DesignPatterns.Factory.AbstractFactory
{
    public abstract class Pizza
    {
        public string Name { get; set; }
        protected Dough dough;
        protected Sauce sauce;
        protected Veggies[] veggies;
        protected Cheese cheese;
        protected Pepperoni pepperoni;
        protected Clams clams;

        public abstract void Prepare();

        //bake
        //cut
        //box
    }

    public class CheesePizza : Pizza
    {
        private readonly IPizzaIngredientFactory pizzaIngredientFactory;

        public CheesePizza(IPizzaIngredientFactory pizzaIngredientFactory)
        {
            this.pizzaIngredientFactory = pizzaIngredientFactory;
        }

        public override void Prepare()
        {
            System.Console.WriteLine($"Preparing {Name}");
            dough = pizzaIngredientFactory.CreateDough();
            sauce = pizzaIngredientFactory.CreateSauce();
            //create others ingredients...
        }
    }

    public class ClamPizza : Pizza
    {
        private readonly IPizzaIngredientFactory pizzaIngredientFactory;

        public ClamPizza(IPizzaIngredientFactory pizzaIngredientFactory)
        {
            this.pizzaIngredientFactory = pizzaIngredientFactory;
        }

        public override void Prepare()
        {
            System.Console.WriteLine($"Preparing {Name}");
            dough = pizzaIngredientFactory.CreateDough();
            sauce = pizzaIngredientFactory.CreateSauce();
            //create others ingredients...
        }
    }

    public abstract class PizzaStore
    {
        public abstract Pizza OrderPizza(string item);
        //some code
    }

    public class NYPizzaStore : PizzaStore
    {
        public override Pizza OrderPizza(string item)
        {
            Pizza pizza = CreatePizza(item);
            return pizza;
        }

        protected Pizza CreatePizza(string item)
        {
            Pizza pizza = null;
            IPizzaIngredientFactory ingredientFactory =
                new NYPizzaIngredientFactory();

            if(item.Equals("cheese"))
            {
                pizza = new CheesePizza(ingredientFactory);
                pizza.Name = "New york style cheese pizza";
            }
            //other types of pizza, cheese, clam e etc...

            return pizza;
        }
    }

}
