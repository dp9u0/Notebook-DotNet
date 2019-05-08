#region

using System;
using System.Reflection;
using System.Reflection.Emit;

#endregion

namespace RuntimeIL.NET {
    /// <summary>
    ///     Ctor
    /// </summary>
    public class IL_002 {
        public static void Run() {
            //1.构建程序集
            AssemblyName asmName = new AssemblyName("IL_002");
            AssemblyBuilder asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName,
                AssemblyBuilderAccess.RunAndSave);

            //2.创建模块 
            //ModuleBuilder mdlBldr = asmBuilder.DefineDynamicModule("Test2");
            ModuleBuilder mdlBldr = asmBuilder.DefineDynamicModule("IL_002", "IL_002.dll");

            TypeBuilder typeBuilder = mdlBldr.DefineType("Test2", TypeAttributes.Class | TypeAttributes.Public);

            FieldBuilder fieldBiulder = typeBuilder.DefineField("_test", typeof(string), FieldAttributes.Private);

            PropertyBuilder propertyBuilder = typeBuilder.DefineProperty("Test", PropertyAttributes.None,
                typeof(string), null);

            MethodBuilder getMethod = typeBuilder.DefineMethod("getTest",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, typeof(string),
                Type.EmptyTypes);

            EmitGetMethodBuilderIL(getMethod, fieldBiulder);

            MethodBuilder setMethod = typeBuilder.DefineMethod("setTest",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, typeof(void),
                new[] {
                    typeof(string)
                });

            EmitSetMethodBuilderIL(setMethod, fieldBiulder);

            propertyBuilder.SetGetMethod(getMethod);

            propertyBuilder.SetSetMethod(setMethod);

            ConstructorBuilder ctor = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard,
                new[] {
                    typeof(string)
                });

            ILGenerator ctorIl = ctor.GetILGenerator();
            ctorIl.Emit(OpCodes.Ldarg_0);
            ctorIl.Emit(OpCodes.Ldarg_1);
            ctorIl.Emit(OpCodes.Stfld, fieldBiulder);
            ctorIl.Emit(OpCodes.Ret);
            Type type = typeBuilder.CreateType();
            object ob = Activator.CreateInstance(type, "Test1234");

            Console.WriteLine(type.GetProperty("Test", typeof(string)).GetValue(ob, null));
            type.GetProperty("Test", typeof(string)).SetValue(ob, "tttttttttttt", null);
            Console.WriteLine(type.GetProperty("Test", typeof(string)).GetValue(ob, null));

            asmBuilder.Save("IL_002.dll");
        }

        private static void EmitGetMethodBuilderIL(MethodBuilder getMethodBuilder, FieldBuilder fieldBuilder) {
            //ILGenerator
            ILGenerator getAIL = getMethodBuilder.GetILGenerator();
            getAIL.Emit(OpCodes.Ldarg_0); //this
            getAIL.Emit(OpCodes.Ldfld, fieldBuilder); //numA
            getAIL.Emit(OpCodes.Ret); //return numA
        }

        private static void EmitSetMethodBuilderIL(MethodBuilder setMethodBuilder, FieldBuilder fieldBuilder) {
            ILGenerator setAIL = setMethodBuilder.GetILGenerator();
            setAIL.Emit(OpCodes.Ldarg_0); //this
            setAIL.Emit(OpCodes.Ldarg_1); //value
            setAIL.Emit(OpCodes.Stfld, fieldBuilder); //numA = value;
            setAIL.Emit(OpCodes.Ret); //return;
        }


        public static void Test() {
            Program2 prog2 = new Program2();
        }

        #region Nested type: MyStruct

        private struct MyStruct {
            public int Age { get; set; }
        }

        #endregion

        #region Nested type: Person2

        public class Person2 {
            public string Name { get; set; }

            public int Age { get; set; }
        }

        #endregion

        #region Nested type: Program2

        private class Program2 {
            private double dou = 3.14;

            private Person2 person = new Person2 {
                Name = "1234",
                Age = 26
            };

            private string pro = "pro";

            private Random rand = new Random(DateTime.Now.Millisecond);

            private MyStruct st = new MyStruct {
                Age = 12434
            };

            public Program2() {
                dou = 3.14;
                person = new Person2 {
                    Name = "1234",
                    Age = 26
                };
                pro = "pro";
                rand = new Random(DateTime.Now.Millisecond);
                st = new MyStruct {
                    Age = 12434
                };
            }

            public Program2(string name) {
                dou = 3.14;
                person = new Person2 {
                    Name = name,
                    Age = 26
                };
                pro = "pro" + name;
                rand = new Random(DateTime.Now.Millisecond);
                st = new MyStruct {
                    Age = 12434
                };
            }
        }

        #endregion
    }
}