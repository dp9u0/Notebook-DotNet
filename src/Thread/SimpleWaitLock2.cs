using System;
using System.Threading;

namespace ThreadSample {
    public sealed class SimpleWaitLock2 : IDisposable {

        private readonly Semaphore m_available;

        public SimpleWaitLock2(Int32 maxConcurrent) {
            m_available = new Semaphore(maxConcurrent, maxConcurrent);
        }
        public void Enter() {
            // Block in kernel until resource available
            m_available.WaitOne();
        }
        public void Leave() {
            // Let another thread access the resource
            m_available.Release(1);
        
        }
        public void Dispose() { m_available.Close(); }
    }
}
