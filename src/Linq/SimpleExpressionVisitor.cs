using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Linq {
    class SimpleExpressionVisitor : ExpressionVisitor {

        public SimpleExpressionVisitor(Expression expression) {
            _expression = expression;
        }

        private StringBuilder sb;
        private Expression _expression;

        public string Translate() {
            if (sb == null) {
                sb = new StringBuilder();
                Visit(_expression);
            }
            return sb.ToString();
        }

        public override string ToString() {
            return sb?.ToString();
        }

        public override Expression Visit(Expression node) {
            return base.Visit(node);
        }

        protected override Expression VisitBinary(BinaryExpression b) {
            sb.Append("(");
            Visit(b.Left);
            switch (b.NodeType) {
                case ExpressionType.And:
                    sb.Append(" AND ");
                    break;
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    sb.Append(" OR ");
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
                        sb.Append("String(");
                        sb.Append(c.Value);
                        sb.Append(")");
                        break;
                    case TypeCode.Object:
                        throw new NotSupportedException(string.Format("常量{0}不支持", c.Value));
                    default:
                        sb.Append(Type.GetTypeCode(c.Value.GetType()) + "(");
                        sb.Append(c.Value);
                        sb.Append(")");
                        break;
                }
            }
            return c;
        }


        protected override Expression VisitLambda<T>(Expression<T> lambda) {
            return Visit(lambda.Body);
        }

        protected override Expression VisitMember(MemberExpression m) {
            if ((m.Expression != null) && (m.Expression.NodeType == ExpressionType.Parameter)) {
                sb.Append(m.Member.Name);
                return m;
            }
            throw new NotSupportedException(string.Format("成员{0}不支持", m.Member.Name));
        }


        private static Expression StripQuotes(Expression e) {
            while (e.NodeType == ExpressionType.Quote) {
                e = ((UnaryExpression)e).Operand;
            }
            return e;
        }

        protected override Expression VisitMethodCall(MethodCallExpression m) {
            switch (m.Method.Name) {
                case "Where":
                    sb.Append("SELECT * FROM (");
                    Visit(m.Arguments[0]);
                    sb.Append(") AS T WHERE ");
                    LambdaExpression lambda = (LambdaExpression)StripQuotes(m.Arguments[1]);
                    Visit(lambda.Body);
                    break;
                case "Sum":
                    sb.Append("SELECT COUNT(");
                    LambdaExpression lambda1 = (LambdaExpression)StripQuotes(m.Arguments[1]);
                    Visit(lambda1.Body);
                    sb.Append(") FROM (");
                    Visit(m.Arguments[0]);
                    sb.Append(")");
                    break;
                case "Average":
                    sb.Append("SELECT Average(");
                    LambdaExpression lambda2 = (LambdaExpression)StripQuotes(m.Arguments[1]);
                    Visit(lambda2.Body);
                    sb.Append(") FROM (");
                    Visit(m.Arguments[0]);
                    sb.Append(")");
                    break;
                default:
                    break;
            }
            return m;
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
    }
}
