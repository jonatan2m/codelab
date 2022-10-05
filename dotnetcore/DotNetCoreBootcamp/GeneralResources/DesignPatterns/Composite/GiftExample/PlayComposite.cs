using System;

namespace DesignPatterns.Composite.GiftExample
{
    public static class PlayComposite
    {
        public static void Run()
        {
            Console.WriteLine("Example from: https://code-maze.com/composite/");

            CompositeGift rootGift = new CompositeGift("Gift Box");

            CompositeGift cardGift = new CompositeGift("Card Box");
            var magic = new SingleGift("Magic",45);
            var yogioh = new SingleGift("Yogioh",30);
            cardGift.Add(magic);
            cardGift.Add(yogioh);

            CompositeGift toyBox = new CompositeGift("Toy Box");
            var superman = new SingleGift("Super Man", 100);
            var spiderMan = new SingleGift("Sider Man", 140);
            toyBox.Add(superman);
            toyBox.Add(spiderMan);

            rootGift.Add(cardGift);
            rootGift.Add(toyBox);

            Console.WriteLine($"Total price of this composite present is: {rootGift.CalculateTotalPrice()}");
        }
    }
}