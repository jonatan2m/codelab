using System;
using System.Collections.Generic;
using System.Text;

namespace BadSmellsTest.ExtractMethod
{
    class Pedido
    {
        public int ObterValor()
        {
            return 1;
        }
    }

    class Antes
    {
        public readonly string Nome;
        private readonly List<Pedido> _pedidos;

        public Antes(string nome, List<Pedido> pedidos)
        {
            Nome = nome;
            _pedidos = pedidos;
        }


    }

    class Depois
    {

    }
}
