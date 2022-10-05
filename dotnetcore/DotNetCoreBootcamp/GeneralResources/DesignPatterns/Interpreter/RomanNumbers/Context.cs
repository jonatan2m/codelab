using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Interpreter.RomanNumbers
{
    /// <summary>
    /// http://devman.pl/programtech/design-patterns-interpreter/
    /// </summary>
    public class Context
    {
        private string _input;
        private int _output;

        public Context(string input)
        {
            _input = input;
        }

        public string Input
        {
            get { return _input; }
            set { _input = value; }
        }

        public int Output
        {
            get { return _output; }
            set { _output = value; }
        }

        public static int Run(string roman)
        {
            //while (!string.IsNullOrEmpty(roman = Console.ReadLine()))
            {
                Context context = new Context(roman);

                List<Expression> tree = new List<Expression>();
                tree.Add(new ThousandExpression());
                tree.Add(new HundredExpression());
                tree.Add(new TenExpression());
                tree.Add(new OneExpression());

                foreach (Expression exp in tree)
                {
                    exp.Interpret(context);
                }

                return context.Output;
            }
        }
    }
}
