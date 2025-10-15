using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class AttributeSyntaxExtensions
	{
		public static AttributeSyntax WithArguments(this AttributeSyntax attribute, params AttributeArgumentSyntax[] arguments)
		{
			return attribute.WithArgumentList(SyntaxHelper.CreateArgumentList(arguments));
		}
	}
}