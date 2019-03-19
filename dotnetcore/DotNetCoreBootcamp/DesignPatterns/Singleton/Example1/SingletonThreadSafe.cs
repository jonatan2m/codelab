using System;
namespace DesignPatterns.Singleton.Example1
{
    /// <summary>
    /// thread-safe
    /// </summary>
    public class SingletonThreadSafe
    {
        private SingletonThreadSafe()
        {
        }

        public static SingletonThreadSafe getInstance()
        {
            return SingletonHolder.INSTANCE;
        }

        private static class SingletonHolder
        {
            internal static readonly SingletonThreadSafe INSTANCE = new SingletonThreadSafe();
        }
    }

}
