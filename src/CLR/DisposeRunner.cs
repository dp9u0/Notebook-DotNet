using Common;
using System;

namespace CLR {
    class DisposeRunner : Runner {
        protected override void RunCore() {
            using (var disposedemo = new DisposeDemo()) {

            }
        }

        class DisposeDemo : IDisposable {
            #region IDisposable Support

            private bool disposedValue = false;

            protected virtual void Dispose(bool disposing) {
                if (!disposedValue) {
                    if (disposing) {
                        // 释放托管对象
                    }
                    // 释放未托管的对象
                    // 大型字段设置为 null
                    disposedValue = true;
                }
            }

            ~DisposeDemo() {
                Dispose(false);
            }

            void IDisposable.Dispose() {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            #endregion
        }
    }
}
