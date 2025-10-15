using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class InvocationExpressionSyntaxExtensions
	{
		public static InvocationExpressionSyntax WithArguments(this InvocationExpressionSyntax invocation, params ArgumentSyntax[] arguments)
		{
			return invocation.WithArgumentList(
				SyntaxHelper.CreateArgumentList(arguments)
			);
		}

		public static InvocationExpressionSyntax Call(this InvocationExpressionSyntax expression, string method, bool withNullCheck = false, params ArgumentSyntax[] arguments)
		{
			return SyntaxHelper.CreateMemberAccessCall(expression, method.ToIdentifierName(), withNullCheck, arguments);
		}
	}
}