using Common;
using System;
using System.Reflection;

namespace CLR {

    /// <summary>
    /// ClrRunner
    /// </summary>
    public class ClrRunner : Runner {
        protected override void RunCore() {
            new AllILCodeRunner().Run();
            //RunRunner<PInvokeRunner>();
            //RunRunner<HandleCollectorRunner>();
            RunRunner<ExceptionRunner>();
        }
    }
}
