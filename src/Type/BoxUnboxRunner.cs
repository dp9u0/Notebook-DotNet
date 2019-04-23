using Common;
using System;
using System.Reflection.Emit;
using System.Threading;

namespace Type {
    public class BoxUnboxRunner : Runner {
        protected override void RunCore() {
            BoxUnBox();
            ValueTypeBoxUnbox();
            PrimitiveBoxUnbox();
        }

        private void BoxUnBox() {
            {
                int x1 = 9;
                /* 0x0000027A 8C15000001  IL_0006: box System.Int32 */
                object o1 = x1; // boxing the int
                /* 0x00000281 A515000001  IL_000D: unbox.any System.Int32 */
                int x2 = (int)o1; // unboxing o1
            }
            try {
                object o1 = null;
                int x1 = (int)o1; // NullReferenceException
            } catch (Exception ex) {
                Console.WriteLine(ex.GetType().FullName + ":" + ex.Message);
            }
            try {
                object o1 = 16;
                short x1 = (short)(int)o1; // OK
                short x2 = (short)o1; // InvalidCastException
            } catch (Exception ex) {
                Console.WriteLine(ex.GetType().FullName + ":" + ex.Message);
            }
        }

        private void ValueTypeBoxUnbox() {
            var val = new SomeValue();
            Object val1 = val;
            Object val2 = val;
            Console.WriteLine(Object.ReferenceEquals(val1, val2));
            val.Run();// call(this somevalue)
            val.GetType();// call(this object)
            val.ToString();// callvirt
            Monitor.Enter(val);
            Monitor.Exit(val); // Error Not Same Object
        }
       
        private void PrimitiveBoxUnbox() {
            byte val0 = 1;
            val0.ToString();
            int val1 = 1;
            val1.ToString();
            long val2 = 1L;
            val2.ToString();
            float val3 = 3.333F;
            val3.ToString();
            double val4 = 4.444D;
            val4.ToString();
            decimal val5 = 5M;
            val5.ToString();
        }
        struct SomeValue {

            public int Filed;

            public long Field2;

            public void Run() {
            }
        }
    }
}
