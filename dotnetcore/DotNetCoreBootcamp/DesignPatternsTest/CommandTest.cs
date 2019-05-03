using DesignPatterns.Command.Example2;
using System;
using Xunit;

namespace DesignPatternsTest
{
    public class CommandTest
    {
        [Fact]
        public void Command_Example2()
        {
            Controller controller = new Controller();

            BaseCommand addNumbersCommand = new AddNumbersCommand();
            var comandReference = controller.AddCommand(addNumbersCommand);

            var expected = "1234";
            controller.GetCommandAt(comandReference).Execute(expected);

            Assert.Equal(expected, controller.GetBuiltString());
        }

        [Fact]
        public void Command_Example2_0()
        {
            var controller = new Controller();
            var addCommandReference = controller.AddCommand(new AddTextCommand());
            var text1 = "abc";
            controller.GetCommandAt(addCommandReference).Execute(text1);
            var text2 = "def";
            controller.GetCommandAt(addCommandReference).Execute(text2);
            controller.GetCommandAt(addCommandReference).Undo();
            Assert.Equal($"{text1}", controller.GetBuiltString());
        }

        [Fact]
        public void Command_Example2_1()
        {
            var controller = new Controller();
            var addCommandReference = controller.AddCommand(new AddTextCommand());
            var text1 = "abc";
            controller.GetCommandAt(addCommandReference).Execute(text1);
            var text2 = "def";
            controller.GetCommandAt(addCommandReference).Execute(text2);
            Assert.Equal($"{text1}{text2}", controller.GetBuiltString());
        }
    }
}
