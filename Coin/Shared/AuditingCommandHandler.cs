using Inforigami.Regalo.Core;

namespace Coin.Shared
{
    public class AuditingCommandHandler : ICommandHandler<object>
    {
        public void Handle(object command)
        {
            throw new System.NotImplementedException();
             // TODO: Write to never-ending audit table with json serialised command
        }
    }
}