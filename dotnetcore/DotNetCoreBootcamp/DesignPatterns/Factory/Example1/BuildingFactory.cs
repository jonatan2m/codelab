using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Factory.Example1
{
    public class BuildingFactory
    {
        static Dictionary<string, IBuilding> buildings;

        static BuildingFactory()
        {
            buildings = new Dictionary<string, IBuilding>();

            buildings.Add("house", new House());
            buildings.Add("edifice", new Edifice());
        }

        public static IBuilding GetInstanceOf(string type)
        {
            return buildings[type];
        }
    }
}
