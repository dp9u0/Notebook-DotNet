using Common;
using System;

namespace Type {
    class StringRunner : Runner {
        protected override void RunCore() {
            ConcatString();
            ConcatString2();
            StringInterning();
        }

        private void ConcatString() {
            var str = "1" + "2";
            Console.WriteLine(str);
        }

        private void ConcatString2() {
            var str = "1";
            var str2 = "2";
            var str3 = str + str2;
            Console.WriteLine(str3);
        }

        private void StringInterning() {
            var str1 = "aaaa";
            var str2 = "aaaa";
            var str3 = "aa";
            var str4 = str3 + str3;
            var str5 = String.Intern(str3 + str3);
            Console.WriteLine(Object.ReferenceEquals(str1, str2));// TRUE
            Console.WriteLine(Object.ReferenceEquals(str4, str2));// FALSE
            Console.WriteLine(Object.ReferenceEquals(str5, str2));// TRUE
        }
    }
}