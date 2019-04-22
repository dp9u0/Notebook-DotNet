using Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Type {
    class AttributeRunner : Runner {
        protected override void RunCore() {
            var some = new SomeType<string>();
        }

        //[assembly: SomeAttr] // Applied to assembly 
        //[module: SomeAttr] // Applied to module 
        [type: SomeAttr("SomeType Type")] // Applied to type 
        internal sealed class SomeType<[typevar: SomeAttr] T> { // Applied to generic type variable

            [field: SomeAttr("SomeField field")] // Applied to field 
            public Int32 SomeField = 0;

            [return: SomeAttr("SomeMethod return")] // Applied to return value 
            [method: SomeAttr("SomeMethod method")] // Applied to method 
            public Int32 SomeMethod([param: SomeAttr("SomeParam param")] Int32 SomeParam) { return SomeParam; }


            [property: SomeAttr("SomeField field")] // Applied to property 
            public String SomeProp {
                [method: SomeAttr("SomeProp property get")] // Applied to get accessor method 
                get { return null; }
            }

            [event: SomeAttr("SomeEvent event")] // Applied to event 
            [field: SomeAttr("SomeEvent field")] // Applied to compiler-generated field 
            [method: SomeAttr("SomeEvent method")] // Applied to compiler-generated add & remove methods 
            public event EventHandler SomeEvent;
        }

        public class SomeAttr : Attribute {

            public SomeAttr() {

            }
            public SomeAttr(string arg) {

            }
        }
    }
}
