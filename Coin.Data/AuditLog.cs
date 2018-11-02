using System;
using System.Collections.Generic;

namespace Coin.Data
{
    public partial class AuditLog
    {
        public int Id { get; set; }
        public Guid MessageId { get; set; }
        public Guid CorrelationId { get; set; }
        public Guid CausationId { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string MessageTypeName { get; set; }
        public string PayloadJson { get; set; }
    }
}
