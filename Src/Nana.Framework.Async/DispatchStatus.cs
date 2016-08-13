using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nana.Framework.Async
{
    public enum DispatchStatus
    {
        /// <summary>
        /// Before dispatching
        /// </summary>
        NotStarted = 0,

        /// <summary>
        /// Dispatching
        /// </summary>
        Dispatching = 1,

        /// <summary>
        /// Cancelling for one task's failure.
        /// </summary>
        Cancelling = 2,

        /// <summary>
        /// Waiting for the tasks' finishes because of one task's failure.
        /// </summary>
        Waiting = 3,

        /// <summary>
        /// Finished
        /// </summary>
        Finished = 4
    }
}
