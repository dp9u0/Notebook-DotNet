using Common;
using System;

namespace Type {
    public class PropertyRunner : Runner {
        protected override void RunCore() {
        }

        public class SomeType {
            private string _field1;
            private string _field2;

            public String Prop1 {
                get { return _field1; }
                set { _field1 = value; }
            }

            public String Prop2 {
                get;
                set;
            }
        }
    }
}
