using Microsoft.Data.SqlClient;
using System.Collections.Concurrent;
using System.Runtime.Serialization;

namespace GeneralResources.ConcorrenciaSqlServer
{
    public class Example1
    {
        //Subir o SQL Server
        //docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=Sqlserver102js@@" -p 1433:1433 --name sqlserver -d mcr.microsoft.com/mssql/server:2022-latest
        
        //Criação do banco e da tabela de teste
        /*
        CREATE DATABASE TesteConcorrencia;
USE TesteConcorrencia;

        CREATE TABLE FluxoTeste(
            Id INT PRIMARY KEY,
            Status NVARCHAR(50),
    DataAtualizacao DATETIME
);

        INSERT INTO FluxoTeste(Id, Status, DataAtualizacao)
VALUES(1, 'Criado', GETDATE());
        */
          
        /// <summary>
        /// Cenário onde o problema acontece
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ConcurrenceScenarioReadCommited()
        {
            var output = new Queue<string>();

            string connectionString = "Data Source=127.0.0.1,1433;Initial Catalog=TesteConcorrencia;User Id=sa;Password=Sqlserver102js@@;TrustServerCertificate=True";

            // Fluxo 1
            var fluxo1Task = Task.Run(async () =>
            {
                output.Enqueue($"Fluxo 1: Iniciado {DateTime.Now.ToLongTimeString()}");

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    await Task.Delay(1000);
                    using (var transaction = connection.BeginTransaction())
                    {
                        var updateCommand = new SqlCommand(
                            "UPDATE FluxoTeste SET Status = 'Enviado', DataAtualizacao = GETDATE() WHERE Id = 1",
                            connection, transaction);
                        await updateCommand.ExecuteNonQueryAsync();
                        output.Enqueue($"Fluxo 1: Status alterado para 'Enviado'. {DateTime.Now.ToLongTimeString()}");

                        // Simula alguma demora para concluir
                        //await Task.Delay(1000);

                        transaction.Commit();
                        output.Enqueue($"Fluxo 1: Commit realizado. {DateTime.Now.ToLongTimeString()}");
                    }
                }
            });

            // Fluxo 2
            var fluxo2Task = Task.Run(async () =>
            {
                output.Enqueue($"Fluxo 2: Iniciado {DateTime.Now.ToLongTimeString()}");
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    // Simula alguma demora para concluir
                    
                    using (var transaction = connection.BeginTransaction())
                    {
                        var readCommand = new SqlCommand(
                            "SELECT Status FROM FluxoTeste WHERE Id = 1",
                            connection, transaction);
                        var status = (string?)await readCommand.ExecuteScalarAsync();
                        output.Enqueue($"Fluxo 2: Status lido = {status}  {DateTime.Now.ToLongTimeString()}");

                        // Simula processamento demorado
                        await Task.Delay(3000);

                        var updateCommand = new SqlCommand(
                            "UPDATE FluxoTeste SET Status = 'Criado', DataAtualizacao = GETDATE() WHERE Id = 1",
                            connection, transaction);
                        await updateCommand.ExecuteNonQueryAsync();
                        output.Enqueue($"Fluxo 2: Status alterado para 'Criado'. {DateTime.Now.ToLongTimeString()}");

                        transaction.Commit();
                        output.Enqueue($"Fluxo 2: Commit realizado.  {DateTime.Now.ToLongTimeString()}");
                    }
                }
            });

            // Executa ambos os fluxos simultaneamente
            await Task.WhenAll(fluxo2Task, fluxo1Task);

        }

        /// <summary>
        /// Forma de resolver utilizando Snapshot isolation level.
        /// Necessita implementar um retry, pois a transação será abortada caso seja detectado alteração na linha em comum
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ConcurrenceScenarioSnapshot()
        {
            var output = new Queue<string>();

            string connectionString = "Data Source=127.0.0.1,1433;Initial Catalog=TesteConcorrencia;User Id=sa;Password=Sqlserver102js@@;TrustServerCertificate=True";

            // Fluxo 1
            var fluxo1Task = Task.Run(async () =>
            {
                output.Enqueue($"Fluxo 1: Iniciado {DateTime.Now.ToLongTimeString()}");

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    await Task.Delay(1000);
                    using (var transaction = connection.BeginTransaction())
                    {
                        var updateCommand = new SqlCommand(
                            "UPDATE FluxoTeste SET Status = 'Enviado', DataAtualizacao = GETDATE() WHERE Id = 1",
                            connection, transaction);
                        await updateCommand.ExecuteNonQueryAsync();
                        output.Enqueue($"Fluxo 1: Status alterado para 'Enviado'. {DateTime.Now.ToLongTimeString()}");

                        // Simula alguma demora para concluir
                        //await Task.Delay(1000);

                        transaction.Commit();
                        output.Enqueue($"Fluxo 1: Commit realizado. {DateTime.Now.ToLongTimeString()}");
                    }
                }
            });

            // Fluxo 2
            var fluxo2Task = Task.Run(async () =>
            {
                output.Enqueue($"Fluxo 2: Iniciado {DateTime.Now.ToLongTimeString()}");

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    // Simula alguma demora para concluir

                    using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.Snapshot))
                    {
                        var readCommand = new SqlCommand(
                            "SELECT Status FROM FluxoTeste WHERE Id = 1",
                            connection, transaction);
                        var status = (string?)await readCommand.ExecuteScalarAsync();
                        output.Enqueue($"Fluxo 2: Status lido = {status}  {DateTime.Now.ToLongTimeString()}");

                        // Simula processamento demorado
                        await Task.Delay(3000);

                        var updateCommand = new SqlCommand(
                            "UPDATE FluxoTeste SET Status = 'Criado', DataAtualizacao = GETDATE() WHERE Id = 1",
                            connection, transaction);
                        await updateCommand.ExecuteNonQueryAsync();
                        output.Enqueue($"Fluxo 2: Status alterado para 'Criado'. {DateTime.Now.ToLongTimeString()}");

                        transaction.Commit();
                        output.Enqueue($"Fluxo 2: Commit realizado.  {DateTime.Now.ToLongTimeString()}");
                    }
                }
            });

            // Executa ambos os fluxos simultaneamente
            await Task.WhenAll(fluxo2Task, fluxo1Task);

        }

        /// <summary>
        /// Bloqueia as transações concorrentes para garantir a ordem das modificações
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task ConcurrenceScenarioSerialize()
        {
            var output = new Queue<string>();

            string connectionString = "Data Source=127.0.0.1,1433;Initial Catalog=TesteConcorrencia;User Id=sa;Password=Sqlserver102js@@;TrustServerCertificate=True";

            // Fluxo 1
            var fluxo1Task = Task.Run(async () =>
            {
                output.Enqueue($"Fluxo 1: Iniciado {DateTime.Now.ToLongTimeString()}");
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    await Task.Delay(1000);
                    using (var transaction = connection.BeginTransaction())
                    {
                        var updateCommand = new SqlCommand(
                            "UPDATE FluxoTeste SET Status = 'Enviado', DataAtualizacao = GETDATE() WHERE Id = 1",
                            connection, transaction);
                        await updateCommand.ExecuteNonQueryAsync();
                        output.Enqueue($"Fluxo 1: Status alterado para 'Enviado'. {DateTime.Now.ToLongTimeString()}");

                        // Simula alguma demora para concluir
                        //await Task.Delay(1000);

                        transaction.Commit();
                        output.Enqueue($"Fluxo 1: Commit realizado. {DateTime.Now.ToLongTimeString()}");
                    }
                }
            });

            // Fluxo 2
            var fluxo2Task = Task.Run(async () =>
            {
                output.Enqueue($"Fluxo 2: Iniciado {DateTime.Now.ToLongTimeString()}");
                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    // Simula alguma demora para concluir

                    using (var transaction = connection.BeginTransaction(System.Data.IsolationLevel.Serializable))
                    {
                        var readCommand = new SqlCommand(
                            "SELECT Status FROM FluxoTeste WHERE Id = 1",
                            connection, transaction);
                        var status = (string?)await readCommand.ExecuteScalarAsync();
                        output.Enqueue($"Fluxo 2: Status lido = {status}  {DateTime.Now.ToLongTimeString()}");

                        // Simula processamento demorado
                        await Task.Delay(3000);

                        var updateCommand = new SqlCommand(
                            "UPDATE FluxoTeste SET Status = 'Criado', DataAtualizacao = GETDATE() WHERE Id = 1",
                            connection, transaction);
                        await updateCommand.ExecuteNonQueryAsync();
                        output.Enqueue($"Fluxo 2: Status alterado para 'Criado'. {DateTime.Now.ToLongTimeString()}");

                        transaction.Commit();
                        output.Enqueue($"Fluxo 2: Commit realizado.  {DateTime.Now.ToLongTimeString()}");
                    }
                }
            });

            // Executa ambos os fluxos simultaneamente
            await Task.WhenAll(fluxo2Task, fluxo1Task);

        }
    }
}
