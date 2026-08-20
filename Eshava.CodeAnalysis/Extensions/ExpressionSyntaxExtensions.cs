using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class ExpressionSyntaxExtensions
	{
		public static ExpressionSyntax AddNullFallback(this ExpressionSyntax expression, ExpressionSyntax nullFallback)
		{
			return SyntaxHelper.AddNullFallback(expression, nullFallback);
		}

		public static ExpressionSyntax Assign(this ExpressionSyntax left, ExpressionSyntax right)
		{
			return SyntaxHelper.Assign(left, right);
		}

		public static ExpressionSyntax AddAssign(this ExpressionSyntax left, ExpressionSyntax right)
		{
			return SyntaxHelper.AddAssign(left, right);
		}

		public static ExpressionSyntax SubtractAssign(this ExpressionSyntax left, ExpressionSyntax right)
		{
			return SyntaxHelper.SubtractAssign(left, right);
		}

		public static ExpressionSyntax MultiplyAssign(this ExpressionSyntax left, ExpressionSyntax right)
		{
			return SyntaxHelper.MultiplyAssign(left, right);
		}

		public static ExpressionSyntax DivideAssign(this ExpressionSyntax left, ExpressionSyntax right)
		{
			return SyntaxHelper.DivideAssign(left, right);
		}

		public static ExpressionSyntax ModuloAssign(this ExpressionSyntax left, ExpressionSyntax right)
		{
			return SyntaxHelper.ModuloAssign(left, right);
		}

		public static ExpressionSyntax AndAssign(this ExpressionSyntax left, ExpressionSyntax right)
		{
			return SyntaxHelper.AndAssign(left, right);
		}

		public static ExpressionSyntax OrAssign(this ExpressionSyntax left, ExpressionSyntax right)
		{
			return SyntaxHelper.OrAssign(left, right);
		}

		public static ArgumentSyntax ToArgument(this ExpressionSyntax expression)
		{
			return SyntaxHelper.CreateArgument(expression);
		}

		public static EqualsValueClauseSyntax ToEqualsValueClause(this ExpressionSyntax expression)
		{
			return SyntaxHelper.CreateEqualsValueClause(expression);
		}

		public static ExpressionStatementSyntax ToExpressionStatement(this ExpressionSyntax expression)
		{
			return SyntaxHelper.CreateExpressionStatement(expression);
		}

		/// <summary>
		/// Creates an as-expression. <paramref name="toNullableType"/> has no default on purpose:
		/// whether the target type becomes nullable changes the generated code and is a decision of
		/// the caller.
		/// </summary>
		public static BinaryExpressionSyntax AsType(this ExpressionSyntax expression, TypeSyntax type, bool toNullableType)
		{
			return SyntaxHelper.AsType(expression, type, toNullableType);
		}

		public static BinaryExpressionSyntax And(this ExpressionSyntax left, ExpressionSyntax right)
		{
			return SyntaxHelper.CreateBinaryExpression(left, right, SyntaxKind.LogicalAndExpression);
		}

		public static BinaryExpressionSyntax Or(this ExpressionSyntax left, ExpressionSyntax right)
		{
			return SyntaxHelper.CreateBinaryExpression(left, right, SyntaxKind.LogicalOrExpression);
		}

		public static BinaryExpressionSyntax ToEquals(this ExpressionSyntax left, ExpressionSyntax right)
		{
			return SyntaxHelper.CreateBinaryExpression(left, right, SyntaxKind.EqualsExpression);
		}

		public static BinaryExpressionSyntax NotEquals(this ExpressionSyntax left, ExpressionSyntax right)
		{
			return SyntaxHelper.CreateBinaryExpression(left, right, SyntaxKind.NotEqualsExpression);
		}

		public static BinaryExpressionSyntax GreaterThan(this ExpressionSyntax left, ExpressionSyntax right)
		{
			return SyntaxHelper.CreateBinaryExpression(left, right, SyntaxKind.GreaterThanExpression);
		}

		public static BinaryExpressionSyntax GreaterThanOrEqual(this ExpressionSyntax left, ExpressionSyntax right)
		{
			return SyntaxHelper.CreateBinaryExpression(left, right, SyntaxKind.GreaterThanOrEqualExpression);
		}

		public static BinaryExpressionSyntax LessThan(this ExpressionSyntax left, ExpressionSyntax right)
		{
			return SyntaxHelper.CreateBinaryExpression(left, right, SyntaxKind.LessThanExpression);
		}

		public static BinaryExpressionSyntax LessThanOrEqual(this ExpressionSyntax left, ExpressionSyntax right)
		{
			return SyntaxHelper.CreateBinaryExpression(left, right, SyntaxKind.LessThanOrEqualExpression);
		}

		public static ReturnStatementSyntax Return(this ExpressionSyntax statement)
		{
			return SyntaxHelper.ToReturn(statement);
		}

		public static InvocationExpressionSyntax Call(this ExpressionSyntax target, SimpleNameSyntax method, bool withNullCheck = false, params ArgumentSyntax[] arguments)
		{
			return SyntaxHelper.CreateMemberAccessCall(target, method, withNullCheck, arguments);
		}

		public static InvocationExpressionSyntax Call(this ExpressionSyntax target, params ArgumentSyntax[] arguments)
		{
			return SyntaxHelper.Call(target, arguments.Where(a => a is not null).ToArray());
		}

		public static ExpressionSyntax Access(this ExpressionSyntax target, string name, bool withNullCheck = false)
		{
			return SyntaxHelper.CreateMemberAccess(target, name, withNullCheck);
		}

		public static ExpressionSyntax Access(this ExpressionSyntax target, SimpleNameSyntax name, bool withNullCheck = false)
		{
			return SyntaxHelper.CreateMemberAccess(target, name, withNullCheck);
		}

		/// <summary>
		/// Creates an element access expression. <see cref="AccessArray"/>, <see cref="AccessList"/>
		/// and <see cref="AccessDictionary"/> are names for the same syntax and exist to make the
		/// intention readable at the call site.
		/// </summary>
		public static ExpressionSyntax AccessElement(this ExpressionSyntax target, params ArgumentSyntax[] arguments)
		{
			return SyntaxHelper.CreateElementAccess(target, arguments);
		}

		/// <inheritdoc cref="AccessElement"/>
		public static ExpressionSyntax AccessArray(this ExpressionSyntax target, params ArgumentSyntax[] arguments)
		{
			return target.AccessElement(arguments);
		}

		/// <inheritdoc cref="AccessElement"/>
		public static ExpressionSyntax AccessList(this ExpressionSyntax target, params ArgumentSyntax[] arguments)
		{
			return target.AccessElement(arguments);
		}

		/// <inheritdoc cref="AccessElement"/>
		public static ExpressionSyntax AccessDictionary(this ExpressionSyntax target, params ArgumentSyntax[] arguments)
		{
			return target.AccessElement(arguments);
		}

		public static IfStatementSyntax If(this ExpressionSyntax condition, params StatementSyntax[] statements)
		{
			return SyntaxHelper.CreateIfStatement(condition, statements);
		}

		public static ConditionalExpressionSyntax ShortIf(this ExpressionSyntax condition, ExpressionSyntax whenTrue, ExpressionSyntax whenFalse)
		{
			return SyntaxHelper.CreateShortIf(condition, whenTrue, whenFalse);
		}

		public static AttributeArgumentSyntax ToAttributeArgument(this ExpressionSyntax expressionSyntax)
		{
			return SyntaxHelper.CreateAttributeArgument(expressionSyntax);
		}

		public static AwaitExpressionSyntax Await(this ExpressionSyntax expressionSyntax)
		{
			return SyntaxHelper.CreateAwaitExpression(expressionSyntax);
		}

		public static IsPatternExpressionSyntax IsNull(this ExpressionSyntax expressionSyntax)
		{
			return SyntaxHelper.CreateIsNullExpression(expressionSyntax);
		}

		public static IsPatternExpressionSyntax IsNotNull(this ExpressionSyntax expressionSyntax)
		{
			return SyntaxHelper.CreateIsNotNullExpression(expressionSyntax);
		}

		public static PrefixUnaryExpressionSyntax Not(this ExpressionSyntax expressionSyntax)
		{
			return SyntaxHelper.CreateNegateExpression(expressionSyntax);
		}

		public static InterpolationSyntax Interpolate(this ExpressionSyntax expressionSyntax)
		{
			return SyntaxHelper.CreateStringInterpolation(expressionSyntax);
		}

		public static ExpressionSyntax ToList(this ExpressionSyntax expressionSyntax)
		{
			return expressionSyntax.Access("ToList").Call();
		}

		public static CastExpressionSyntax Cast(this ExpressionSyntax expressionSyntax, TypeSyntax type)
		{
			return SyntaxHelper.ToCast(expressionSyntax, type);
		}

		public static ForEachStatementSyntax ForEach(this ExpressionSyntax enumerable, string itemName, IEnumerable<StatementSyntax> bodyStatements, IdentifierNameSyntax itemType = null)
		{
			return SyntaxHelper.CreateForEachStatement(enumerable, itemName, bodyStatements, itemType);
		}

		public static ExpressionSyntax Parenthesize(this ExpressionSyntax expressionSyntax)
		{
			return SyntaxHelper.Parenthesize(expressionSyntax);
		}

		public static InitializerExpressionSyntax ToComplexElementInitializerExpression(this ExpressionSyntax expression, params ExpressionSyntax[] expressions)
		{
			var collection = new List<ExpressionSyntax>(expressions.Length + 1)
			{
				expression
			};

			collection.AddRange(expressions);

			return SyntaxHelper.CreateComplexElementInitializerExpression(collection.ToArray());
		}

		public static SwitchStatementSyntax ToSwitchStatement(this ExpressionSyntax variable, params SwitchSectionSyntax[] switchSections)
		{
			return SyntaxHelper.CreateSwitchStatement(variable, switchSections);
		}
	}
}