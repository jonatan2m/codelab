// See https://aka.ms/new-console-template for more information

//definir as configurações
public class Cromossomo
    {
        public int[] Genes { get; set; }
        public double Fitness { get; set; }

        public Cromossomo(int tamanho)
        {
            Genes = new int[tamanho];
            Random random = new Random();
            for (int i = 0; i < tamanho; i++)
            {
                Genes[i] = random.Next(5);
            }
        }

        public void CalcularFitness()
        {
            Fitness = 0;
            foreach (int gene in Genes)
            {
                Fitness += gene;
            }
        }

        public Cromossomo Crossover(Cromossomo outro)
        {
            Cromossomo filho = new Cromossomo(Genes.Length);
            Random random = new Random();

            for (int i = 0; i < Genes.Length; i++)
            {
                if (random.NextDouble() < 0.5)
                {
                    filho.Genes[i] = Genes[i];
                }
                else
                {
                    filho.Genes[i] = outro.Genes[i];
                }
            }

            return filho;
        }

        public void Mutacao(double taxaMutacao)
        {
            Random random = new Random();
            for (int i = 0; i < Genes.Length; i++)
            {
                if (random.NextDouble() < taxaMutacao)
                {
                    Genes[i] = 1 - Genes[i];
                }
            }
        }

        public override string ToString()
        {
            string texto = "";
            foreach (int gene in Genes)
            {
                texto += gene.ToString();
            }
            return texto;
        }
    }