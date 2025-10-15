using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class MemberAccessExpressionSyntaxExtensions
	{
		public static ArgumentSyntax ToArgument(this MemberAccessExpressionSyntax memberAccessExpressionSyntax)
		{
			return SF.Argument(memberAccessExpressionSyntax);
		}
	}
}