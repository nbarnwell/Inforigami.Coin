using System;
using Coin.Data;
using Inforigami.Regalo.Core;
using Inforigami.Regalo.Interfaces;
using Newtonsoft.Json;

namespace Coin.Shared
{
    public class MessageAuditingHandler : ICommandHandler<object>, ICommandHandler<ICommand>, IEventHandler<IEvent>
    {
        private readonly ILogger _logger;

        public MessageAuditingHandler(ILogger logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger;
        }

        public void Handle(object command)
        {
            var cmd = command as ICommand;
            if (cmd == null)
            {
                _logger.Warn(this, "Command is being processed that does not derive from Command");

                using (var db = new Database())
                {
                    db.AuditLogs.Add(
                        new AuditLog
                        {
                            Timestamp = DateTimeOffset.Now,
                            MessageTypeName = command.GetType().FullName,
                            PayloadJson = JsonConvert.SerializeObject(command, Formatting.Indented)
                        });
                    db.SaveChanges();
                }
            }
            else
            {
                Handle(cmd);
            }
        }

        public void Handle(ICommand command)
        {
            Handle((IMessage)command);
        }

        public void Handle(IEvent evt)
        {
            Handle((IMessage)evt);
        }

        private void Handle(IMessage message)
        {
            using (var db = new Database())
            {
                db.AuditLogs.Add(
                    new AuditLog
                    {
                        CausationId     = message.CausationId,
                        CorrelationId   = message.CorrelationId,
                        MessageId       = message.MessageId,
                        Timestamp       = message.Timestamp,
                        MessageTypeName = message.GetType().FullName,
                        PayloadJson     = JsonConvert.SerializeObject(message, Formatting.Indented)
                    });
                db.SaveChanges();
            }
        }
    }
}