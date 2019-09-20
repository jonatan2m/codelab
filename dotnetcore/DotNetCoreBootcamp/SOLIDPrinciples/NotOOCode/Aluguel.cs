using System;
using System.Collections.Generic;
using System.Text;

namespace SOLIDPrinciples.NotOOCode
{
    public class Dvd {
        public decimal Preco { get; set; }
    }

    public class Cliente { /*...*/ }


    public class Aluguel
    {
        public IList<Dvd> Filmes = new List<Dvd>();
        public Cliente Cliente { get; set; }

        public decimal PrecoTotal { get; set; }
    }


    public class GerenciadorLocadora
    {
        public Aluguel AlugarDvd(Cliente cliente, IEnumerable<Dvd> filmes)
        {
            Aluguel novoAluguel = new Aluguel();
            novoAluguel.Cliente = cliente;
            decimal precoTotal = 0;
            foreach (var dvd in filmes)
            {
                novoAluguel.Filmes.Add(dvd);
                precoTotal += dvd.Preco;
            }
            novoAluguel.PrecoTotal = precoTotal;
            return novoAluguel;
        }
    }

}
