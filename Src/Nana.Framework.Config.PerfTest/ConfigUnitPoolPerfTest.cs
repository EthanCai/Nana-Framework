using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Nana.Framework.Utility;

namespace Nana.Framework.Config.PerfTest
{
    /// <summary>
    /// 比较基于ConcurrentDictionary和ReadWriteLockSlim实现的缓存的性能
    /// </summary>
    public class ConfigUnitPoolPerfTest
    {
        private int _times = 10000;
        private int _size = 1000;

        public void TestPerformanceWithoutUsingLock()
        {
            Dictionary<int, int> cache = new Dictionary<int, int>();

            CodeTimer.Time("Write to Dictionary", 1, () =>
            {
                for (int i = 0; i < this._size; i++)
                {
                    cache[i] = i;
                }
            });

            CodeTimer.Time("Read from Dictionary", this._times, () =>
            {
                for (int i = 0; i < this._size; i++)
                {
                    int value = cache[i];
                }
            });
        }

        public void TestPerformanceUsingConcurrentDictionary()
        {
            CodeTimer.Initialize();

            ConcurrentDictionary<int, int> cache = new ConcurrentDictionary<int, int>();

            CodeTimer.Time("Write to Concurrent Dictionary", 1, () =>
            {
                for (int i = 0; i < this._size; i++)
                {
                    cache.TryAdd(i, i);
                }
            }); 

            CodeTimer.Time("Read from Concurrent Dictionary", this._times, () =>
            {
                for (int i = 0; i < this._size; i++)
                {
                    int value = 0;
                    cache.TryGetValue(i, out value);
                }
            });
        }

        public void TestPerformanceUsingReadWriteLockSlim()
        {
            CodeTimer.Initialize();

            Dictionary<int, int> cache = new Dictionary<int, int>();
            ReaderWriterLockSlim locker = new ReaderWriterLockSlim();

            CodeTimer.Time("Write Using ReadWriteLockSlim", 1, () =>
            {
                for (int i = 0; i < this._size; i++)
                {
                    try
                    {
                        locker.EnterWriteLock();
                        cache[i] = i;
                    }
                    finally
                    {
                        locker.ExitWriteLock();
                    }
                }
            });

            CodeTimer.Time("Read Using ReadWriteLockSlim", this._times, () =>
            {
                for (int i = 0; i < this._size; i++)
                {
                    try
                    {
                        locker.EnterReadLock();
                        int value = cache[i];
                    }
                    finally
                    {
                        locker.ExitReadLock();
                    }
                }
            });
        }
    }
}
