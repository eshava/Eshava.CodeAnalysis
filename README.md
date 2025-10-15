# Eshava.CodeAnalysis
A Roslyn-based library that defines extension methods to simplify the use of the SyntaxFactory.

The purpose of this library is to generate complex C# code via compilation units.
To this end, chainable extension methods are provided.

## Structure
Components of this library

* Eshava.CodeAnalysis
	* SyntaxHelper: Contains all methods to create syntax element
	* SyntaxConstants: Predefined types and expressions
* Eshava.CodeAnalysis.Extensions
	* Extension methods that all call the SyntaxHelper

## Example

### C# Code (Output)
```csharp
var output = input?.ToList() ?? new List<string>();
```

### Roslyn Syntax (Input)
https://roslynquoter.azurewebsites.net/

```csharp
LocalDeclarationStatement(
    VariableDeclaration(
        IdentifierName(
            Identifier(
                TriviaList(),
                SyntaxKind.VarKeyword,
                "var",
                "var",
                TriviaList())))
    .WithVariables(
        SingletonSeparatedList<VariableDeclaratorSyntax>(
            VariableDeclarator(
                Identifier("output"))
            .WithInitializer(
                EqualsValueClause(
                    BinaryExpression(
                        SyntaxKind.CoalesceExpression,
                        ConditionalAccessExpression(
                            IdentifierName("input"),
                            InvocationExpression(
                                MemberBindingExpression(
                                    IdentifierName("ToList")))),
                        ObjectCreationExpression(
                            GenericName(
                                Identifier("List"))
                            .WithTypeArgumentList(
                                TypeArgumentList(
                                    SingletonSeparatedList<TypeSyntax>(
                                        PredefinedType(
                                            Token(SyntaxKind.StringKeyword))))))
                        .WithArgumentList(
                            ArgumentList())))))))
```

### Eshava.CodeAnalysis (Input)
```csharp
var listInstance = "List"
	.AsGeneric("string")
	.ToInstance();

var toListCall = "input"
	.ToIdentifierName()
	.Access("ToList".ToIdentifierName(), true)
	.Call()
	.AddNullFallback(listInstance);

var result = "output"
	.ToVariableStatement(toListCall);
```