using Common;
using System;
using System.Dynamic;

namespace Type {


    public class DynamicRunner : Runner {

        public class DynamicType1 {
            public DynamicType1() {
                Filed1 = "DynamicType1";
            }
            public string Filed1;
        }

        public class DynamicType2 {
            public DynamicType2() {
                Filed2 = "DynamicType2";
            }
            public string Filed2;
        }

        public class DynamicType3 : DynamicObject {
            public DynamicType3() {

            }

        }

        protected override void RunCore() {
            dynamic val = new DynamicType1();
            String vf1 = val.Filed1;
            Console.WriteLine(vf1);

            val = new DynamicType2();
            Console.WriteLine(val.Filed2);

            val = new DynamicType3();
            val.Filed3 = "DynamicType3";
            Console.WriteLine(val.Filed3);
        }
    }


}
