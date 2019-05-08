#region

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Reflection.Emit;

#endregion

namespace RuntimeIL.NET {
    public static class Test {
        public static void Run() {
            Console.WriteLine("------------------------------");
            Test001();
            Console.WriteLine("------------------------------");
            Test002();
            Console.WriteLine("------------------------------");
            Test003();
            Console.WriteLine("------------------------------");
            Test004();
            Console.WriteLine("------------------------------");
            Test005();
            Console.WriteLine("------------------------------");
            Test006();
            Console.WriteLine("------------------------------");
            Test007();
            Console.WriteLine("------------------------------");
            Test008();
            Console.WriteLine("------------------------------");
            Test009();
            Console.WriteLine("------------------------------");
            Test010();
            Console.WriteLine("==============================");
            Console.ReadLine();
        }

        [Description("")]
        private static void Test001() {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(int), "x");
            Expression<Func<int, int>> expr =
                Expression.Lambda<Func<int, int>>(
                    Expression.Add(parameterExpression, Expression.Constant(1, typeof(int))), parameterExpression);
            Func<int, int> func = expr.Compile();
            Console.WriteLine(expr.ToString());
            Console.WriteLine(func(123123));
            expr.SaveToAssembly("Test001", typeof(int), new[] {
                typeof(int)
            });
        }

        private static void Test002() {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(int), "x");
            Expression<Func<int, int>> expr =
                Expression.Lambda<Func<int, int>>(
                    Expression.Add(parameterExpression, Expression.Constant(1, typeof(int))), parameterExpression);
            Console.WriteLine(expr.ToString());
            Expression<Func<int, int>> lambdaExpr = expr;
            Console.WriteLine(lambdaExpr.Body);
            Console.WriteLine(lambdaExpr.ReturnType.ToString());
            foreach (ParameterExpression parameter in lambdaExpr.Parameters) {
                Console.WriteLine("Name:{0}, Type:{1}, ", parameter.Name, parameter.Type);
            }
            lambdaExpr.SaveToAssembly("Test002", typeof(int), new[] {
                typeof(int)
            });
        }

        private static void Test003() {
            LoopExpression loop = Expression.Loop(Expression.Call(typeof(Console).GetMethod("WriteLine", new[] {typeof(string)
            }) ?? throw new InvalidOperationException(), Expression.Constant("Hello")));
            BlockExpression block = Expression.Block(loop);
            Expression<Action> lambdaExpression = Expression.Lambda<Action>(block, Array.Empty<ParameterExpression>());
            lambdaExpression.SaveToAssembly("Test003", null, null);
        }

        private static void Test004() {
            ConstantExpression left = Expression.Constant(1);
            ConstantExpression right = Expression.Constant(2);
            BinaryExpression addExpression = Expression.Add(left, right);
            //����һ��LabdaExpression
            LambdaExpression lambdaE = Expression.Lambda(addExpression, Array.Empty<ParameterExpression>());
            lambdaE.SaveToAssembly("Test004_0", typeof(int), null);
            //����һ������ʽ��
            Expression<Func<int>> lambdaExpression = Expression.Lambda<Func<int>>(addExpression,
                Array.Empty<ParameterExpression>());
            lambdaExpression.SaveToAssembly("Test004_1", typeof(int), null);
        }

        [Description("Expression.Loop")]
        private static void Test005() {
            LabelTarget labelBreak = Expression.Label("label");
            ParameterExpression loopIndex = Expression.Parameter(typeof(int), "index");
            BlockExpression block = Expression.Block(new[] {
                    loopIndex
                }, Expression.Assign(loopIndex, Expression.Constant(1)),
                Expression.Loop(
                    Expression.IfThenElse(Expression.LessThanOrEqual(loopIndex, Expression.Constant(10)),
                        Expression.Block(Expression.Call(null, typeof(Console).GetMethod("WriteLine", new[] {
                            typeof(string)
                        }) ?? throw new InvalidOperationException(), Expression.Constant("Hello")), Expression.PostIncrementAssign(loopIndex)),
                        Expression.Break(labelBreak)), labelBreak));
            Expression<Action> lambdaExpression = Expression.Lambda<Action>(block, Array.Empty<ParameterExpression>());
            lambdaExpression.SaveToAssembly("Test005", null, null);
        }


        private static void Test006() {
            LabelTarget returnTarget = Expression.Label(typeof(int));
            LabelExpression returnLabel = Expression.Label(returnTarget, Expression.Constant(998, typeof(int)));
            ParameterExpression inParam1 = Expression.Parameter(typeof(int));
            ParameterExpression inParam2 = Expression.Parameter(typeof(int));
            ParameterExpression inParam3 = Expression.Parameter(typeof(int));
            BlockExpression block3 = Expression.Block(
                new[] {inParam1, inParam2},
                Expression.AddAssign(inParam2, Expression.Constant(67677)),
                Expression.IfThen(
                    Expression.GreaterThan(inParam3, Expression.Constant(999)),
                    Expression.Block(
                        Expression.AddAssign(inParam3, Expression.Constant(10)),
                        Expression.Return(returnTarget, inParam3))),
                Expression.IfThen(
                    Expression.Equal(inParam3, Expression.Constant(962)),
                    Expression.Block(
                        Expression.AddAssign(inParam1, Expression.Constant(112340)),
                        Expression.Return(returnTarget, inParam1))),
                returnLabel);
            /*             
        int num2;
		num2 += 67677;
		if (num > 999)
		{
			num += 10;
			return num;
		}
		if (num == 962)
		{
			int num3;
			num3 += 112340;
			return num3;
		}
		return 998;
        */
            var lambdaExpression = Expression.Lambda<Func<int, int>>(block3, inParam3);
            lambdaExpression.SaveToAssembly("Test006", typeof(int), new[] {typeof(int)});
        }

        [Description("TypeBinaryExpression:Expression.TypeIs")]
        private static void Test007() {
            ParameterExpression inputParamExp = Expression.Parameter(typeof(object), "input");
            TypeBinaryExpression typeBinaryExpression = Expression.TypeIs(inputParamExp, typeof(int));
            var lambdaExr = Expression.Lambda<Func<object, bool>>(typeBinaryExpression, inputParamExp);
            lambdaExr.SaveToAssembly("Test007", typeof(bool), new[] {typeof(object)});
        }

        [Description("IndexExpression")]
        private static void Test008() {
            ParameterExpression arrayExpr = Expression.Parameter(typeof(int[]), "array");
            ParameterExpression indexExpr = Expression.Parameter(typeof(int), "index");
            ParameterExpression valueExpr = Expression.Parameter(typeof(int), "value");
            IndexExpression arrayAccessExpr = Expression.ArrayAccess(
                arrayExpr,
                indexExpr
            );
            Expression<Func<int[], int, int, int>> lambdaExpr = Expression.Lambda<Func<int[], int, int, int>>(
                Expression.Assign(arrayAccessExpr, Expression.Add(arrayAccessExpr, valueExpr)),
                arrayExpr,
                indexExpr,
                valueExpr
            );
            Console.WriteLine(lambdaExpr.Compile().Invoke(new[] {1, 4, 12, 45, 125, 15}, 4, 2));
            lambdaExpr.SaveToAssembly("Test008", typeof(int), new[] {typeof(int[]), typeof(int), typeof(int)});
        }

        [Description("New ��ֵ��")]
        private static void Test009() {
            ParameterExpression dicLocVar = Expression.Parameter(typeof(Dictionary<int, string>), "dic");
            ParameterExpression intParam = Expression.Parameter(typeof(int), "key");
            ParameterExpression stringParm = Expression.Parameter(typeof(string), "value");
            ParameterExpression defaultDic = Expression.Parameter(typeof(Dictionary<int, string>), "default");
            ParameterExpression defaultKey = Expression.Parameter(typeof(int), "defaultKey");
            NewExpression newDictionaryExpression = Expression.New(typeof(Dictionary<int, string>));
            LabelTarget returnTarget = Expression.Label(typeof(Dictionary<int, string>), "return");
            LabelExpression returnLabel = Expression.Label(returnTarget, newDictionaryExpression);
            var block = Expression.Block(new[] {defaultKey},
                Expression.Assign(defaultKey, Expression.Constant(1, typeof(int))),
                Expression.IfThenElse(
                    Expression.Call(defaultDic, typeof(Dictionary<int, string>).GetMethod("ContainsKey") ?? throw new InvalidOperationException(), defaultKey),
                    Expression.Return(returnTarget, defaultDic, typeof(Dictionary<int, string>)),
                    Expression.Block(
                        new[] {dicLocVar},
                        Expression.Assign(defaultKey, Expression.Constant(2, typeof(int))),
                        Expression.Assign(dicLocVar, newDictionaryExpression),
                        Expression.Call(dicLocVar, typeof(Dictionary<int, string>).GetMethod("Add") ?? throw new InvalidOperationException(), intParam,
                            stringParm),
                        Expression.Call(dicLocVar, typeof(Dictionary<int, string>).GetMethod("Add") ?? throw new InvalidOperationException(), defaultKey,
                            Expression.Constant("defalut", typeof(string))),
                        Expression.Return(returnTarget, dicLocVar, typeof(Dictionary<int, string>)))
                ),
                returnLabel
            );

            var expressTDelegate =
                Expression.Lambda<Func<int, string, Dictionary<int, string>, Dictionary<int, string>>>(block, intParam,
                    stringParm, defaultDic);
            expressTDelegate.SaveToAssembly("Test009", typeof(Dictionary<int, string>),
                new[] {typeof(int), typeof(string), typeof(Dictionary<int, string>)});
        }

        [Description("InvocationExpression ������ Call")]
        private static void Test010() {
            Expression<Func<int, int, bool>> largeSumTest = (num1, num2) => num1 + num2 > 1000;
            //Invoke �е���������,Ӧ��һ�� Expression/Expression<Func/Action>
            InvocationExpression invocationExpression = Expression.Invoke(
                largeSumTest,
                Expression.Constant(539),
                Expression.Constant(281));
            var expressTDelegate = Expression.Lambda<Func<bool>>(invocationExpression);
            expressTDelegate.SaveToAssembly("Test010", typeof(bool), null);
        }

        private static void SaveToAssembly(this LambdaExpression lambdaExpr, string assemblyName, Type returnType,
            Type[] paramTypes) {
            AssemblyBuilder assemblyBuilder =
                AppDomain.CurrentDomain.DefineDynamicAssembly(new AssemblyName(assemblyName),
                    AssemblyBuilderAccess.RunAndSave);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(assemblyName, assemblyName + ".dll");
            TypeBuilder typeBuilder = moduleBuilder.DefineType(assemblyName + "Class", TypeAttributes.Public);
            MethodBuilder methodBuilder = typeBuilder.DefineMethod(assemblyName + "Method",
                MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Static, returnType, paramTypes);
            lambdaExpr.CompileToMethod(methodBuilder);
            typeBuilder.CreateType();
            assemblyBuilder.Save(assemblyName + ".dll");
        }
    }

    public class MyModel {
        public string Name { get; set; }

        public int Age { get; set; }

        public string MyProperty { get; set; }
    }
}