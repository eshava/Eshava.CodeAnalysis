using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class PropertyDeclarationSyntaxExtensions
	{
		public static PropertyDeclarationSyntax AddGetAccessor(this PropertyDeclarationSyntax property, SyntaxKind? modifier = null)
		{
			return property.AddAccessor(SyntaxKind.GetAccessorDeclaration, modifier);
		}

		public static PropertyDeclarationSyntax AddSetAccessor(this PropertyDeclarationSyntax property, SyntaxKind? modifier = null)
		{
			return property.AddAccessor(SyntaxKind.SetAccessorDeclaration, modifier);
		}

		private static PropertyDeclarationSyntax AddAccessor(this PropertyDeclarationSyntax property, SyntaxKind accessortype, SyntaxKind? modifier = null)
		{
			var accessorDeclaration = SF.AccessorDeclaration(accessortype);
			if (modifier.HasValue)
			{
				accessorDeclaration = accessorDeclaration
					.WithModifiers(SyntaxHelper.CreateTokenList(modifier.Value));
			}

			accessorDeclaration = accessorDeclaration
				.WithSemicolonToken(SF.Token(SyntaxKind.SemicolonToken)
					.WithTrailingTrivia(SF.Space)
				);

			return property.AddAccessorListAccessors(accessorDeclaration);
		}

		public static PropertyDeclarationSyntax WithExpressionBody(this PropertyDeclarationSyntax propertyDeclaration, ExpressionSyntax expression)
		{
			return SyntaxHelper.AddExpressionBodyToProperty(propertyDeclaration, expression);
		}
	}
}