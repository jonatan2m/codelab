using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Web3_1.AsyncAwait;

namespace Web3_1.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AsyncAwaitController : ControllerBase
    {
        [HttpGet, Route("ex01")]
        public async Task<IActionResult> Example01()
        {
            Console.WriteLine("Example01 starting");
            var ex01 = new AsyncAwait.Example01();

            var result = await ex01.Run();

            Console.WriteLine($"Example01 {result}");

            Console.WriteLine("Example01 finishing");

            return Ok("Example01 done");
        }

        [HttpGet, Route("ex02")]
        public async Task<IActionResult> Example02()
        {
            var example2 = new Example02();

            await example2.Run().ConfigureAwait(false);

            return Ok("Example02 done");
        }
    }
}
