using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nana.Framework.Async
{
    internal class AsyncTaskException : Exception
    {
        public AsyncTask Task { get; private set; }

        internal AsyncTaskException(AsyncTask task, Exception ex)
            : base("Exception occurred", ex)
        {
            this.Task = task;
        }
        
    }
}
