#region

using CLR;
using System;
using Type;

#endregion

namespace Run {
    class Program {
        static void Main(string[] args) {
            //RunTypeRunner();
            RunILRunner();
        }

        private static void RunTypeRunner() {
            new AllDerivedFromObjectRunner().Run();
            new PrimitiveTypeRunner().Run();
            new ValueAndReferenceTypeRunner().Run();
            new ValueTypeLayoutRunner().Run();
            new ConvertRunner().Run();
            new BoxUnboxRunner().Run();
        }

        private static void RunILRunner() {
            new AllILCodeRunner().Run();
        }
    }
}