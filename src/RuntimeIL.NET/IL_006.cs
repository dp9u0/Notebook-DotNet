#region

using System;
using System.Reflection;
using System.Reflection.Emit;

#endregion

namespace RuntimeIL.NET {
    public class IL_006 {
        public static void Run() {
            //1.构建程序集
            AssemblyName asmName = new AssemblyName("IL_006");
            AssemblyBuilder asmBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(asmName,
                AssemblyBuilderAccess.RunAndSave);

            //2.创建模块 
            ModuleBuilder mdlBldr = asmBuilder.DefineDynamicModule("IL_006", "IL_006.dll");

            //3.定义类, public class Add
            TypeBuilder typeBldr = mdlBldr.DefineType("Add", TypeAttributes.Public | TypeAttributes.BeforeFieldInit);

            //4. 定义属性和字段
            //4.1字段 FieldBuilder
            FieldBuilder fieldABuilder = typeBldr.DefineField("numA", typeof(int), FieldAttributes.Private);
            //fieldABuilder.SetConstant(0); 此处为副初始值, 这里可省略

            FieldBuilder fieldBBuilder = typeBldr.DefineField("numB", typeof(int), FieldAttributes.Private);

            //5.定义属性numA的get;set;方法
            PropertyBuilder propertyABuilder = typeBldr.DefineProperty("NumA", PropertyAttributes.None, typeof(int),
                null);

            //5.1 get方法
            MethodBuilder getPropertyABuilder = typeBldr.DefineMethod("getNumA",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, typeof(int),
                Type.EmptyTypes);
            GetPropertyIL(getPropertyABuilder, fieldABuilder);

            //5.2 set方法
            MethodBuilder setPropertyABuilder = typeBldr.DefineMethod("setNumA",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, typeof(void),
                new[] {
                    typeof(int)
                });
            SetPropertyIL(setPropertyABuilder, fieldABuilder);

            //5.3 绑定
            propertyABuilder.SetGetMethod(getPropertyABuilder);
            propertyABuilder.SetSetMethod(setPropertyABuilder);

            //6.定义属性numB的get;set;方法
            PropertyBuilder propertyBBuilder = typeBldr.DefineProperty("NumB", PropertyAttributes.None, typeof(int),
                null);
            MethodBuilder getPropertyBBuilder = typeBldr.DefineMethod("getNumB",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, typeof(int),
                Type.EmptyTypes);
            GetPropertyIL(getPropertyBBuilder, fieldBBuilder);

            MethodBuilder setPropertyBBuilder = typeBldr.DefineMethod("setNumB",
                MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.HideBySig, typeof(void),
                new[] {
                    typeof(int)
                });
            SetPropertyIL(setPropertyBBuilder, fieldBBuilder);

            propertyBBuilder.SetGetMethod(getPropertyBBuilder);
            propertyBBuilder.SetSetMethod(setPropertyBBuilder);

            //7.定义构造函数 ConstructorBuilder
            ConstructorBuilder constructorBuilder =
                typeBldr.DefineConstructor(MethodAttributes.Public | MethodAttributes.HideBySig,
                    CallingConventions.HasThis, new[] {
                        typeof(int), typeof(int)
                    });
            ILGenerator ctorIL = constructorBuilder.GetILGenerator();
            // numA = a;
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Ldarg_1);
            ctorIL.Emit(OpCodes.Stfld, fieldABuilder);
            //NumB = b;
            ctorIL.Emit(OpCodes.Ldarg_0);
            ctorIL.Emit(OpCodes.Ldarg_2);
            ctorIL.Emit(OpCodes.Stfld, fieldBBuilder);
            ctorIL.Emit(OpCodes.Ret);

            //8.定义方法 MethodBuilder
            MethodBuilder calcMethodBuilder = typeBldr.DefineMethod("Calc",
                MethodAttributes.Public | MethodAttributes.HideBySig, typeof(int), Type.EmptyTypes);
            ILGenerator calcIL = calcMethodBuilder.GetILGenerator();
            //加载私有字段numA
            calcIL.Emit(OpCodes.Ldarg_0);
            calcIL.Emit(OpCodes.Ldfld, fieldABuilder);
            //加载属性NumB
            calcIL.Emit(OpCodes.Ldarg_0);
            calcIL.Emit(OpCodes.Ldfld, fieldBBuilder);
            //相加并返回栈顶的值
            calcIL.Emit(OpCodes.Add);
            calcIL.Emit(OpCodes.Ret);

            //9.结果
            Type type = typeBldr.CreateType();
            int a = 2;
            int b = 3;
            object ob = Activator.CreateInstance(type, a, b);
            Console.WriteLine("The Result of {0} + {1} is {2}", type.GetProperty("NumA").GetValue(ob, null),
                type.GetProperty("NumB").GetValue(ob, null), ob.GetType().GetMethod("Calc").Invoke(ob, null));

            asmBuilder.Save("IL_006.dll");
            Console.ReadKey();
        }

        private static void GetPropertyIL(MethodBuilder getPropertyBuilder, FieldBuilder fieldBuilder) {
            //ILGenerator
            ILGenerator getAIL = getPropertyBuilder.GetILGenerator();
            getAIL.Emit(OpCodes.Ldarg_0); //this
            getAIL.Emit(OpCodes.Ldfld, fieldBuilder); //numA
            getAIL.Emit(OpCodes.Ret); //return numA
        }

        private static void SetPropertyIL(MethodBuilder setPropertyBuilder, FieldBuilder fieldBuilder) {
            //ILGenerator
            ILGenerator setAIL = setPropertyBuilder.GetILGenerator();
            //setAIL.Emit(OpCodes.Nop);   //这句可省略
            setAIL.Emit(OpCodes.Ldarg_0); //this
            setAIL.Emit(OpCodes.Ldarg_1); //value
            setAIL.Emit(OpCodes.Stfld, fieldBuilder); //numA = value;
            setAIL.Emit(OpCodes.Ret); //return;
        }
    }
}