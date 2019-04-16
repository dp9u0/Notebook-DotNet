using Common;
using System;
using System.Reflection;
using System.Reflection.Emit;

namespace CLR {
    public class AllILCodeRunner : Runner {
        protected override void RunCore() {
            var publicStaticFileds = typeof(OpCodes).GetFields(BindingFlags.Static | BindingFlags.Public);
            //foreach (var opFiled in publicStaticFileds) {
            //    OpCode code = (OpCode)opFiled.GetValue(null);
            //    Console.WriteLine($"{code.Name},{(ushort)code.Value},{code.Size}");
            //}
        }
    }
}
