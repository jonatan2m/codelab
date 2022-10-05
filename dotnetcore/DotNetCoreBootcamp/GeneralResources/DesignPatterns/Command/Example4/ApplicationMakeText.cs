using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Command.Example4
{
    public class ApplicationMakeText
    {
        public string CurrentText { get; set; }

        public string currentCode = "-1";

        public const string EXIT_CODE = "0";

        CommandHistory history;

        Func<ApplicationMakeText, TextEditor, BaseCommand> Insert =
            (ApplicationMakeText app, TextEditor editor) =>
            {
                return new InsertTextCommand(app, editor);
            };

        Func<ApplicationMakeText, TextEditor, BaseCommand> Remove =
            (ApplicationMakeText app, TextEditor editor) =>
            {
                return new RemoveTextCommand(app, editor);
            };

        Func<ApplicationMakeText, TextEditor, BaseCommand> UndoCommand =
            (ApplicationMakeText app, TextEditor editor) =>
            {
                return new UndoCommand(app, editor);
            };

        public void ExecuteCommand(BaseCommand command)
        {
            if(command.Execute())
                history.Push(command);
        }

        public void Undo()
        {            
            var command = history.Pop();
            command?.Undo();
        }

        public static void Run()
        {
            var app = new ApplicationMakeText();
            var editor = new TextEditor();
            app.history = new CommandHistory();
            

            Action createMenu = () => {
                Console.Clear();
                Console.WriteLine("Choose a command and type the word/phrase you should put into the system");
                Console.WriteLine("e.g. 1|Put this text into the system");
                Console.WriteLine("Options:");
                Console.WriteLine("1 - Insert text");
                Console.WriteLine("2 - Remove text");
                Console.WriteLine("3 - Undo");
                Console.WriteLine("0 - Exit");
            };

            Action getUserInput = () =>
            {
                var input = Console.ReadLine().Split("|");
                if(input.Length > 1)
                    app.CurrentText = input[1];

                app.currentCode = input[0];
            };

            Dictionary<string, Func<BaseCommand>> commands = new Dictionary<string, Func<BaseCommand>>();

            commands.Add("1", () => app.Insert(app, editor));
            commands.Add("2", () => app.Remove(app, editor));
            commands.Add("3", () => app.UndoCommand(app, editor));
                       
            createMenu();
            getUserInput();

            while (app.currentCode != EXIT_CODE)
            {
                var command = commands[app.currentCode]();
                app.ExecuteCommand(command);

                createMenu();
                
                Console.WriteLine("\n\nLast Result:");
                Console.WriteLine(editor.Text);

                getUserInput();
            }


        }
    }
}
