﻿using System;

namespace SOLIDPrinciples.CouplingProblem.Example01.GoodEvents
{
    public class Fatura
    {
        public double GetValorMensal()
        {
            return new Random().NextDouble() * 100;
        }
    }
}
