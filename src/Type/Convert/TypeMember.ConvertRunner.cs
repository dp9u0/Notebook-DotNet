
using System;
using Common;

namespace Type {
    public class ConvertRunner : Runner {

        protected override void RunCore() {

        }

        private void ConvertOperator() {
            ClassSecond value = new ClassFirst();
            ClassFirst value2 = (ClassFirst)new ClassSecond();
            Object value4 = new ClassFirst();
            ClassFirst value3 = (ClassFirst)value4;
        }

        private void CastClass() {
            DerivedType derivedType1_1 = new DerivedType();
            BaseType base1 = derivedType1_1;
            DerivedType derivedType1_2 = (DerivedType)base1;

            DerivedType2 derivedType2_1 = new DerivedType2();
            BaseType base2 = derivedType2_1;
            DerivedType2 derivedType2_2 = (DerivedType2)base2;
        }

        private void ConvertPrimitiveValueType() {

            int int1 = 10;
            long long1 = 10L;
            float float1 = 10f;
            double double1 = 10d;

            int int2 = (int)long1;
            long long2 = int1;
            int int3 = (int)float1;
            double double2 = long1;
            long long3 = (long)double1;
        }

        public class ClassFirst {

            public static explicit operator ClassFirst(ClassSecond second) {
                Console.WriteLine("ClassSecond explicit convert to ClassFirst");
                return new ClassFirst();
            }

            public static implicit operator ClassSecond(ClassFirst first) {
                Console.WriteLine("ClassFirst implicit convert to ClassSecond");
                return new ClassSecond();
            }
        }

        public class ClassSecond {

        }

        public class BaseType { }

        public class DerivedType : BaseType { }

        public class DerivedType2 : BaseType { }
    }
}