namespace Eshava.CodeAnalysis.Extensions
{
	public static class ObjectExtensions
	{
		public static T[] AsArray<T>(this T value)
		{
			return new [] { value };
		}
	}
}