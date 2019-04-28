using Common;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadSample {
    class MutexRunner : Runner {
        protected override void RunCore() {

        }

        private readonly Mutex m_lock = new Mutex();
        public void Method1() {
            m_lock.WaitOne();
            // Do whatever...
            Method2(); // Method2 recursively acquires the lock
            m_lock.ReleaseMutex();
        }
        public void Method2() {
            m_lock.WaitOne();
            // Do whatever...
            m_lock.ReleaseMutex();
        }
        public void Dispose() { m_lock.Dispose(); }
    }
}
