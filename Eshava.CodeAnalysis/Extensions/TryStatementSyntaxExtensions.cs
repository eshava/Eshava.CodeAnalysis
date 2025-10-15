using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class TryStatementSyntaxExtensions
	{
		public static TryStatementSyntax Finally(this TryStatementSyntax tryStatement, List<StatementSyntax> finallyBlockStatements)
		{
			return SyntaxHelper.AddFinally(tryStatement, finallyBlockStatements);
		}
	}
}