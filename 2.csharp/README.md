# CSharp

* [CSharp](#csharp)
  * [dynamic](#dynamic)
  * [yield](#yield)
  * [async&await](#asyncawait)
  * [匿名方法和lambda表达式](#%E5%8C%BF%E5%90%8D%E6%96%B9%E6%B3%95%E5%92%8Clambda%E8%A1%A8%E8%BE%BE%E5%BC%8F)
  * [查询表达式/表达式树](#%E6%9F%A5%E8%AF%A2%E8%A1%A8%E8%BE%BE%E5%BC%8F%E8%A1%A8%E8%BE%BE%E5%BC%8F%E6%A0%91)

下面介绍一些C#实现的语法糖,实际是代替开发者生成大量代码,这些功能并不是CLR实现的,只是C#编译器提供的语法糖功能.

## dynamic

编译阶段生成代码,运行时反射获取方法/字段,并进行调用.

## yield

语法糖,生成一个 IEnumerable,内部维护状态.

[yield](../src/CSharp/YieldRunner.cs)

## async&await

[async&await](../src/CSharp/AsyncRunner.cs)

更多关于异步任务的内容,会在后面的async thread章节介绍

## 匿名方法和lambda表达式

lambda也好 匿名方法也好都是C#编译器在编译阶段生成类

匿名方法和lambda使用过程中会出现闭包的概念,其实本质也是声明一个内部类型持有对应的局部变量.

因此需要考虑一系列内存泄漏问题.

[匿名方法和lambda表达式](../src/CSharp/LambdaRunner.cs)

更多关于 lambda linq 表达式树的内容会在后面的章节介绍

## 查询表达式/表达式树

IEnumarable 的扩展方法是针对Func Action的
C# 对 IEnumarable 提供查询表达式语法支持:

```cs
var result = from t in enumarable select t.MyProperty;
// 会被编译为
var result = enumarable.Select(new Func<LinqRunnerTest.MyModel, string>(LinqRunnerTest.<>c.<>9.<Test001>b__3_0);
```

IQueryable 的扩展方法都是 Expression 表达式

对于 IQueryable 提供 提供查询表达式和简单语法的表达式树生成支持:

```cs
 var result2 = from t in queryable select t.MyProperty;
// 等价于
var result = queryable.Select(t => t.MyProperty);
// 会被编译为
ParameterExpression parameterExpression = Expression.Parameter(typeof(MyModel), "t");
var indexExpression = Expression.Property(parameterExpression, "MyProperty");
var lambda = Expression.Lambda<Func<MyModel, string>>(indexExpression, parameterExpression);
var result = queryable.Select(lambda);
```