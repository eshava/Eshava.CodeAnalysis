using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Eshava.CodeAnalysis
{
	/// <summary>
	/// Predefined types and expressions. Syntax nodes are immutable, so every constant is created
	/// once and shared.
	/// </summary>
	public static class SyntaxConstants
	{
		public static readonly TypeSyntax Object = SF.PredefinedType(SF.Token(SyntaxKind.ObjectKeyword));
		public static readonly TypeSyntax Void = SF.PredefinedType(SF.Token(SyntaxKind.VoidKeyword));
		public static readonly TypeSyntax Bool = SF.PredefinedType(SF.Token(SyntaxKind.BoolKeyword));
		public static readonly TypeSyntax String = SF.PredefinedType(SF.Token(SyntaxKind.StringKeyword));
		public static readonly TypeSyntax Int = SF.PredefinedType(SF.Token(SyntaxKind.IntKeyword));

		public static readonly LiteralExpressionSyntax Default = SF.LiteralExpression(SyntaxKind.DefaultLiteralExpression, SF.Token(SyntaxKind.DefaultKeyword));
		public static readonly LiteralExpressionSyntax True = SF.LiteralExpression(SyntaxKind.TrueLiteralExpression);
		public static readonly LiteralExpressionSyntax False = SF.LiteralExpression(SyntaxKind.FalseLiteralExpression);
		public static readonly LiteralExpressionSyntax Null = SF.LiteralExpression(SyntaxKind.NullLiteralExpression);
		public static readonly ThisExpressionSyntax This = SF.ThisExpression();
		public static readonly ContinueStatementSyntax Continue = SF.ContinueStatement();
		public static readonly BreakStatementSyntax Break = SF.BreakStatement();

		public static readonly BaseExpressionSyntax Base = SF.BaseExpression();
		public static readonly IdentifierNameSyntax NameOf = SF.IdentifierName(SF.Identifier(SF.TriviaList(), SyntaxKind.NameOfKeyword, "nameof", "nameof", SF.TriviaList()));

		public static readonly SyntaxToken CommaToken = SF.Token(SyntaxKind.CommaToken);
		public static readonly SyntaxToken SemicolonToken = SF.Token(SyntaxKind.SemicolonToken);

		public static readonly ClassOrStructConstraintSyntax ClassConstraint = SF.ClassOrStructConstraint(SyntaxKind.ClassConstraint);
		public static readonly ClassOrStructConstraintSyntax StructConstraint = SF.ClassOrStructConstraint(SyntaxKind.StructConstraint);
		public static readonly ConstructorConstraintSyntax NewConstraint = SF.ConstructorConstraint();
	}
}