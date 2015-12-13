using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BoardGame.Domain.Helpers
{
    public class BlockingPredicateCollection<T>
    {
        private readonly object lockObject = new object();
        private readonly List<T> collection = new List<T>();

        public void Add(T item)
        {
            lock (lockObject)
            {
                collection.Add(item);
                Monitor.PulseAll(lockObject);
            }
        }

        public void Remove(T item)
        {
            lock (lockObject)
            {
                collection.Remove(item);
                Monitor.PulseAll(lockObject);
            }
        }

        public bool TryTake(out T item, int timeout = 0, Predicate<T> predicate = null)
        {
            Stopwatch sw = Stopwatch.StartNew();
            TimeSpan timeoutSpan = new TimeSpan(0, 0, 0, 0, timeout);

            lock (lockObject)
            {
                int index;

                while ((index = collection.FindIndex(predicate)) < 0)
                {
                    TimeSpan elapsed = sw.Elapsed;

                    if (elapsed > timeoutSpan || !Monitor.Wait(lockObject, timeoutSpan - elapsed))
                    {
                        item = default(T);
                        return false;
                    }
                }

                item = collection[index];
                collection.RemoveAt(index);
                return true;
            }
        }


    }
}
