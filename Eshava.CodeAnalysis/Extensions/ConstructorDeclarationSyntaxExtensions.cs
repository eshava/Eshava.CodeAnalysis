using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class ConstructorDeclarationSyntaxExtensions
	{
		/// <summary>
		/// Adds a call to the base constructor (<c>: base(…)</c>).
		/// </summary>
		public static ConstructorDeclarationSyntax WithBaseInitializer(this ConstructorDeclarationSyntax constructorDeclaration, params ArgumentSyntax[] arguments)
		{
			return constructorDeclaration.WithInitializer(
				SF.ConstructorInitializer(SyntaxKind.BaseConstructorInitializer, SyntaxHelper.CreateArgumentList(arguments))
			);
		}

		/// <summary>
		/// Adds a call to another constructor of the same type (<c>: this(…)</c>).
		/// </summary>
		public static ConstructorDeclarationSyntax WithThisInitializer(this ConstructorDeclarationSyntax constructorDeclaration, params ArgumentSyntax[] arguments)
		{
			return constructorDeclaration.WithInitializer(
				SF.ConstructorInitializer(SyntaxKind.ThisConstructorInitializer, SyntaxHelper.CreateArgumentList(arguments))
			);
		}
	}
}