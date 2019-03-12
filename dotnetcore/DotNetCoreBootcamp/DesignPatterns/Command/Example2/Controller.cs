using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Command.Example2
{
    /// <summary>
    /// https://github.com/skimedic/presentations/blob/master/Patterns/BehavioralPatterns/Command
    /// </summary>
    public class Controller
    {
        private readonly StringBuilder _stringBuilder = new StringBuilder();
        private readonly List<IAppCommand> _commands = new List<IAppCommand>();

        public IAppCommand GetCommandAt(int x)
        {
            return _commands[x];
        }

        public int AddCommand(BaseCommand command)
        {
            command.Sb = _stringBuilder;
            _commands.Add(command);
            return _commands.IndexOf(command);
        }

        public void RemoveCommand(int position)
        {
            _commands.RemoveAt(position);
        }

        public string GetBuiltString()
        {
            return _stringBuilder.ToString();
        }

    }
}
