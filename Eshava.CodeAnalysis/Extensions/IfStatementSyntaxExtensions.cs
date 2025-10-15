using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class IfStatementSyntaxExtensions
	{
		public static IfStatementSyntax Else(this IfStatementSyntax ifStatement, params StatementSyntax[] statements)
		{
			return SyntaxHelper.CreateElseStatement(ifStatement, statements);
		}

		public static IfStatementSyntax ElseIf(this IfStatementSyntax ifStatement, ExpressionSyntax elseIfStatement, params StatementSyntax[] statements)
		{
			return SyntaxHelper.CreateElseIfStatement(ifStatement, SyntaxHelper.CreateIfStatement(elseIfStatement, statements));
		}
	}
}