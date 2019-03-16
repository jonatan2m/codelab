using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Command.Example4
{
    public class InsertTextCommand : BaseCommand
    {
        public InsertTextCommand(ApplicationMakeText app, TextEditor editor) : base(app, editor)
        {
        }

        public override bool Execute()
        {
            SaveBackup();
            editor.Insert(app.CurrentText);
            return true;
        }
    }
}
