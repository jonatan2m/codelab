﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Command.Example2
{
    public abstract class BaseCommand : IAppCommand
    {
        public StringBuilder Sb { get; set; }
        protected readonly List<string> Entries = new List<string>();

        public abstract void Execute(string text);

        public abstract void Undo();
    }
}
