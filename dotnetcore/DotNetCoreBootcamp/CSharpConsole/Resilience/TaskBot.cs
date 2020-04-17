using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CSharpConsole.Resilience
{
    public class TaskBot
    {
        public async Task<bool> ExecuteSomeRandomResultTaskAsync()
        {
            var randomResult = new Random().Next(1, 20);

            if (randomResult % 2 != 0)
            {
                throw new Exception("error");
            }

            return await Task.FromResult(true);
        }

        public bool ExecuteSomeRandomResultTask()
        {
            var randomResult = new Random().Next(1, 20);

            if (randomResult % 2 != 0)
            {
                throw new Exception("error");
            }

            return true;
        }

        public async Task<bool> ExecuteFailureTaskWithExceptionAsync()
        {
            //var randomResult = new Random().Next(1, 20);

            //if (randomResult % 2 != 0)
            //{
                throw new Exception("error");
            //}

            //return await Task.FromResult(false);
        }

        public bool ExecuteFailureTask()
        {
            var randomResult = new Random().Next(1, 20);

            if (randomResult % 2 != 0)
            {
                throw new Exception("error");
            }

            return false;
        }
    }
}
