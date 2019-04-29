using Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadSample {
    class AsyncLockRunner : Runner {

        SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1);

        protected override void RunCore() {
            var task = AccessResourceViaAsyncSynchronization(semaphoreSlim);
            task.Wait();
        }

        private static async Task AccessResourceViaAsyncSynchronization(SemaphoreSlim asyncLock) {
            await asyncLock.WaitAsync();
            // do something need lock
            asyncLock.Release();
            // do some thing don not need to wait
        }

        private static void ConcurrentExclusiveSchedulerDemo() {
            var cesp = new ConcurrentExclusiveSchedulerPair();
            var tfExclusive = new TaskFactory(cesp.ExclusiveScheduler);
            var tfConcurrent = new TaskFactory(cesp.ConcurrentScheduler);
            for (Int32 operation = 0; operation < 5; operation++) {
                var exclusive = operation < 2; // 2 exclusive & 3 concurrent
                (exclusive ? tfExclusive : tfConcurrent).StartNew(() =>
                {
                    Console.WriteLine("{0} access", exclusive ? "exclusive" : "concurrent");
                    // TODO: Do exclusive write or concurrent read computation here...
                });
            }
        }

        public enum OneManyMode { Exclusive, Shared }
        public sealed class AsyncOneManyLock {
            #region Lock code
            private SpinLock m_lock = new SpinLock(true); // Don't use readonly with a SpinLock
            private void Lock() { Boolean taken = false; m_lock.Enter(ref taken); }
            private void Unlock() { m_lock.Exit(); }
            #endregion
            #region Lock state and helper methods
            private Int32 m_state = 0;
            private Boolean IsFree { get { return m_state == 0; } }
            private Boolean IsOwnedByWriter { get { return m_state == -1; } }
            private Boolean IsOwnedByReaders { get { return m_state > 0; } }
            private Int32 AddReaders(Int32 count) { return m_state += count; }
            private Int32 SubtractReader() { return --m_state; }
            private void MakeWriter() { m_state = -1; }
            private void MakeFree() { m_state = 0; }
            #endregion
            // For the no-contention case to improve performance and reduce memory consumption
            private readonly Task m_noContentionAccessGranter;
            // Each waiting writers wakes up via their own TaskCompletionSource queued here
            private readonly Queue<TaskCompletionSource<Object>> m_qWaitingWriters = new Queue<TaskCompletionSource<Object>>();
            // All waiting readers wake up by signaling a single TaskCompletionSource
            private TaskCompletionSource<Object> m_waitingReadersSignal = new TaskCompletionSource<Object>();
            private Int32 m_numWaitingReaders = 0;
            public AsyncOneManyLock() {
                m_noContentionAccessGranter = Task.FromResult<Object>(null);
            }
            public Task WaitAsync(OneManyMode mode) {
                Task accressGranter = m_noContentionAccessGranter; // Assume no contention
                Lock();
                switch (mode) {
                    case OneManyMode.Exclusive:
                        if (IsFree) {
                            MakeWriter(); // No contention
                        } else {
                            // Contention: Queue new writer task & return it so writer waits
                            var tcs = new TaskCompletionSource<Object>();
                            m_qWaitingWriters.Enqueue(tcs);
                            accressGranter = tcs.Task;
                        }
                        break;
                    case OneManyMode.Shared:
                        if (IsFree || (IsOwnedByReaders && m_qWaitingWriters.Count == 0)) {
                            AddReaders(1); // No contention
                        } else { // Contention
                                 // Contention: Increment waiting readers & return reader task so reader waits
                            m_numWaitingReaders++;
                            accressGranter = m_waitingReadersSignal.Task.ContinueWith(t => t.Result);
                        }
                        break;
                }
                Unlock();
                return accressGranter;
            }

            public void Release() {
                TaskCompletionSource<Object> accessGranter = null; // Assume no code is released
                Lock();
                if (IsOwnedByWriter) MakeFree(); // The writer left
                else SubtractReader(); // A reader left
                if (IsFree) {
                    // If free, wake 1 waiting writer or all waiting readers
                    if (m_qWaitingWriters.Count > 0) {
                        MakeWriter();
                        accessGranter = m_qWaitingWriters.Dequeue();
                    } else if (m_numWaitingReaders > 0) {
                        AddReaders(m_numWaitingReaders);
                        m_numWaitingReaders = 0;
                        accessGranter = m_waitingReadersSignal;
                        // Create a new TCS for future readers that need to wait
                        m_waitingReadersSignal = new TaskCompletionSource<Object>();
                    }
                }
                Unlock();
                // Wake the writer/reader outside the lock to reduce
                // chance of contention improving performance
                if (accessGranter != null) accessGranter.SetResult(null);
            }
        }
    }
}
