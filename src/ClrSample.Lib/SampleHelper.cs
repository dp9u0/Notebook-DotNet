using System;

namespace ClrSample.Lib {
    class SampleHelper : SampleBase {
        public override string Command => "U";

        public override string Usage => $"U -gc : gc \n" +
            $"U -gc -n: gc gen <n>\n";

        protected override void InvokeCore(params string[] args) {
            if (args.Length != 0) {

                switch (args[0]) {
                    case "-g":
                        GC.Collect();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
