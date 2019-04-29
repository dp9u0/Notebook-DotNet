using Common;

namespace ThreadSample {
    public class ThreadRunner : Runner {
        protected override void RunCore() {
            RunRunner<BarrierRunner>();
        }
    }
}
