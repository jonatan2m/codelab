﻿using DesignPatterns.Factory.Example1;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DesignPatternsTest
{
    public class FactoryTest
    {
        [Fact]
        public void Factory_Example1()
        {
            IBuilding house = DesignPatterns.Factory.Example1.BuildingFactory.GetInstanceOf("house");
            Assert.IsType<House>(house);
        }
    }
}