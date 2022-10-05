using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CSharp.Enums.LikeJava
{
    public class PlanetAttribute : Attribute
    {
        public double Mass { get; private set; }
        public double Radius { get; private set; }

        public PlanetAttribute(double mass, double radius) =>
            (Mass, Radius) = (mass, radius);
    }

    public static class Planets
    {
        public static double GetSurfaceGravity(this PlanetEnum p)
        {
            PlanetAttribute attr = GetAttr(p);
            return G * attr.Mass / (attr.Radius * attr.Radius);
        }

        public static double GetSurfaceWeight(this PlanetEnum p, double otherMass)
        {
            return otherMass * p.GetSurfaceGravity();
        }

        public const double G = 6.67300E-11;

        private static PlanetAttribute GetAttr(PlanetEnum p)
        {
            return (PlanetAttribute)Attribute.GetCustomAttribute(
                ForValue(p), typeof(PlanetAttribute));
        }

        private static MemberInfo ForValue(PlanetEnum p)
        {
            return typeof(PlanetEnum).GetField(Enum.GetName(typeof(PlanetEnum), p));
        }
    }

    public enum PlanetEnum
    {
        [Planet(3.303e+23, 2.4397e6)] MERCURY,
        [Planet(4.869e+24, 6.0518e6)] VENUS,
        [Planet(5.976e+24, 6.37814e6)] EARTH,
        [Planet(6.421e+23, 3.3972e6)] MARS,
        [Planet(1.9e+27, 7.1492e7)] JUPITER,
        [Planet(5.688e+26, 6.0268e7)] SATURN,
        [Planet(8.686e+25, 2.5559e7)] URANUS,
        [Planet(1.024e+26, 2.4746e7)] NEPTUNE,
        [Planet(1.27e+22, 1.137e6)] PLUTO
    }
}
