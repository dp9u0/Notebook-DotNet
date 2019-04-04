# CTS

本章节介绍类型系统

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

![Demo](../src/Type/AllDerivedFromObject.cs)