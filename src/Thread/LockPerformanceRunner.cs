using Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace ThreadSample {
    class LockPerformanceRunner : Runner {
        protected override void RunCore() {
            Int32 x = 0;
            const Int32 iterations = 10000000; // 10 million
                                               // How long does it take to increment x 10 million times?
            Stopwatch sw = Stopwatch.StartNew();
            for (Int32 i = 0; i < iterations; i++) {
                x++;
            }
            Console.WriteLine("Incrementing x: {0:N0}", sw.ElapsedMilliseconds);
            // How long does it take to increment x 10 million times 
            // adding the overhead of calling a method that does nothing?
            sw.Restart();
            for (Int32 i = 0; i < iterations; i++) {
                M(); x++; M();
            }
            Console.WriteLine("Incrementing x in M: {0:N0}", sw.ElapsedMilliseconds);
            // How long does it take to increment x 10 million times 
            // adding the overhead of calling an uncontended SimpleSpinLock?
            SpinLock sl = new SpinLock(false);
            sw.Restart();
            for (Int32 i = 0; i < iterations; i++) {
                Boolean taken = false; sl.Enter(ref taken); x++; sl.Exit();
            }
            Console.WriteLine("Incrementing x in SpinLock: {0:N0}", sw.ElapsedMilliseconds);

            // How long does it take to increment x 10 million times 
            // adding the overhead of calling an uncontended SimpleWaitLock?
            using (SimpleWaitLock swl = new SimpleWaitLock()) {
                sw.Restart();
                for (Int32 i = 0; i < iterations; i++) {
                    swl.Enter();
                    x++;
                    swl.Leave();
                }
                Console.WriteLine("Incrementing x in SimpleWaitLock: {0:N0}", sw.ElapsedMilliseconds);
            }
        }


        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void M() { /* This method does nothing but return */ }
    }
}
