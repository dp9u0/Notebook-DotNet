using System;

namespace ClrSample.Lib {
    class SampleAppDomain : SampleBase {

        private static int Count;

        public override string Command => "Ad";

        public override string Usage => "Ad \n";

        protected override void InvokeCore(params string[] args) {
            var ad = AppDomain.CreateDomain($"Ad {Count++}");
        }
    }
}
