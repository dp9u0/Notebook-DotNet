using Common;
using System.Linq;
namespace Linq {
    public class LinqRunner : Runner {
        protected override void RunCore() {
            RunRunner<SimpleLinqToSQLRunner>();
        }
    }
}
