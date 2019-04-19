#region

using System;
using System.Runtime.InteropServices;
using Common;

#endregion

namespace Type {
    public class ValueTypeLayoutRunner : Runner {
        protected override void RunCore() {
            SomeValType2 type2 = new SomeValType2(8, 256);
            Console.WriteLine(type2.X); // 264 = 256 + 8
        }

        // Let the CLR arrange the fields to improve 
        // performance for this value type. 
        [StructLayout(LayoutKind.Auto)]
        internal struct SomeValType {
            private readonly Byte m_b;
            private readonly Int16 m_x;
        } // The developer explicitly arranges the fields of this value type. 

        [StructLayout(LayoutKind.Explicit)]
        internal struct SomeValType2 {
            public SomeValType2(Byte b, Int16 x) {
                X = x;
                B = b;
            }
            [FieldOffset(0)] public readonly Byte B; // The  B and X fields overlap each
            [FieldOffset(0)] public readonly Int16 X; // other in instances of this type
        }
    }
}