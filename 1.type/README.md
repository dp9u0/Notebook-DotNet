# CTS

本章节介绍类型系统.

* [CTS](#cts)
  * [类型概述](#%E7%B1%BB%E5%9E%8B%E6%A6%82%E8%BF%B0)
  * [常见类型](#%E5%B8%B8%E8%A7%81%E7%B1%BB%E5%9E%8B)
    * [Primitive](#primitive)
    * [ReferenceType](#referencetype)
    * [ValueType](#valuetype)
    * [box-unbox](#box-unbox)
    * [dynamic](#dynamic)

## 类型概述

CLR 类型系统包括

* 类(`class`)
* 结构(`struct`)
* 枚举(`enum`)
* 委托(`delegate`)

当然还有个特殊的类型或者说不是类型的类型:

* 接口(`interface`)

所有的类型(`type`) 都派生自 `System.Object`,包括通过以下关键字声明的类型 : `class` `enum` `delegate` `struct`

> 如果不把 interface 看做类型而是看做接口的话...

当声明一个委托时,

```cs
public delegate object DelegateType(object arg);
```

所对应的IL代码声明

```il
.class public auto ansi sealed Type.DelegateType extends [netstandard]System.MulticastDelegate
````

可见DelegateType 派生自 System.MulticastDelegate(当然System.MulticastDelegate也派生自 System.Object),但是如果使用声明类的语法声明委托,编译器会报错(无法从特殊类派生)...

```cs
public class DelegateType : MulticastDelegate { }
```

包括 `enum` `delegate` `struct` 都是这样,只能通过关键字而无法通过派生语法声明.

这仅仅是 `C#`编译器的限制,并非 `CLR/CTS`的限制,看IL代码,发现除了声明派生类,还需要创建一些特殊字段,特殊方法等.

至于原因仅仅猜测这些类型比较特殊需要通过一些特殊字段使得CLR可以特殊对待吧..

[Demo](../src/Type/AllDerivedFromObjectRunner.cs)

## 常见类型

### Primitive

对比下面两种变量声明方式:

```cs
int value;
Int32 value2 = new Int32();
```

这种被编译器支持的类型简写被称为 Primitive Type.

> Primitive Type 的定义与 Javascript 定义有所不同.

[Demo](../src/Type/PrimitiveTypeRunner.cs)

* Primitive使用的观点问题. CLR via C# 上 ,建议使用FCL类型名称,完全不要使用基元类型,理由如下:
  
1. C#的string（关键字）映射到System.String（一个FCL类型）,所以两者没有区别.类似的,一些开发人员说如果在32位机器上运行int代表32位整数,在64位机器上跑代表64位整数,这个说法不正确,int是映射到System.Int32的,永远是32位整数,如果使用FCL类型,就不会有这样的误解了.
2. C#的long映射到Sytem.Int64,但是C++/CLI的long是System.Int32.习惯一种语言的人看另外一种语言时候容易误解代码意图.而且很多语言甚至不把long当成关键字.
3. FCL都将类型名称作为方法名的一部分.例如:BinaryReader类型包含ReadBoolean,ReadInt32,ReadSingle等,Convert类型包含ToBoolean,ToInt32,ToSingle,如以下float那一行代码很不容易理解.
BinaReader br=new BinaryReader();
float val=br.ReadSingle();
Single val=br.ReadSingle();
4. 平时只用C#的程序员经常忘记还可以使用其他语言写面向CLR的代码.比如FCL团队向库里引入了像Array的GetLongLength的方法,该方法返回Int64值,在C#中是long,但在其他语言中不是,再比如:System.Linq.Enumerable的LongCount方法.

* Primitive 某些情况下编译器会对值进行转换

```cs
Int32 i = 5; // cast from Int32 to Int32
Int64 l = i; // cast from Int32 to Int64
Single s = i; // cast from Int32 to Single
```

* check uncheck

### ReferenceType

### ValueType

### box-unbox

### dynamic