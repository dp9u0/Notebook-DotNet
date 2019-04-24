using Common;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CLR {
    class GCNotificationRunner : Runner {

        // Variable for continual checking in the 
        // While loop in the WaitForFullGCProc method.
        static bool checkForNotify = false;

        // Variable for suspending work 
        // (such servicing allocated server requests)
        // after a notification is received and then 
        // resuming allocation after inducing a garbage collection.
        static bool bAllocate = false;

        // Variable for ending the example.
        static bool finalExit = false;

        // Collection for objects that  
        // simulate the server request workload.
        static List<byte[]> load = new List<byte[]>();
        protected override void RunCore() {
            try {
                // Register for a notification. 
                GC.RegisterForFullGCNotification(1, 1);
                Console.WriteLine("Registered for GC notification.");
                GCNotification.GCDone += GCNotification_GCDone;
                checkForNotify = true;
                bAllocate = true;
                // Start a thread using WaitForFullGCProc.
                Thread thWaitForFullGC = new Thread(new ThreadStart(WaitForFullGCProc));
                thWaitForFullGC.Start();
                // While the thread is checking for notifications in
                // WaitForFullGCProc, create objects to simulate a server workload.
                try {
                    int lastCollCount = 0;
                    int newCollCount = 0;
                    while (true) {
                        //if (thWaitForFullGC.ThreadState != ThreadState.Running) {
                        //    thWaitForFullGC.Start();
                        //}
                        if (bAllocate) {
                            load.Add(new byte[10000]);
                            newCollCount = GC.CollectionCount(2);
                            if (newCollCount != lastCollCount) {
                                // Show collection count when it increases:
                                Console.WriteLine($"Gen 2 GC count: {GC.CollectionCount(2)}");
                                Console.WriteLine($"After GC Memory Alloced {GC.GetTotalMemory(false)}");
                                lastCollCount = newCollCount;
                            }
                            // For ending the example (arbitrary).
                            if (newCollCount == 100) {
                                finalExit = true;
                                checkForNotify = false;
                                break;
                            }
                        }
                    }
                } catch (OutOfMemoryException) {
                    Console.WriteLine("Out of memory.");
                }
                finalExit = true;
                checkForNotify = false;
                GC.CancelFullGCNotification();
                Console.WriteLine("Cancel for GC notification.");
            } catch (InvalidOperationException invalidOp) {
                Console.WriteLine("GC Notifications are not supported while concurrent GC is enabled.\n"
                    + invalidOp.Message);
            }
        }

        private void GCNotification_GCDone(int obj) {
            if (obj == 2) {
                Console.WriteLine($"....... Gen 2 GC");
            }
        }

        public static void OnFullGCApproachNotify() {
            //Console.WriteLine("Redirecting requests.");
            // Method that tells the request queuing  
            // server to not direct requests to this server. 
            bAllocate = false;
            // Method that provides time to 
            // finish processing pending requests. 
            load.Clear();
            // This is a good time to induce a GC collection
            // because the runtime will induce a full GC soon.
            // To be very careful, you can check precede with a
            // check of the GC.GCCollectionCount to make sure
            // a full GC did not already occur since last notified.
            GC.Collect();
            //Console.WriteLine("Induced a collection.");
        }


        public static void OnFullGCCompleteEndNotify() {
            // Method that informs the request queuing server
            // that this server is ready to accept requests again.
            bAllocate = true;
            //Console.WriteLine("Accepting requests again.");
        }

        public static void WaitForFullGCProc() {
            while (true) {
                try {
                    // CheckForNotify is set to true and false in Main.
                    while (checkForNotify) {
                        // Check for a notification of an approaching collection.
                        Console.WriteLine("GC.WaitForFullGCApproach()");
                        GCNotificationStatus s = GC.WaitForFullGCApproach();
                        Console.WriteLine($"Before GC Memory Alloced {GC.GetTotalMemory(false)}");
                        if (s == GCNotificationStatus.Succeeded) {
                            //Console.WriteLine("GC Notification raised.");
                            OnFullGCApproachNotify();
                        } else if (s == GCNotificationStatus.Canceled) {
                            //Console.WriteLine("GC Notification cancelled.");
                            // break;
                        } else {
                            // This can occur if a timeout period
                            // is specified for WaitForFullGCApproach(Timeout) 
                            // or WaitForFullGCComplete(Timeout)  
                            // and the time out period has elapsed. 
                            //Console.WriteLine("GC Notification not applicable.");
                            //  break;
                        }
                        // Check for a notification of a completed collection.
                        Console.WriteLine("GC.WaitForFullGCComplete()");
                        GCNotificationStatus status = GC.WaitForFullGCComplete();
                        if (status == GCNotificationStatus.Succeeded) {
                            //Console.WriteLine("GC Notification raised.");
                            OnFullGCCompleteEndNotify();
                        } else if (status == GCNotificationStatus.Canceled) {
                            //Console.WriteLine("GC Notification cancelled.");
                            // break;
                        } else {
                            // Could be a time out.
                            //Console.WriteLine("GC Notification not applicable.");
                            // break;
                        }
                    }
                    Thread.Sleep(500);
                    Console.WriteLine($"CheckForNotify: {checkForNotify}");
                    // FinalExit is set to true right before  
                    // the main thread cancelled notification.
                    if (finalExit) {
                        break;
                    }
                } catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
            Console.WriteLine($"--------------------------------Finished--------------------------------");
        }

    }
}
