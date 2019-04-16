# CTS

本章节介绍类型系统.

* [CTS](#cts)
  * [类型概述](#%E7%B1%BB%E5%9E%8B%E6%A6%82%E8%BF%B0)
  * [常见类型](#%E5%B8%B8%E8%A7%81%E7%B1%BB%E5%9E%8B)
    * [Primitive](#primitive)
    * [引用类型与值类型](#%E5%BC%95%E7%94%A8%E7%B1%BB%E5%9E%8B%E4%B8%8E%E5%80%BC%E7%B1%BB%E5%9E%8B)
    * [box-unbox](#box-unbox)
    * [Equals ReferenceEquals GetHashCode](#equals-referenceequals-gethashcode)
    * [dynamic](#dynamic)
  * [类型成员](#%E7%B1%BB%E5%9E%8B%E6%88%90%E5%91%98)
    * [字段](#%E5%AD%97%E6%AE%B5)
    * [方法](#%E6%96%B9%E6%B3%95)
    * [事件](#%E4%BA%8B%E4%BB%B6)
    * [属性](#%E5%B1%9E%E6%80%A7)
  * [类型修饰符](#%E7%B1%BB%E5%9E%8B%E4%BF%AE%E9%A5%B0%E7%AC%A6)
    * [可访问性](#%E5%8F%AF%E8%AE%BF%E9%97%AE%E6%80%A7)
    * [继承与多态](#%E7%BB%A7%E6%89%BF%E4%B8%8E%E5%A4%9A%E6%80%81)
  * [接口](#%E6%8E%A5%E5%8F%A3)
  * [泛型](#%E6%B3%9B%E5%9E%8B)
  * [Number](#number)
  * [String](#string)
  * [Enum](#enum)
  * [Array](#array)
  * [Type Convert](#type-convert)
  * [类型格式化](#%E7%B1%BB%E5%9E%8B%E6%A0%BC%E5%BC%8F%E5%8C%96)
  * [Delegate](#delegate)
  * [Attribute](#attribute)

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

```cs
unchecked {
    // Start of checked block
    Byte b = 100;
    // add
    b = (Byte) (b + 200); // This expression is checked for overflow.
    Console.WriteLine(b);
} // End of checked block
```

### 引用类型与值类型

CLR 支持两种类型:引用类型和值类型,引用类型在堆中分配,值类型可能分配在栈中(局部变量),也可能作为引用类型的字段分配在堆中.

```cs
// IL_0001: newobj instance void Type.ValueAndReferenceTypeRunner/ClassType::.ctor()
ClassType r1 = new ClassType();
// IL_0009: initobj  Type.ValueAndReferenceTypeRunner/StructType
// initobj 将位于指定地址的值类型的每个字段初始化为空引用或适当的基元类型的 0
StructType s1 = new StructType();
// 如果使用下面声明,虽然 s0已经分配了空间,但是由于没有调用initobj初始化依旧会编译错误
// StructType s0;
// Int32 a = s0.x; // error CS0170: Use of possibly unassigned field 'x
```

[引用类型与值类型Demo](../src/Type/ValueAndReferenceTypeRunner.cs)

当满足以下条件可以声明为值类型否则建议引用类型:

* 较小(16bytes以下)或者较大但是不用作方法调用,仅在本地方法内部使用:值拷贝开销
* 不可变
* 不继承不派生

值类型赋值是字段赋值,引用类型仅仅是引用地址赋值.

可以控制值类型字段的布局:

```cs
[StructLayout(LayoutKind.Explicit)]
internal struct SomeValType2 {
    public SomeValType2(Byte b,Int16 x) {
        X = x;
        B = b;
    }
    [FieldOffset(0)] public readonly Byte B; // The  B and X fields overlap each
    [FieldOffset(0)] public readonly Int16 X; // other in instances of this type
}
// SomeValType2 type2 = new SomeValType2(8,256);
// Console.WriteLine(type2.X); // 264 = 256 + 8
```

### box-unbox

CLR中设计了值类型和引用类型.

以局部变量为例,访问变量的一个字段,值类型只需要一次访存(栈),引用类型需要两次(引用,堆).

因此值类型的存在是为了满足某些情况下不必要的性能开销.

但是如果方法调用等场景下,参数定义为 Object,但是传入值类型会发生什么?

参数定义为 Object 类型,需要传入的是个引用类型即变量的地址,但是值类型是按照值拷贝方式传递的,因此需要将值类型转换成引用类型(放到堆上)并且将转换成的引用类型传给参数,将值类型转换成引用类型的过程称为 `box` 即装箱.

将装箱的引用类型转换成值类型的操作称为拆箱 `unbox`.

```cs
int x1 = 9;
object o1 = x1; // boxing the int
int x2 = (int)o1; // unboxing o1
```

### Equals ReferenceEquals GetHashCode

`Object.Equals`(相等性) 可以被重载,默认实现为 `Object.ReferenceEquals` (同一性)

`ValueType` 则对 `Equals` 进行了重载,利用反射机制对每个字段进行 `Object.Equals` 调用.当然这是在 VlaueType 持有对象引用字段的情况下.

如果 `ValueType` 字段全部都是值类型,那么可以直接调用内存比较.

重写 `Equals` 就必须重写 `GetHashCode`,保证两者相等性一致.相等的对象拥有相同的`HashCode`.

### dynamic

C# 允许使用`dynamic`动态创建动态类型,编译器不需要确定变量的具体类型,运行期判断和获取值.

```cs
dynamic val = new DynamicType1();
String vf1 = val.Filed1;
Console.WriteLine(vf1);
val = new DynamicType2();
Console.WriteLine(val.Filed2);
```

编译阶段会生成将 dynamic 通过 CallSite 功能在运行阶段生成委托,可以通过生成的委托执行获取/设置字段,调用方法等操作.

```cs
object val = new DynamicRunner.DynamicType1();
if (DynamicRunner.<>o__3.<>p__1 == null)
{
  DynamicRunner.<>o__3.<>p__1 = CallSite<Func<CallSite, object, string>>.Create(Binder.Convert(CSharpBinderFlags.None, typeof(string), typeof(DynamicRunner)));
}
Func<CallSite, object, string> target = DynamicRunner.<>o__3.<>p__1.Target;
CallSite <>p__ = DynamicRunner.<>o__3.<>p__1;
if (DynamicRunner.<>o__3.<>p__0 == null)
{
  DynamicRunner.<>o__3.<>p__0 = CallSite<Func<CallSite, object, object>>.Create(Binder.GetMember(CSharpBinderFlags.None, "Filed1", typeof(DynamicRunner), new CSharpArgumentInfo[]
  {
    CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, null)
  }));
}
string vf = target(<>p__, DynamicRunner.<>o__3.<>p__0.Target(DynamicRunner.<>o__3.<>p__0, val));
Console.WriteLine(vf);
```

## 类型成员

### 字段

字段是数据的承载,常量,静态字段,实例字段等都可以归为字段,只是在创建和使用时行为有所不同.

常量处理很特殊,声明的常量在使用时,直接会在编译阶段就将值复制过去.

例如

```cs
/* 0x000007CC 7215010070   */
// IL_0008: ldstr     "asfasdf"
/* 0x000007D1 7D0D000004   */
// IL_000D: stfld     string Type.FieldRunner / SomeType::Field2
val.Field2 = SomeType.Field1; // Field1 is a constant
```

[字段Demo](../src/Type/TypeMember.FieldRunner.cs)

因此如果当前程序及引用另一个程序集的常量,如果更新了这个常量,当前程序集也需要重新编译,哪怕没有任何代码变化.

非常量字段分为类型(static)和实例(默认)两种.在CLR中会根据两种情况在不同的区域分配内存.在介绍CLR时会详细探讨这一点.

### 方法

方法是行为.用于改变字段.

调用方法时,需要传入参数,而CLR中实例方法调用会将实例作为第一个参数传入.

调用方法主要通过两个指令进行:

* call : 不做任何检查,一般用作静态方法调用,或者 this确定不为空的非虚方法调用.
* callvirt : 检查 this,如果是调用虚方法,需要调用实例实际类型的方法表中的方法.

callvirt 实际使用比看上去范围更广:不仅仅是调用虚方法时.

```cs
private void CallVirt() {
    SomeType val = new SomeType();
    // callvirt  instance void Type.MethodRunner/BaseType::MethodVirtual()
    val.MethodVirtual();
}

private void CallVirt2() {
    // callvirt  instance void Type.MethodRunner/BaseType::MethodVirtual()
    BaseType val = new SomeType();
    val.MethodVirtual();
}

private void CallStatic() {
    // call void Type.MethodRunner/BaseType::MethodStatic()
    BaseType.MethodStatic();
}

private void CallMethod() {
    //  NOTE: callvirt 可以检查 this 是否为 null
    // 哪怕 Method 不使用 this;
    // callvirt  instance void Type.MethodRunner/SomeType::Method()
    var val = new SomeType();
    val.Method();
}

private void CallMethod2() {
    // 这种情况下确定this不为空
    // call instance void Type.MethodRunner/SomeType::Method()
    new SomeType().Method();
}

public class BaseType {
    public static void MethodStatic() { }
    public virtual void MethodVirtual() { }
}

public class SomeType : BaseType {
    public void Method() { }
    public override void MethodVirtual() {
        // 虚方法中调用 base方法,防止递归调用
        // call instance void Type.MethodRunner/BaseType::MethodVirtual()
        base.MethodVirtual();
    }
}
```

CLR 中类型成员实际只有 字段和方法两种.

至于 C# 中的属性事件构造方法等都可以看做是C#的对CLR功能包装而提供的语法糖.

### 事件

通过反编译可以看到,事件类型(`EventHandler`)实际是一个 `MulticastDelegate` 的派生类.

通过 event 声明类型成员,会

1. 声明`EventHandler` 类型的私有字段.
2. 自动 addEvent 和 removeEvent,合并字段.
3. 播发事件实际调用的是  EventHandler 的实例方法 `Invoke()`;

这样就可以通过 C# 代码中的 运算符 `+=` `-=` 调用这两个方法了

[事件](../src/Type/TypeMember.EventRunner.cs)

### 属性

C# 定义的属性实际会被编译成 set_Property 和 getProperty 两个方法.

并且根据需要创建 backField

[属性](../src/Type/TypeMember.PropertyRunner.cs)

## 类型修饰符

### 可访问性

* public
* internal
* protected
* private

### 继承与多态

* overload
* virtual
* override
* new

## 接口

## 泛型

## Number

## String

## Enum

## Array

## Type Convert

* Cast
* Coercion

## 类型格式化

## Delegate

* Func<>
* Action<>
* Predicate<>

## Attribute
