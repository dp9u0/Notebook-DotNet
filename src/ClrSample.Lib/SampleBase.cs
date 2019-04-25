using System;

namespace ClrSample.Lib {

    public abstract class SampleBase {

        public void Invoke(params string[] args) {
            InvokeCore(args);
        }

        protected abstract void InvokeCore(params string[] args);

        public abstract string Command { get; }

        public virtual string Description { get { return GetType().Name; } }

        public abstract string Usage { get; }
    }
}
