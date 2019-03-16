using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Command.Example4
{
    public class UndoCommand : BaseCommand
    {
        public UndoCommand(ApplicationMakeText app, TextEditor editor) : base(app, editor)
        {
        }

        public override bool Execute()
        {           
            app.Undo();
            return false;
        }
    }
}
