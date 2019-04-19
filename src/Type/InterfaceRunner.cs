using Common;
using System;

namespace Type {
    public class InterfaceRunner : Runner {
        protected override void RunCore() {
            SimpleType st = new SimpleType();
            // This calls the public Dispose method implementation 
            st.Dispose();
            // This calls IDisposable's Dispose method implementation 
            IDisposable d = st;
            d.Dispose();
        }

        internal sealed class SimpleType : IDisposable {
            public void Dispose() {
                Console.WriteLine("public Dispose");
            }

            // 禁止设置可访问性,但是可访问性为 private,只能通过 IDisposable 接口调用该方法.
            void IDisposable.Dispose() {
                Console.WriteLine("IDisposable Dispose");
            }
        }
    }
}