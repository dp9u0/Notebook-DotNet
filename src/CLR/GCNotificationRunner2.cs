using Common;
using System;
using System.Collections.Generic;

namespace CLR {
    partial class GCNotificationRunner2 : Runner {

        // Variable for suspending work 
        // (such servicing allocated server requests)
        // after a notification is received and then 
        // resuming allocation after inducing a garbage collection.
        static bool bAllocate = false;

        // Collection for objects that  
        // simulate the server request workload.
        static List<byte[]> load = new List<byte[]>();

        protected override void RunCore() {
            try {
                bAllocate = true;
                GCNotification.GCDone += GCNotification_GCDone;
                try {
                    int lastCollCount = 0;
                    int newCollCount = 0;
                    while (true) {
                        if (bAllocate) {
                            load.Add(new byte[100000]);
                            newCollCount = GC.CollectionCount(2);
                            if (newCollCount != lastCollCount) {
                                Console.WriteLine($"Gen 2 GC count: {GC.CollectionCount(2)}");
                                Console.WriteLine($"After GC Memory Alloced {GC.GetTotalMemory(false)}");
                                lastCollCount = newCollCount;
                            }
                            // For ending the example (arbitrary).
                            if (newCollCount == 100) {
                                break;
                            }
                        }
                    }
                } catch (OutOfMemoryException) {
                    Console.WriteLine("Out of memory.");
                }
                GCNotification.GCDone -= GCNotification_GCDone;
                Console.WriteLine("Cancel for GC notification.");
            } catch (InvalidOperationException invalidOp) {
                Console.WriteLine("GC Notifications are not supported while concurrent GC is enabled.\n"
                    + invalidOp.Message);
            }
        }

        private void GCNotification_GCDone(int obj) {
            if (obj == 2) {
                Console.WriteLine($"Before GC Memory Alloced {GC.GetTotalMemory(false)}");
                bAllocate = false;
                load.Clear();
                bAllocate = true;
            }
        }
    }
}
