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

		/// <summary>
		/// Appends parameters to the parameters the method already has.
		/// </summary>
		public static MethodDeclarationSyntax AddParameter(this MethodDeclarationSyntax methodDeclaration, params ParameterSyntax[] parameters)
		{
			return SyntaxHelper.AddMethodParameter(methodDeclaration, parameters);
		}

		/// <summary>
		/// Appends type parameters to the type parameters the method already has.
		/// </summary>
		public static MethodDeclarationSyntax AddTypeParameter(this MethodDeclarationSyntax methodDeclaration, params TypeParameterSyntax[] typeParameters)
		{
			return SyntaxHelper.AddMethodTypeParameter(methodDeclaration, typeParameters);
		}

		/// <summary>
		/// Appends constraint clauses to the clauses the method already has.
		/// </summary>
		public static MethodDeclarationSyntax AddConstraints(this MethodDeclarationSyntax methodDeclaration, params (string Name, TypeParameterConstraintSyntax[] Constraints)[] constraints)
		{
			return SyntaxHelper.AddConstraints(methodDeclaration, constraints);
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