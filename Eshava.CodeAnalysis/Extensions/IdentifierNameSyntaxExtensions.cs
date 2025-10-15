using Microsoft.CodeAnalysis.CSharp.Syntax;

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
	}
}