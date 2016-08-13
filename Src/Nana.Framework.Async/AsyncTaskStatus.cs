using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nana.Framework.Async
{
    public enum AsyncTaskStatus
    {
        /// <summary>
        /// Marked as Succeeded when dispatching
        /// </summary>
        MarkedAsSucceeded,

        /// <summary>
        /// Dispatched and succeeded
        /// </summary>
        Succeeded, 

        /// <summary>
        /// Dispatched but failed
        /// </summary>
        Failed,

        /// <summary>
        /// Not dispatched
        /// </summary>
        Pending,

        /// <summary>
        /// Marked as Cancelled when dispatching
        /// </summary>
        MarkedAsCancelled,

        /// <summary>
        /// Dispatched but cancelling for another's failure
        /// </summary>
        Cancelling,

        /// <summary>
        /// Dispatched but cancelled
        /// </summary>
        Cancelled,

        /// <summary>
        /// Dispatched and executing
        /// </summary>
        Executing
    }
}
