using Common;
using System;
using System.Runtime.InteropServices;

namespace CLR {
    class PInvokeRunner : Runner {

        // Import user32.dll (containing the function we need) and define
        // the method corresponding to the native function.
        [DllImport("user32.dll")]

        public static extern int MessageBox(IntPtr hWnd, String text, String caption, int options);
        // Define a delegate that corresponds to the unmanaged function.
        delegate bool EnumWC(IntPtr hwnd, IntPtr lParam);

        // Import user32.dll (containing the function we need) and define
        // the method corresponding to the native function.
        [DllImport("user32.dll")]
        static extern int EnumWindows(EnumWC lpEnumFunc, IntPtr lParam);

        // Define the implementation of the delegate; here, we simply output the window handle.
        static bool OutputWindow(IntPtr hwnd, IntPtr lParam) {
            Console.WriteLine(hwnd.ToInt64());
            return true;
        }

        protected override void RunCore() {
            // Invoke the function as a regular managed method.
            MessageBox(IntPtr.Zero, "Command-line message box", "Attention!", 0);
            // Invoke the method; note the delegate as a first parameter.
            EnumWindows(OutputWindow, IntPtr.Zero);
        }

        public struct CBool {
            [MarshalAs(UnmanagedType.U1)]
            public bool b;
        }
    }
}
