using Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadSample {
    class TaskRunner1 : Runner {
        protected override void RunCore() {
            SimpleTaskSample();
        }

        private void SimpleTaskSample() {
            var task = Task.Run(() => ComputeBoundOp(5));
            Console.WriteLine(task.Result);

            Task.Factory.StartNew(() => ComputeBoundOp(5));
            Console.WriteLine(task.Result);
        }

        private static string ComputeBoundOp(Object state) {
            // This method is executed by a thread pool thread 
            Console.WriteLine("In ComputeBoundOp: state={0}", state);
            Thread.Sleep(1000);
            return "string";
        }

        private void TaskDiffWithFactory() {

            var task1 = Task.Factory.StartNew(async () => "1111");

            /*
Task<Task<TResult>> outerTask = Task<Task<TResult>>.Factory.StartNew(function, cancellationToken, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
return new UnwrapPromise<TResult>(outerTask, true);
             */
            var task2 = Task.Run(async () => "1111");

            var task3 = Task.Run(async () => Console.WriteLine(""));


            SynchronizationContext.Current.Post((value) =>
            {
            }, 1);

        }
    }
}
