using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class EnumerableExtensions
	{
		public static InterpolatedStringExpressionSyntax ToRawStringExpression(this IEnumerable<InterpolatedStringContentSyntax> interpolationParts)
		{
			return SyntaxHelper.CreateInterpolatedRawStringExpression(interpolationParts.ToArray());
		}

		public static InterpolatedStringExpressionSyntax ToStringExpression(this IEnumerable<InterpolatedStringContentSyntax> interpolationParts, bool isMultiline = false)
		{
			return SyntaxHelper.CreateInterpolatedStringExpression(isMultiline, interpolationParts.ToArray());
		}

		public static TryStatementSyntax TryCatch(this IEnumerable<StatementSyntax> tryBlockStatements, IEnumerable<StatementSyntax> catchBlockStatements)
		{
			return SyntaxHelper.CreateTryCatchBlock(tryBlockStatements, catchBlockStatements);
		}
	}
}