using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Eshava.CodeAnalysis.Extensions
{
	public static class StringExtension
	{
		public static NameColonSyntax ToColon(this string name)
		{
			return SF.NameColon(name);
		}

		public static SyntaxToken ToIdentifier(this string name)
		{
			return SF.Identifier(name);
		}

		public static IdentifierNameSyntax ToIdentifierName(this string name)
		{
			return SF.IdentifierName(name);
		}

		public static AttributeSyntax ToAttribute(this string text)
		{
			return SF.Attribute(text.ToIdentifierName());
		}

		public static ArgumentSyntax ToArgument(this string name)
		{
			return name.ToIdentifierName().ToArgument();
		}

		public static ExpressionSyntax Access(this string name, string memberName, bool withNullCheck = false)
		{
			return name.ToIdentifierName().Access(memberName, withNullCheck);
		}

		public static ExpressionSyntax Access(this string name, SimpleNameSyntax memberName, bool withNullCheck = false)
		{
			return name.ToIdentifierName().Access(memberName, withNullCheck);
		}

		public static ArgumentSyntax ToLiteralArgument(this string text)
		{
			return SF.Argument(text.ToLiteralString());
		}

		public static TypeSyntax ToType(this string typeName, bool removeNullable = false)
		{
			if (removeNullable && typeName.EndsWith("?"))
			{
				typeName = typeName.Substring(0, typeName.Length - 1); 
			}

			return SF.ParseTypeName(typeName);
		}

		public static ParameterSyntax ToParameter(this string parameter)
		{
			return SyntaxHelper.CreateParameter(parameter);
		}

		public static TypeParameterSyntax ToTypeParameter(this string typeName)
		{
			return SF.TypeParameter(typeName.ToIdentifier());
		}

		public static LiteralExpressionSyntax ToLiteralString(this string value)
		{
			return SF.LiteralExpression(SyntaxKind.StringLiteralExpression, SF.Literal(value));
		}

		public static LiteralExpressionSyntax ToLiteralInt(this string value)
		{
			return SF.LiteralExpression(SyntaxKind.NumericLiteralExpression, SF.Literal(Convert.ToInt32(value)));
		}

		public static LiteralExpressionSyntax ToLiteralLong(this string value)
		{
			return SF.LiteralExpression(SyntaxKind.NumericLiteralExpression, SF.Literal(Convert.ToInt64(value)));
		}

		public static LiteralExpressionSyntax ToLiteralBool(this string value)
		{
			return Convert.ToBoolean(value)
				? SF.LiteralExpression(SyntaxKind.TrueLiteralExpression)
				: SF.LiteralExpression(SyntaxKind.FalseLiteralExpression);
		}

		public static MemberAccessExpressionSyntax ToConstantExpression(this string expression)
		{
			var parts = expression.Split('.');

			return SF.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression, SF.IdentifierName(parts[0]), SF.IdentifierName(parts[1]));
		}

		public static FieldDeclarationSyntax ToField(this string name, TypeSyntax type, ExpressionSyntax expression = null)
		{
			return SyntaxHelper.CreateField(type, name, false, false, false, expression);
		}

		public static FieldDeclarationSyntax ToConstField(this string name, TypeSyntax type, ExpressionSyntax expression = null)
		{
			return SyntaxHelper.CreateField(type, name, true, false, false, expression);
		}

		public static FieldDeclarationSyntax ToStaticField(this string name, TypeSyntax type, ExpressionSyntax expression = null)
		{
			return SyntaxHelper.CreateField(type, name, false, false, true, expression);
		}

		public static FieldDeclarationSyntax ToReadonlyField(this string name, TypeSyntax type, ExpressionSyntax expression = null)
		{
			return SyntaxHelper.CreateField(type, name, false, true, false, expression);
		}

		public static FieldDeclarationSyntax ToStaticReadonlyField(this string name, TypeSyntax type, ExpressionSyntax expression = null)
		{
			return SyntaxHelper.CreateField(type, name, false, true, true, expression);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="genericType"></param>
		/// <returns></returns>
		public static GenericNameSyntax AsGeneric(this string name, params TypeSyntax[] genericType)
		{
			return SyntaxHelper.CreateGenericName(name, genericType);
		}

		public static GenericNameSyntax AsGeneric(this string name, params string[] genericType)
		{
			return SyntaxHelper.CreateGenericName(name, genericType);
		}

		public static VariableDeclarationSyntax ToVariable(this string variableName, ExpressionSyntax variableAssignment)
		{
			return SyntaxHelper.CreateVariableDeclaration(variableName, variableAssignment);
		}

		public static LocalDeclarationStatementSyntax ToVariableStatement(this string variableName, ExpressionSyntax variableAssignment)
		{
			return SyntaxHelper.CreateVariableDeclarationStatement(variableName, variableAssignment);
		}

		public static MethodDeclarationSyntax ToMethodDefinition(this string name, TypeSyntax returnType, params SyntaxKind[] modifier)
		{
			return SyntaxHelper.CreateMethodDefinition(
				name,
				returnType,
				modifier
			);
		}

		public static MethodDeclarationSyntax ToMethod(this string name, TypeSyntax returnType, IEnumerable<StatementSyntax> bodyStatements, params SyntaxKind[] modifier)
		{
			return SyntaxHelper.CreateMethod(
				name,
				returnType,
				bodyStatements,
				modifier
			);
		}

		public static PropertyDeclarationSyntax ToProperty(this string propertyName, TypeSyntax type, SyntaxKind modifier, bool addGetter, bool addSetter, SyntaxKind? modifierGetter = null, SyntaxKind? modifierSetter = null, IEnumerable<AttributeSyntax> attributes = null)
		{
			return SyntaxHelper.CreateProperty(propertyName, type, modifier, addGetter, addSetter, modifierGetter, modifierSetter, attributes);
		}

		public static SimpleLambdaExpressionSyntax ToPropertyExpression(this string parameter, string propertyName, bool toIsNullCheck = false, bool toIsNotNullCheck = false)
		{
			return SyntaxHelper.CreatePropertyExpression(parameter, propertyName, toIsNullCheck, toIsNotNullCheck);
		}

		public static InvocationExpressionSyntax Call(this string target, string method, bool withNullCheck = false, params ArgumentSyntax[] arguments)
		{
			return SyntaxHelper.CreateMemberAccessCall(target, method, withNullCheck, arguments);
		}

		public static NamespaceDeclarationSyntax ToNamespace(this string @namespace)
		{
			return SyntaxHelper.CreateNamespace(@namespace);
		}

		public static EnumDeclarationSyntax ToEnumeration(this string enumerationName, IEnumerable<EnumMemberDeclarationSyntax> enumerationMembers, params SyntaxKind[] modifiers)
		{
			return SyntaxHelper.CreateEnumeration(enumerationName, enumerationMembers, modifiers);
		}

		public static EnumMemberDeclarationSyntax ToEnumerationMember(this string memberName, int memberValue)
		{
			return SyntaxHelper.CreateEnumerationMember(memberName, memberValue);
		}

		public static ClassDeclarationSyntax ToClass(this string className, IEnumerable<AttributeSyntax> attributes, params SyntaxKind[] modifiers)
		{
			return SyntaxHelper.CreateClass(className, attributes, modifiers);
		}

		public static InterfaceDeclarationSyntax ToInterface(this string interfaceName, IEnumerable<AttributeSyntax> attributes, params SyntaxKind[] modifiers)
		{
			return SyntaxHelper.CreateInterface(interfaceName, attributes, modifiers);
		}

		public static ConstructorDeclarationSyntax ToContructor(this string name, IEnumerable<NameAndType> parameters, IEnumerable<StatementSyntax> bodyStatements, params SyntaxKind[] modifier)
		{
			return SyntaxHelper.CreateConstructor(name, parameters, bodyStatements, modifier);
		}

		public static SimpleLambdaExpressionSyntax ToParameterExpression(this string parameter, ExpressionSyntax expressionSyntax = null)
		{
			return SyntaxHelper.CreateLambdaExpression(parameter, expressionSyntax);
		}

		public static SimpleLambdaExpressionSyntax ToParameterExpression(this string parameter, params StatementSyntax[] statements)
		{
			return SyntaxHelper.CreateLambdaExpression(parameter, statements);
		}

		public static InterpolatedStringTextSyntax Interpolate(this string text)
		{
			return SyntaxHelper.CreateInterpolatedStringText(text);
		}

		public static SimpleBaseTypeSyntax ToSimpleBaseType(this string type)
		{
			return SyntaxHelper.CreateSimpleBaseType(type.ToType());
		}

		public static DefaultExpressionSyntax DefaultOf(this string type)
		{
			return SyntaxHelper.CreateDefaultOf(type.ToType());
		}
	}
}