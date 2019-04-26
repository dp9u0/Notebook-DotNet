#region

using CLR;
using Collection;
using CSharp;
using Linq;
using RuntimeCompiler;
using ThreadSample;
using Type;

#endregion

namespace Run {
    class Program {
        static void Main() {
            //new TypeRunner().Run();
            //new CSharpRunner().Run();
            //new ClrRunner().Run();
            new ThreadRunner().Run();
            //new CollectionRunner().Run();
            //new LinqRunner().Run();
            //new RuntimeCompilerRunner().Run();
        }
    }
}