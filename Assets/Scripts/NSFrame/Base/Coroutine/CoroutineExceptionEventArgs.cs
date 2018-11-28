using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NSFrame
{
    public class CoroutineExceptionEventArgs : EventArgs
    {
        public Exception Exception
        {
            get;
            private set;
        }
        public CoroutineExceptionEventArgs(Exception exception)
        {
            this.Exception = exception;
        }
    }
}
