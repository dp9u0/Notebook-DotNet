using Common;
namespace Type {
    class MethodRunner : Runner {
        protected override void RunCore() { }

        private void CallVirt() {
            SomeType val = new SomeType();
            // callvirt  instance void Type.MethodRunner/BaseType::MethodVirtual()
            val.MethodVirtual();
        }

        private void CallVirt2() {
            // callvirt  instance void Type.MethodRunner/BaseType::MethodVirtual()
            BaseType val = new SomeType();
            val.MethodVirtual();
        }

        private void CallStatic() {
            // call void Type.MethodRunner/BaseType::MethodStatic()
            BaseType.MethodStatic();
        }

        private void CallMethod() {
            // callvirt  instance void Type.MethodRunner/SomeType::Method()
            var val = new SomeType();
            val.Method();
        }

        private void CallMethod2() {
            // call instance void Type.MethodRunner/SomeType::Method()
            new SomeType().Method();
        }

        public class BaseType {
            public static void MethodStatic() { }
            public virtual void MethodVirtual() { }
        }

        public class SomeType : BaseType {
            public void Method() { }
            public override void MethodVirtual() {
                // call instance void Type.MethodRunner/BaseType::MethodVirtual()
                base.MethodVirtual();
            }
        }
    }
}
