#region

using System;
using Common;

#endregion

namespace Type {
    public class PrimitiveTypeRunner : Runner {
        protected override void RunCore() {
            SameCode();
            DonnotUsePrimitiveType();
            ImplicitConvert();
            CompilerDoForPrimitiveType();
            Check();
            Uncheck();
            Default();
        }

        private static void SameCode() {
            int value = 1000;
            Int32 value2 = new Int32();
            value += value2;
            Console.WriteLine("output:" + value);
            string str2 = 2.ToString();
            Console.WriteLine("output:" + str2);
        }

        private static void DonnotUsePrimitiveType() {
            //
        }

        private static void ImplicitConvert() {
            Int32 i = 5; // Implicit cast from Int32 to Int32 
            Int64 l = i; // Implicit cast from Int32 to Int64 
            Single s = i; // Implicit cast from Int32 to Single 
            Byte b = (Byte)i; // Explicit cast from Int32 to Byte 
            Int16 v = (Int16)s; // Explicit cast from Single to Int16
        }

        private static void CompilerDoForPrimitiveType() {
            Console.WriteLine(123.ToString() + 456.ToString()); // "123456"
            Boolean found = false; // Generated code sets found to 0 
            Int32 x = 100 + 20 + 3; // Generated code sets x to 123 
            String s = "a " + "bc"; // Generated code sets s to "a bc"
            Console.WriteLine($"{found}{x}{s}"); // "123456"
        }

        private static void Check() {
            try {
                checked {
                    // Start of checked block 
                    Byte b = 100;
                    // add.ovf
                    b = (Byte)(b + 200); // This expression is checked for overflow. 
                    Console.WriteLine(b);
                } // End of checked block
            } catch (Exception) {
                Console.WriteLine("Convert 100+200(Int32) to Byte Should Throw Exception");
            }
        }

        private static void Uncheck() {
            unchecked {
                // Start of checked block 
                Byte b = 100;
                // add
                b = (Byte)(b + 200); // This expression is checked for overflow. 
                Console.WriteLine(b);
            } // End of checked block
        }

        private static void Default() {
            // Start of checked block 
            Byte b = 100;
            // add
            b = (Byte)(b + 200); // This expression is checked for overflow. 
            Console.WriteLine(b);
        }
    }
}