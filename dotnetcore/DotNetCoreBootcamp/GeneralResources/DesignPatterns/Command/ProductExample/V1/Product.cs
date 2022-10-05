using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Command.ProductExample.V1
{
    /// <summary>
    /// Receiver
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Only to start the scenario
        /// Act as Client class
        /// </summary>
        public static void Play()
        {
            //Invoker
            var modifyPrice = new ModifyPrice();
            //Receiver
            var product = new Product("Phone", 500);

            Execute(product, modifyPrice, new ProductIncreasePriceCommand(product, 100));
            Execute(product, modifyPrice, new ProductIncreasePriceCommand(product, 50));
            Execute(product, modifyPrice, new ProductDecreasePriceCommand(product, 25));

            modifyPrice.Undo();
            modifyPrice.Undo();
            modifyPrice.Undo();

            Console.WriteLine(product);
        }

        private static void Execute(Product product, ModifyPrice modifyPrice, ICommand productCommand)
        {
            modifyPrice.SetCommand(productCommand);
            modifyPrice.Invoke();
        }

        public string Name { get; set; }
        public int Price { get; set; }

        public Product(string name, int price)
        {
            Name = name;
            Price = price;
        }

        public void IncreasePrice(int amount)
        {
            Price += amount;
            Console.WriteLine($"The price for the {Name} has been increased by {amount}$.");
        }

        public void DecreasePrice(int amount)
        {
            if (amount < Price)
            {
                Price -= amount;
                Console.WriteLine($"The price for the {Name} has been decreased by {amount}$.");
            }
        }

        public override string ToString() => $"Current price for the {Name} product is {Price}$.";
    }

    /// <summary>
    /// Abstract Command
    /// </summary>
    public interface ICommand
    {
        void ExecuteAction();
        void UndoAction();
    }

    public enum PriceAction
    {
        Increase,
        Decrease
    }

    /// <summary>
    /// Concrete Command
    /// </summary>
    public class ProductIncreasePriceCommand : ICommand
    {
        private readonly Product _product;
        private readonly int _amount;

        public ProductIncreasePriceCommand(Product product, int amount)
        {
            _product = product;
            _amount = amount;
        }

        public void ExecuteAction()
        {
            _product.IncreasePrice(_amount);
        }

        public void UndoAction()
        {
            _product.DecreasePrice(_amount);
        }
    }

    /// <summary>
    /// Concrete Command
    /// </summary>
    public class ProductDecreasePriceCommand : ICommand
    {
        private readonly Product _product;
        private readonly int _amount;

        public ProductDecreasePriceCommand(Product product, int amount)
        {
            _product = product;
            _amount = amount;
        }

        public void ExecuteAction()
        {
            _product.DecreasePrice(_amount);
        }

        public void UndoAction()
        {
            _product.IncreasePrice(_amount);
        }
    }

    /// <summary>
    /// Invoker
    /// </summary>
    public class ModifyPrice
    {
        private readonly Stack<ICommand> _commands;
        private ICommand _command;

        public ModifyPrice()
        {
            _commands = new Stack<ICommand>();
        }

        public void SetCommand(ICommand command) => _command = command;

        public void Invoke()
        {
            _commands.Push(_command);
            _command.ExecuteAction();
            
        }

        public void Undo()
        {
            _command = _commands.Pop();
            _command.UndoAction();
        }
    }
}
