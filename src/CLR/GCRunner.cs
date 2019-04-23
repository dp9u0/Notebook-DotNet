using Common;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace CLR {
    class GCRunner : Runner {

        private static object var = null;

        protected override void RunCore() {
            GCNotification.GCDone += GCNotification_GCDone;
            Task.Factory.StartNew(Alloc);
            Task.Factory.StartNew(Alloc);
            Task.Factory.StartNew(Alloc);
            Task.Factory.StartNew(Alloc);
            Task.Factory.StartNew(Alloc);
            Task.Factory.StartNew(Alloc);
            Task.Factory.StartNew(Alloc);
            Task.Factory.StartNew(Alloc);
            Task.Factory.StartNew(Alloc);
            Console.ReadKey();
        }

        private void Alloc() {
            while (true) {
                for (int i = 0; i < 1000; i++) {
                    var = new Object();
                }
                Thread.Sleep(1000);
            }
        }

        private void GCNotification_GCDone(int obj) {
            Console.WriteLine("GC...");
            Console.WriteLine($"{GC.CollectionCount(0)},{GC.CollectionCount(1)},{GC.CollectionCount(2)}");
            Console.WriteLine(GC.GetTotalMemory(true));
        }

        public static class GCNotification {
            private static Action<Int32> s_gcDone = null; // The event's field
            public static event Action<Int32> GCDone {
                add {
                    // If there were no registered delegates before, start reporting notifications now
                    if (s_gcDone == null) {
                        new GenObject(0);
                        new GenObject(2);
                    }
                    s_gcDone += value;
                }
                remove { s_gcDone -= value; }
            }
            private sealed class GenObject {

                private Int32 m_generation;
                public GenObject(Int32 generation) {
                    m_generation = generation;
                }

                ~GenObject() { // This is the Finalize method
                               // If this object is in the generation we want (or higher), 
                               // notify the delegates that a GC just completed
                    if (GC.GetGeneration(this) >= m_generation) {
                        Volatile.Read(ref s_gcDone)?.Invoke(m_generation);
                    }
                    // Keep reporting notifications if there is at least one delegate registered,
                    // the AppDomain isn't unloading, and the process isn’t shutting down
                    if ((s_gcDone != null)
                    && !AppDomain.CurrentDomain.IsFinalizingForUnload()
                    && !Environment.HasShutdownStarted) {
                        // For Gen 0, create a new object; for Gen 2, resurrect the object 
                        // & let the GC call Finalize again the next time Gen 2 is GC'd
                        if (m_generation == 0) {
                            new GenObject(0);
                        } else {
                            GC.ReRegisterForFinalize(this);
                        }
                    } else {
                        /* Let the objects go away */
                    }
                }
            }
        }
    }
}
