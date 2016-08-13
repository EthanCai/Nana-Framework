using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace Nana.Framework.Async
{
    public class DispatchException : ApplicationException
    {
        private Dictionary<object, AsyncTask> m_tasks;

        private Dictionary<object, Exception> m_taskExceptions;
         
        internal DispatchException(Dictionary<object, AsyncTask> tasks, Dictionary<object, Exception> taskExceptions)
            : base("One or more tasks failed.")
        {
            this.m_tasks = tasks;
            this.m_taskExceptions = taskExceptions;
        }

        public AsyncTask GetTask(object id)
        {
            AsyncTask result;
            return this.m_tasks.TryGetValue(id, out result) ? result : null;
        }

        public Exception GetTaskException(object id)
        {
            Exception ex;
            return this.m_taskExceptions.TryGetValue(id, out ex) ? ex : null;
        }

        public Dictionary<object, Exception> TaskExceptions
        {
            get { return m_taskExceptions; }
        }
    }
}
