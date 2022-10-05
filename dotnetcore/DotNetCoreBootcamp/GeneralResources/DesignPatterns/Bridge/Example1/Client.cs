using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Bridge.Example1
{
    public class Client
    {
        public void ClientCode(View view)
        {
            var result = view.Show();
            Console.WriteLine(result);
        }

    }
}
