using System;
using System.Collections.Generic;
using System.Linq;
using Eshava.CodeAnalysis.Extensions;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SF = Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

namespace Eshava.CodeAnalysis
{
	public static class SyntaxHelper
	{
		public static CompilationUnitSyntax CreateCompilationUnit()
		{
			return SF.CompilationUnit();
		}

		public static NamespaceDeclarationSyntax CreateNamespace(string @namespace)
		{
			return SF.NamespaceDeclaration(SF.ParseName(@namespace)).NormalizeWhitespace();
		}

		public static EnumDeclarationSyntax CreateEnumeration(string enumerationName, IEnumerable<EnumMemberDeclarationSyntax> enumMembers, params SyntaxKind[] modifiers)
		{
			var enumDeclaration = SF.EnumDeclaration(enumerationName)
				.WithModifiers(CreateTokenList(modifiers))
				.WithMembers(CreateSeparatedList(true, enumMembers.ToArray()));

			return enumDeclaration;
		}

		public static ClassDeclarationSyntax CreateClass(string className, IEnumerable<AttributeSyntax> attributes, params SyntaxKind[] modifiers)
		{
			var classDeclaration = SF.ClassDeclaration(className)
				.WithModifiers(CreateTokenList(modifiers));

			if (attributes?.Any() ?? false)
			{
				var attributeLists = attributes.Select(attribute => SF.AttributeList(SF.SingletonSeparatedList(attribute))).ToList();
				classDeclaration = classDeclaration.WithAttributeLists(SF.List(attributeLists));
			}

			return classDeclaration;
		}

		public static InterfaceDeclarationSyntax CreateInterface(string interfaceName, IEnumerable<AttributeSyntax> attributes, params SyntaxKind[] modifiers)
		{
			var interfaceDeclaration = SF.InterfaceDeclaration(interfaceName)
				.WithModifiers(CreateTokenList(modifiers));

			if (attributes?.Any() ?? false)
			{
				var attributeLists = attributes.Select(attribute => SF.AttributeList(SF.SingletonSeparatedList(attribute))).ToList();
				interfaceDeclaration = interfaceDeclaration.WithAttributeLists(SF.List(attributeLists));
			}

			return interfaceDeclaration;
		}

		public static MethodDeclarationSyntax AddConstaints(MethodDeclarationSyntax methodDeclaration, params (string Name, ClassOrStructConstraintSyntax[] Constraints)[] constraints)
		{
			var typeParameterConstraints = constraints.Select(
				c => SF.TypeParameterConstraintClause(c.Name.ToIdentifierName())
					.WithConstraints(CreateSeparatedList<TypeParameterConstraintSyntax>(true, c.Constraints))
			);

			return methodDeclaration
				.WithConstraintClauses(SF.List(typeParameterConstraints));
		}

		public static MethodDeclarationSyntax AddSemicolon(MethodDeclarationSyntax methodDeclaration)
		{
			return methodDeclaration.WithSemicolonToken(SyntaxConstants.SemicolonToken);
		}

		public static EqualsValueClauseSyntax CreateEqualsValueClause(ExpressionSyntax expression)
		{
			return SF.EqualsValueClause(expression);
		}

		public static SimpleBaseTypeSyntax CreateSimpleBaseType(TypeSyntax type)
		{
			return SF.SimpleBaseType(type);
		}

		public static GenericNameSyntax CreateGenericName(string name, params TypeSyntax[] genericType)
		{
			return SF.GenericName(SF.Identifier(name))
				.WithTypeArgumentList(SF.TypeArgumentList(CreateSeparatedList(true, genericType)));
		}

		public static GenericNameSyntax CreateGenericName(string name, params string[] genericTypeNames)
		{
			var genericTypes = genericTypeNames.Select(t => t.ToType()).ToArray();

			return CreateGenericName(name, genericTypes);
		}

		public static ArgumentSyntax CreateArgument(ExpressionSyntax expression)
		{
			return SF.Argument(expression);
		}

		public static AttributeArgumentSyntax CreateAttributeArgument(ExpressionSyntax expression)
		{
			return SF.AttributeArgument(expression);
		}

		public static ArgumentListSyntax CreateArgumentList()
		{
			return SF.ArgumentList();
		}

		public static ArgumentListSyntax CreateArgumentList(params string[] stringLiterals)
		{
			return CreateArgumentList(stringLiterals.Select(literal => literal.ToLiteralArgument()).ToArray());
		}

		public static ArgumentListSyntax CreateArgumentList(params ArgumentSyntax[] arguments)
		{
			return SF.ArgumentList(CreateSeparatedList(true, arguments));
		}

		public static TypeParameterListSyntax CreateArgumentList(params TypeParameterSyntax[] arguments)
		{
			return SF.TypeParameterList(CreateSeparatedList(true, arguments));
		}

		public static AttributeArgumentListSyntax CreateArgumentList(params AttributeArgumentSyntax[] arguments)
		{
			return SF.AttributeArgumentList(CreateSeparatedList(true, arguments));
		}

		public static SeparatedSyntaxList<T> CreateSeparatedList<T>(bool withLineBreak, params T[] arguments) where T : CSharpSyntaxNode
		{
			if (arguments.Length == 1)
			{
				return SF.SingletonSeparatedList<T>(arguments[0]);
			}

			return SF.SeparatedList<T>(CreateSyntaxNodeOrTokenArray(withLineBreak, arguments));
		}

		public static SyntaxNodeOrToken[] CreateSyntaxNodeOrTokenArray<T>(bool withLineBreak, params T[] arguments) where T : CSharpSyntaxNode
		{
			var syntaxNodeOrToken = new List<SyntaxNodeOrToken>();

			for (var i = 0; i < arguments.Length; i++)
			{
				if (i > 0)
				{
					if (withLineBreak)
					{
						syntaxNodeOrToken.Add(SF.Token(SF.TriviaList(), SyntaxKind.CommaToken, SF.TriviaList(SF.LineFeed)));
					}
					else
					{
						syntaxNodeOrToken.Add(SF.Token(SyntaxKind.CommaToken));
					}
				}

				syntaxNodeOrToken.Add(arguments[i]);
			}

			return syntaxNodeOrToken.ToArray();
		}

		public static VariableDeclarationSyntax CreateVarStatement()
		{
			return SF.VariableDeclaration(
				SF.IdentifierName(
					SF.Identifier(
						SF.TriviaList(),
						SyntaxKind.VarKeyword,
						"var",
						"var",
						SF.TriviaList()
					)
				)
			);
		}

		public static FieldDeclarationSyntax CreateField(TypeSyntax type, string name, bool isConstant, bool isReadOnly, bool isStatic, ExpressionSyntax valueAssignment)
		{
			var variableNameDeclarator = SF.VariableDeclarator(name);

			if (valueAssignment is not null)
			{
				variableNameDeclarator = variableNameDeclarator
					.WithInitializer(SF.EqualsValueClause(valueAssignment));
			}

			var variableDeclaration = SF.VariableDeclaration(type)
						.AddVariables(variableNameDeclarator);

			var accessTokens = new List<SyntaxToken>
			{
				SF.Token(SyntaxKind.PrivateKeyword)
			};

			if (isConstant)
			{
				accessTokens.Add(SF.Token(SyntaxKind.ConstKeyword));
			}
			else
			{
				if (isReadOnly)
				{
					accessTokens.Add(SF.Token(SyntaxKind.ReadOnlyKeyword));
				}

				if (isStatic)
				{
					accessTokens.Add(SF.Token(SyntaxKind.StaticKeyword));
				}
			}

			var fieldDeclaration = SF.FieldDeclaration(variableDeclaration)
				.AddModifiers(accessTokens.ToArray());

			return fieldDeclaration;
		}

		public static IfStatementSyntax CreateIfStatement(ExpressionSyntax condition, params StatementSyntax[] statements)
		{
			var block = statements.Length == 0
				? SF.Block()
				: SF.Block(SF.List<StatementSyntax>(statements));

			return SF.IfStatement(
					condition,
					block
			);
		}

		public static IfStatementSyntax CreateElseIfStatement(IfStatementSyntax ifStatement, IfStatementSyntax elseIifStatement)
		{
			return ifStatement.WithElse(
				SF.ElseClause(elseIifStatement)
			);
		}

		public static IfStatementSyntax CreateElseStatement(IfStatementSyntax ifStatement, params StatementSyntax[] statements)
		{
			var block = statements.Length == 0
				? SF.Block()
				: SF.Block(SF.List<StatementSyntax>(statements));

			return ifStatement.WithElse(
				SF.ElseClause(block)
			);
		}

		public static ReturnStatementSyntax ToReturn(ExpressionSyntax statement)
		{
			return SF.ReturnStatement(statement);
		}

		public static ExpressionSyntax Assign(ExpressionSyntax left, ExpressionSyntax right)
		{
			return SF.AssignmentExpression(
				SyntaxKind.SimpleAssignmentExpression,
				left,
				right
			);
		}

		public static ExpressionSyntax AddAssign(ExpressionSyntax left, ExpressionSyntax right)
		{
			return SF.AssignmentExpression(
				SyntaxKind.AddAssignmentExpression,
				left,
				right
			);
		}

		public static ExpressionSyntax SubtractAssign(ExpressionSyntax left, ExpressionSyntax right)
		{
			return SF.AssignmentExpression(
				SyntaxKind.SubtractAssignmentExpression,
				left,
				right
			);
		}

		public static ExpressionSyntax MultiplyAssign(ExpressionSyntax left, ExpressionSyntax right)
		{
			return SF.AssignmentExpression(
				SyntaxKind.MultiplyAssignmentExpression,
				left,
				right
			);
		}

		public static ExpressionSyntax DivideAssign(ExpressionSyntax left, ExpressionSyntax right)
		{
			return SF.AssignmentExpression(
				SyntaxKind.DivideAssignmentExpression,
				left,
				right
			);
		}

		public static ExpressionSyntax ModuloAssign(ExpressionSyntax left, ExpressionSyntax right)
		{
			return SF.AssignmentExpression(
				SyntaxKind.ModuloAssignmentExpression,
				left,
				right
			);
		}

		public static ExpressionSyntax AndAssign(ExpressionSyntax left, ExpressionSyntax right)
		{
			return SF.AssignmentExpression(
				SyntaxKind.AndAssignmentExpression,
				left,
				right
			);
		}

		public static ExpressionSyntax OrAssign(ExpressionSyntax left, ExpressionSyntax right)
		{
			return SF.AssignmentExpression(
				SyntaxKind.OrAssignmentExpression,
				left,
				right
			);
		}

		public static BinaryExpressionSyntax AsType(ExpressionSyntax expression, TypeSyntax type, bool toNullableType)
		{
			return CreateBinaryExpression(
				expression,
				((type is NullableTypeSyntax) || !toNullableType) ? type : SF.NullableType(type),
				SyntaxKind.AsExpression
			);
		}

		public static BinaryExpressionSyntax CreateBinaryExpression(this ExpressionSyntax left, ExpressionSyntax right, SyntaxKind kind)
		{
			return SF.BinaryExpression(
				kind,
				left,
				right
			);
		}

		public static PrefixUnaryExpressionSyntax CreateNegateExpression(this ExpressionSyntax expression)
		{
			return SF.PrefixUnaryExpression(
				SyntaxKind.LogicalNotExpression,
				expression
			);
		}

		public static ExpressionSyntax AddNullFallback(ExpressionSyntax expression, ExpressionSyntax nullFallback)
		{
			return SF.BinaryExpression(
				SyntaxKind.CoalesceExpression,
				expression,
				nullFallback
			);
		}

		public static VariableDeclarationSyntax CreateVariableDeclaration(string variableName, ExpressionSyntax variableAssignment)
		{
			var varStatement = CreateVarStatement();
			var variableDeclarator = SF.VariableDeclarator(variableName.ToIdentifier());

			if (variableAssignment is not null)
			{
				variableDeclarator = variableDeclarator
					.WithInitializer(
						SF.EqualsValueClause(variableAssignment)
					);
			}

			return varStatement.WithVariables(
				SF.SingletonSeparatedList<VariableDeclaratorSyntax>(
					variableDeclarator
				)
			);
		}

		public static LocalDeclarationStatementSyntax CreateVariableDeclarationStatement(string variableName, ExpressionSyntax variableAssignment)
		{
			return SF.LocalDeclarationStatement(
				CreateVariableDeclaration(variableName, variableAssignment)
			);
		}

		public static ExpressionStatementSyntax CreateExpressionStatement(ExpressionSyntax expression)
		{
			return SF.ExpressionStatement(expression);
		}

		public static InvocationExpressionSyntax Call(ExpressionSyntax target, params ArgumentSyntax[] arguments)
		{
			var invocation = SF.InvocationExpression(target);

			if (arguments.Length > 0)
			{
				invocation = invocation
					.WithArgumentList(CreateArgumentList(arguments));
			}

			return invocation;
		}

		public static InvocationExpressionSyntax CreateMemberAccessCall(string target, string method, bool withNullCheck = false, params ArgumentSyntax[] arguments)
		{
			return CreateMemberAccessCall(
				target.ToIdentifierName(),
				method.ToIdentifierName(),
				false,
				arguments
			);
		}

		public static InvocationExpressionSyntax CreateMemberAccessCall(ExpressionSyntax target, SimpleNameSyntax method, bool withNullCheck = false, params ArgumentSyntax[] arguments)
		{
			var methodCall = SF.InvocationExpression(CreateMemberAccess(target, method, withNullCheck));

			// SF.LineFeed doesn't work or is ignored by the formatter
			//var argumentList = arguments.Length > 0
			//	? CreateArgumentList(arguments)
			//	: SF.ArgumentList()
			//	;

			//argumentList = argumentList.WithCloseParenToken(SF.Token(SF.TriviaList(), SyntaxKind.CloseParenToken, SF.TriviaList(SF.LineFeed)));

			//methodCall = methodCall
			//	.WithArgumentList(argumentList);

			if (arguments.Length > 0)
			{
				var argumentList = CreateArgumentList(arguments);

				methodCall = methodCall
					.WithArgumentList(argumentList);
			}

			return methodCall;
		}

		public static ExpressionSyntax CreateMemberAccess(ExpressionSyntax expression, SimpleNameSyntax name, bool withNullCheck = false)
		{
			if (withNullCheck)
			{
				return SF.ConditionalAccessExpression(
					expression,
					SF.MemberBindingExpression(name)
				);
			}

			return SF.MemberAccessExpression(
				SyntaxKind.SimpleMemberAccessExpression,
				expression,
				name
			);
		}

		public static ExpressionSyntax CreateEnumerableAccess(ExpressionSyntax target, params ArgumentSyntax[] arguments)
		{
			return SF.ElementAccessExpression(target)
				.WithArgumentList(
					SF.BracketedArgumentList(
						CreateSeparatedList(false, arguments)
					)
				);
		}

		public static ConditionalExpressionSyntax CreateShortIf(ExpressionSyntax condition, ExpressionSyntax whenTrue, ExpressionSyntax whenFalse)
		{
			return SF.ConditionalExpression(condition, whenTrue, whenFalse);
		}

		public static ExpressionSyntax CreateMemberAccess(ExpressionSyntax expression, string name, bool withNullCheck = false)
		{
			return CreateMemberAccess(expression, name.ToIdentifierName(), withNullCheck);
		}

		public static ExpressionSyntax CreateMemberAccess(string expression, string name, bool withNullCheck = false)
		{
			return CreateMemberAccess(expression.ToIdentifierName(), name.ToIdentifierName(), withNullCheck);
		}

		public static SimpleLambdaExpressionSyntax CreateLambdaExpression(string parameter, ExpressionSyntax expressionSyntax = null)
		{
			var lambdaExpression = CreateLambdaExpression(CreateParameter(parameter));

			if (expressionSyntax is null)
			{
				return lambdaExpression;
			}

			return lambdaExpression.WithExpressionBody(expressionSyntax);
		}

		public static SimpleLambdaExpressionSyntax CreateLambdaExpression(string parameter, params StatementSyntax[] statements)
		{
			var lambdaExpression = CreateLambdaExpression(CreateParameter(parameter));

			if (statements.Length == 0)
			{
				return lambdaExpression;
			}

			return lambdaExpression.WithBlock(SF.Block(statements));
		}

		public static ParameterSyntax AddModifiers(ParameterSyntax parameter, params SyntaxKind[] kinds)
		{
			return parameter
				.WithModifiers(
					SF.TokenList(kinds.Select(SF.Token))
				);
		}

		public static ParameterSyntax AddAttributes(ParameterSyntax parameter, params AttributeSyntax[] attributes)
		{
			if (attributes.Length == 0)
			{
				return parameter;
			}

			return parameter.WithAttributeLists(
				SF.List(
					attributes
						.Select(a => SF.AttributeList(SF.SingletonSeparatedList(a)))
						.ToArray()
				)
			);
		}

		public static SimpleLambdaExpressionSyntax CreateLambdaExpression(ParameterSyntax parameter)
		{
			return SF.SimpleLambdaExpression(parameter);
		}

		public static TypeOfExpressionSyntax CreateTypeOf(TypeSyntax type)
		{
			return SF.TypeOfExpression(type);
		}

		public static DefaultExpressionSyntax CreateDefaultOf(TypeSyntax type)
		{
			return SF.DefaultExpression(type);
		}

		public static TypeSyntax CreateNullableType(TypeSyntax type)
		{
			return SF.NullableType(type);
		}

		public static ArgumentSyntax AddArgumentName(ArgumentSyntax argumentSyntax, string name)
		{
			return argumentSyntax.WithNameColon(SF.NameColon(SF.IdentifierName(name)));
		}

		public static UsingStatementSyntax CreateUsingStatement(VariableDeclarationSyntax variableDeclaration, IEnumerable<StatementSyntax> statments)
		{
			return SF.UsingStatement(SF.Block(statments))
				.WithDeclaration(variableDeclaration);
		}

		public static TryStatementSyntax CreateTryCatchBlock(IEnumerable<StatementSyntax> tryBlockStatements, IEnumerable<StatementSyntax> catchBlockStatements)
		{
			return SF.TryStatement(
				SF.SingletonList<CatchClauseSyntax>(
					SF.CatchClause()
					.WithDeclaration(
						SF.CatchDeclaration(
							SF.IdentifierName("Exception"))
						.WithIdentifier(
							SF.Identifier("ex")))
					.WithBlock(
						SF.Block(catchBlockStatements))))
				.WithBlock(SF.Block(tryBlockStatements));
		}

		public static TryStatementSyntax AddFinally(TryStatementSyntax tryStatement, IEnumerable<StatementSyntax> finallyBlockStatements)
		{
			return tryStatement.WithFinally(
				SF.FinallyClause(
					SF.Block(finallyBlockStatements)
				)
			);
		}

		public static SimpleLambdaExpressionSyntax CreatePropertyExpression(string parameter, string property, bool toIsNullCheck = false, bool toIsNotNullCheck = false)
		{
			var member = CreateMemberAccess(parameter, property);
			if (toIsNullCheck)
			{
				member = CreateIsNullExpression(member);
			}
			else if (toIsNotNullCheck)
			{
				member = CreateIsNotNullExpression(member);
			}

			return SF.SimpleLambdaExpression(CreateParameter(parameter))
				.WithExpressionBody(member);
		}

		public static ParameterSyntax CreateParameter(string parameter)
		{
			return SF.Parameter(SF.Identifier(parameter));
		}

		public static ObjectCreationExpressionSyntax CreateInstance(TypeSyntax type, bool withoutArguments, params ArgumentSyntax[] arguments)
		{
			var instance = SF.ObjectCreationExpression(type);

			if (!withoutArguments)
			{
				instance = instance
					.WithArgumentList(CreateArgumentList(arguments));
			}

			return instance;
		}

		public static ObjectCreationExpressionSyntax WithInitializer(ObjectCreationExpressionSyntax objectCreationExpression, params ExpressionSyntax[] expressions)
		{
			return objectCreationExpression
				.WithInitializer(
					SF.InitializerExpression(
						SyntaxKind.CollectionInitializerExpression,
						CreateSeparatedList<ExpressionSyntax>(true, expressions)
					)
				);
		}

		public static CollectionExpressionSyntax CreateCollectionExpression(this TypeSyntax type, params ExpressionSyntax[] expressions)
		{
			if (expressions.Length == 0)
			{
				return SF.CollectionExpression();
			}

			if (expressions.Length == 1)
			{
				return SF.CollectionExpression(SF.SingletonSeparatedList<CollectionElementSyntax>(SF.ExpressionElement(expressions[0])));
			}

			return SF.CollectionExpression(
				SF.SeparatedList<CollectionElementSyntax>(
					CreateSyntaxNodeOrTokenArray(true, expressions.Select(SF.ExpressionElement).ToArray())
				)
			);
		}

		public static InitializerExpressionSyntax CreateComplexElementInitializerExpression(ExpressionSyntax[] expressions)
		{
			return SF.InitializerExpression(SyntaxKind.ComplexElementInitializerExpression, CreateSeparatedList(false, expressions));
		}

		public static ConstructorDeclarationSyntax CreateConstructor(string name, IEnumerable<NameAndType> parameters, IEnumerable<StatementSyntax> bodyStatements, params SyntaxKind[] modifier)
		{
			var constructorDeclaration = SF.ConstructorDeclaration(SF.Identifier(name))
				.WithBody(SF.Block());

			if (modifier.Any())
			{
				constructorDeclaration = constructorDeclaration.WithModifiers(CreateTokenList(modifier));
			}

			if (parameters?.Any() ?? false)
			{
				var parameterList = parameters.Select(p => SF.Parameter(SF.Identifier(p.Name)).WithType(p.Type)).ToArray();

				constructorDeclaration = constructorDeclaration.WithParameterList(SF.ParameterList(CreateSeparatedList(true, parameterList)));
			}

			if (bodyStatements?.Any() ?? false)
			{
				constructorDeclaration = constructorDeclaration.AddBodyStatements(bodyStatements.ToArray());
			}

			return constructorDeclaration;
		}

		public static PropertyDeclarationSyntax CreateProperty(string name, TypeSyntax type, SyntaxKind[] modifiers, bool addGetter, bool addSetter, SyntaxKind? modifierGetter = null, SyntaxKind? modifierSetter = null, IEnumerable<AttributeSyntax> attributes = null)
		{
			var modifiersToken = modifiers.Select(m => SF.Token(m).WithTrailingTrivia(SF.Space)).ToArray();
			var propertyDeclaration = SF.PropertyDeclaration(type.WithTrailingTrivia(SF.Space), name)
				.AddModifiers(modifiersToken)
				.WithLeadingTrivia(SF.Tab, SF.Tab)
				.WithTrailingTrivia(SF.Space)
				;

			if (addGetter)
			{
				propertyDeclaration = propertyDeclaration.AddGetAccessor(modifierGetter);
			}

			if (addSetter)
			{
				propertyDeclaration = propertyDeclaration.AddSetAccessor(modifierSetter);
			}

			if (attributes?.Any() ?? false)
			{
				var attributeLists = attributes.Select(attribute => SF.AttributeList(SF.SingletonSeparatedList(attribute))).ToList();
				propertyDeclaration = propertyDeclaration.WithAttributeLists(SF.List(attributeLists));
			}

			return propertyDeclaration;
		}

		public static PropertyDeclarationSyntax CreateProperty(string name, string type, SyntaxKind[] modifiers, bool addGetter, bool addSetter, SyntaxKind? modifierGetter = null, SyntaxKind? modifierSetter = null, IEnumerable<AttributeSyntax> attributes = null)
		{
			return CreateProperty(name, SF.ParseTypeName(type), modifiers, addGetter, addSetter, modifierGetter, modifierSetter, attributes);
		}

		public static MethodDeclarationSyntax AddExpressionBodyToMethod(MethodDeclarationSyntax methodDeclaration, ExpressionSyntax expression)
		{
			return methodDeclaration
				.WithExpressionBody(SF.ArrowExpressionClause(expression))
				.WithSemicolonToken(SyntaxConstants.SemicolonToken);
			;
		}


		public static PropertyDeclarationSyntax AddExpressionBodyToProperty(PropertyDeclarationSyntax propertyDeclaration, ExpressionSyntax expression)
		{
			return propertyDeclaration
				.WithExpressionBody(SF.ArrowExpressionClause(expression))
				.WithSemicolonToken(SyntaxConstants.SemicolonToken);
			;
		}

		public static MethodDeclarationSyntax CreateMethodDefinition(string name, TypeSyntax returnType, params SyntaxKind[] modifier)
		{
			var methodDeclaration = SF.MethodDeclaration(returnType, name)
				.WithModifiers(modifier);

			return methodDeclaration;
		}

		public static MethodDeclarationSyntax CreateMethod(string name, TypeSyntax returnType, IEnumerable<StatementSyntax> bodyStatements, params SyntaxKind[] modifier)
		{
			var methodDeclaration = CreateMethodDefinition(name, returnType, modifier)
				.WithBody(SF.Block());

			if (bodyStatements?.Any() ?? false)
			{
				methodDeclaration = methodDeclaration.AddBodyStatements(bodyStatements.ToArray());
			}

			return methodDeclaration;
		}

		public static MethodDeclarationSyntax AddMethodParameter(MethodDeclarationSyntax methodDeclaration, params ParameterSyntax[] parameters)
		{
			return methodDeclaration.WithParameterList(SF.ParameterList(CreateSeparatedList(true, parameters)));
		}

		public static MethodDeclarationSyntax AddMethodTypeParameter(MethodDeclarationSyntax methodDeclaration, params TypeParameterSyntax[] typeParameters)
		{
			return methodDeclaration.WithTypeParameterList(CreateArgumentList(typeParameters));
		}

		public static AwaitExpressionSyntax CreateAwaitExpression(ExpressionSyntax expression)
		{
			return SF.AwaitExpression(expression);
		}

		public static IsPatternExpressionSyntax CreateIsNullExpression(ExpressionSyntax expression)
		{
			return SF.IsPatternExpression(expression, SF.ConstantPattern(SyntaxConstants.Null));
		}

		public static IsPatternExpressionSyntax CreateIsNotNullExpression(ExpressionSyntax expression)
		{
			return SF.IsPatternExpression(expression, SF.UnaryPattern(SF.ConstantPattern(SyntaxConstants.Null)));
		}

		public static AnonymousObjectCreationExpressionSyntax CreateAnonymousObject(params (ExpressionSyntax Property, string Name)[] members)
		{
			var memberList = new List<AnonymousObjectMemberDeclaratorSyntax>();

			foreach (var member in members)
			{
				var property = SF.AnonymousObjectMemberDeclarator(member.Property);
				if (!String.IsNullOrEmpty(member.Name))
				{
					property = property.WithNameEquals(SF.NameEquals(member.Name.ToIdentifierName()));
				}

				memberList.Add(property);
			}

			return SF.AnonymousObjectCreationExpression(
				CreateSeparatedList<AnonymousObjectMemberDeclaratorSyntax>(true, memberList.ToArray())
			);
		}

		public static ForEachStatementSyntax CreateForEachStatement(ExpressionSyntax enumerable, string itemName, IEnumerable<StatementSyntax> bodyStatements, IdentifierNameSyntax itemType = null)
		{
			if (itemType is null)
			{
				itemType = SF.IdentifierName(
					SF.Identifier(
						SF.TriviaList(),
						SyntaxKind.VarKeyword,
						"var",
						"var",
						SF.TriviaList())
				);
			}

			return SF.ForEachStatement(
				itemType,
				SF.Identifier(itemName),
				enumerable,
				SF.Block(bodyStatements.ToArray())
			);
		}

		public static UsingDirectiveSyntax[] CreateUsings(IEnumerable<string> @usings)
		{
			return @usings.Select(u => SF.UsingDirective(SF.ParseName(u))).ToArray();
		}

		public static SyntaxTokenList CreateTokenList(params string[] tokens)
		{
			var classModifierList = SF.TokenList();

			foreach (var token in tokens)
			{
				var syntaxToken = GetToken(token);
				if (syntaxToken is null)
				{
					continue;
				}

				classModifierList = classModifierList.Add(syntaxToken.Value);
			}

			return classModifierList;
		}

		public static SyntaxTokenList CreateTokenList(params SyntaxKind[] syntaxKinds)
		{
			var classModifierList = SF.TokenList();

			foreach (var syntaxKind in syntaxKinds)
			{
				classModifierList = classModifierList.Add(SF.Token(syntaxKind));
			}

			return classModifierList;
		}

		public static TupleElementSyntax CreateTupleElement(TypeSyntax typeSyntax)
		{
			return SF.TupleElement(typeSyntax);
		}

		public static TupleTypeSyntax CreateTupleType(TupleElementSyntax[] elements)
		{
			return SF.TupleType(CreateSeparatedList(true, elements));
		}

		public static TupleExpressionSyntax CreateTuple(ArgumentSyntax[] arguments)
		{
			return SF.TupleExpression(CreateSeparatedList(true, arguments));
		}

		public static InterpolatedStringExpressionSyntax CreateInterpolatedRawStringExpression(params InterpolatedStringContentSyntax[] stringContent)
		{
			// work arround
			return SF.InterpolatedStringExpression(SF.Token(SF.TriviaList(), SyntaxKind.InterpolatedMultiLineRawStringStartToken, "$\"\"\"\n", "$\t\t\t\"\"\"\n", SF.TriviaList()))
				.WithContents(SF.List<InterpolatedStringContentSyntax>(stringContent))
				.WithStringEndToken(SF.Token(SF.TriviaList(), SyntaxKind.InterpolatedRawStringEndToken, "\n\"\"\"", "\n\"\"\"", SF.TriviaList()));

			// https://roslynquoter.azurewebsites.net/: Should work but it doesn't
			//return SF.InterpolatedStringExpression(SF.Token(SyntaxKind.InterpolatedMultiLineRawStringStartToken))
			//	.WithContents(SF.List<InterpolatedStringContentSyntax>(stringContent))
			//	.WithStringEndToken(SF.Token(SyntaxKind.InterpolatedRawStringEndToken));
		}

		public static InterpolatedStringExpressionSyntax CreateInterpolatedStringExpression(bool isMultiline, params InterpolatedStringContentSyntax[] stringContent)
		{
			var startToken = isMultiline ? SyntaxKind.InterpolatedVerbatimStringStartToken : SyntaxKind.InterpolatedStringStartToken;
			var endToken = SyntaxKind.InterpolatedStringEndToken;

			return SF.InterpolatedStringExpression(SF.Token(startToken))
				.WithContents(SF.List<InterpolatedStringContentSyntax>(stringContent))
				.WithStringEndToken(SF.Token(endToken));
		}

		public static InterpolatedStringTextSyntax CreateInterpolatedStringText(string text)
		{
			return SF.InterpolatedStringText()
				.WithTextToken(
					SF.Token(
						SF.TriviaList(),
						SyntaxKind.InterpolatedStringTextToken,
						text,
						text,
						SF.TriviaList()
					)
				);
		}

		public static InterpolationSyntax CreateStringInterpolation(ExpressionSyntax expressionSyntax)
		{
			return SF.Interpolation(expressionSyntax);
		}

		public static CastExpressionSyntax ToCast(ExpressionSyntax expressionSyntax, TypeSyntax type)
		{
			return SF.CastExpression(type, expressionSyntax);
		}

		public static ExpressionSyntax Parenthesize(ExpressionSyntax expressionSyntax)
		{
			return SF.ParenthesizedExpression(expressionSyntax);
		}

		public static EnumMemberDeclarationSyntax CreateEnumerationMember(string memberName, int memberValue)
		{
			return SF.EnumMemberDeclaration(
				SF.Identifier(memberName)
			)
			.WithEqualsValue(
				SF.EqualsValueClause(
					SF.LiteralExpression(
						SyntaxKind.NumericLiteralExpression,
						SF.Literal(memberValue)
					)
				)
			);
		}

		public static SwitchStatementSyntax CreateSwitchStatement(ExpressionSyntax variable, SwitchSectionSyntax[] switchSections)
		{
			return SF.SwitchStatement(variable)
				.WithSections(SF.List(switchSections));
		}

		public static SwitchSectionSyntax CreateSwitchSection((ExpressionSyntax Condition, BinaryExpressionSyntax WhenClause)[] switchConditions, List<StatementSyntax> switchStatements)
		{
			switchStatements.Add(SF.BreakStatement());

			var conditions = new List<SwitchLabelSyntax>();
			foreach (var switchCondition in switchConditions)
			{
				if (switchCondition.WhenClause is null)
				{
					conditions.Add(
						SF.CaseSwitchLabel(switchCondition.Condition)
					);
				}
				else
				{
					conditions.Add(
						SF.CasePatternSwitchLabel(
							SF.ConstantPattern(switchCondition.Condition),
							SF.Token(SyntaxKind.ColonToken)
						)
						.WithWhenClause(
							SF.WhenClause(switchCondition.WhenClause)
						)
					);
				}
			}

			return SF.SwitchSection()
				.WithLabels(SF.List(conditions))
				.WithStatements(SF.List(switchStatements));
		}

		public static SwitchSectionSyntax CreateDefaultSwitchSection(List<StatementSyntax> switchStatements)
		{
			switchStatements.Add(SF.BreakStatement());

			return SF.SwitchSection()
				.WithLabels(SF.SingletonList<SwitchLabelSyntax>(SF.DefaultSwitchLabel()))
				.WithStatements(SF.List(switchStatements));
		}

		private static SyntaxToken? GetToken(string token)
		{
			return token.ToLower() switch
			{
				"abstract" => (SyntaxToken?)SF.Token(SyntaxKind.AbstractKeyword),
				"async" => (SyntaxToken?)SF.Token(SyntaxKind.AsyncKeyword),
				"internal" => (SyntaxToken?)SF.Token(SyntaxKind.InternalKeyword),
				"override" => (SyntaxToken?)SF.Token(SyntaxKind.OverrideKeyword),
				"partial" => (SyntaxToken?)SF.Token(SyntaxKind.PartialKeyword),
				"private" => (SyntaxToken?)SF.Token(SyntaxKind.PrivateKeyword),
				"protected" => (SyntaxToken?)SF.Token(SyntaxKind.ProtectedKeyword),
				"public" => (SyntaxToken?)SF.Token(SyntaxKind.PublicKeyword),
				"sealed" => (SyntaxToken?)SF.Token(SyntaxKind.SealedKeyword),
				"static" => (SyntaxToken?)SF.Token(SyntaxKind.StaticKeyword),
				"virtual" => (SyntaxToken?)SF.Token(SyntaxKind.VirtualKeyword),
				_ => null,
			};
		}
	}
}
