using System;
using System.Collections.Generic;
using static DesignPatterns.Memento.Example3.Editor;

namespace DesignPatterns.Memento.Example3
{
    public class Caretaker
    {
        Stack<Snapshot> snapshots = new Stack<Snapshot>();

        Editor editor;
        public Caretaker(Editor editor)
        {
            this.editor = editor;
        }

        public void Backup()
        {
            Console.WriteLine("Caretaker: Saving Editor...");
            snapshots.Push(this.editor.Save());
        }

        public void Undo()
        {
            if (snapshots.Count == 0) return;

            var snapshot = snapshots.Pop();
            editor.Restore(snapshot);

            Console.WriteLine("Caretaker: Restoring Editor...");
        }

       
    }
}
