using Common;
using System;

namespace Type {
    public class EventRunner : Runner {
        protected override void RunCore() {
            var someType = new SomeType();
            someType.SomeEvent += SomeType_SomeEvent;
        }

        private void SomeType_SomeEvent(object sender, EventArgs e) {

        }

        public sealed class SomeType {

            public event EventHandler SomeEvent; // Instance event 

            private void TriggeEvent() {
                SomeEvent?.Invoke(this, EventArgs.Empty);
            }
        }
    }
}
