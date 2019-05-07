using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDPrinciples.ValidationClass.Example01
{
    /// <summary>
    /// Data transfer class that needs to be validated:
    /// </summary>
    public class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public bool ConsumesAlcohol { get; set; }
    }
}
