using Common;
using System;
using System.Threading;

namespace ThreadSample {
    class ThreadPoolRunner : Runner {
        protected override void RunCore() {
            Console.WriteLine("Main thread: queuing an asynchronous operation");
            ThreadPool.QueueUserWorkItem(ComputeBoundOp, 5);
            Console.WriteLine("Main thread: Doing other work here...");
            Thread.Sleep(10000); // Simulating other work (10 seconds) 
            Console.WriteLine("Hit <Enter> to end this program...");
            Console.ReadLine();
        }

        private static void ComputeBoundOp(Object state) {
            // This method is executed by a thread pool thread 
            Console.WriteLine("In ComputeBoundOp: state={0}", state);
            Thread.Sleep(1000); 
        }
    }
}
