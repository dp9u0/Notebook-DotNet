#region

using System;
using System.Reflection.Emit;

#endregion

namespace RuntimeIL.NET {
    public class IL_003 {
        public static void Run() {
            DynamicMethod method = Build();
            Action<int> action = (Action<int>) method.CreateDelegate(typeof(Action<int>));

            Console.WriteLine("-------------正常执行---------------");
            action.Invoke(10);
            Console.WriteLine("-------------异常执行---------------");
            action.Invoke(-10);
            Console.ReadKey();
        }

        private static DynamicMethod Build() {
            //定义一个动态方法
            DynamicMethod method = new DynamicMethod("Test", null, new[] {
                typeof(int)
            });
            ILGenerator IL = method.GetILGenerator();

            //定义标签
            Label label1 = IL.DefineLabel();
            Label loopLabel = IL.DefineLabel();
            Label endLabel = IL.DefineLabel();

            //定义本地变量
            IL.DeclareLocal(typeof(int));
            IL.DeclareLocal(typeof(int));

            IL.Emit(OpCodes.Ldc_I4_0);
            IL.Emit(OpCodes.Stloc_0); //sum = 0

            IL.Emit(OpCodes.Ldc_I4_1);
            IL.Emit(OpCodes.Stloc_1); // i = 1

            //Concat(string,int)
            IL.Emit(OpCodes.Ldstr, "enter number : num = ");
            IL.Emit(OpCodes.Ldarg_0);

            //实现方式一 装箱
            IL.Emit(OpCodes.Box, typeof(int)); //装箱
            var methodInfo = typeof(string).GetMethod("Concat", new[] {
                typeof(string), typeof(object)
            });
            IL.Emit(OpCodes.Call, methodInfo);
            IL.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new[] {
                typeof(string)
            }));

            IL.BeginExceptionBlock();
            IL.Emit(OpCodes.Ldarg_0); //num
            IL.Emit(OpCodes.Ldc_I4_1); // 1
            IL.Emit(OpCodes.Bge, label1); //num >= 1 -> label1, 注: try里面的跳转, 不能跳出try语句, 只能在内部

            IL.Emit(OpCodes.Ldstr, "num is less than 1");
            IL.Emit(OpCodes.Newobj, typeof(Exception).GetConstructor(new[] {
                typeof(string)
            })); //new Exception();
            IL.Emit(OpCodes.Throw); //throw

            IL.MarkLabel(label1);
            IL.Emit(OpCodes.Ldloc_1); //i
            IL.Emit(OpCodes.Ldarg_0); //num
            IL.Emit(OpCodes.Bgt, loopLabel); // i > num -> endLabel

            IL.Emit(OpCodes.Ldloc_0);
            IL.Emit(OpCodes.Ldloc_1);
            IL.Emit(OpCodes.Add);
            IL.Emit(OpCodes.Stloc_0); // sum += i;

            IL.Emit(OpCodes.Ldloc_1);
            IL.Emit(OpCodes.Ldc_I4_1);
            IL.Emit(OpCodes.Add);
            IL.Emit(OpCodes.Stloc_1); //i+=1;
            IL.Emit(OpCodes.Br_S, label1);

            IL.MarkLabel(loopLabel);
            IL.Emit(OpCodes.Ldstr, "executed successfully : Sum = ");
            IL.Emit(OpCodes.Ldloc_0);
            //实现方式二 调用Convert.ToString(int num)方法
            IL.Emit(OpCodes.Call, typeof(Convert).GetMethod("ToString", new[] {
                typeof(int)
            }));
            IL.Emit(OpCodes.Call, typeof(string).GetMethod("Concat", new[] {
                typeof(string), typeof(string)
            }));
            IL.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new[] {
                typeof(object)
            }));

            IL.MarkLabel(endLabel);
            IL.Emit(OpCodes.Nop);

            IL.BeginCatchBlock(typeof(Exception)); //catch
            IL.Emit(OpCodes.Callvirt, typeof(Exception).GetMethod("get_Message"));
            IL.Emit(OpCodes.Call, typeof(Console).GetMethod("WriteLine", new[] {
                typeof(string)
            }));
            IL.EndExceptionBlock(); //end

            IL.Emit(OpCodes.Ret);

            return method;
        }

        public static void Test(int num) {
            try {
                if (num < 1) {
                    throw new Exception("num is less than 1");
                }
                int sum = 0;
                for (int i = 1; i <= num; i++) {
                    sum += i;
                }
                Console.WriteLine("executed successfully : Sum = " + sum);
            } catch (Exception e) {
                Console.WriteLine("error happened : " + e.Message);
            }
        }
    }
}