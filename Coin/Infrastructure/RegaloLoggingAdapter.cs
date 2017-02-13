using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Inforigami.Regalo.Core;

namespace Coin.Infrastructure
{
    public class RegaloLoggingAdapter : ILogger
    {
        private string _eventLogSource = "Coin";

        public void Debug(object sender, string format, params object[] args)
        {
            EventLog.WriteEntry(_eventLogSource, FormatMessage(sender, "Debug", format, args), EventLogEntryType.SuccessAudit);
        }

        public void Info(object sender, string format, params object[] args)
        {
            EventLog.WriteEntry(_eventLogSource, FormatMessage(sender, "Info", format, args), EventLogEntryType.Information);
        }

        public void Warn(object sender, string format, params object[] args)
        {
            EventLog.WriteEntry(_eventLogSource, FormatMessage(sender, "Warn", format, args), EventLogEntryType.Warning);
        }

        public void Error(object sender, Exception exception, string format, params object[] args)
        {
            EventLog.WriteEntry(_eventLogSource, FormatMessage(sender, "Error", format, args) + "\r\n\r\n" + GetExceptionReport(exception), EventLogEntryType.Error);
        }

        private static string FormatMessage(object sender, string logLevel, string format, object[] args)
        {
            return
                string.Format("Source: {0}{1}Level: {2}{1}Message: {3}",
                    sender.GetType().Name,
                    Environment.NewLine,
                    logLevel,
                    string.Format(format, args));
        }

        private string GetExceptionReport(Exception exception)
        {
            var report = new StringBuilder();

            foreach (var ex in GetExceptions(exception))
            {
                report.AppendLine(FormatException(ex));
            }

            return report.ToString();
        }

        private string FormatException(Exception ex)
        {
            var result = new StringBuilder();

            result.AppendFormat("*** {0}: \"{1}\"", ex.GetType().Name, ex.Message).AppendLine();
            result.AppendFormat("{0}", ex.StackTrace).AppendLine();

            var dbEntityValidationException = ex as DbEntityValidationException;
            if (dbEntityValidationException != null)
            {
                result.AppendLine();
                AppendDbEntityValidationErrors(result, dbEntityValidationException);
            }

            return result.ToString();
        }

        private static void AppendDbEntityValidationErrors(
            StringBuilder result,
            DbEntityValidationException dbEntityValidationException)
        {
            result.AppendLine("Entity Validation Errors:");

            int validationErrorIndex = 0;
            foreach (var entityValidationError in dbEntityValidationException.EntityValidationErrors)
            {
                result.AppendFormat("   Validation Error {0}", validationErrorIndex).AppendLine();
                result.AppendFormat("      Current values:").AppendLine();
                foreach (var propertyName in entityValidationError.Entry.CurrentValues.PropertyNames)
                {
                    result.AppendFormat(
                        "         {0} = {1}", propertyName,
                        entityValidationError.Entry.CurrentValues[propertyName])
                          .AppendLine();
                }

                result.AppendFormat("      Errors:").AppendLine();
                foreach (var validationError in entityValidationError.ValidationErrors)
                {
                    result.AppendFormat(
                        "         {0}: {1}", validationError.PropertyName,
                        validationError.ErrorMessage)
                          .AppendLine();
                }
            }
        }

        private IEnumerable<Exception> GetExceptions(Exception exception)
        {
            if (exception == null) throw new ArgumentNullException(nameof(exception));

            do
            {
                yield return exception;
            } while ((exception = exception.InnerException) != null);
        }
    }
}