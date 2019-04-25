using System;
using System.Threading;
using System.Threading.Tasks;

namespace HelloWorld {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello World");
            Console.ReadLine();
        }

        private void Call() {
            string input = "";
            Task.Factory.StartNew(() =>
            {
                while (input != "Q") {
                    Console.WriteLine("Hello World");
                    Thread.Sleep(1000);
                }
            });
            while (input != "Q") {
                input = Console.ReadLine();
            }
        }
    }
}
