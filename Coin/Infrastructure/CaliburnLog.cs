using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using Caliburn.Micro;

namespace Coin.Infrastructure
{
    public class CaliburnLog : ILog
    {
        public void Info(string format, params object[] args)
        {
            Debug.WriteLine("Caliburn: I " + string.Format(format, args));
        }

        public void Warn(string format, params object[] args)
        {
            Debug.WriteLine("Caliburn: W " + string.Format(format, args));
        }

        public void Error(Exception exception)
        {
            Debug.WriteLine("Caliburn: E " + GetExceptionReport(exception));
            Debug.Fail(exception.ToString());
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
            if (ex is TargetInvocationException)
            {
                return "*** TargetInvocationException";
            }

            var result = new StringBuilder();

            result.AppendFormat("*** {0}", ex.GetType().Name).AppendLine();
            result.AppendFormat("\"{0}\"", ex.Message).AppendLine();
            result.AppendFormat("{0}", ex.StackTrace).AppendLine();

            return result.ToString();
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
