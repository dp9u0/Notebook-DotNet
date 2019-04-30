using Common;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadSample {
    class ParallelRunner : Runner {
        protected override void RunCore() {

        }

        private long ParallelExample1(string path, string searchPattern, SearchOption searchOption) {
            var files = Directory.EnumerateFiles(path, searchPattern, searchOption);
            Int64 masterTotal = 0;
            ParallelLoopResult result = Parallel.ForEach<String, Int64>(
            files,
            () =>
            { // localInit: Invoked once per task at start
              // Initialize that this task has seen 0 bytes
                return 0; // Set taskLocalTotal initial value to 0
            },
            (file, loopState, index, taskLocalTotal) =>
            { // body: Invoked once per work item
              // Get this file's size and add it to this task's running total
                Int64 fileLength = 0;
                FileStream fs = null;
                try {
                    fs = File.OpenRead(file);
                    fileLength = fs.Length;
                } catch (IOException) { /* Ignore any files we can't access */ } finally { if (fs != null) fs.Dispose(); }
                return taskLocalTotal + fileLength;
            },
            taskLocalTotal =>
            { // localFinally: Invoked once per task at end
              // Atomically add this task's total to the "master" total
                Interlocked.Add(ref masterTotal, taskLocalTotal);
            });
            return masterTotal;
        }


        private void PLinq() {
            Assembly assembly = Assembly.GetExecutingAssembly();

            var query = from type in assembly.GetExportedTypes().AsParallel()
                        from method in type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
                        let obsoleteAttrType = typeof(ObsoleteAttribute)
                        where Attribute.IsDefined(method, obsoleteAttrType)
                        orderby type.FullName
                        let obsoleteAttrObj = (ObsoleteAttribute)Attribute.GetCustomAttribute(method, obsoleteAttrType)
                        select String.Format("Type={0}\nMethod={1}\nMessage={2}\n",
                        type.FullName, method.ToString(), obsoleteAttrObj.Message);

            // Display the results
            foreach (var result in query) Console.WriteLine(result);

        }
    }
}
