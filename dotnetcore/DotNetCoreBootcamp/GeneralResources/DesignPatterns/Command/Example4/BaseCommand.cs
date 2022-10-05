using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Command.Example4
{
    public interface ICommand
    {
        bool Execute();
        void Undo();
    }

    public abstract class BaseCommand : ICommand
    {
        protected TextEditor editor;
        protected ApplicationMakeText app;
        protected string backup;

        public BaseCommand(ApplicationMakeText app, TextEditor editor)
        {
            this.app = app;
            this.editor = editor;
        }

        public void SaveBackup()
        {
            backup = editor.Text;
        }

        public void Undo()
        {
            editor.Text = backup;
        }

        public abstract bool Execute();
    }
}
