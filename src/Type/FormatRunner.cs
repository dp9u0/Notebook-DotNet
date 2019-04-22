using Common;
using System;
using System.Globalization;

namespace Type {
    class FormatRunner : Runner {
        protected override void RunCore() {
            FormatExample1();
            FormatExample2();
        }

        private void FormatExample1() {
            decimal value = 1603.42m;
            Console.WriteLine(value.ToString("C3", new CultureInfo("en-US")));
            Console.WriteLine(value.ToString("C3", new CultureInfo("fr-FR")));
            Console.WriteLine(value.ToString("C3", new CultureInfo("de-DE")));
        }

        private void FormatExample2() {
            long value = 3210662321;
            byte value1 = 214;
            byte value2 = 19;
            Console.WriteLine(String.Format(new ByteByByteFormatter(), "{0:X}", value));
            Console.WriteLine(String.Format(new ByteByByteFormatter(), "{0:X} And {1:X} = {2:X} ({2:000})", value1, value2, value1 & value2));
            Console.WriteLine(String.Format(new ByteByByteFormatter(), "{0,10:N0}", value));
        }


        public class ByteByByteFormatter : IFormatProvider, ICustomFormatter {
            public object GetFormat(System.Type formatType) {
                if (formatType == typeof(ICustomFormatter))
                    return this;
                else
                    return null;
            }

            public string Format(string format, object arg,
                                   IFormatProvider formatProvider) {
                if (!formatProvider.Equals(this)) return null;

                // Handle only hexadecimal format string.
                if (!format.StartsWith("X")) return null;
                // 返回null 表示不匹配格式化 需要使用 其他格式化方式处理这个对象

                byte[] bytes;
                string output = null;

                // Handle only integral types.
                if (arg is Byte)
                    bytes = BitConverter.GetBytes((Byte)arg);
                else if (arg is Int16)
                    bytes = BitConverter.GetBytes((Int16)arg);
                else if (arg is Int32)
                    bytes = BitConverter.GetBytes((Int32)arg);
                else if (arg is Int64)
                    bytes = BitConverter.GetBytes((Int64)arg);
                else if (arg is SByte)
                    bytes = BitConverter.GetBytes((SByte)arg);
                else if (arg is UInt16)
                    bytes = BitConverter.GetBytes((UInt16)arg);
                else if (arg is UInt32)
                    bytes = BitConverter.GetBytes((UInt32)arg);
                else if (arg is UInt64)
                    bytes = BitConverter.GetBytes((UInt64)arg);
                else
                    return null;

                for (int ctr = bytes.Length - 1; ctr >= 0; ctr--)
                    output += String.Format("{0:X2} ", bytes[ctr]);

                return output.Trim();
            }
        }
    }
}
