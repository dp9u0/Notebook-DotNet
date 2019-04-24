using Common;
using System;
using System.Runtime.InteropServices;

namespace CLR {
    class GCHandlerRunner : Runner {
        protected override void RunCore() {
            fixedCode();
            fixedCode2();
            GCHandleNormal();
        }

        unsafe private static void fixedCode() {
            // Allocate a bunch of objects that immediately become garbage
            for (Int32 x = 0; x < 10000; x++) new Object();
            IntPtr originalMemoryAddress;
            Byte[] bytes = new Byte[1000]; // Allocate this array after the garbage objects
                                           // Get the address in memory of the Byte[]
            fixed (Byte* pbytes = bytes) {
                originalMemoryAddress = (IntPtr)pbytes;
            }
            // Force a collection; the garbage objects will go away & the Byte[] might be compacted
            GC.Collect();
            // Get the address in memory of the Byte[] now & compare it to the first address
            fixed (Byte* pbytes = bytes) {
                Console.WriteLine("The Byte[] did{0} move during the GC",
                (originalMemoryAddress == (IntPtr)pbytes) ? " not" : null);
            }
        }

        unsafe private static void fixedCode2() {
            // Allocate a bunch of objects that immediately become garbage
            for (Int32 x = 0; x < 10000; x++) new Object();
            IntPtr originalMemoryAddress;
            Byte[] bytes = new Byte[1000]; // Allocate this array after the garbage objects
                                           // Get the address in memory of the Byte[]
            fixed (Byte* pbytes = bytes) {
                originalMemoryAddress = (IntPtr)pbytes;
                // Force a collection; the garbage objects will go away & the Byte[] might be compacted
                GC.Collect();
                Console.WriteLine("The Byte[] did{0} move during the GC",
                (originalMemoryAddress == (IntPtr)pbytes) ? " not" : null);
            }
        }

        private static void GCHandleNormal() {
            for (Int32 x = 0; x < 10000; x++) new Object();
            IntPtr originalMemoryAddress;
            Byte[] bytes = new Byte[1000]; // Allocate this array after the garbage objects
                                           // Get the address in memory of the Byte[]
            var handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            originalMemoryAddress = handle.AddrOfPinnedObject();
            handle.Free();
            // Force a collection; the garbage objects will go away & the Byte[] might be compacted
            GC.Collect();
            var handle2 = GCHandle.Alloc(bytes, GCHandleType.Pinned);
            // Get the address in memory of the Byte[] now & compare it to the first address
            Console.WriteLine("The Byte[] did{0} move during the GC",
            (originalMemoryAddress == handle2.AddrOfPinnedObject()) ? " not" : null);
        }
    }
}
