using System;
using System.Collections.Generic;

namespace DesignPatterns.Composite.GiftExample
{
    /// <summary>
    /// Composite têm uma coleção de sub-elementos, que podem ser Leafs ou outros Composites.
    /// Se comunica com seus filhos através da interface do Component.
    /// </summary>
    public class CompositeGift : GiftBase, IGiftOperations
    {
        private List<GiftBase> _gifts;

        public CompositeGift(string name, int price = 0)
            : base(name, price)
        {
            _gifts = new List<GiftBase>();
        }

        public void Add(GiftBase gift)
        {
            _gifts.Add(gift);
        }

        public void Remove(GiftBase gift)
        {
            _gifts.Remove(gift);
        }

        public override int CalculateTotalPrice()
        {
            int total = 0;

            Console.WriteLine($"{name} contains the following products with prices:");

            foreach (var gift in _gifts)
            {
                total += gift.CalculateTotalPrice();
            }

            return total;
        }
    }
}