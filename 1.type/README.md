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
      * [特殊方法](#%E7%89%B9%E6%AE%8A%E6%96%B9%E6%B3%95)
      * [方法参数](#%E6%96%B9%E6%B3%95%E5%8F%82%E6%95%B0)
    * [事件](#%E4%BA%8B%E4%BB%B6)
    * [属性](#%E5%B1%9E%E6%80%A7)
      * [索引器](#%E7%B4%A2%E5%BC%95%E5%99%A8)
  * [类型修饰符](#%E7%B1%BB%E5%9E%8B%E4%BF%AE%E9%A5%B0%E7%AC%A6)
    * [可访问性](#%E5%8F%AF%E8%AE%BF%E9%97%AE%E6%80%A7)
    * [继承与多态](#%E7%BB%A7%E6%89%BF%E4%B8%8E%E5%A4%9A%E6%80%81)
  * [泛型](#%E6%B3%9B%E5%9E%8B)
    * [逆变与协变](#%E9%80%86%E5%8F%98%E4%B8%8E%E5%8D%8F%E5%8F%98)
    * [泛型约束](#%E6%B3%9B%E5%9E%8B%E7%BA%A6%E6%9D%9F)
  * [接口](#%E6%8E%A5%E5%8F%A3)
  * [Number](#number)
  * [String](#string)
  * [Enum](#enum)
  * [Array](#array)
  * [Delegate](#delegate)
  * [Type Convert](#type-convert)
  * [类型格式化](#%E7%B1%BB%E5%9E%8B%E6%A0%BC%E5%BC%8F%E5%8C%96)
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

#### 特殊方法

* 实例构造器 实例构造方法
* 类型构造器 静态构造方法
* 操作符方法

```cs
public static SomeType operator +(SomeType first, SomeType second)
{
  return first;
}
```

* 转换操作符

```cs
public static implicit operator SomeType(int val)
{
  return new SomeType();
}

public static explicit operator SomeType(string val)
{
  return new SomeType();
}
```

* 扩展方法 : static(this)
* 分部方法 : 在 partial类中实现

#### 方法参数

* 可选参数和命名参数
* out ref : 按引用传递

```cs
string val = "111";
/* 0x000008B8 1200         IL_0008: ldloca.s  val*/
/* 0x000008BA 2821000006   IL_000A: call      instance void Type.MethodRunner3::RefMethd(string&)*/
RefMethd(ref val);
```

* params

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

C# 定义的属性实际会被编译成 `set_Property` 和 `get_Property` 两个方法.

并且根据需要创建 backField

[属性](../src/Type/TypeMember.PropertyRunner.cs)

#### 索引器

```cs
.property instance bool Item(int32 bitPos)
{
  // Token: 0x0600004F RID: 79 RVA: 0x00002A58 File Offset: 0x00000C58
  .get instance bool Type.PropertyRunner/BitArray::get_Item(int32)
  // Token: 0x06000050 RID: 80 RVA: 0x00002AA4 File Offset: 0x00000CA4
  .set instance void Type.PropertyRunner/BitArray::set_Item(int32, bool)
}
```

## 类型修饰符

### 可访问性

* public : 所有
* protected internal : protected or internal
* internal : 程序集
* private protected : private or (internal protected)
* protected : private + 派生类型
* private : 类型和嵌套类型中访问

### 继承与多态

* new : 覆盖
* override : 重写,重写基类的 virtual abstract 方法.
* overload : 方法名相同但是参数不同,重载

* abstract : 抽象方法,必须被重写
* virtual : 虚方法,可能被重写
* sealed : 不可派生/ sealed override (方法不可被派生类重写)

## 泛型

1. `ing DateTimeList = System.Collections.Generic.List<System.DateTime>;` 简化代码
2. 泛型代码爆炸 : 确定类型参数后JIT会对泛型编译为不同的类型参数组合生成不同的代码.这回导致泛型代码爆炸.
   1. 如果类型参数是引用类型,CLR会复用JIT代码.这是由于引用类型都是64位地址,具有类似的逻辑
   2. 但是如果是值类型则无法避免的会出现代码爆炸问题.

* 泛型接口
* 泛型委托
* 泛型方法 : 类型推断

### 逆变与协变

通过逆变和协变可以将类型相同但是类型参数不同的泛型进行转换.

```cs
Func<Object, ArgumentException> fn1 = null;
Func<String, Exception>fn2 = fn1; // No explicit cast is required here
Exception e = fn2("");
```

* Invariant 不变量
* Contra-variant 逆变量,in,作为参数等输入
* Covariant 协变量,out,作为返回值等输出

### 泛型约束

* 主要约束 : 主要约束可以是一个引用类型,class或者struct,如果指定一个引用类型,则实参必须是该类型或者该类型派生类型,class规定实参必须是一个引用类型.struct规定了参数必须是一个结构
* 次要约束 : 次要约束规定了参数必须实现所有次要约束中规定的接口
* 构造器约束 : 约束必须实现默认构造器
  
## 接口

* 显示隐式实现接口
* 实现的多个接口拥有相同的方法签名
* 基类 or 接口 : IS-A , CAN-DO

## Number

* System.SByte(sbyte)
* System.Int16(short)
* System.Int32(int)
* System.Int64(long)
* System.Byte(byte)
* System.UInt16(ushort)
* System.UInt32(uint)
* System.UInt64(ulong)
* IEEE 浮点数 : System.Single(float) System.Double(double)
* System.Decimal(decimal)
* SIMD : Vector2,Vector3,Vector4,Matrix3x2,Matrix4x4 等

## String

* 字符串是不可变的
* 字面量字符串拼接,编译器在编译阶段就会拼接好,但是如果是非字面量,每个拼接运算都需要在堆中申请新的对象.这样比较浪费,因此需要用 `StringBuilder`
* 另外一个不要使用字符串拼接(+ or Concat)的原因是: 值类型和字符串拼接会装箱.Concat() 参数都是Object类型.而 StringBuilder 定义了各种参数类型的 Append();
* String Intering

```cs
var str1 = "aaaa";
var str2 = "aaaa";
var str3 = "aa";
var str4 = str3 + str3;
var str5 = String.Intern(str3 + str3); // Intering
Console.WriteLine(Object.ReferenceEquals(str1, str2));// TRUE
Console.WriteLine(Object.ReferenceEquals(str4, str2));// FALSE
Console.WriteLine(Object.ReferenceEquals(str5, str2));// TRUE
```

CLR内部维护Hash表,用作字符留用,程序集加载时,会默认将所有字面量留用(`Intering`)

* StringBuilder : 通过 fixed代码 直接内存copy `string.wstrcpy(destPtr, valuePtr, valueLen);`

## Enum

## Array

* 数组转型 : 不存在的,请使用Array.Copy(). 而且 Copy 还可以自动调整宽度 `Int32[] -> Double[]`
* 所有数组隐式派生自 System.Array ,并且C#会默认实现 IList等接口 `FileStream [] fileStreams;`
* 不安全数组访问

## Delegate

delegate 关键字实际会创建一个委托类派生自 MulticastDelegate

```cs
.class nested public auto ansi sealed DelegateType
    extends [netstandard]System.MulticastDelegate
  {
    // Methods
    // Token: 0x0600003A RID: 58
    .method public hidebysig specialname rtspecialname
      instance void .ctor (
        object 'object',
        native int 'method'
      ) runtime managed
    {
    } // end of method DelegateType::.ctor

    // Token: 0x0600003C RID: 60
    .method public hidebysig newslot virtual
      instance class [netstandard]System.IAsyncResult BeginInvoke (
        object arg,
        class [netstandard]System.AsyncCallback callback,
        object 'object'
      ) runtime managed
    {
    } // end of method DelegateType::BeginInvoke

    // Token: 0x0600003D RID: 61
    .method public hidebysig newslot virtual
      instance object EndInvoke (
        class [netstandard]System.IAsyncResult result
      ) runtime managed
    {
    } // end of method DelegateType::EndInvoke

    // Token: 0x0600003B RID: 59
    .method public hidebysig newslot virtual
      instance object Invoke (
        object arg
      ) runtime managed
    {
    } // end of method DelegateType::Invoke

  } // end of class DelegateType
```

MulticastDelegate 中三个重要字段:

* _target : method 调用的this值
* _methodPtr : method
* _invocationList : 委托链

不要自己定义委托:

* Func<>
* Action<>
* Predicate<>

C# 对委托进行了简化:

* 无需定义委托对象
* 不需要定义回调(lambda)
* 局部变量可以访问(closure)

## Type Convert

类型转换分为下面几种情况,对于CLR来说,类型转换有两种情况 : castclass 这可以称为`铸形`.

另外一种情况是 conv,对数字进行转换.

C#通过转换操作符等方法扩展了类型转换的含义,实际还是通过方法调用的方式实现类型转换,这部分称为`强制`.

* 引用类型转换 : castclass 派生类->基类(接口), 基类(接口)->派生类
* 数值类型转换 : conv
* operator : Implicit Explicit ,自定义转换定义位置,A->B 定义在类型C中 : 用户定义的转换必须是转换成封闭类型，或者从封闭类型转换 (CS0556)
* IConvertible : 定义转换到Primitive类型的方法
* Convert : Primitive类型 之间互相转换
* TypeConverter : 提供一种将值的类型转换为其他类型以及访问标准值和子属性的统一方法

## 类型格式化

* IFormattable
  * public string ToString (string format, IFormatProvider formatProvider);
    * format : 格式说明符,例如 标准说明符'G', 自定义 '#,##0.0'
    * formatProvider : 用于获取当前区域性的 IFormatProvider 对象,例如数字格式化会GetFormat(typeof(NumberFormatInfo))
* IFormatProvider
  * System.Globalization.CultureInfo : 用于获取 CurrentCultureInfp 下的 DateTimeFormatInfo 和 NumberFormatInfo
  * System.Globalization.DateTimeFormatInfo : 定义了年月日等文本显示内容
  * System.Globalization.NumberFormatInfo : 定义了小数点,货币符号等
* ICustomFormatter : 通过 IFormatProvider.GetFormat(typeof(ICustomFormatter)) 获取提供自定义格式化器
  * Format(String, Object, IFormatProvider)

[属性](../src/Type/FormatRunner.cs)

## Attribute

[属性](../src/Type/AttributeRunner.cs)
