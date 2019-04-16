using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Type {
    class FieldRunner : Runner {
        protected override void RunCore() {
            var val = new SomeType();
            /* 0x000007CC 7215010070   */
            // IL_0008: ldstr     "asfasdf"
            /* 0x000007D1 7D0D000004   */
            // IL_000D: stfld     string Type.FieldRunner / SomeType::Field2
            val.Field2 = SomeType.Field1; // Field1 is a constant
        }

        public class SomeType {
            public const string Field1 = "asfasdf";
            public string Field2;
        }
    }
}
