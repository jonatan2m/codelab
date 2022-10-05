using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Command.Example4
{
    public class TextEditor
    {
        private StringBuilder sb = new StringBuilder();

        public string Text
        {
            get
            {
                return sb.ToString();
            }
            set
            {
                sb.Clear();
                sb.Append($"{value} ");
            }
        }

        public void Insert(string text)
        {
            sb.Append(text);
        }

        public void Remove(string text)
        {
            var indexToRemove = sb.ToString().IndexOf(text);
            if (indexToRemove != -1)
                sb.Remove(indexToRemove, text.Length);
        }

    }
}
