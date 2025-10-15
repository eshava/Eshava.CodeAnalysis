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

		public static CollectionExpressionSyntax ToCollectionExpressionWithInitializer(this TypeSyntax type, params ExpressionSyntax[] expressions)
		{
			return SyntaxHelper.CreateCollectionExpression(type, expressions);
		}

		public static CollectionExpressionSyntax ToCollectionExpression(this TypeSyntax type)
		{
			return SyntaxHelper.CreateCollectionExpression(type);
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
	}
}