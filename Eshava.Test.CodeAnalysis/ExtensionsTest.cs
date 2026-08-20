using System.Collections.Generic;
using System.Linq;
using Eshava.CodeAnalysis;
using Eshava.CodeAnalysis.Extensions;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Eshava.Test.CodeAnalysis
{
	[TestClass, TestCategory("CodeAnalysis")]
	public class ExtensionsTest
	{
		[TestMethod]
		public void AsTypeWithoutNullableTest()
		{
			// Act
			var result = "alpha".ToIdentifierName().AsType(SyntaxConstants.String, false);

			// Assert
			result.NormalizeWhitespace().ToFullString().Should().Be("alpha as string");
		}

		[TestMethod]
		public void AsTypeWithNullableTest()
		{
			// Act
			var result = "alpha".ToIdentifierName().AsType(SyntaxConstants.String, true);

			// Assert
			result.NormalizeWhitespace().ToFullString().Should().Be("alpha as string?");
		}

		[TestMethod]
		public void AsTypeKeepsAlreadyNullableTypeTest()
		{
			// Act
			var result = "alpha".ToIdentifierName().AsType(SyntaxConstants.String.AsNullable(), true);

			// Assert
			result.NormalizeWhitespace().ToFullString().Should().Be("alpha as string?");
		}

		[TestMethod]
		public void AccessElementTest()
		{
			// Act
			var result = "alphas".ToIdentifierName().AccessElement("0".ToLiteralInt().ToArgument());

			// Assert
			result.NormalizeWhitespace().ToFullString().Should().Be("alphas[0]");
		}

		[TestMethod]
		public void AccessArrayAndAccessElementAreEqualTest()
		{
			// Act
			var element = "alphas".ToIdentifierName().AccessElement("0".ToLiteralInt().ToArgument());
			var array = "alphas".ToIdentifierName().AccessArray("0".ToLiteralInt().ToArgument());

			// Assert
			array.ToFullString().Should().Be(element.ToFullString());
		}

		[TestMethod]
		public void ToCollectionExpressionTest()
		{
			// Act
			var result = "List".AsGeneric("string").ToCollectionExpression();

			// Assert
			result.ToFullString().Should().Be("[]");
		}

		[TestMethod]
		public void ToCollectionExpressionWithInitializerTest()
		{
			// Act
			var result = "List".AsGeneric("string").ToCollectionExpressionWithInitializer("alpha".ToLiteralString());

			// Assert
			result.NormalizeWhitespace().ToFullString().Should().Be("[\"alpha\"]");
		}

		[TestMethod]
		public void ElseIfWithoutStatementsTest()
		{
			// Arrange
			var ifStatement = "alpha".ToIdentifierName().If(SyntaxConstants.Break);

			// Act
			var result = ifStatement.ElseIf([]);

			// Assert
			result.Else.Should().BeNull();
		}

		[TestMethod]
		public void ElseIfWithoutStatementsButWithElseStatementsTest()
		{
			// Arrange
			var ifStatement = "alpha".ToIdentifierName().If(SyntaxConstants.Break);

			// Act
			var result = ifStatement.ElseIf([], SyntaxConstants.Continue);

			// Assert
			result.Else.Should().NotBeNull();
			result.NormalizeWhitespace().ToFullString().Should().Contain("continue;");
		}

		[TestMethod]
		public void ElseIfChainTest()
		{
			// Arrange
			var ifStatement = "alpha".ToIdentifierName().If(SyntaxConstants.Break);
			var elseIfStatements = new[]
			{
				"beta".ToIdentifierName().If(SyntaxConstants.Break),
				"gamma".ToIdentifierName().If(SyntaxConstants.Break)
			};

			// Act
			var result = ifStatement.ElseIf(elseIfStatements, SyntaxConstants.Continue);

			// Assert
			var code = result.NormalizeWhitespace().ToFullString();
			code.Should().Contain("if (alpha)");
			code.Should().Contain("else if (beta)");
			code.Should().Contain("else if (gamma)");
			code.Should().Contain("continue;");
		}

		[TestMethod]
		public void ToSwitchSectionDoesNotModifyStatementsTest()
		{
			// Arrange
			var statements = new List<StatementSyntax>
			{
				SyntaxConstants.True.Return()
			};

			// Act
			statements.ToSwitchSection(("CompareOperator".Access("Contains"), null));
			var section = statements.ToSwitchSection(("CompareOperator".Access("Contains"), null));

			// Assert
			statements.Should().HaveCount(1);
			section.Statements.Should().HaveCount(2);
		}

		[TestMethod]
		public void ToRawStringExpressionWithDefaultIndentationTest()
		{
			// Act
			var result = new List<InterpolatedStringContentSyntax> { "alpha".Interpolate() }.ToRawStringExpression();

			// Assert
			result.StringStartToken.ValueText.Should().Be("$\t\t\t\"\"\"\n");
		}

		[TestMethod]
		public void ToRawStringExpressionWithExplicitIndentationTest()
		{
			// Act
			var result = new List<InterpolatedStringContentSyntax> { "alpha".Interpolate() }.ToRawStringExpression(1);

			// Assert
			result.StringStartToken.ValueText.Should().Be("$\t\"\"\"\n");
		}

		[TestMethod]
		public void ToRawStringExpressionEmittedCodeIsIndependentOfIndentationTest()
		{
			// Arrange
			var parts = new List<InterpolatedStringContentSyntax> { "alpha".Interpolate() };

			// Act
			var withDefaultIndentation = parts.ToRawStringExpression().ToFullString();
			var withExplicitIndentation = parts.ToRawStringExpression(1).ToFullString();

			// Assert
			withExplicitIndentation.Should().Be(withDefaultIndentation);
			withDefaultIndentation.Should().Be("$\"\"\"\nalpha\n\"\"\"");
		}

		[TestMethod]
		public void WithBaseInitializerTest()
		{
			// Act
			var result = "Alpha"
				.ToConstructor(null, null, Microsoft.CodeAnalysis.CSharp.SyntaxKind.PublicKeyword)
				.WithBaseInitializer("beta".ToArgument());

			// Assert
			result.NormalizeWhitespace().ToFullString().Should().Contain(": base(beta)");
		}

		[TestMethod]
		public void WithThisInitializerTest()
		{
			// Act
			var result = "Alpha"
				.ToConstructor(null, null, Microsoft.CodeAnalysis.CSharp.SyntaxKind.PublicKeyword)
				.WithThisInitializer("beta".ToArgument());

			// Assert
			result.NormalizeWhitespace().ToFullString().Should().Contain(": this(beta)");
		}

		[TestMethod]
		public void AddUsingsWithoutUsingsTest()
		{
			// Act
			var result = SyntaxHelper.CreateCompilationUnit().AddUsings((IEnumerable<string>)null);

			// Assert
			result.Usings.Should().BeEmpty();
		}

		[TestMethod]
		public void AddUsingsIsSortedTest()
		{
			// Act
			var result = SyntaxHelper.CreateCompilationUnit().AddUsings(new[] { "System.Linq", "System" });

			// Assert
			result.Usings.Select(@using => @using.Name.ToString())
				.Should().ContainInOrder("System", "System.Linq");
		}

		[TestMethod]
		public void AsArrayTest()
		{
			// Act
			var result = SyntaxConstants.ClassConstraint.AsArray();

			// Assert
			result.Should().HaveCount(1);
		}
	}
}
