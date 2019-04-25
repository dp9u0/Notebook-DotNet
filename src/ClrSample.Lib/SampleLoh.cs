using System;
using System.Collections.Generic;

namespace ClrSample.Lib {
    class SampleLoh : SampleBase {

        static LohPaload _payload = new LohPaload();
        public class LohPaload {

            private List<object> _objList = new List<object>();

            public void Alloc(int size) {
                var obj = new byte[size];
                obj[0] = 0xAA;
                obj[1] = 0xBB;
                obj[2] = 0xCC;
                _objList.Add(obj);
            }

            public void Release() {
                _objList.Clear();
            }

        }
        public override string Command => "Loh";

        public override string Usage => "Loh : Create Loh Object\n" +
            "Loh -r: Release All Object" +
            "Loh -s: Create Small Object";

        protected override void InvokeCore(params string[] args) {
            if (args.Length == 0) {
                _payload.Alloc(85001);
            } else {
                switch (args[0]) {
                    case "-r":
                        _payload.Release();
                        break;
                    case "-s":
                        _payload.Alloc(1234);
                        break;
                }
            }
        }
    }
}
