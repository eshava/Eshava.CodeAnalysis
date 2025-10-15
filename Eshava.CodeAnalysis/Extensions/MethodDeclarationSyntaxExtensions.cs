using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class MethodDeclarationSyntaxExtensions
	{
		public static MethodDeclarationSyntax WithModifiers(this MethodDeclarationSyntax methodDeclaration, IEnumerable<string> modifiers)
		{
			if (!(modifiers?.Any() ?? false))
			{
				return methodDeclaration;
			}

			return methodDeclaration.WithModifiers(SyntaxHelper.CreateTokenList(modifiers.ToArray()));
		}

		public static MethodDeclarationSyntax WithModifiers(this MethodDeclarationSyntax methodDeclaration, params string[] modifiers)
		{
			if (!(modifiers?.Any() ?? false))
			{
				return methodDeclaration;
			}

			return methodDeclaration.WithModifiers(SyntaxHelper.CreateTokenList(modifiers));
		}

		public static MethodDeclarationSyntax WithModifiers(this MethodDeclarationSyntax methodDeclaration, params SyntaxKind[] modifiers)
		{
			if (!(modifiers?.Any() ?? false))
			{
				return methodDeclaration;
			}

			return methodDeclaration.WithModifiers(SyntaxHelper.CreateTokenList(modifiers));
		}

		public static MethodDeclarationSyntax WithParameter(this MethodDeclarationSyntax methodDeclaration, params ParameterSyntax[] parameters)
		{
			return SyntaxHelper.AddMethodParameter(methodDeclaration, parameters);
		}

		public static MethodDeclarationSyntax WithTypeParameter(this MethodDeclarationSyntax methodDeclaration, params TypeParameterSyntax[] typeParameters)
		{
			return SyntaxHelper.AddMethodTypeParameter(methodDeclaration, typeParameters);
		}

		public static MethodDeclarationSyntax WithConstraints(this MethodDeclarationSyntax methodDeclaration, params (string Name, ClassOrStructConstraintSyntax[] Constraints)[] constraints)
		{
			return SyntaxHelper.AddConstaints(methodDeclaration, constraints);
		}

		public static MethodDeclarationSyntax WithExpressionBody(this MethodDeclarationSyntax methodDeclaration, ExpressionSyntax expression)
		{
			return SyntaxHelper.AddExpressionBodyToMethod(methodDeclaration, expression);
		}

		public static MethodDeclarationSyntax AddSemicolon(this MethodDeclarationSyntax methodDeclaration)
		{
			return SyntaxHelper.AddSemicolon(methodDeclaration);
		}
	}
}