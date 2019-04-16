
using System;
using Common;

namespace Type {
    public class ConvertRunner : Runner {

        protected override void RunCore() {
            ClassSecond value = new ClassFirst();
            ClassFirst value2 = (ClassFirst)new ClassSecond();
            Object value4 = new ClassFirst();
            ClassFirst value3 = (ClassFirst)value4;
        }

        public class ClassFirst {

            public static explicit operator ClassFirst(ClassSecond second) {
                Console.WriteLine("ClassSecond explicit convert to ClassFirst");
                return new ClassFirst();
            }
        }

        public class ClassSecond {
            public static implicit operator ClassSecond(ClassFirst first) {
                Console.WriteLine("ClassFirst implicit convert to ClassSecond");
                return new ClassSecond();
            }
        }
    }
}