using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeneralResources.Paralelismo
{
    public class TestParalelismo
    {

        [Fact]
        public static async Task Main()
        {
            // Criar uma instância de Stream de leitura do servidor
            Stream serverDataStream = await LerDadosDoServidorAsync();

            // Iniciar tarefa de processamento dos dados
            Task processDataTask = Task.Run(() =>
            {
                Debug.WriteLine("Iniciando tarefa de processamento dos dados...");
                // Simulação de processamento dos dados enquanto são lidos
                StreamReader reader = new StreamReader(serverDataStream);
                string linha;
                while ((linha = reader.ReadLine()) != null)
                {
                    ProcessarDado(linha);
                }
                Debug.WriteLine("Dados processados com sucesso!");
            });

            // Aguardar a conclusão da tarefa de processamento
            await processDataTask;

            Debug.WriteLine("Programa finalizado.");
        }

        static Task<MemoryStream> LerDadosDoServidorAsync()
        {
            // Simulação de leitura assíncrona dos dados do servidor
            return Task.Run(async () =>
            {
                Debug.WriteLine("Iniciando leitura dos dados do servidor...");
                MemoryStream stream = new MemoryStream();
                StreamWriter writer = new StreamWriter(stream);
                await writer.WriteLineAsync("Dado 1");
                await writer.WriteLineAsync("Dado 2");
                await writer.WriteLineAsync("Dado 3");
                writer.Flush();
                stream.Position = 0;
                Debug.WriteLine("Dados do servidor lidos com sucesso!");
                return stream;
            });
        }

        static void ProcessarDado(string dado)
        {
            // Simulação de processamento do dado
            Debug.WriteLine($"Processando dado: {dado}");
            Thread.Sleep(1000);
        }

        [Fact]
        public void Main2()
        {
            BlockingCollection<string> dataQueue = new BlockingCollection<string>();

            // Simulação de leitura dos dados do servidor
            Task producer = Task.Run(() =>
            {
                string[] data = { "Dado 1", "Dado 2", "Dado 3" };
                foreach (var item in data)
                {
                    Debug.WriteLine($"Servidor: {item}");
                    dataQueue.Add(item);
                    Thread.Sleep(1000); // Simulação do atraso do servidor
                }

                dataQueue.CompleteAdding(); // Nenhum dado adicional será adicionado à coleção
            });

            // Simulação de processamento dos dados
            Task consumer = Task.Run(() =>
            {
                while (!dataQueue.IsCompleted)
                {
                    string item;
                    if (dataQueue.TryTake(out item))
                    {
                        Debug.WriteLine($"Processando: {item}");
                        Thread.Sleep(2000); // Simulação do tempo de processamento
                    }
                }
            });

            Task.WaitAll(producer, consumer);
        }
    }
}
