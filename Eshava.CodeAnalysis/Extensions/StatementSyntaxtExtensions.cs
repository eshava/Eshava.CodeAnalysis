using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class StatementSyntaxtExtensions
	{
		public static SwitchSectionSyntax ToSwitchSection(this List<StatementSyntax> switchStatements, params (ExpressionSyntax Condition, BinaryExpressionSyntax WhenClause)[] switchConditions)
		{
			return SyntaxHelper.CreateSwitchSection(switchConditions, switchStatements);
		}

		public static SwitchSectionSyntax ToDefaultSwitchSection(this List<StatementSyntax> switchStatements)
		{
			return SyntaxHelper.CreateDefaultSwitchSection(switchStatements);
		}
	}
}