using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Command.Example4
{
    public class CommandHistory
    {
        Stack<BaseCommand> commands = new Stack<BaseCommand>();

        public void Push(BaseCommand command)
        {
            commands.Push(command);
        }

        public BaseCommand Pop()
        {
            if(commands.Count > 0)
                return commands.Pop();

            return default(BaseCommand);
            
        }
    }
}
