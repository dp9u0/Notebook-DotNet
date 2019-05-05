using Common;
using System;
using System.Threading;

namespace ThreadSample {
    class ExecutionContextRunner : Runner {

        static ThreadLocal<String> NameStore = new ThreadLocal<string>();

        protected override void RunCore() {
           
            try {
                ExecutionContext.Capture().GetObjectData(null, new System.Runtime.Serialization.StreamingContext());
            } catch (Exception ex) {
                Console.WriteLine(ex);
            }
            Console.ReadLine();
        }
    }
}
