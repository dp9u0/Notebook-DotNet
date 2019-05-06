using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Linq {
    class SimpleQueryable<T> : IQueryable<T> {

        private readonly Expression _expression;
        private readonly SimpleQueryProvider _provider;

        public SimpleQueryable(SimpleQueryProvider provider) {
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
            _expression = Expression.Constant(this);
        }

        public SimpleQueryable(Expression expression, SimpleQueryProvider provider) {
            _expression = expression ?? throw new ArgumentNullException(nameof(expression));
            if (!typeof(IQueryable<T>).IsAssignableFrom(expression.Type)) {
                throw new ArgumentException(nameof(expression));
            }
            _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        }

        public Type ElementType {
            get {
                return typeof(T);
            }
        }

        public Expression Expression {
            get {
                return _expression;
            }
        }

        public IQueryProvider Provider {
            get {
                return _provider;
            }
        }

        public IEnumerator<T> GetEnumerator() {
            return null;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return null;
        }

        public override string ToString() {
            return _provider.GetQueryText(_expression);
        }
    }
}
