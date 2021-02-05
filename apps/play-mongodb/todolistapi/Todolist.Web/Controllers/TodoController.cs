using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Todolist.Web.Domain.Entities;

namespace Todolist.Web.Controllers
{
    [ApiController]
    [Route("todo")]
    public class TodoController : ControllerBase
    {
        public async Task<IEnumerable<Todo>> GetTodos()
        {
            return Enumerable.Repeat<Todo>(new Todo(), 7);
        }
    }
}