using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class CompilationUnitSyntaxExtensions
	{
		public static CompilationUnitSyntax AddUsings(this CompilationUnitSyntax compilationUnit, IEnumerable<string> @usings)
		{
			return compilationUnit.AddUsings(SyntaxHelper.CreateUsings(@usings.OrderBy(@using => @using)));
		}
	}
}