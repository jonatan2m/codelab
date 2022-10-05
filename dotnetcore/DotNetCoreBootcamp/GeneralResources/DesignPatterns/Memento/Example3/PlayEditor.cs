using System;
namespace DesignPatterns.Memento.Example3
{
    public class PlayEditor
    {
        public static void Run()
        {
            Editor editor = new Editor();
            Caretaker caretaker = new Caretaker(editor);

            caretaker.Backup();
            editor.DoSomething();

            caretaker.Backup();
            editor.DoSomething();

            caretaker.Backup();
            editor.DoSomething();

            caretaker.Backup();
            editor.DoSomething();

            caretaker.Undo();
            caretaker.Undo();
            caretaker.Undo();
            caretaker.Undo();
            caretaker.Undo();

        }
    }
}
