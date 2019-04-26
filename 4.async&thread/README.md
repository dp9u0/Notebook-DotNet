# async parallel thread

## thread

### 基本概念

* CLR线程与OS线程:等同
* 线程优先级
* 前台/后台线程

### 线程同步

* Volatile : [Demo](../src/Thread/VolatileRunner.cs)
  
  在一段代码中,如果重复只读的形式访问一个字段,JIT会对这个字段进行优化,只从RAM中读取一次然后放在寄存器中,之后访问都直接访问寄存器了,不会再访存.

  并发场景下,如果另外一个线程写入了这个字段,上面代码段中将不会检测到变更.

  因此出现了 volatile 关键字防止JIT对此进行优化.

  但是因此也带来了一些问题:

  1. a = a + a ,如果 a 添加了 volatile 修饰,那么每次将会读取内存2次.
  2. out/ref/in 参数不支持 volatile

## async

## parallel