using Common;
using System;
namespace Type {
    public class GenericsRunner : Runner {
        protected override void RunCore() {
            Func<Object, ArgumentException> fn1 = (input) =>
            {
                Console.WriteLine(input);
                return new ArgumentException();
            };
            Func<string, Exception> fn2 = fn1;
            fn2("string");
        }
    }
}