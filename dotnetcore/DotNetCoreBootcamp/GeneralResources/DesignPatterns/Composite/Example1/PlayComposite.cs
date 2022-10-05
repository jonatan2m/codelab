using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Composite.Example1
{
    public class PlayComposite
    {
        public static void Run()
        {
            Client client = new Client();

            // This way the client code can support the simple leaf
            // components...
            Leaf leaf = new Leaf();
            Console.WriteLine("Client: I get a simple component:");
            client.ClientCode(leaf);

            // ...as well as the complex composites.
            Composite tree = new Composite();

            Composite branch1 = new Composite();
            branch1.Add(new Leaf());
            branch1.Add(new Leaf());
            
            Composite branch2 = new Composite();
            branch2.Add(new Leaf());

            tree.Add(branch1);
            tree.Add(branch2);
            Console.WriteLine("Client: Now I've got a composite tree:");

            client.ClientCode(tree);

            Console.Write("Client: I don't need to check the components classes even when managing the tree:\n");
            client.ClientCode(tree, leaf);

        }
    }
}
