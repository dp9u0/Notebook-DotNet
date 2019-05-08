# BCL

* [BCL](#bcl)
  * [Collection](#collection)
  * [Linq](#linq)
  * [Emit](#emit)
  * [Expression](#expression)
  * [CallSite](#callsite)

## Collection

[选择集合类](https://docs.microsoft.com/zh-cn/dotnet/standard/collections/selecting-a-collection-class)

* 是否需要顺序列表（其中通常在检索元素值后就将该元素丢弃）?
  * 在需要的情况下,如果需要先进先出 (FIFO) 行为,请考虑使用 `Queue` 类或 `Queue<T>` 泛型类. 如果需要后进先出 (LIFO) 行为,请考虑使用 `Stack` 类或 `Stack<T>` 泛型类. 若要从多个线程进行安全访问,请使用并发版本（`ConcurrentQueue<T>` 和 `ConcurrentStack<T>`）.
  * 如果不需要,请考虑使用其他集合.
* 是否需要以特定顺序（如先进先出、后进先出或随机）访问元素?
  * Queue 类和 `Queue<T>` 或 `ConcurrentQueue<T>` 泛型类提供先进先出访问. 有关详细信息,请参阅何时使用线程安全集合.
  * Stack 类和 `Stack<T>` 或 `ConcurrentStack<T>` 泛型类提供后进先出的访问. 有关详细信息,请参阅何时使用线程安全集合.
  * `LinkedList<T>` 泛型类允许从开头到末尾或从末尾到开头的顺序访问.
* 是否需要按索引访问每个元素?
  * `ArrayList` 和 `StringCollection` 类以及 `List<T>` 泛型类按从零开始的元素索引提供对其元素的访问.
  * `Hashtable、SortedList、ListDictionary` 和 `StringDictionary` 类以及 `Dictionary<TKey,TValue>` 和 `SortedDictionary<TKey,TValue>` 泛型类按元素的键提供对其元素的访问.
  * `NameObjectCollectionBase` 和 `NameValueCollection` 类以及 `KeyedCollection<TKey,TItem>` 和 `SortedList<TKey,TValue>` 泛型类按从零开始的元素索引或元素的键提供对其元素的访问.
* 是否每个元素都包含一个值、一个键和一个值的组合或一个键和多个值的组合?
  * 一个值：使用任何基于 `IList` 接口或 `IList<T>` 泛型接口的集合.
  * 一个键和一个值：使用任何基于 `IDictionary` 接口或 `IDictionary<TKey,TValue>` 泛型接口的集合.
  * 带有嵌入键的值：使用 `KeyedCollection<TKey,TItem>` 泛型类.
  * 一个键和多个值：使用 `NameValueCollection` 类.
* 是否需要以与输入方式不同的方式对元素进行排序?
  * `Hashtable` 类按其哈希代码对其元素进行排序.
  * `SortedList` 类以及 `SortedList<TKey,TValue>` 和 `SortedDictionary<TKey,TValue>` 泛型类按键对元素进行排序. 排序顺序的依据为,实现 `SortedList` 类的 `IComparer` 接口和实现 SortedList<TKey,TValue> 和 `SortedDictionary<TKey,TValue>` 泛型类的 `IComparer<T>` 泛型接口. 在这两种泛型类型中,虽然 `SortedDictionary<TKey,TValue>` 的性能优于 `SortedList<TKey,TValue>`,但 `SortedList<TKey,TValue>` 占用的内存更少.
  * ArrayList 提供了一种 Sort 方法,此方法采用 `IComparer` 实现作为参数. 其泛型对应项（`List<T>` 泛型类）提供一种 `Sort` 方法,此方法采用 `IComparer<T>` 泛型接口的实现作为参数.
* 是否需要快速搜索和信息检索?
  * 对于小集合（10 项或更少）,`ListDictionary` 速度比 `Hashtable` 快. `Dictionary<TKey,TValue>` 泛型类提供比 `SortedDictionary<TKey,TValue>` 泛型类更快的查找. 多线程的实现为 `ConcurrentDictionary<TKey,TValue>`. `ConcurrentBag<T>` 为无序数据提供快速的多线程插入. 有关这两种多线程类型的详细信息,请参阅何时使用线程安全集合.
* 是否需要只接受字符串的集合?
  * `StringCollection`（基于 `IList`）和 `StringDictionary`（基于 `IDictionary`）位于 `System.Collections.Specialized` 命名空间.
  * 此外,通过指定其泛型类参数的 `String` 类,可以使用 `System.Collections.Generic` 命名空间中的任何泛型集合类作为强类型字符串集合. 例如,可以将变量声明为采用 `List<String>` 或 `Dictionary<String,String>` 类型.

另外MS还发布了一系列不可变集合  `System.Collections.Immutable`,有点类似与值类型,Add等操作不会改变原始集合,而是返回一个新的集合.

## Linq

* IEnumerable -> Enumerable (`Func`,`Action`)
* IQueryable -> Queryable(`Expression` `Expression<TDelegate>`)

[Simple Linq Demo](../src/Linq/SimpleLinqToSqlRunner.cs)

## Emit

Emit 是对 IL 层的封装.允许通过ILCode 运行阶段生成托管代码.

* AssemblyBuilder ModuleBuilder TypeBuilder MethodBuilder
* DynamicMethod
* ILGenerator

[Emit Demo](../src/RuntimeIL.NET/IL_001.cs)

[Emit Demo](../src/RuntimeIL.NET/IL_002.cs)

[Emit Demo](../src/RuntimeIL.NET/IL_003.cs)

[Emit Demo](../src/RuntimeIL.NET/IL_004.cs)

[Emit Demo](../src/RuntimeIL.NET/IL_005.cs)

[Emit Demo](../src/RuntimeIL.NET/IL_006.cs)

## Expression

Func<> -(Compiler/Expression&Expression.Lambda)-> Expression<Func<>>/LambdaExpression -.Compiner()-> Func

Lambda表达式生成的委托 经过编译器生成将强类型化的 Lambda 表达式表示为表达式树形式的数据结构(Expression<Func<>> / LambdaExpression).

这时候可以通过 Compile 编译成 委托供执行

[Expression Demo](../src/RuntimeIL.NET/Test.cs)

## CallSite

反射 + 缓存