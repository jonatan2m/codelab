using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Builder.GiftExample
{
    public abstract class GiftBase
    {
        protected string name;
        protected int price;

        public GiftBase(string name, int price)
        {
            this.name = name;
            this.price = price;
        }

        public abstract int CalculateTotalPrice();
    }

    public class GiftBuilder
    {
        private readonly GiftBase _gift;

        public GiftBuilder(GiftBase gift)
        {
            _gift = gift;
        }
    }
}
