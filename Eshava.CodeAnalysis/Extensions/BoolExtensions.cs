using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class BoolExtensions
	{
		public static LiteralExpressionSyntax ToExpression(this bool isTrue)
		{
			return isTrue
				? SyntaxConstants.True
				: SyntaxConstants.False;
		}

		public static ReturnStatementSyntax ToReturnStatement(this bool isTrue)
		{
			return isTrue.ToExpression().Return();
		}
	}
}