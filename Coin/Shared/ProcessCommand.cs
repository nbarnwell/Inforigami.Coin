using System;
using Caliburn.Micro;

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
            throw new NotImplementedException();

            var completed = Completed;
            if (completed != null)
            {
                completed(this, new ResultCompletionEventArgs());
            }
        }
    }
}