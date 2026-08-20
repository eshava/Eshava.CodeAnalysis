using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class StatementSyntaxExtensions
	{
		/// <summary>
		/// Creates a switch section terminated with a break statement. The statements passed in are
		/// not modified.
		/// </summary>
		public static SwitchSectionSyntax ToSwitchSection(this IEnumerable<StatementSyntax> switchStatements, params (ExpressionSyntax Condition, BinaryExpressionSyntax WhenClause)[] switchConditions)
		{
			return SyntaxHelper.CreateSwitchSection(switchConditions, switchStatements);
		}

		/// <inheritdoc cref="ToSwitchSection"/>
		public static SwitchSectionSyntax ToDefaultSwitchSection(this IEnumerable<StatementSyntax> switchStatements)
		{
			return SyntaxHelper.CreateDefaultSwitchSection(switchStatements);
		}
	}
}