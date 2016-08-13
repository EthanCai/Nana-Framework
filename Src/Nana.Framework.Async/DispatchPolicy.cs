using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nana.Framework.Async
{
    public enum DispatchPolicy
    {
        /// <summary>
        /// Dispatch normally
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Mark the job as cancelled
        /// </summary>
        MarkAsCancelled = 1,

        /// <summary>
        /// Mark the job as completed
        /// </summary>
        MarkAsSucceeded = 2
    }
}
