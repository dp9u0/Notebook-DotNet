#region

using CLR;
using System;
using Type;

#endregion

namespace Run {
    class Program {
        static void Main() {
            RunTypeRunner();
            RunILRunner();
        }

        private static void RunTypeRunner() {
            new TypeRunner().Run() ;
        }

        private static void RunILRunner() {
            new ClrRunner().Run();
        }
    }
}