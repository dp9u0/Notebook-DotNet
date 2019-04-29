using Common;
using System;
using System.Threading;

namespace ThreadSample {
    class SingletonRunner : Runner {
        protected override void RunCore() {

        }

        internal sealed class Singleton {
            // s_lock is required for thread safety and having this object assumes that creating 
            // the singleton object is more expensive than creating a System.Object object and that 
            // creating the singleton object may not be necessary at all. Otherwise, it is more 
            // efficient and easier to just create the singleton object in a class constructor
            private static readonly Object s_lock = new Object();
            // This field will refer to the one Singleton object
            private static Singleton s_value = null;

            // Private constructor prevents any code outside this class from creating an instance 
            private Singleton() {
                // Code to initialize the one Singleton object goes here...
            }
            // Public, static method that returns the Singleton object (creating it if necessary) 
            public static Singleton GetSingleton() { // If the Singleton was already created, just return it (this is fast)
                if (s_value != null) return s_value;
                Monitor.Enter(s_lock); // Not created, let 1 thread create it
                if (s_value == null) {
                    // Still not created, create it
                    Singleton temp = new Singleton();
                    // 防止 s_value != null 但是 Singleton 构造还没有执行完
                    // Singleton temp = new Singleton(); 不是原子操作,可能被更改执行顺序: 1. 分配内存 2 构造函数 3 变量赋值
                    Volatile.Write(ref s_value, temp);
                }
                Monitor.Exit(s_lock);
                // Return a reference to the one Singleton object 
                return s_value;
            }
        }

        internal sealed class Singleton2 {
            private static Singleton2 s_value = new Singleton2();

            // Private constructor prevents any code outside this class from creating an instance 
            private Singleton2() {
                // Code to initialize the one Singleton object goes here...
            }
            // Public, static method that returns the Singleton object (creating it if necessary) 
            public static Singleton2 GetSingleton() { return s_value; }
        }

        internal sealed class Singleton3 {
            private static Singleton3 s_value = null;

            // Private constructor prevents any code outside this class from creating an instance 
            private Singleton3() {
                // Code to initialize the one Singleton object goes here...
            }
            // Public, static method that returns the Singleton object (creating it if necessary) 
            public static Singleton3 GetSingleton() {
                if (s_value != null) return s_value;
                // 可能会出现多次创建的问题
                // 但是这是无锁的情况
                Interlocked.CompareExchange(ref s_value, new Singleton3(), null);
                return s_value;
            }
        }
    }
}
