using System;
using System.Threading;

namespace ThreadSample {

    /// <summary>
    /// 
    /// </summary>
    class SimpleHybridLock {

        private const int MAX_SPIN_COUNT = 100;
        private int _threads = 0; // 多少个线程等待或者正在持有这个锁
        private int _owningThreadId = 0;
        private int _recursionCount = 0;
        private readonly AutoResetEvent _lock = new AutoResetEvent(false);

        public void Enter() {
            var currentThreadId = Thread.CurrentThread.ManagedThreadId;
            if (_owningThreadId == currentThreadId) {
                _recursionCount++;
            }
            var spinWaiter = new SpinWait();
            for (int i = 0; i < MAX_SPIN_COUNT; i++) {
                spinWaiter.SpinOnce();
                if (Interlocked.CompareExchange(ref _threads, 1, 0) == 1) {
                    GotLock(currentThreadId);
                    return;
                }
            }
            if (Interlocked.Increment(ref _threads) > 1) {
                _lock.WaitOne();
            }
            GotLock(currentThreadId);
        }

        private void GotLock(int currentThreadId) {
            _recursionCount = 1;
            _owningThreadId = currentThreadId;
            Volatile.Write(ref _threads, 1);
        }
        public void Leave() {
            var currentThreadId = Thread.CurrentThread.ManagedThreadId;
            if (_owningThreadId != currentThreadId) {
                throw new InvalidOperationException();
            }
            _recursionCount--;
            if (_recursionCount >= 0) {
                return;
            }
            _owningThreadId = 0;
            if (Interlocked.Decrement(ref _threads) == 0) {
                return;
            }
            _lock.Set();
        }

        public void Dispose() { _lock.Dispose(); }

    }
}
