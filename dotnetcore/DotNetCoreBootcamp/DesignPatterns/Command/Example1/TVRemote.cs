using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Command.Example1
{
    public class TVRemote
    {
        public static IElectronicDevice GetDevice()
        {
            return new Television();
        }
    }
}
