using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class ArgumentSyntaxtExtensions
	{
		public static ArgumentSyntax WithName(this ArgumentSyntax argumentSyntax, string name)
		{
			return SyntaxHelper.AddArgumentName(argumentSyntax, name);
		}

		public static TupleExpressionSyntax ToTuple(this ArgumentSyntax element, params ArgumentSyntax[] elements)
		{
			return SyntaxHelper.CreateTuple(new[] { element }.Concat(elements).ToArray());
		}
	}
}
