#region

using System;
using System.Reflection;
using System.Reflection.Emit;

#endregion

namespace RuntimeIL.NET {
    /// <summary>
    /// </summary>
    public class IL_001 {
        /// <summary>
        /// </summary>
        public static void Run() {
            //定义Assembly
            AssemblyBuilder builder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("IL_001"),
                AssemblyBuilderAccess.RunAndSave);
            //定义Modlue
            ModuleBuilder moduleBuilder = builder.DefineDynamicModule("IL_001", "IL_001.exe");
            //创建Class:TestClass
            TypeBuilder typeBuilder = moduleBuilder.DefineType("TestClass", TypeAttributes.Public);
            //为TestClass创建静态方法 Begin
            MethodBuilder methodBuilder = typeBuilder.DefineMethod("Begin",
                MethodAttributes.Public | MethodAttributes.Static, null,
                null
            );
            //写入静态方法的IL
            ILGenerator ilGenerator = methodBuilder.GetILGenerator();
            Emit(ilGenerator);
            //设置程序集EntryPoint
            Type type = typeBuilder.CreateType();
            builder.SetEntryPoint(type.GetMethod("Begin"));
            builder.Save("IL_001.exe");
        }

        private static void Emit(ILGenerator il) {
            var sum = il.DeclareLocal(typeof(int));
            //push 1 
            il.Emit(OpCodes.Ldc_I4_1);
            //push 2
            il.Emit(OpCodes.Ldc_I4_2);
            //push 3
            il.Emit(OpCodes.Ldc_I4_3);
            //add
            il.Emit(OpCodes.Add);
            il.Emit(OpCodes.Stloc_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Ldloc_0);
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new[] {
                typeof(int)
            }));
            //add
            il.Emit(OpCodes.Add);
            //push string 
            il.Emit(OpCodes.Ldstr, "1+2+3=");
            //call Console.Write(string)
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("Write", new[] {
                typeof(string)
            }));
            //call Console.WriteLine(int)
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new[] {
                typeof(int)
            }));
            //call Console.ReadLine()
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("ReadLine"));
            il.Emit(OpCodes.Ret);
        }
    }
}