#region

using System;
using System.Reflection;
using System.Reflection.Emit;

#endregion

namespace RuntimeIL.NET {
    /// <summary>
    ///     Locala Locala_s的区别
    ///     ldloca.s 指令为使用局部变量 0 到 255 提供有效的编码。
    /// </summary>
    public class IL_005 {
        /// <summary>
        /// </summary>
        public static void Run() {
            //定义Assembly
            AssemblyBuilder builder = AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName("IL_005"),
                AssemblyBuilderAccess.RunAndSave);
            //定义Modlue
            ModuleBuilder moduleBuilder = builder.DefineDynamicModule("IL_005", "IL_005.exe");
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
            builder.Save("IL_005.exe");
        }

        private static void Emit(ILGenerator il) {
            int sum = 0;
            LocalBuilder local10 = null;
            LocalBuilder local255 = null;
            LocalBuilder local299 = null;

            for (int i = 0; i < 300; i++) {
                LocalBuilder local = il.DeclareLocal(typeof(int));
                if (i == 10) {
                    local10 = local;
                }
                if (i == 255) {
                    local255 = local;
                }
                if (i == 299) {
                    local299 = local;
                }
            }

            for (int i = 0; i < 300; i++) {
                sum += i;
                il.Emit(OpCodes.Ldc_I4, i);
                il.Emit(OpCodes.Stloc, i);
            }

            LocalBuilder sumLocal = il.DeclareLocal(typeof(int));
            il.Emit(OpCodes.Ldc_I4, sum);
            il.Emit(OpCodes.Stloc, sumLocal);

            #region local10

            il.Emit(OpCodes.Ldloca, local10);
            il.Emit(OpCodes.Call, typeof(int).GetMethod("ToString", Type.EmptyTypes));
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new[] {
                typeof(string)
            }));

            il.Emit(OpCodes.Ldloca_S, local10);
            il.Emit(OpCodes.Call, typeof(int).GetMethod("ToString", Type.EmptyTypes));
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new[] {
                typeof(string)
            }));

            #endregion

            #region local255

            il.Emit(OpCodes.Ldloca, local255);
            il.Emit(OpCodes.Call, typeof(int).GetMethod("ToString", Type.EmptyTypes));
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new[] {
                typeof(string)
            }));

            il.Emit(OpCodes.Ldloca_S, local255);
            il.Emit(OpCodes.Call, typeof(int).GetMethod("ToString", Type.EmptyTypes));
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new[] {
                typeof(string)
            }));

            #endregion

            #region local299

            il.Emit(OpCodes.Ldloca, local299);
            il.Emit(OpCodes.Call, typeof(int).GetMethod("ToString", Type.EmptyTypes));
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new[] {
                typeof(string)
            }));

            //System.InvalidOperationException: MSIL 指令无效，或者索引超出界限
            //il.Emit(OpCodes.Ldloca_S, local299);
            il.Emit(OpCodes.Ldloca_S, local10);
            il.Emit(OpCodes.Call, typeof(int).GetMethod("ToString", Type.EmptyTypes));
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new[] {
                typeof(string)
            }));

            #endregion

            #region sumLocal

            il.Emit(OpCodes.Ldloca, sumLocal);
            il.Emit(OpCodes.Call, typeof(int).GetMethod("ToString", Type.EmptyTypes));
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new[] {
                typeof(string)
            }));

            // System.InvalidOperationException: MSIL 指令无效，或者索引超出界限
            //il.Emit(OpCodes.Ldloca_S, sumLocal);
            il.Emit(OpCodes.Ldloca_S, local10);
            il.Emit(OpCodes.Call, typeof(int).GetMethod("ToString", Type.EmptyTypes));
            il.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new[] {
                typeof(string)
            }));

            #endregion

            il.Emit(OpCodes.Call, typeof(Console).GetMethod("ReadLine"));
            il.Emit(OpCodes.Ret);
        }

        private void Test() {
            for (int i = 0; i < 3333; i++) {
                int sumLocal = 2123;
                var test = sumLocal.ToString();
            }
            Console.WriteLine("OK");
        }
    }
}