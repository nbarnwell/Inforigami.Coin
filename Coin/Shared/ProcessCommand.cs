using System;
using Caliburn.Micro;
using Inforigami.Regalo.Core;

namespace Coin.Shared
{
    public class ProcessCommand : IResult
    {
        private readonly object _command;

        public ProcessCommand(object command)
        {
            if (command == null) throw new ArgumentNullException(nameof(command));
            _command = command;
        }

        public event EventHandler<ResultCompletionEventArgs> Completed;

        public void Execute(CoroutineExecutionContext context)
        {
            var processor = (ICommandProcessor)AppBootstrapper.Container.GetInstance(typeof(ICommandProcessor), "CommandProcessor");

            processor.Process(_command);

            var completed = Completed;
            completed?.Invoke(this, new ResultCompletionEventArgs());
        }
    }
}