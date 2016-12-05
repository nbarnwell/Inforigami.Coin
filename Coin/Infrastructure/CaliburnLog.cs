using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;

namespace Coin.Infrastructure
{
    public class CaliburnLog : ILog
    {
        public void Info(string format, params object[] args)
        {
            Debug.WriteLine("I " + string.Format(format, args));
        }

        public void Warn(string format, params object[] args)
        {
            Debug.WriteLine("W " + string.Format(format, args));
        }

        public void Error(Exception exception)
        {
            Debug.Fail(exception.ToString());
        }
    }
}
