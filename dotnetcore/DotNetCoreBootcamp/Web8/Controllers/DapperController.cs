using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web3_1.Entities.DapperExample;

namespace Web3_1.Controllers
{
    /*
     * How can I organize the simple queries and the mapping between Entities and DTOs
     */

    [Route("[controller]")]
    [ApiController]
    public class DapperController : ControllerBase
    {
        private const string ConnectionString = "Server=.;Database=AdventureWorks2017;Trusted_Connection=true;";

        [HttpGet]
        public async Task<IActionResult> First([FromQuery] string firstName)
        {
            var sql = @"
SELECT [Title]
      ,[FirstName]
      ,[MiddleName]
      ,[LastName]      
  FROM [Person].[Person]
WHERE FirstName = @firstName";
            using (var connection = new SqlConnection(ConnectionString))
            {
                var persons = await connection.QueryAsync<Person>(sql, new { firstName });
                return Ok(persons);
            }
        }

        [HttpGet("only/{only}")]
        public async Task<IActionResult> OnlyFirst([FromRoute] bool only, [FromQuery] string firstName)
        {
            var sql = @"
SELECT [Title]
      ,[FirstName]
      ,[MiddleName]
      ,[LastName]      
  FROM [Person].[Person]
";

            var dynamicParameters = new DynamicParameters();

            if (only)
            {
                sql += " WHERE FirstName = @firstName";
                dynamicParameters.Add("firstName", firstName);
            }

            using (var connection = new SqlConnection(ConnectionString))
            {
                var persons = await connection.QueryAsync<Person>(sql, dynamicParameters);
                return Ok(persons);
            }
        }

        [HttpGet("random")]
        public async Task<IActionResult> Random()
        {
            var sql = @"
INSERT INTO [dbo].[MyTableTest]
           ([Name])
     VALUES
           (@name)
";

            using (var connection = new SqlConnection(ConnectionString))
            {
                //transaction need a connection Opened 
                await connection.OpenAsync();

                using (var transaction = await connection.BeginTransactionAsync())
                {
                    for (int i = 0; i < 10000; i++)
                    {
                        //forcing an error
                        //if (i == 5000) throw new Exception("Bad!");

                        await connection.ExecuteAsync(sql, new { name = $"testing {DateTime.Now.Ticks}" }, transaction);
                    }

                    await transaction.CommitAsync();
                }
            }

            return Ok();
        }


        private string TestTable = @"
CREATE TABLE MyTableTest (
    Id int IDENTITY PRIMARY KEY,
    Name nvarchar(50)        
);";
    }
}
