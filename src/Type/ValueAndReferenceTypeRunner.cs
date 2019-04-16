#region

using System;
using Common;

#endregion

namespace Type {
    public class ValueAndReferenceTypeRunner : Runner {
        protected override void RunCore() {
            // IL_0001: newobj instance void Type.ValueAndReferenceTypeRunner/ClassType::.ctor()
            ClassType r1 = new ClassType();
            // IL_0009: initobj  Type.ValueAndReferenceTypeRunner/StructType
            // initobj 将位于指定地址的值类型的每个字段初始化为空引用或适当的基元类型的 0
            StructType v1 = new StructType();

            v1.x = 5; // Changed on stack 
            Console.WriteLine(r1.x); // Displays "5" 
            Console.WriteLine(v1.x); // Also displays "5" 
            // The left side of Figure 5-2 reflects the situation 
            // after the lines above have executed. 

            ClassType r2 = r1; // Copies reference (pointer) only 
            StructType v2 = v1; // Allocate on stack & copies members 
            r1.x = 8; // Changes r1.x and r2.x 
            v1.x = 9; // Changes v1.x, not v2.x 
            Console.WriteLine(r1.x); // Displays "8" 
            Console.WriteLine(r2.x); // Displays "8" 
            Console.WriteLine(v1.x); // Displays "9" 
            Console.WriteLine(v2.x); // Displays "5" 
        }

        /// <summary>
        ///     class
        /// </summary>
        public class ClassType {
            public Int32 x;
        }

        /// <summary>
        ///     struct
        /// </summary>
        public struct StructType {
            public Int32 x;
        }
    }
}