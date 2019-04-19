using Common;
using System;
namespace Type {
    public class GenericsConstraintRunner : Runner {
        protected override void RunCore() {
            Func<Object, ArgumentException> fn1 = (input) =>
            {
                Console.WriteLine(input);
                return new ArgumentException();
            };
            Func<string, Exception> fn2 = fn1;
            fn2("string");
        }
        //主要约束限定T继承自Exception类型
        public class ClassT1<T> where T : Exception {
            private T myException;
            public ClassT1(T t) {
                myException = t;
            }
            public override string ToString() {
                //主要约束保证了myException拥有Source成员
                return myException.Source;
            }
        }
        //主要约束限定T是引用类型
        public class ClassT2<T> where T : class {
            public T myT = default;
            public void Clear() {
                //T是引用类型，可以置null
                myT = null;
            }
        }
        //主要约束限定T是值类型
        public class ClassT3<T> where T : struct {
            private T myT = default;
            public override string ToString() {
                //T是值类型，不会发生NullReferenceException异常
                return myT.ToString();
            }
        }

        public class ClassT4<T> where T : IDisposable {
            private T myT = default;
            public void Dispose() {
                myT.Dispose();
            }
        }

        /// <summary>构造器约束</summary>
        public class ClassT5<T> where T : new() {
            public T Get() {
                return new T();
            }
        }
    }
}