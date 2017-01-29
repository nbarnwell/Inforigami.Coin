using System;
using System.Diagnostics;
using Caliburn.Micro;
using Inforigami.Regalo.Core;

namespace Coin.Infrastructure
{
    public class CaliburnLog : ILog
    {
        private readonly ILogger _logger;

        public CaliburnLog(ILogger logger)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));
            _logger = logger;
        }

        public void Info(string format, params object[] args)
        {
            // Caliburn's "info" output is better suited to the "debug" level
            //_logger.Debug(this, format, args);
        }

        public void Warn(string format, params object[] args)
        {
            _logger.Warn(this, format, args);
        }

        public void Error(Exception exception)
        {
            _logger.Error(this, exception, "Caliburn Error");
        }
    }
}
