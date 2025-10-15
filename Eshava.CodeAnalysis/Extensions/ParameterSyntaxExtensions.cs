using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class ParameterSyntaxExtensions
	{
		public static SimpleLambdaExpressionSyntax CreateLambdaExpression(this ParameterSyntax parameter)
		{
			return SyntaxHelper.CreateLambdaExpression(parameter);
		}

		public static ParameterSyntax AddThisModifier(this ParameterSyntax parameter)
		{
			return SyntaxHelper.AddModifiers(parameter, Microsoft.CodeAnalysis.CSharp.SyntaxKind.ThisKeyword);
		}

		public static ParameterSyntax WithAttributes(this ParameterSyntax parameter, params AttributeSyntax[] attributes)
		{
			return SyntaxHelper.AddAttributes(parameter, attributes);
		}
	}
}