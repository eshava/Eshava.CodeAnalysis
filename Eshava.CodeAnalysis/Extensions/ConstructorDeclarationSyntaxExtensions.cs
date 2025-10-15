using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class ConstructorDeclarationSyntaxExtensions
	{
		public static ConstructorDeclarationSyntax WithInitializer(this ConstructorDeclarationSyntax constructorDeclaration, params ArgumentSyntax[] arguments)
		{
			return constructorDeclaration.WithInitializer(
				SF.ConstructorInitializer(SyntaxKind.BaseConstructorInitializer, SyntaxHelper.CreateArgumentList(arguments))
			);
		}
	}
}