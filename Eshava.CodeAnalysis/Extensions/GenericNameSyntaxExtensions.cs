using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class GenericNameSyntaxExtensions
	{
		public static SimpleBaseTypeSyntax ToSimpleBaseType(this GenericNameSyntax type)
		{
			return SyntaxHelper.CreateSimpleBaseType(type);
		}
	}
}