using Common;
using System;
using System.Reflection.Emit;

namespace Type {
    public class BoxUnboxRunner : Runner {
        protected override void RunCore() {
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
    }
}
