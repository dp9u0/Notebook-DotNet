using Common;
using System;
using System.IO.Pipes;
using System.Text;
using System.Threading.Tasks;

namespace ThreadSample {

    class AsyncTaskRunner : Runner {

        protected override void RunCore() {

        }


        private static async void Caller() {
            string result = await IssueClientRequestAsync("", "");
        }
        private static async Task<string> IssueClientRequestAsync(String serverName, String message) {

            using (var pipe = new NamedPipeClientStream(serverName, "PipeName", PipeDirection.InOut,
            PipeOptions.Asynchronous | PipeOptions.WriteThrough)) {
                pipe.Connect(); // Must Connect before setting ReadMode
                pipe.ReadMode = PipeTransmissionMode.Message;
                // Asynchronously send data to the server
                Byte[] request = Encoding.UTF8.GetBytes(message);
                await pipe.WriteAsync(request, 0, request.Length);
                // Asynchronously read the server's response
                Byte[] response = new Byte[1000];
                Int32 bytesRead = await pipe.ReadAsync(response, 0, response.Length);
                return Encoding.UTF8.GetString(response, 0, bytesRead);
            } // Close the pipe
        }

        internal sealed class Type1 { }
        internal sealed class Type2 { }
        private static async Task<Type1> Method1Async() {
            /* Does some async thing that returns a Type1 object */
            return await Task.Run<Type1>(() => new Type1());
        }

        private static async Task<Type2> Method2Async() {
            /* Does some async thing that returns a Type2 object */
            return await Task.Run<Type2>(() => new Type2());
        }

        private static async Task<String> MyMethodAsync(Int32 argument) {
            Int32 local = argument;
            try {
                Type1 result1 = await Method1Async();
                for (Int32 x = 0; x < 3; x++) {
                    Type2 result2 = await Method2Async();
                }
            } catch (Exception) {
                Console.WriteLine("Catch");
            } finally {
                Console.WriteLine("Finally");
            }
            return "Done";
        }
    }
}
