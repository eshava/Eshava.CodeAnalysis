using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class TypeOfExpressionSyntaxExtensions
	{
		public static ExpressionSyntax Name(this TypeOfExpressionSyntax typeOfExpression)
		{
			return typeOfExpression.Access("Name");
		}
	}
}
