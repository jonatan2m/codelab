using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Singleton.Example1
{
    //https://csharpindepth.com/articles/Singleton

    /// <summary>
    /// not thread-safe
    /// </summary>
    public class Singleton
    {
        private static Singleton _instance;
        private int count;

        private Singleton() { }

        public static Singleton Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new Singleton();
                return _instance;
            }
        }

        internal void DoStuff()
        {
            count += 1;
        }
    }

    public class SampleUse
    {
        public void SomeMethod()
        {
            //call method
            Singleton.Instance.DoStuff();

            //assign to another variable
            var myObject = Singleton.Instance;
            myObject.DoStuff();

            //pass as parameter
            SomeOtherMethod(Singleton.Instance);
        }

        private void SomeOtherMethod(Singleton instance)
        {
            instance.DoStuff();
        }
    }
}
