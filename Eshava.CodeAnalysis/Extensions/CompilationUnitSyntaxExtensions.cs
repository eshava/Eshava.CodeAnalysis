using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class CompilationUnitSyntaxExtensions
	{
		public static CompilationUnitSyntax AddUsings(this CompilationUnitSyntax compilationUnit, IEnumerable<string> @usings)
		{
			// The comparer is deliberately the default one: switching to StringComparer.Ordinal
			// reorders the using block of every generated file. Worth doing one day, but as a
			// deliberate change with an expected diff, not as a side effect of a null guard.
			return compilationUnit.AddUsings(SyntaxHelper.CreateUsings(@usings?.OrderBy(@using => @using)));
		}
	}
}