using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Eshava.CodeAnalysis
{
	public class NameAndType
	{
		public NameAndType()
		{

		}

		public NameAndType(string name, TypeSyntax type)
		{
			Name = name;
			Type = type;
		}

		public string Name { get; set; }
		public TypeSyntax Type { get; set; }
	}
}