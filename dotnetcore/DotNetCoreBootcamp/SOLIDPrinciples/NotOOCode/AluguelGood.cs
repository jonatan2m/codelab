using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace SOLIDPrinciples.NotOOCode.Good
{
    public class Dvd {
        public decimal Preco { get; set; }
    }

    public class Cliente { /*...*/ }


    public class Aluguel
    {
        private readonly List<Dvd> filmes;
        private readonly Cliente cliente;

        public decimal PrecoTotal
        {
            get
            {
                return filmes.Sum(x => x.Preco);
            }
        }

        public Aluguel(Cliente cliente)
        {
            this.cliente = cliente;
            filmes = new List<Dvd>();
        }

        public void Adicionar(Dvd dvd)
        {
            filmes.Add(dvd);
        }

        public IReadOnlyCollection<Dvd> GetFilmes =>
            filmes.AsReadOnly();
    }


    public class GerenciadorLocadora
    {
        public Aluguel AlugarDvd(Cliente cliente, IEnumerable<Dvd> filmes)
        {
            Aluguel novoAluguel = new Aluguel(cliente);
            foreach (var dvd in filmes)
            {
                novoAluguel.Adicionar(dvd);
            }
            
            return novoAluguel;
        }
    }

}
