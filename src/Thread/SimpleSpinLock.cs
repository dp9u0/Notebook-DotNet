using System;
using System.Threading;

namespace ThreadSample {
    class SimpleSpinLock {
        private Int32 m_ResourceInUse; // 0=false (default), 1=true
        private const Int32 OptimalMaxSpinWaitsPerSpinIteration = 1 << 32;
        public void Enter() {
            Int32 count = 0;
            while (true) {
                // Always set resource to in-use
                // When this thread changes it from not in-use, return
                if (Interlocked.Exchange(ref m_ResourceInUse, 1) == 0) return;
                Int32 n = OptimalMaxSpinWaitsPerSpinIteration;
                if ((1 << count) < n) {
                    n = 1 << count;
                }
                Thread.SpinWait(n);
                count++;
            }
        }
        public void Leave() {
            // Set resource to not in-use
            Volatile.Write(ref m_ResourceInUse, 0);
        }
    }
    // And here is a class that shows how to use the SimpleSpinLock.
    public sealed class SomeResource {
        private SimpleSpinLock m_sl = new SimpleSpinLock();
        public void AccessResource() {
            m_sl.Enter();
            // Only one thread at a time can get in here to access the resource...
            m_sl.Leave();
        }
    }
}
