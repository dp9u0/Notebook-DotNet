using Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CSharp {
    class AsyncRunner : Runner {
        protected override void RunCore() {
            Console.WriteLine("测试1开始:{0}", Thread.CurrentThread.ManagedThreadId);
            SyncMethodCallSyncWork(0, false);
            Console.WriteLine("测试1结束:{0}", Thread.CurrentThread.ManagedThreadId);


            Console.WriteLine("测试2开始:{0}", Thread.CurrentThread.ManagedThreadId);
            SyncMethodCallAsyncWork(0, false);
            Console.WriteLine("测试2结束:{0}", Thread.CurrentThread.ManagedThreadId);


            Console.WriteLine("测试3开始:{0}", Thread.CurrentThread.ManagedThreadId);
            //只有标记为 async的异步方法 才能使用 await 调用 另外一个 async的方法
            //await AsyncMethod(0);
            //这样的调用方式是没法等待的
            //开始任务后 直接开始执行后面的代码
            //AsyncMethod 依旧在执行 不过当前线程不会等待执行完成
            var task = AsyncMethodCallAsyncWork(0, false);
            Console.WriteLine("测试3结束(其实还没结束):{0}", Thread.CurrentThread.ManagedThreadId);

            while (!task.IsCompleted) {
            }
            Console.WriteLine("异常测试:{0}", Thread.CurrentThread.ManagedThreadId);

            Console.WriteLine("测试4开始:{0}", Thread.CurrentThread.ManagedThreadId);
            SyncMethodCallSyncWork(0, true);
            Console.WriteLine("测试4结束:{0}", Thread.CurrentThread.ManagedThreadId);


            Console.WriteLine("测试5开始:{0}", Thread.CurrentThread.ManagedThreadId);
            SyncMethodCallAsyncWork(0, true);
            Console.WriteLine("测试5结束:{0}", Thread.CurrentThread.ManagedThreadId);


            Console.WriteLine("测试6开始:{0}", Thread.CurrentThread.ManagedThreadId);
            //只有标记为 async的异步方法 才能使用 await 调用 另外一个 async的方法
            //await AsyncMethod(0);
            //这样的调用方式是没法等待的
            //开始任务后 直接开始执行后面的代码
            //AsyncMethod 依旧在执行 不过当前线程不会等待执行完成
            // ReSharper disable once UnusedVariable
            var task2 = AsyncMethodCallAsyncWork(0, true);
            Console.WriteLine("测试6结束(其实还没结束):{0}", Thread.CurrentThread.ManagedThreadId);

            Console.ReadKey();
        }


        // 同步操作
        private static void SyncMethodCallSyncWork(int input, bool throwEx) {
            Console.WriteLine("进入同步操作:{0}", Thread.CurrentThread.ManagedThreadId);
            try {
                var result = Work(input, throwEx);
                Console.WriteLine("最终结果{0}:{1}", result, Thread.CurrentThread.ManagedThreadId);
            } catch (Exception ex) {
                ConsoleWrite(ex);
            }
            Console.WriteLine("退出同步操作:{0}", Thread.CurrentThread.ManagedThreadId);
        }


        // 同步操作
        private static void SyncMethodCallAsyncWork(int input, bool throwEx) {
            Console.WriteLine("进入同步操作:{0}", Thread.CurrentThread.ManagedThreadId);
            try {
                // 启动异步任务
                var task = AsyncWork(input, throwEx);
                //取Result时 会阻塞 会等待任务执行完成
                int result = task.Result;
                // 下面是同步方法
                //var result = Work(input);
                Console.WriteLine("最终结果{0}:{1}", result, Thread.CurrentThread.ManagedThreadId);
            } catch (Exception ex) {
                ConsoleWrite(ex);
            }
            Console.WriteLine("退出同步操作:{0}", Thread.CurrentThread.ManagedThreadId);
        }

        // 异步操作
        // 没有任何返回值的异步方法可以定义为 async void 或者 async Tack
        // async void 的返回值 没法等待
        // 因此 最好使用 async Task
        // private static async void AsyncMethod(int input) {
        private static async Task AsyncMethodCallAsyncWork(int input, bool throwEx) {
            //同步执行
            Console.WriteLine("进入异步操作:{0}", Thread.CurrentThread.ManagedThreadId);
            //此处会开新线程处理 AsyncWork 任务，然后方法马上返回  
            try {
                var task = AsyncWork(input, throwEx);
                //到await 这里会阻塞 等待任务Complete
                var result = await task;
                //await 后面的代码作为task完成时候的回调执行
                Console.WriteLine("最终结果{0}:{1}", result, Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine("退出异步操作！:{0}", Thread.CurrentThread.ManagedThreadId);
            } catch (Exception ex) {
                ConsoleWrite(ex);
            }
            Console.WriteLine("退出异步操作:{0}", Thread.CurrentThread.ManagedThreadId);
        }


        // 模拟耗时操作（异步方法）
        private static async Task<int> AsyncWork(int val, bool throwEx) {
            Console.WriteLine("进入异步工作:{0}", Thread.CurrentThread.ManagedThreadId);
            for (int i = 0; i < 5; ++i) {
                //延迟100ms 后执行下面的任务
                await Task.Delay(100);
                if ((i == 4) && throwEx) {
                    throw new OperationCanceledException("Operation Canceled!!!");
                }
                Console.WriteLine("耗时操作{0}:{1}", i, Thread.CurrentThread.ManagedThreadId);
                val++;
            }
            Console.WriteLine("退出异步工作:{0}", Thread.CurrentThread.ManagedThreadId);
            return val;
        }


        // 模拟耗时操作（同步方法）
        private static int Work(int val, bool throwEx) {
            Console.WriteLine("进入同步工作:{0}", Thread.CurrentThread.ManagedThreadId);
            for (int i = 0; i < 5; ++i) {
                Task.Delay(100);
                if ((i == 4) && throwEx) {
                    throw new OperationCanceledException("Operation Canceled!!!");
                }
                Console.WriteLine("耗时操作{0}:{1}", i, Thread.CurrentThread.ManagedThreadId);
                val++;
            }
            Console.WriteLine("进入同步工作:{0}", Thread.CurrentThread.ManagedThreadId);
            return val;
        }


        private static void ConsoleWrite(Exception ex) {
            var exception = ex;
            while (exception != null) {
                Console.WriteLine(exception.Message + ":" + Thread.CurrentThread.ManagedThreadId);
                Console.WriteLine(exception.StackTrace);
                exception = exception.InnerException;
            }
        }
    }
}
