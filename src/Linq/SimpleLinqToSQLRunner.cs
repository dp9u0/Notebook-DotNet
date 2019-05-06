using Common;
using System;
using System.Linq;

namespace Linq {
    class SimpleLinqToSQLRunner : Runner {
        protected override void RunCore() {
            var queryable = DataSource.Create<Data>();
            var sum = queryable.Where(data => data.Field1 == "11"
            || data.Field1 == "22").Sum(data => data.Field2);

            var average = queryable.Where(data => data.Field2 == 22 || data.Field1 == null).Average(data => data.Field2);

            Console.WriteLine(sum);


        }
    }
}
