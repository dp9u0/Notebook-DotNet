using Common;
using System;
using System.Threading;

namespace ThreadSample {
    class VolatileRunner : Runner {
        protected override void RunCore() {
            StrangeBehavior.Run();
            Console.ReadKey();
        }

        internal static class StrangeBehavior {
            // As you'll see later, mark this field as volatile to fix the problem
            private static Boolean s_stopWorker = false;
            public static void Run() {
                Console.WriteLine("Main: letting worker run for 5 seconds");
                Thread t = new Thread(Worker);
                t.IsBackground = false;
                t.Start();
                Thread.Sleep(50000);
                s_stopWorker = true;
                Console.WriteLine("Main: waiting for worker to stop");
                t.Join();
            }
            private static void Worker(Object o) {
                Int32 x = 0;
                /**
                C:\Users\guodp\Source\Notebook-DotNet\src\Thread\VolatileRunner.cs @ 27:
                00007ff9`64b3387e 85c9            test    ecx,ecx
                00007ff9`64b33880 74fa            je      00007ff9`64b3387c
                **/

                /**
                00007ff9`74433aba 48b9d86c3174f97f0000 mov rcx,7FF974316CD8h
                00007ff9`74433ac4 ba03000000      mov     edx,3
                00007ff9`74433ac9 e8e27cae5f      call    coreclr!JIT_GetSharedNonGCStaticBase_SingleAppDomain (00007ff9`d3f1b7b0)
                00007ff9`74433ace 48b90d6d3174f97f0000 mov rcx,7FF974316D0Dh
                00007ff9`74433ad8 0fb609          movzx   ecx,byte ptr [rcx]
                00007ff9`74433adb 85c9            test    ecx,ecx
                00007ff9`74433add 0f94c1          sete    cl
                00007ff9`74433ae0 0fb6c9          movzx   ecx,cl
                00007ff9`74433ae3 894de8          mov     dword ptr [rbp-18h],ecx
                00007ff9`74433ae6 837de800        cmp     dword ptr [rbp-18h],0
                00007ff9`74433aea 75c4            jne     00007ff9`74433ab0
                00007ff9`74433ab1 8b4dec          mov     ecx,dword ptr [rbp-14h]
                00007ff9`74433ab4 ffc1            inc     ecx
                00007ff9`74433ab6 894dec          mov     dword ptr [rbp-14h],ecx
                 **/
                while (!s_stopWorker) {
                    x++;
                }
                Console.WriteLine("Worker: stopped when x={0}", x);
            }
        }
        internal sealed class ThreadsSharingData {
            public static void Run() {

            }

            private Int32 m_flag = 0;
            private Int32 m_value = 0;
            // This method is executed by one thread 
            public void Thread1() {
                // Note: These could execute in reverse order
                m_value = 5;
                m_flag = 1;
            }
            // This method is executed by another thread 
            public void Thread2() {// Note: m_value could be read before m_flag
                if (m_flag == 1)
                    Console.WriteLine(m_value);
            }
        }
    }
}
