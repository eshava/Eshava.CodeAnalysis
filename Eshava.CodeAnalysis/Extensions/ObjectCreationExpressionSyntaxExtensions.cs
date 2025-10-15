using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class ObjectCreationExpressionSyntaxExtensions
	{
		public static ObjectCreationExpressionSyntax WithInitializer(this ObjectCreationExpressionSyntax objectCreationExpression, params ExpressionSyntax[] expressions)
		{
			return SyntaxHelper.WithInitializer(objectCreationExpression, expressions);
		}
	}
}
