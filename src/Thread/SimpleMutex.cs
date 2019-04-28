using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ThreadSample {
    class SimpleMutex {

        private readonly AutoResetEvent _lock;
        private int _owningThreadId = 0;
        private int _recursionCount = 0;

        public SimpleMutex() {
            _lock = new AutoResetEvent(true); // Initially free
        }
        public void Enter() {
            if (_owningThreadId == Thread.CurrentThread.ManagedThreadId) {
                _recursionCount++;
                return;
            }
            _lock.WaitOne();
            _owningThreadId = Thread.CurrentThread.ManagedThreadId;
            _recursionCount = 1;
        }
        public void Leave() {
            if (_owningThreadId != Thread.CurrentThread.ManagedThreadId) {
                throw new InvalidOperationException();
            }
            _recursionCount--;
            if (_recursionCount == 0) {
                _owningThreadId = 0;
                _lock.Set();
            }
        }
        public void Dispose() { _lock.Dispose(); }
    }
}
