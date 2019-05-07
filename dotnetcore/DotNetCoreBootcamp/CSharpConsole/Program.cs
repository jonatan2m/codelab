using CSharp.Enums.LikeJava;
using System;
using System.Collections.Generic;

namespace CSharpConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Planet planetStatic = Planet.MERCURY;
            Console.WriteLine(planetStatic.SurfaceGravity());

            PlanetEnum planet = PlanetEnum.MERCURY;
            Console.WriteLine(planet.GetSurfaceGravity());            

            Console.WriteLine("Hello World!");
            Console.ReadKey();
        }
    }
}
