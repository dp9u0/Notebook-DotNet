namespace RuntimeIL.NET {
    public class IL_004 {
        public static void Run() {
        }

        public static void Test(IL_002.Person2 person, int test) {
            int i = 1;
            int j = 100;
            string me = "";
            string me2 = "";
            const string message = "test:{0}{1}";
            const string message2 = "test:{0}{1}{2}";
            string result = string.Format(message, i, me);
            string result2 = string.Format(message2, i, j, me2, me2, person);
        }

        /*         
	IL_0000: nop
	IL_0001: ldc.i4.1
	IL_0002: stloc.0
	IL_0003: ldstr ""
	IL_0008: stloc.1
	IL_0009: ldstr ""
	IL_000e: stloc.2
	IL_000f: ldstr "test:{0}{1}"
	IL_0014: ldc.i4.1
	IL_0015: box [mscorlib]System.Int32
	IL_001a: ldloc.1
	IL_001b: call string [mscorlib]System.String::Format(string, object, object)
	IL_0020: stloc.3
	IL_0021: ldstr "test:{0}{1}{2}"
	IL_0026: ldc.i4.1
	IL_0027: box [mscorlib]System.Int32
	IL_002c: ldloc.1
	IL_002d: ldloc.2
	IL_002e: call string [mscorlib]System.String::Format(string, object, object, object)
	IL_0033: stloc.s result2
	IL_0035: ret         
         */
    }
}