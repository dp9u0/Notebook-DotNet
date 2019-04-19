using Common;
using System;

namespace Type {
    internal class TypeMemberRunner : Runner {
        protected override void RunCore() {
            var val = new SomeType();
        }

        public sealed class SomeType {

            public readonly Int32 _field = 2; // field                               
            private const Int32 SOME_CONSTANT_FIELD = 1; // Constant, read-only, and static read/write field
            private readonly Int32 _readonlyField = 2; // readonly field
            public static Int32 SomeStaticField = 3; // static field
            static SomeType() { } // Type constructor 
            public SomeType(Int32 x) { _field = x; } // Instance constructors 
            public SomeType() { } // Instance constructors 
            private String InstanceMethod() { return null; } // Instance Methods 
            public static void Main() { } // Static methods 
            public Int32 SomeProp { //  Instance property 
                get { return _readonlyField; }
                set { }
            }
            public Int32 this[String s] { //  Instance parameterful property (indexer)
                get { return 0; }
                set { }
            }

            public EventHandler SomeEvent2 = null; // Instance event 

            public event EventHandler SomeEvent; // Instance event 

            private class SomeNestedType { } // Nested class 

            private void Raise() {
                SomeEvent?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
