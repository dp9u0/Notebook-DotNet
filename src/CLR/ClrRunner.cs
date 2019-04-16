using Common;

namespace CLR {

    /// <summary>
    /// ClrRunner
    /// </summary>
    public class ClrRunner : Runner {
        protected override void RunCore() {
            new AllILCodeRunner().Run();
        }
    }
}
