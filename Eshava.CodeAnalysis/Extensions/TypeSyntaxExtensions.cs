using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class TypeSyntaxExtensions
	{
		public static ObjectCreationExpressionSyntax ToInstanceWithInitializer(this TypeSyntax type, params ExpressionSyntax[] expressions)
		{
			return SyntaxHelper.CreateInstance(type, true).WithInitializer(expressions);
		}

		public static ObjectCreationExpressionSyntax ToInstance(this TypeSyntax type, params ArgumentSyntax[] arguments)
		{
			return SyntaxHelper.CreateInstance(type, false, arguments);
		}

		/// <summary>
		/// Creates a collection expression for the elements passed in. A collection expression is
		/// target typed, so <paramref name="type"/> is not part of the generated syntax — it only
		/// keeps the call chain readable.
		/// </summary>
		public static CollectionExpressionSyntax ToCollectionExpressionWithInitializer(this TypeSyntax type, params ExpressionSyntax[] expressions)
		{
			return SyntaxHelper.CreateCollectionExpression(expressions);
		}

		/// <inheritdoc cref="ToCollectionExpressionWithInitializer"/>
		public static CollectionExpressionSyntax ToCollectionExpression(this TypeSyntax type)
		{
			return SyntaxHelper.CreateCollectionExpression();
		}

		public static TupleElementSyntax ToTupleElement(this TypeSyntax typeSyntax)
		{
			return SyntaxHelper.CreateTupleElement(typeSyntax);
		}

		public static SimpleBaseTypeSyntax ToSimpleBaseType(this TypeSyntax type)
		{
			return SyntaxHelper.CreateSimpleBaseType(type);
		}

		public static TypeSyntax AsNullable(this TypeSyntax name)
		{
			return SyntaxHelper.CreateNullableType(name);
		}

		/// <summary>
		/// Creates a type constraint, so a type parameter can be constrained to an interface or a
		/// base class (<c>where T : IAlpha</c>).
		/// </summary>
		public static TypeConstraintSyntax ToConstraint(this TypeSyntax type)
		{
			return SyntaxHelper.CreateTypeConstraint(type);
		}
	}
}