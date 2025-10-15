using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class LiteralExpressionSyntaxExtensions
	{
		public static ArgumentSyntax ToArgument(this LiteralExpressionSyntax literalExpressionSyntax)
		{
			return SF.Argument(literalExpressionSyntax);
		}		
	}
}