# CLR

* [CLR](#clr)
  * [Metadata](#metadata)
  * [互操作性](#%E4%BA%92%E6%93%8D%E4%BD%9C%E6%80%A7)
  * [Runtime](#runtime)
    * [Managed Code](#managed-code)
    * [Managed Memory](#managed-memory)
    * [AppDomain](#appdomain)

## Metadata  

* PE头
* 表流: TypeDef TypeRef等
* String
* US
* Blob
* Guid

```cs
.entrypoint  
.maxstack  3  
.locals ([0] int32 ValueOne,  
         [1] int32 ValueTwo,  
         [2] int32 V_2,  
         [3] int32 V_3)  
IL_0000:  ldc.i4.s   10  
IL_0002:  stloc.0  
IL_0003:  ldc.i4.s   20  
IL_0005:  stloc.1  
IL_0006:  ldstr      "The Value is: {0}"  
IL_000b:  ldloc.0  
IL_000c:  ldloc.1  
IL_000d:  call int32 ConsoleApplication.MyApp::Add(int32,int32) /* 06000003 */  
```

调用 Add 方法 (/* 06000003 */) 的元数据标记,该标记参考 MethodDef 表的第三行

## 互操作性

* P/Invoke

```cs
// Import user32.dll (containing the function we need) and define
// the method corresponding to the native function.
[DllImport("user32.dll")]
public static extern int MessageBox(IntPtr hWnd, String text, String caption, int options);


protected override void RunCore() {
    // Invoke the function as a regular managed method.
    MessageBox(IntPtr.Zero, "Command-line message box", "Attention!", 0);
}
```

* type-marshalling

`MarshalAs`

```cs
public struct CBool {
    [MarshalAs(UnmanagedType.U1)]
    public bool b;
}
```

## Runtime

### Managed Code

MSIL ---JIT--> Native Code

### Managed Memory

Stack

Heap

托管堆上的对象都有类型对象指针,同步块索引

GetType 其实利用了类型对象指针

* GC

回收策略 : 可达图 : 标记对象不可达, 从根对象开始遍历如果能访问到则标记为可达,最后不可达的选择清除.

清除后会进行 compact,整理内存碎片

分代回收. Gen 0 ,Gen 1 ,Gen 2 通常情况下仅对 Gen 0 进行标记清除.

如果 Gen 1 or Gen 2 引用了 Gen 0, JIT代码生成时会添加 write barrier 写入 对象会将对应的 card table 中设置标志位. 垃圾回收 Gen 0 时,会检查被设置了 标志位的 老代对象是否被修改并且是否引用了 Gen 0. 被引用Gen 0 也被看做可达对象存活.

Gen 0 1 2 的内存预算都会根据实际情况进行调节 : 回收的多就会减少,回收的少就会增加,这有一个启发式算法进行调节.

### AppDomain