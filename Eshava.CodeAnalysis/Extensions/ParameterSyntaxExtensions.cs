using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class ParameterSyntaxExtensions
	{
		public static SimpleLambdaExpressionSyntax ToLambdaExpression(this ParameterSyntax parameter)
		{
			return SyntaxHelper.CreateLambdaExpression(parameter);
		}

		public static ParameterSyntax AddThisModifier(this ParameterSyntax parameter)
		{
			return SyntaxHelper.AddModifiers(parameter, SyntaxKind.ThisKeyword);
		}

		/// <summary>
		/// Appends attributes to the attributes the parameter already has.
		/// </summary>
		public static ParameterSyntax AddAttributes(this ParameterSyntax parameter, params AttributeSyntax[] attributes)
		{
			return SyntaxHelper.AddAttributes(parameter, attributes);
		}
	}
}