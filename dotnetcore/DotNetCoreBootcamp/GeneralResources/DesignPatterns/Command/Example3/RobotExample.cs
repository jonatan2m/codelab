using System;
using System.Collections.Generic;
using System.Text;

namespace DesignPatterns.Command.Example3
{
    public abstract class RobotCommandBase
    {
        protected Robot _robot;

        public RobotCommandBase(Robot robot)
        {
            _robot = robot;
        }

        public abstract void Execute();

        public abstract void Undo();
    }

    public class MoveCommand : RobotCommandBase
    {
        public int ForwardDistance { get; set; }

        public MoveCommand(Robot robot) : base(robot) { }

        public override void Execute()
        {
            _robot.Move(ForwardDistance);
        }

        public override void Undo()
        {
            _robot.Move(-ForwardDistance);
        }
    }

    public class RotateLeftCommand : RobotCommandBase
    {
        public double LeftRotationAngle { get; set; }

        public RotateLeftCommand(Robot robot) : base(robot) { }

        public override void Execute()
        {
            _robot.RotateLeft(LeftRotationAngle);
        }

        public override void Undo()
        {
            _robot.RotateRight(LeftRotationAngle);
        }
    }

    public class RotateRightCommand : RobotCommandBase
    {
        public double RightRotationAngle { get; set; }

        public RotateRightCommand(Robot robot) : base(robot) { }

        public override void Execute()
        {
            _robot.RotateRight(RightRotationAngle);
        }

        public override void Undo()
        {
            _robot.RotateLeft(RightRotationAngle);
        }
    }

    public class TakeSampleCommand : RobotCommandBase
    {
        public bool TakeSample { get; set; }

        public TakeSampleCommand(Robot robot) : base(robot) { }

        public override void Execute()
        {
            _robot.TakeSample(true);
        }

        public override void Undo()
        {
            _robot.TakeSample(false);
        }
    }

    public class RobotController
    {
        public Queue<RobotCommandBase> Commands;
        private Stack<RobotCommandBase> _undoStack;

        public RobotController()
        {
            Commands = new Queue<RobotCommandBase>();
            _undoStack = new Stack<RobotCommandBase>();
        }

        public void ExecuteCommands()
        {
            Console.WriteLine("EXECUTING COMMANDS.");

            while (Commands.Count > 0)
            {
                RobotCommandBase command = Commands.Dequeue();
                command.Execute();
                _undoStack.Push(command);
            }
        }

        public void UndoCommands(int numUndos)
        {
            Console.WriteLine("REVERSING {0} COMMAND(S).", numUndos);

            while (numUndos > 0 && _undoStack.Count > 0)
            {
                RobotCommandBase command = _undoStack.Pop();
                command.Undo();
                numUndos--;
            }
        }
    }

    public class Robot
    {
        public void Move(int distance)
        {
            if (distance > 0)
                Console.WriteLine("Robot moved forwards {0}mm.", distance);
            else
                Console.WriteLine("Robot moved backwards {0}mm.", -distance);
        }

        public void RotateLeft(double angle)
        {
            if (angle > 0)
                Console.WriteLine("Robot rotated left {0} degrees.", angle);
            else
                Console.WriteLine("Robot rotated right {0} degrees.", -angle);
        }

        public void RotateRight(double angle)
        {
            if (angle > 0)
                Console.WriteLine("Robot rotated right {0} degrees.", angle);
            else
                Console.WriteLine("Robot rotated left {0} degrees.", -angle);
        }

        public void TakeSample(bool take)
        {
            if (take)
                Console.WriteLine("Robot took sample");
            else
                Console.WriteLine("Robot released sample");
        }
    }

    public class PlayRobot
    {
        public static void Run()
        {
            var robot = new Robot();
            var controller = new RobotController();

            var move = new MoveCommand(robot);
            move.ForwardDistance = 1000;
            controller.Commands.Enqueue(move);

            var rotate = new RotateLeftCommand(robot);
            rotate.LeftRotationAngle = 45;
            controller.Commands.Enqueue(rotate);

            var scoop = new TakeSampleCommand(robot);
            scoop.TakeSample = true;
            controller.Commands.Enqueue(scoop);

            controller.ExecuteCommands();
            controller.UndoCommands(3);
        }
    }
}
