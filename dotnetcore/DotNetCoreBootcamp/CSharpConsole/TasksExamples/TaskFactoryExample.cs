using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpConsole.TasksExamples
{

    class State
    {
        public Action BackgroundTask;
        public EventWaitHandle Finished;
    }

    public class TaskFactoryExample
    {
        private static TaskFactory taskFactory;

        static TaskFactoryExample()
        {
            taskFactory = new TaskFactory();
        }

        public static void StartNewExample(Action backgroundTask, EventWaitHandle finished = null)
        {
            taskFactory.StartNew(_ =>
                {
                    var state = (State) _;
                    try
                    {
                        state.BackgroundTask();
                    }
                    finally
                    {
                        if (state.Finished != null)
                            state.Finished.Set();
                    }
                },
                new State {BackgroundTask = backgroundTask, Finished = finished},
                CancellationToken.None,
                TaskCreationOptions.LongRunning,
                TaskScheduler.Default);
        }
    }
}
