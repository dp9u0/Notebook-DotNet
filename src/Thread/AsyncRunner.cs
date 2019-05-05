using Common;
using System.IO.Pipes;
using System.Threading.Tasks;

namespace ThreadSample {
    class AsyncRunner : Runner {
        protected override void RunCore() {

        }

        private Task<string> AsyncTask(string value) {
            return Task.FromResult(value);
        }

        private async Task<string> SomeAsyncMethod() {
            await AsyncTask("1");
            await AsyncTask("2");
            await AsyncTask("3");
            return "return";
        }
    }
}
