using Common;
using System;
using System.Reflection;

namespace CLR {
    class LoadAssemblyTwiceRunner : Runner {

        protected override void RunCore() {

            try {
                //var assembly = Assembly.ReflectionOnlyLoadFrom("Test.Lib.001.dll");
                var assembly = Assembly.LoadFrom("Test.Lib.002.dll");
                dynamic someType = assembly.CreateInstance("Test.Lib.SomeType");
                Console.WriteLine(someType.Version);
            } catch (Exception ex) {
                Console.WriteLine(ex);
            }
        }
    }
}
