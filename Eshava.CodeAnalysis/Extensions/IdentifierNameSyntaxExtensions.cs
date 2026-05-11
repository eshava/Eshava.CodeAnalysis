using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class IdentifierNameSyntaxExtensions
	{
		public static ArgumentSyntax ToArgument(this IdentifierNameSyntax name)
		{
			return SyntaxHelper.CreateArgument(name);
		}

		public static TypeOfExpressionSyntax TypeOf(this IdentifierNameSyntax name)
		{
			return SyntaxHelper.CreateTypeOf(name);
		}

		public static TypeSyntax AsNullable(this IdentifierNameSyntax name)
		{
			return SyntaxHelper.CreateNullableType(name);
		}

		public static NameEqualsSyntax ToNameEquals(this IdentifierNameSyntax name)
		{
			return SF.NameEquals(name);
		}

		public static NameColonSyntax ToColon(this IdentifierNameSyntax name)
		{
			return SF.NameColon(name);
		}
	}
}