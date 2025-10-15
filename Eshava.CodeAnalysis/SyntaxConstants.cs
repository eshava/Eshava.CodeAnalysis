using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Eshava.CodeAnalysis
{
	public static class SyntaxConstants
	{
		public static TypeSyntax Object => SF.PredefinedType(SF.Token(SyntaxKind.ObjectKeyword));
		public static TypeSyntax Void => SF.PredefinedType(SF.Token(SyntaxKind.VoidKeyword));
		public static TypeSyntax Bool => SF.PredefinedType(SF.Token(SyntaxKind.BoolKeyword));
		public static TypeSyntax String => SF.PredefinedType(SF.Token(SyntaxKind.StringKeyword));
		public static TypeSyntax Int => SF.PredefinedType(SF.Token(SyntaxKind.IntKeyword));

		public static LiteralExpressionSyntax Default => SF.LiteralExpression(SyntaxKind.DefaultLiteralExpression, SF.Token(SyntaxKind.DefaultKeyword));
		public static LiteralExpressionSyntax True => SF.LiteralExpression(SyntaxKind.TrueLiteralExpression);
		public static LiteralExpressionSyntax False => SF.LiteralExpression(SyntaxKind.FalseLiteralExpression);
		public static LiteralExpressionSyntax Null => SF.LiteralExpression(SyntaxKind.NullLiteralExpression);
		public static ThisExpressionSyntax This => SF.ThisExpression();
		public static ContinueStatementSyntax Continue => SF.ContinueStatement();
		public static BreakStatementSyntax Break => SF.BreakStatement();

		public static BaseExpressionSyntax Base => SF.BaseExpression();
		public static IdentifierNameSyntax NameOf => SF.IdentifierName(SF.Identifier(SF.TriviaList(), SyntaxKind.NameOfKeyword, "nameof", "nameof", SF.TriviaList()));
		
		public static SyntaxToken CommaToken => SF.Token(SyntaxKind.CommaToken);
		public static SyntaxToken SemicolonToken => SF.Token(SyntaxKind.SemicolonToken);

		public static ClassOrStructConstraintSyntax ClassConstraint => SF.ClassOrStructConstraint(SyntaxKind.ClassConstraint);
	}
}