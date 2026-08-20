# Changelog

Notable changes per released version, newest first. Versions before 1.0.7 are not documented here —
the Git history is the source for those.

## 1.0.7

### Breaking

Renames and signature changes. The migration is mechanical: the compiler points at every call site.

| 1.0.6 | 1.0.7 |
|---|---|
| `WithParameter` | `AddParameter` — and it appends instead of replacing |
| `WithTypeParameter` | `AddTypeParameter` — appends |
| `WithConstraints` | `AddConstraints` — appends, takes any `TypeParameterConstraintSyntax` |
| `SyntaxHelper.AddConstaints` | `SyntaxHelper.AddConstraints` |
| `ToContructor` | `ToConstructor` |
| `WithInitializer` (constructor) | `WithBaseInitializer` |
| `WithAttributes` (parameter) | `AddAttributes` — appends |
| `CreateLambdaExpression` (parameter) | `ToLambdaExpression` |
| `SyntaxHelper.CreateEnumerableAccess` | `SyntaxHelper.CreateElementAccess` |
| `SyntaxHelper.CreateArgumentList(TypeParameterSyntax[])` | `SyntaxHelper.CreateTypeParameterList` |
| `StringExtension` | `StringExtensions` |
| `ObjectExtensions.AsArray` | `SyntaxNodeExtensions.AsArray`, constrained to `SyntaxNode` |
| `AsType(type)` | `AsType(type, toNullableType)` — the default is gone, because whether the target type becomes nullable changes the generated code |
| `ToSwitchSection`, `ToDefaultSwitchSection`, `Finally`, `Using` taking `List<T>` | the same methods taking `IEnumerable<T>` |

`SyntaxConstants` now holds `static readonly` fields instead of properties. Source compatible;
recompiling against 1.0.7 is enough.

**The renames are the whole migration — the generated code does not change.** Verified before the
release by generating everything the example generator of `Eshava.DomainDrivenDesign.CodeAnalysis`
produces with both versions and comparing: 595 sources, byte for byte identical.

### Fixed

* `Call` on a string ignored its `withNullCheck` argument, so `"input".Call("ToList", true)`
  produced `input.ToList()` instead of `input?.ToList()`.
* Building a switch section appended the `break` statement to the caller's list instead of to a
  copy. A list used for two sections ended up with two `break` statements, the second of them
  unreachable. An explicit `break` at the end is now kept instead of doubled.
* `ToConstantExpression` dropped everything after the second segment (`Alpha.Beta.Gamma` became
  `Alpha.Beta`) and threw `IndexOutOfRangeException` for a name without a dot. It now keeps every
  segment and reports a single segment as `ArgumentException`.
* Modifiers the library did not know were dropped without a trace, which silently removed them from
  the generated code — `readonly`, `const`, `new`, `required` and others among them. The set is now
  complete and an unknown modifier raises `ArgumentException`.
* `AddParameter`, `AddTypeParameter`, `AddConstraints`, `AddAttributes` and `AddModifiers` replaced
  what was already there instead of appending, so `AddParameter(a).AddParameter(b)` lost `a`.
* `AsType` defaulted to making the target type nullable, which produced `x as string?` unasked.
* The collection expression helper carried a type parameter it never read. A collection expression
  is target typed, so the type is gone from `SyntaxHelper`; the extensions on `TypeSyntax` remain.
* `ElseIf` threw `IndexOutOfRangeException` when handed an empty array.
* `CreateProperty` wrote two hard coded tabs of indentation, which was wrong at any other nesting
  depth. Formatting is left to `NormalizeWhitespace`.
* String to number conversions were culture dependent, as was the modifier lookup. Both are
  invariant now — this code runs in the compiler process of whoever consumes it.
* Null was passed on unchecked in several places (`CreateEnumeration`, `CreateForEachStatement`,
  `CreateUsings`, `CreateSeparatedList`, `CreateTryCatchBlock`, `CreateUsingStatement` and the
  interpolation extensions).

### Added

* `AccessElement` as the single implementation behind `AccessArray`, `AccessList` and
  `AccessDictionary`.
* `WithThisInitializer` for a `: this(…)` constructor call — `WithBaseInitializer` could only ever
  produce `: base(…)`.
* An exception type and variable name for `CreateTryCatchBlock`, which was fixed to
  `catch (Exception ex)`.
* `SyntaxConstants.StructConstraint`, `SyntaxConstants.NewConstraint`,
  `TypeSyntax.ToConstraint()` and `SyntaxHelper.CreateTypeConstraint`, so a type parameter can be
  constrained to an interface or `new()` and not just to `class`.
* An indentation parameter for the raw string helpers instead of three hard coded tabs. It affects
  the value text of the start token, not the generated code.
* `Eshava.Test.CodeAnalysis`, the first test project of this library.
