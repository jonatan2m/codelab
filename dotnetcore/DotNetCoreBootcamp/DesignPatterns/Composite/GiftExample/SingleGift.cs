using System;

namespace DesignPatterns.Composite.GiftExample
{
    /// <summary>
    /// Leaf - Para o Leaf basta fazer essa implementação, já que não terá filhos
    /// </summary>
    public class SingleGift : GiftBase
    {
        public SingleGift(string name, int price)
            : base(name, price)
        {
        }

        public override int CalculateTotalPrice()
        {
            Console.WriteLine($"{name} with the price {price}");

            return price;
        }
    }
}