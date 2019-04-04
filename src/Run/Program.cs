#region

using System;
using Type;

#endregion

namespace Run {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Hello World!");
            new AllDerivedFromObjectRunner().Run();
            new PrimitiveTypeRunner().Run();
            new ConvertRunner().Run();
        }
    }
}