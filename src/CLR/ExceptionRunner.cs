using Common;

using System;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;

namespace CLR {
    class ExceptionRunner : Runner {
        protected override void RunCore() {
            Demo2();
        }

        private static void Demo2() {
            // Force the code in the finally to be eagerly prepared
            RuntimeHelpers.PrepareConstrainedRegions();
            try {
                Console.WriteLine("In try");
            } finally {
                // Type2’s static constructor is implicitly called in here
                Type2.M();
            }
        }
        public class Type2 {
            static Type2() {
                Console.WriteLine("Type2's static ctor called");
            }

            [ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
            public static void M() {
                Console.WriteLine("Type2's M called");
            }
        }
    }
}
