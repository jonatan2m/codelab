using System.Text;

namespace DesignPatterns.Composite.GiftExample
{
    /// <summary>
    /// Component: Descreve os comportamentos comuns para os elementos simples e complexos da arvore
    /// </summary>
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

    
}
