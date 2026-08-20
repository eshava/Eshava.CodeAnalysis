using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class VariableDeclarationSyntaxExtensions
	{
		public static UsingStatementSyntax Using(this VariableDeclarationSyntax variableDeclaration, IEnumerable<StatementSyntax> statements)
		{
			return SyntaxHelper.CreateUsingStatement(variableDeclaration, statements);
		}
	}
}