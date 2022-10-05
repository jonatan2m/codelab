using System;
namespace DesignPatterns.Memento.Example3
{
    //Originator
    public class Editor
    {
        string colorBackground = "FFF";
        string tabName = String.Empty;
        string text = String.Empty;
        int count = 0;

        public Snapshot Save()
        {
            return new Snapshot(this);
        }

        public void Restore(Snapshot snapshot)
        {
            Editor editor = snapshot.GetEditor();
            colorBackground = editor.colorBackground;
            tabName = editor.tabName;
            text = editor.text;
            count = editor.count;
            Console.WriteLine($"Restored! editor({count})");
        }

        public void DoSomething()
        {
            this.count++;
        }


        //Memento
        public class Snapshot
        {
            Editor editor;
            string colorBackground;
            string tabName;
            string text;
            int count;

            public DateTime Date { get; private set; }

            public string PreviewEditor
            {
                get
                {
                    var dateStr = Date.ToString("yyyy MMMMM dd");
                    return $"{dateStr} - {tabName} - {text.Substring(0, 9)}";
                }
            }

            public Snapshot(Editor editor)
            {
                this.editor = editor;
                colorBackground = editor.colorBackground;
                tabName = editor.tabName;
                text = editor.text;
                count = editor.count;
                this.Date = DateTime.Now;
            }

            internal Editor GetEditor()
            {
                editor.text = text;
                editor.colorBackground = colorBackground;
                editor.tabName = tabName;
                editor.count = count;
                return editor;
            }
        }
    }
}
