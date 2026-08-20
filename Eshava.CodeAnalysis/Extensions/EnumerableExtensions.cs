using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class EnumerableExtensions
	{
		/// <summary>
		/// Creates a multi line raw interpolated string. <paramref name="indentationTabs"/> is the
		/// indentation of the opening quotes in the value text of the start token.
		/// </summary>
		public static InterpolatedStringExpressionSyntax ToRawStringExpression(this IEnumerable<InterpolatedStringContentSyntax> interpolationParts, int indentationTabs = SyntaxHelper.DEFAULT_RAW_STRING_INDENTATION)
		{
			return SyntaxHelper.CreateInterpolatedRawStringExpression(indentationTabs, interpolationParts?.ToArray() ?? Array.Empty<InterpolatedStringContentSyntax>());
		}

		public static InterpolatedStringExpressionSyntax ToStringExpression(this IEnumerable<InterpolatedStringContentSyntax> interpolationParts, bool isMultiline = false)
		{
			return SyntaxHelper.CreateInterpolatedStringExpression(isMultiline, interpolationParts?.ToArray() ?? Array.Empty<InterpolatedStringContentSyntax>());
		}

		public static TryStatementSyntax TryCatch(this IEnumerable<StatementSyntax> tryBlockStatements, IEnumerable<StatementSyntax> catchBlockStatements, string exceptionTypeName = "Exception", string exceptionVariableName = "ex")
		{
			return SyntaxHelper.CreateTryCatchBlock(tryBlockStatements, catchBlockStatements, exceptionTypeName, exceptionVariableName);
		}
	}
}