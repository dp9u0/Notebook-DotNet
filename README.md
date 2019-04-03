# Notebook-DotNet

[Notebook系列](https://github.com/dp9u0/Notebook)

主要内容如下:

* [.NET 体系结构](./0.overview/README.md)

> 这个章节试图总览.NET体系,至少解答:
>
>> * 什么是 .NET ?
>> * .NET Core 与 .NET Framework 有什么区别? .NET Standard又是什么?
>> * CTS CLS FCL BCL 分别是什么意思?
>> * CLR/IL 是如何实现硬件无关的? Core CLR 是如何实现跨平台的?

* [CTS](./1.type/README.md)
  
  > 类型系统是CLR的核心,这一章节介绍如何定义类型以及类型的深层次的东西

  * 类型系统
    * 类
    * 结构
    * 枚举
    * 接口
    * 委托
  * 基础类型 引用类型 值类型
  * 类型成员
    * construct
    * Filed
    * Property
    * Method
    * Event
  * 可访问性与修饰符
    * 访问性
  * 泛型
  * 逆变与协变
  * Delegate Lambda
    * Func<>
    * Action<>
    * Predicate<>
  * 类型转换
  * 类型格式化
  * String
  * Attribute
  * Assembly
  * 互操作性
* [CSharp](./2.csharp/README.md)

  > .NET 中用的最多的是 C#,并且 C# 语言特性非常优秀.这里结合 C# 介绍 CLR 的 CLS.并介绍 C# 的一些独有的语言特性.

* [CLR](./3.clr/README.md)

  > CLR 实现不仅仅包括类型,本章节介绍 CLR 的一些其他细节

  * 垃圾回收
  * Runtime
  * AppDomain
  * IL
* [BCL](./4.bcl/README.md)

  > BCL 提供了大量的类库 例如集合,并行库,Linq等 这一章节将对这些丰富的类库进行选择介绍

  * 集合
  * Emit
  * 并发
  * LINQ
  * Expression

* Application

  > MS 提供了一系列 FCL 供开发者使用构建大型应用程序,这个章节介绍这些构建应用的类库: FCL

  * [ASP.NET](./5.asp.net/README.md)
  * [WCF](./6.wcf/README.md)
  * [WPF](./7.wpf/README.md)
  * [Entity Framework](./8.ef/README.md)