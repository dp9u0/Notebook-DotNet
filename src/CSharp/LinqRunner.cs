using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace CSharp {
    class LinqRunner : Runner {
        protected override void RunCore() { LinqRunnerTest.Test005(); }

    }
    public static class LinqRunnerTest {

        private static IEnumerable<MyModel> enumarable = null;
        private static IQueryable<MyModel> queryable = null;

        static LinqRunnerTest() {
            queryable = new EnumerableQuery<MyModel>(new List<MyModel>());
            enumarable = new List<MyModel>();
        }

        public static void Test001() {
            // see ref Test003() 
            var result = from t in enumarable
                         select t.MyProperty;
        }

        public static void Test003() {
            var result = enumarable.Select(t => t.MyProperty);
        }


        public static void Test002() {
            // see ref Test004
            // see ref Test005
            var result2 = from t in queryable
                          select t.MyProperty;
        }

        public static void Test004() {
            var result = queryable.Select(t => t.MyProperty);
        }

        public static void Test005() {
            ParameterExpression parameterExpression = Expression.Parameter(typeof(MyModel), "t");
            var indexExpression = Expression.Property(parameterExpression, "MyProperty");
            var lambda = Expression.Lambda<Func<MyModel, string>>(indexExpression, parameterExpression);
            var result = queryable.Select(lambda);
        }


        public static void Test006() {
            List<MyModel> myUsers = new List<MyModel>();
            string userSql = myUsers.AsQueryable().ToSql(u => u.Age > 2);
            Console.WriteLine(userSql);
            // SELECT * FROM (SELECT * FROM User) AS T WHERE (Age>2)

            List<MyModel> myUsers2 = new List<MyModel>();
            string userSql2 = myUsers.AsQueryable().ToSql(u => (u.Name == "Jesse") && (u.Name != ""));
            Console.WriteLine(userSql2);
            // SELECT * FROM (SELECT * FROM USER) AS T WHERE (Name='Jesse')
        }

        public static string ToSql<TSource>(this IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate) {
            var expression = Expression.Call(null, ((MethodInfo)MethodBase.GetCurrentMethod())
                    .MakeGenericMethod(typeof(TSource)),
                new[] { source.Expression, Expression.Quote(predicate) });

            var translator = new QueryTranslator();
            return translator.Translate(expression);
        }

        public class QueryTranslator : ExpressionVisitor {
            private StringBuilder sb;

            public string Translate(Expression expression) {
                sb = new StringBuilder();
                Visit(expression);
                return sb.ToString();
            }

            private static Expression StripQuotes(Expression e) {
                while (e.NodeType == ExpressionType.Quote) {
                    e = ((UnaryExpression)e).Operand;
                }
                return e;
            }

            protected override Expression VisitMethodCall(MethodCallExpression m) {
                if ((m.Method.DeclaringType == typeof(LinqRunnerTest)) && (m.Method.Name == "ToSql")) {
                    sb.Append("SELECT * FROM (");
                    Visit(m.Arguments[0]);
                    sb.Append(") AS T WHERE ");
                    LambdaExpression lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
                    Visit(lambda.Body);
                    return m;
                }
                throw new NotSupportedException(string.Format("方法{0}不支持", m.Method.Name));
            }

            protected override Expression VisitUnary(UnaryExpression u) {
                switch (u.NodeType) {
                    case ExpressionType.Not:
                        sb.Append(" NOT ");
                        Visit(u.Operand);
                        break;
                    default:
                        throw new NotSupportedException(string.Format("运算{0}不支持", u.NodeType));
                }
                return u;
            }

            protected override Expression VisitBinary(BinaryExpression b) {
                sb.Append("(");
                Visit(b.Left);
                switch (b.NodeType) {
                    case ExpressionType.And:
                        sb.Append(" AND ");
                        break;
                    case ExpressionType.Or:
                        sb.Append(" OR");
                        break;
                    case ExpressionType.Equal:
                        sb.Append(" = ");
                        break;
                    case ExpressionType.NotEqual:
                        sb.Append(" <> ");
                        break;
                    case ExpressionType.LessThan:
                        sb.Append(" < ");
                        break;
                    case ExpressionType.LessThanOrEqual:
                        sb.Append(" <= ");
                        break;
                    case ExpressionType.GreaterThan:
                        sb.Append(" > ");
                        break;
                    case ExpressionType.GreaterThanOrEqual:
                        sb.Append(" >= ");
                        break;
                    default:
                        throw new NotSupportedException(string.Format("运算符{0}不支持", b.NodeType));
                }
                Visit(b.Right);
                sb.Append(")");
                return b;
            }

            protected override Expression VisitConstant(ConstantExpression c) {
                IQueryable q = c.Value as IQueryable;
                if (q != null) {
                    // 我们假设我们那个Queryable就是对应的表
                    sb.Append("SELECT * FROM ");
                    sb.Append(q.ElementType.Name);
                } else if (c.Value == null) {
                    sb.Append("NULL");
                } else {
                    switch (Type.GetTypeCode(c.Value.GetType())) {
                        case TypeCode.Boolean:
                            sb.Append((bool)c.Value ? 1 : 0);
                            break;
                        case TypeCode.String:
                            sb.Append("'");
                            sb.Append(c.Value);
                            sb.Append("'");
                            break;
                        case TypeCode.Object:
                            throw new NotSupportedException(string.Format("常量{0}不支持", c.Value));
                        default:
                            sb.Append(c.Value);
                            break;
                    }
                }
                return c;
            }

            protected override Expression VisitMember(MemberExpression m) {
                if ((m.Expression != null) && (m.Expression.NodeType == ExpressionType.Parameter)) {
                    sb.Append(m.Member.Name);
                    return m;
                }
                throw new NotSupportedException(string.Format("成员{0}不支持", m.Member.Name));
            }
        }

        public class MyModel {
            public string Name { get; set; }

            public int Age { get; set; }

            public string MyProperty { get; set; }
        }
    }
}
