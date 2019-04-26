using Common;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace CLR {
    class UsingBindingHandleRunner : Runner {

        private const BindingFlags c_bf = BindingFlags.FlattenHierarchy | BindingFlags.Instance |
 BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        protected override void RunCore() {
            // Show size of heap before doing any reflection stuff
            Console.WriteLine("Before doing anything");
            // Build cache of MethodInfo objects for all methods in MSCorlib.dll
            List<MethodBase> methodInfos = new List<MethodBase>();
            foreach (Type t in typeof(Object).Assembly.GetExportedTypes()) {
                // Skip over any generic types
                if (t.IsGenericTypeDefinition) continue;
                MethodBase[] mb = t.GetMethods(c_bf);
                methodInfos.AddRange(mb);
            }
            // Show number of methods and size of heap after binding to all methods
            Console.WriteLine("# of methods={0:N0}", methodInfos.Count);
            Console.WriteLine("After building cache of MethodInfo objects");// Build cache of RuntimeMethodHandles for all MethodInfo objects
            List<RuntimeMethodHandle> methodHandles =
            methodInfos.ConvertAll<RuntimeMethodHandle>(mb => mb.MethodHandle);
            Console.WriteLine("Holding MethodInfo and RuntimeMethodHandle cache");
            GC.KeepAlive(methodInfos); // Prevent cache from being GC'd early
            methodInfos = null; // Allow cache to be GC'd now
            Console.WriteLine("After freeing MethodInfo objects");
            methodInfos = methodHandles.ConvertAll<MethodBase>(
            rmh => MethodBase.GetMethodFromHandle(rmh));
            Console.WriteLine("Size of heap after re-creating MethodInfo objects");
            GC.KeepAlive(methodHandles); // Prevent cache from being GC'd early
            GC.KeepAlive(methodInfos); // Prevent cache from being GC'd early
            methodHandles = null; // Allow cache to be GC'd now
            methodInfos = null; // Allow cache to be GC'd now
            Console.WriteLine("After freeing MethodInfos and RuntimeMethodHandles");
        }
    }
}
