using Common;
using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace CLR {
    class HandleCollectorRunner : Runner {
        protected override void RunCore() {
            //GCNotification.GCDone += GCNotification_GCDone; ;
            MemoryPressureDemo(0); // 0 causes infrequent GCs
            Thread.Sleep(100);
            MemoryPressureDemo(10 * 1024 * 1024); // 10MB causes frequent GCs
            HandleCollectorDemo();
        }

        private void GCNotification_GCDone(int obj) {

            Console.WriteLine($"{GC.CollectionCount(0)},{GC.CollectionCount(1)},{GC.CollectionCount(2)}");
            Console.WriteLine(GC.GetTotalMemory(true));
        }

        private static void MemoryPressureDemo(Int32 size) {
            BigNativeResource.Reset();
            Console.WriteLine();
            Console.WriteLine("MemoryPressureDemo, size={0}", size);
            // Create a bunch of objects specifying their logical size 
            for (Int32 count = 0; count < 15; count++) {
                new BigNativeResource(size);
                Thread.Sleep(100);
            }
            // For demo purposes, force everything to be cleaned-up 
            Console.WriteLine("Call GC...");
            Console.WriteLine($"Before {GC.CollectionCount(0)},{GC.CollectionCount(1)},{GC.CollectionCount(2)}");
            GC.Collect();
            Console.WriteLine($"After {GC.CollectionCount(0)},{GC.CollectionCount(1)},{GC.CollectionCount(2)}");
        }
        private sealed class BigNativeResource {
            public static void Reset() {
                s_count = 1;
            }
            private readonly Int32 m_size;
            private static int s_count = 1;
            private int m_count;
            public BigNativeResource(Int32 size) {
                m_size = size;
                m_count = s_count++;
                // Make the GC think the object is physically bigger 
                if (m_size > 0) {
                    GC.AddMemoryPressure(m_size);
                }
                Console.WriteLine($"BigNativeResource create: {m_count}");
            }
            ~BigNativeResource() {
                // Make the GC think the object released more memory 
                if (m_size > 0) {
                    GC.RemoveMemoryPressure(m_size);
                }
                Console.WriteLine($"{GC.CollectionCount(0)},{GC.CollectionCount(1)},{GC.CollectionCount(2)}");
                Console.WriteLine($"BigNativeResource destroy: {m_count},Gen {GC.GetGeneration(this)}");
            }
        }
        private static void HandleCollectorDemo() {
            Console.WriteLine();
            Console.WriteLine("HandleCollectorDemo");
            for (Int32 count = 0; count < 10; count++) {
                new LimitedResource();
                Thread.Sleep(100);
            }
            // For demo purposes, force everything to be cleaned-up 
            Console.WriteLine("Call GC...");
            Console.WriteLine($"Before {GC.CollectionCount(0)},{GC.CollectionCount(1)},{GC.CollectionCount(2)}");
            GC.Collect();
            Console.WriteLine($"After {GC.CollectionCount(0)},{GC.CollectionCount(1)},{GC.CollectionCount(2)}");
        }
        private sealed class LimitedResource {
            private static int s_count = 1;
            private int m_count;

            // Create a HandleCollector telling it that collections should 
            // occur when two or more of these objects exist in the heap 
            private static readonly HandleCollector s_hc = new HandleCollector("LimitedResource", 2);
            public LimitedResource() {
                m_count = s_count++;
                // Tell the HandleCollector a LimitedResource has been added to the heap 
                s_hc.Add();
                Console.WriteLine("LimitedResource create. Count={0},({1})", s_hc.Count, m_count);
            }
            ~LimitedResource() {
                // Tell the HandleCollector a LimitedResource has been removed from the heap 
                s_hc.Remove();
                Console.WriteLine($"LimitedResource destroy. Count={s_hc.Count},({m_count},Gen {GC.GetGeneration(this)})");
            }
        }
    }
}
