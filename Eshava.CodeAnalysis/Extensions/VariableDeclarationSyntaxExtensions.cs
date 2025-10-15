using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class VariableDeclarationSyntaxExtensions
	{
		public static UsingStatementSyntax Using(this VariableDeclarationSyntax variableDeclaration, List<StatementSyntax> statments)
		{
			return SyntaxHelper.CreateUsingStatement(variableDeclaration, statments);
		}
	}
}