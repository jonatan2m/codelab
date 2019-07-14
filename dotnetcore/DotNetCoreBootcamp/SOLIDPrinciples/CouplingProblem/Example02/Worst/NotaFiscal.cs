﻿using System;

namespace SOLIDPrinciples.CouplingProblem.Example02.Worst
{
    public class NotaFiscal
    {
        private double valor;
        private double imposto;

        public NotaFiscal(double valor, double imposto)
        {
            this.valor = valor;
            this.imposto = imposto;
        }

        public void SetImposto(double imposto)
        {
            this.imposto = imposto;
        }
    }
}