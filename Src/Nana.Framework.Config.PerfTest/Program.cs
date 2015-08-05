using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nana.Framework.Config.PerfTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigUnitPoolPerfTest configUnitPoolPerfTest = new ConfigUnitPoolPerfTest();
            configUnitPoolPerfTest.TestPerformanceWithoutUsingLock();
            configUnitPoolPerfTest.TestPerformanceUsingConcurrentDictionary();
            configUnitPoolPerfTest.TestPerformanceUsingReadWriteLockSlim();
        }
    }
}
