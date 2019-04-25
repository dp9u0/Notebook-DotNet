using System;

namespace ClrSample.Lib {
    class SampleHelloWorld : SampleBase {

        public override string Command => "HW";

        public override string Usage => "HW :Print Hello World";

        protected override void InvokeCore(params string[] args) {
            Console.WriteLine("Hello World");
        }
    }
}
