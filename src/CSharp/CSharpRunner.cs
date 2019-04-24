using Common;

namespace CSharp {
    public class CSharpRunner : Runner {
        protected override void RunCore() {
            RunRunner<YieldRunner>();
            RunRunner<LambdaRunner>();
            //RunRunner<AsyncRunner>();
            RunRunner<LinqRunner>();
        }
    }
}
