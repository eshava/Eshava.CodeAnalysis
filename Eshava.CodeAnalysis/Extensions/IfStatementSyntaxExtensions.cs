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

		public static IfStatementSyntax ElseIf(this IfStatementSyntax ifStatement, IfStatementSyntax[] elseIfStatements, params StatementSyntax[] elseStatements)
		{
			var local = elseIfStatements[elseIfStatements.Length - 1];
			if (elseStatements.Length > 0)
			{
				local = SyntaxHelper.CreateElseStatement(elseIfStatements[elseIfStatements.Length - 1], elseStatements);
			}

			for (var i = elseIfStatements.Length - 2; i >= 0; i--)
			{
				local = SyntaxHelper.CreateElseIfStatement(elseIfStatements[i], local);
			}

			return SyntaxHelper.CreateElseIfStatement(ifStatement, local);
		}
	}
}