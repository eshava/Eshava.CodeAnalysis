using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class TupleElementSyntaxExtensions
	{
		public static TupleTypeSyntax ToTupleType(this TupleElementSyntax element, params TupleElementSyntax[] elements)
		{
			return SyntaxHelper.CreateTupleType(new[] { element }.Concat(elements).ToArray());
		}
	}
}
