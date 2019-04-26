using System;

namespace ClrSample.Lib {
    class SampleHelper : SampleBase {
        public override string Command => "U";

        public override string Usage => $"U -gc : gc \n" +
            $"U -gc -n: gc gen <n>\n";

        protected override void InvokeCore(params string[] args) {
            if (Utils.Match("-g", 0, args)) {
                if (Utils.TryGetInt(out int gen, 1, args)) {
                    GC.Collect(gen);
                } else {
                    GC.Collect();
                }
            } else {

            }
        }
    }
}
