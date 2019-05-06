using System;
using System.Linq;
using System.Linq.Expressions;

namespace Linq {
    class SimpleQueryProvider : IQueryProvider {

        public IQueryable<TElement> CreateQuery<TElement>(Expression expression) {
            return new SimpleQueryable<TElement>(expression, this);
        }

        public IQueryable CreateQuery(Expression expression) {
            throw new NotImplementedException();
            //Type elementType = TypeSystem.GetElementType(expression.Type);
            //try {
            //    return (IQueryable)Activator.CreateInstance(typeof(SimpleQueryable<>).MakeGenericType(elementType), expression, this);
            //} catch (TargetInvocationException tie) {
            //    if (tie.InnerException != null) {
            //        throw tie.InnerException;
            //    }
            //    throw;
            //}
        }

        public object Execute(Expression expression) {
            var queryText = GetQueryText(expression);
            Console.WriteLine(queryText);
            return default;
        }

        public TResult Execute<TResult>(Expression expression) {
            var queryText = GetQueryText(expression);
            Console.WriteLine(queryText);
            return default;
        }

        public string GetQueryText(Expression expression) {
            return new SimpleExpressionVisitor(expression).Translate();
        }
    }
}
