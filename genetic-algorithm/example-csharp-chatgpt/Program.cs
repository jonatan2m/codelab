// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

//definir as configurações
int tamanhoPopulacao = 10;
int tamanhoCromossomo = 10;
int numeroGeracoes = 50;
double taxaCrossover = 0.7;
double taxaMutacao = 0.001;

List<Cromossomo> populacao = new List<Cromossomo>();

//gerando populacao aleatoria
for (int i = 0; i < tamanhoPopulacao; i++)
{
    Cromossomo cromossomo = new Cromossomo(tamanhoCromossomo);
    populacao.Add(cromossomo);
}

//evoluindo a populacao
for (int i = 0; i < numeroGeracoes; i++)
{
    //calculando fitness dos individuos
    foreach (Cromossomo cromossomo in populacao)
    {
        cromossomo.CalcularFitness();
    }

    //ordenando populacao pelo fitness
    populacao = populacao.OrderByDescending(c => c.Fitness).ToList();

    //selecionando os melhores individuos para reproducao
    List<Cromossomo> individuosReproducao = new List<Cromossomo>();

    for (int j = 0; j < (int)(taxaCrossover * tamanhoPopulacao); j++)
    {
        individuosReproducao.Add(populacao[j]);
    }

    //realizando crossover
    List<Cromossomo> filhos = new List<Cromossomo>();
    Random random = new Random();

    for (int j = 0; j < tamanhoPopulacao - individuosReproducao.Count; j++)
    {
        int indicePai1 = random.Next(individuosReproducao.Count);
        int indicePai2 = random.Next(individuosReproducao.Count);
        Cromossomo pai1 = individuosReproducao[indicePai1];
        Cromossomo pai2 = individuosReproducao[indicePai2];
        Cromossomo filho = pai1.Crossover(pai2);
        filhos.Add(filho);
    }

    //realizando mutacao
    foreach (Cromossomo cromossomo in filhos)
    {
        cromossomo.Mutacao(taxaMutacao);
    }

    //juntando populacao antiga e nova
    populacao = populacao.Take((int)(0.5 * tamanhoPopulacao)).ToList();

    populacao.AddRange(filhos);

    Console.WriteLine($"Geração {i + 1}: melhor fitness = {populacao[0].Fitness}");
}

Console.WriteLine($"Melhor solução encontrada: {populacao[0].ToString()}");