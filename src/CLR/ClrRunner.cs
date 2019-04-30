using Common;

namespace CLR {

    /// <summary>
    /// ClrRunner
    /// </summary>
    public class ClrRunner : Runner {
        protected override void RunCore() {
            //RunRunner<PInvokeRunner>();
            RunRunner<HandleCollectorRunner>();
            RunRunner<UsingBindingHandleRunner>();
        }
    }
}
