using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Singleton.Example1
{
    //https://csharpindepth.com/Articles/Singleton
    public sealed class SingletonLazy
    {
        private static readonly Lazy<SingletonLazy>
            lazy =
            new Lazy<SingletonLazy>
                (() => new SingletonLazy());

        public static SingletonLazy Instance => lazy.Value;

        private SingletonLazy()
        {
        }
    }

}
