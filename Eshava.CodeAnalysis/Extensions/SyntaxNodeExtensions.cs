using Microsoft.CodeAnalysis;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class SyntaxNodeExtensions
	{
		/// <summary>
		/// Wraps a single syntax node into an array. Constrained to syntax nodes on purpose, so the
		/// extension does not show up on every type of a consuming project.
		/// </summary>
		public static T[] AsArray<T>(this T value) where T : SyntaxNode
		{
			return new[] { value };
		}
	}
}