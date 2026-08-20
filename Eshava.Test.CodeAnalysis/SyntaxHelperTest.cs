using System;
using System.Collections.Generic;
using System.Linq;
using Eshava.CodeAnalysis;
using Eshava.CodeAnalysis.Extensions;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Eshava.Test.CodeAnalysis
{
	[TestClass, TestCategory("CodeAnalysis")]
	public class SyntaxHelperTest
	{
		[TestMethod]
		public void CreateTokenListWithReadOnlyModifierTest()
		{
			// Act
			var result = SyntaxHelper.CreateTokenList("public", "readonly");

			// Assert
			result.Select(token => token.ValueText).Should().BeEquivalentTo(new[] { "public", "readonly" });
		}

		[TestMethod]
		public void CreateTokenListIsCaseInsensitiveTest()
		{
			// Act
			var result = SyntaxHelper.CreateTokenList("PUBLIC");

			// Assert
			result.Single().IsKind(SyntaxKind.PublicKeyword).Should().BeTrue();
		}

		[TestMethod]
		public void CreateTokenListWithUnknownModifierTest()
		{
			// Act
			var exception = Assert.Throws<ArgumentException>(() => SyntaxHelper.CreateTokenList("public", "readonlx"));

			// Assert
			exception.Message.Should().Contain("readonlx");
		}

		[TestMethod]
		public void CreateTokenListWithKeywordThatIsNoModifierTest()
		{
			// Act, Assert
			Assert.Throws<ArgumentException>(() => SyntaxHelper.CreateTokenList("class"));
		}

		[TestMethod]
		public void CreateSwitchSectionDoesNotModifyStatementsTest()
		{
			// Arrange
			var statements = new List<StatementSyntax>
			{
				SyntaxConstants.True.Return()
			};

			// Act
			var firstSection = SyntaxHelper.CreateSwitchSection([("1".ToLiteralInt(), null)], statements);
			var secondSection = SyntaxHelper.CreateSwitchSection([("2".ToLiteralInt(), null)], statements);

			// Assert
			statements.Should().HaveCount(1);
			firstSection.Statements.Should().HaveCount(2);
			secondSection.Statements.Should().HaveCount(2);
			secondSection.Statements.Last().Should().BeOfType<BreakStatementSyntax>();
		}

		[TestMethod]
		public void CreateDefaultSwitchSectionDoesNotModifyStatementsTest()
		{
			// Arrange
			var statements = new List<StatementSyntax>
			{
				SyntaxConstants.True.Return()
			};

			// Act
			SyntaxHelper.CreateDefaultSwitchSection(statements);
			var section = SyntaxHelper.CreateDefaultSwitchSection(statements);

			// Assert
			statements.Should().HaveCount(1);
			section.Statements.Should().HaveCount(2);
		}

		[TestMethod]
		public void CreateSwitchSectionKeepsExplicitBreakTest()
		{
			// Arrange
			var statements = new List<StatementSyntax>
			{
				SyntaxConstants.Break
			};

			// Act
			var section = SyntaxHelper.CreateDefaultSwitchSection(statements);

			// Assert
			section.Statements.Should().HaveCount(1);
		}

		[TestMethod]
		public void AddMethodParameterAppendsParametersTest()
		{
			// Act
			var result = "Alpha"
				.ToMethodDefinition(SyntaxConstants.Void, SyntaxKind.PublicKeyword)
				.AddParameter("beta".ToParameter())
				.AddParameter("gamma".ToParameter())
				.AddSemicolon();

			// Assert
			result.ParameterList.Parameters.Select(parameter => parameter.Identifier.ValueText)
				.Should().BeEquivalentTo(new[] { "beta", "gamma" });
		}

		[TestMethod]
		public void AddMethodTypeParameterAppendsTypeParametersTest()
		{
			// Act
			var result = "Alpha"
				.ToMethodDefinition(SyntaxConstants.Void, SyntaxKind.PublicKeyword)
				.AddTypeParameter("TBeta".ToTypeParameter())
				.AddTypeParameter("TGamma".ToTypeParameter())
				.AddSemicolon();

			// Assert
			result.TypeParameterList.Parameters.Select(parameter => parameter.Identifier.ValueText)
				.Should().BeEquivalentTo(new[] { "TBeta", "TGamma" });
		}

		[TestMethod]
		public void AddConstraintsSupportsEveryConstraintKindTest()
		{
			// Act
			var result = "Alpha"
				.ToMethodDefinition(SyntaxConstants.Void, SyntaxKind.PublicKeyword)
				.AddTypeParameter("TBeta".ToTypeParameter())
				.AddConstraints(("TBeta", [SyntaxConstants.ClassConstraint, "IGamma".ToType().ToConstraint(), SyntaxConstants.NewConstraint]))
				.AddSemicolon();

			// Assert
			result.NormalizeWhitespace().ToFullString()
				.Should().Contain("where TBeta : class, IGamma, new()");
		}

		[TestMethod]
		public void AddConstraintsAppendsClausesTest()
		{
			// Act
			var result = "Alpha"
				.ToMethodDefinition(SyntaxConstants.Void, SyntaxKind.PublicKeyword)
				.AddTypeParameter("TBeta".ToTypeParameter())
				.AddTypeParameter("TGamma".ToTypeParameter())
				.AddConstraints(("TBeta", [SyntaxConstants.ClassConstraint]))
				.AddConstraints(("TGamma", [SyntaxConstants.StructConstraint]))
				.AddSemicolon();

			// Assert
			result.ConstraintClauses.Should().HaveCount(2);
		}

		[TestMethod]
		public void AddModifiersKeepsExistingModifiersTest()
		{
			// Act
			var result = SyntaxHelper.AddModifiers("alpha".ToParameter(), SyntaxKind.ThisKeyword)
				.AddThisModifier();

			// Assert
			result.Modifiers.Should().HaveCount(2);
		}

		[TestMethod]
		public void CreateTryCatchBlockWithDefaultExceptionTest()
		{
			// Act
			var result = SyntaxHelper.CreateTryCatchBlock([SyntaxConstants.Break], [SyntaxConstants.Continue]);

			// Assert
			result.Catches.Single().Declaration.Type.ToString().Should().Be("Exception");
			result.Catches.Single().Declaration.Identifier.ValueText.Should().Be("ex");
		}

		[TestMethod]
		public void CreateTryCatchBlockWithCustomExceptionTest()
		{
			// Act
			var result = SyntaxHelper.CreateTryCatchBlock([SyntaxConstants.Break], [SyntaxConstants.Continue], "InvalidOperationException", "exception");

			// Assert
			result.Catches.Single().Declaration.Type.ToString().Should().Be("InvalidOperationException");
			result.Catches.Single().Declaration.Identifier.ValueText.Should().Be("exception");
		}

		[TestMethod]
		public void CreateCollectionExpressionWithoutElementsTest()
		{
			// Act
			var result = SyntaxHelper.CreateCollectionExpression();

			// Assert
			result.ToFullString().Should().Be("[]");
		}

		[TestMethod]
		public void CreateSeparatedListWithoutArgumentsTest()
		{
			// Act
			var result = SyntaxHelper.CreateSeparatedList<ArgumentSyntax>(true);

			// Assert
			result.Should().BeEmpty();
		}

		[TestMethod]
		public void CreateSeparatedListWithNullTest()
		{
			// Act
			var result = SyntaxHelper.CreateSeparatedList<ArgumentSyntax>(true, null);

			// Assert
			result.Should().BeEmpty();
		}

		[TestMethod]
		public void CreatePropertyWithoutHardCodedIndentationTest()
		{
			// Act
			var result = "Alpha".ToProperty(SyntaxConstants.String, SyntaxKind.PublicKeyword, true, true);

			// Assert
			result.ToFullString().Should().StartWith("public");
		}

		[TestMethod]
		public void CreateEnumerationWithoutMembersTest()
		{
			// Act
			var result = SyntaxHelper.CreateEnumeration("Alpha", null, SyntaxKind.PublicKeyword);

			// Assert
			result.Members.Should().BeEmpty();
		}

		[TestMethod]
		public void CreateForEachStatementWithoutBodyTest()
		{
			// Act
			var result = SyntaxHelper.CreateForEachStatement("alphas".ToIdentifierName(), "alpha", null);

			// Assert
			result.NormalizeWhitespace().ToFullString().Should().Contain("foreach (var alpha in alphas)");
		}
	}
}
