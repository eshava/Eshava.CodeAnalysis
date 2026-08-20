using System;
using Eshava.CodeAnalysis;
using Eshava.CodeAnalysis.Extensions;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Eshava.Test.CodeAnalysis
{
	[TestClass, TestCategory("CodeAnalysis")]
	public class StringExtensionsTest
	{
		[TestMethod]
		public void CallWithNullCheckTest()
		{
			// Act
			var result = "input".Call("ToList", true);

			// Assert
			result.NormalizeWhitespace().ToFullString().Should().Be("input?.ToList()");
		}

		[TestMethod]
		public void CallWithoutNullCheckTest()
		{
			// Act
			var result = "input".Call("ToList");

			// Assert
			result.NormalizeWhitespace().ToFullString().Should().Be("input.ToList()");
		}

		[TestMethod]
		public void CallWithNullCheckAndArgumentsTest()
		{
			// Act
			var result = "input".Call("Contains", true, "alpha".ToLiteralArgument());

			// Assert
			result.NormalizeWhitespace().ToFullString().Should().Be("input?.Contains(\"alpha\")");
		}

		[TestMethod]
		public void ToConstantExpressionWithTwoSegmentsTest()
		{
			// Act
			var result = "Alpha.Beta".ToConstantExpression();

			// Assert
			result.ToFullString().Should().Be("Alpha.Beta");
		}

		[TestMethod]
		public void ToConstantExpressionWithThreeSegmentsTest()
		{
			// Act
			var result = "Alpha.Beta.Gamma".ToConstantExpression();

			// Assert
			result.ToFullString().Should().Be("Alpha.Beta.Gamma");
		}

		[TestMethod]
		public void ToConstantExpressionWithSingleSegmentTest()
		{
			// Act
			var exception = Assert.Throws<ArgumentException>(() => "Alpha".ToConstantExpression());

			// Assert
			exception.Message.Should().Contain("Alpha");
		}

		[TestMethod]
		public void ToLiteralIntTest()
		{
			// Act
			var result = "42".ToLiteralInt();

			// Assert
			result.Token.Value.Should().Be(42);
		}

		[TestMethod]
		public void ToLiteralLongTest()
		{
			// Act
			var result = "4200000000".ToLiteralLong();

			// Assert
			result.Token.Value.Should().Be(4200000000L);
		}

		[TestMethod]
		public void ToLiteralBoolTest()
		{
			// Act
			var result = "true".ToLiteralBool();

			// Assert
			result.IsKind(SyntaxKind.TrueLiteralExpression).Should().BeTrue();
		}

		[TestMethod]
		public void ToConstructorTest()
		{
			// Act
			var result = "Alpha".ToConstructor(
				[new NameAndType("beta", SyntaxConstants.String)],
				null,
				SyntaxKind.PublicKeyword
			);

			// Assert
			result.NormalizeWhitespace().ToFullString().Should().Contain("public Alpha(string beta)");
		}

		[TestMethod]
		public void ToTypeWithoutNullableTest()
		{
			// Act
			var result = "Alpha?".ToType(true);

			// Assert
			result.ToFullString().Should().Be("Alpha");
		}
	}
}
