# Eshava.CodeAnalysis — Repository Notes

Roslyn-based helper library. Provides chainable extension methods around `SyntaxFactory` so that
complex C# code can be assembled as compilation units. Published as the NuGet package
**`Eshava.CodeAnalysis`**.

**Conventions:** documentation, code and commit messages are written in English. Line endings are
pinned through `.gitattributes` — anything that may run on Linux must be checked out with LF.

## Layout

Two projects:

* **`Eshava.CodeAnalysis`** — the library, targeting `netstandard2.0` because the consumers are
  analyzers and source generators, which Roslyn loads into the compiler.
  * `SyntaxHelper` — every method that creates a syntax element.
  * `SyntaxConstants` — predefined types and expressions, held as `static readonly` fields:
    syntax nodes are immutable, so one instance can be shared.
  * `Eshava.CodeAnalysis.Extensions` — the extension methods, which all delegate to
    `SyntaxHelper`.
* **`Eshava.Test.CodeAnalysis`** — MSTest and FluentAssertions, targeting `net10.0`. The tests
  assert on the generated code (`NormalizeWhitespace().ToFullString()`) or on the syntax nodes
  themselves. It needs no Roslyn reference of its own — see [Roslyn Reference](#roslyn-reference).

## Rules

* **Build syntax elements piece by piece.** Never parse statements or expressions
  (`SyntaxFactory.ParseStatement`, `ParseExpression` and friends). Assembling the tree explicitly
  is the entire point of this library.

  The two exceptions are **names and types**: `ParseName` for namespaces and usings, and
  `ParseTypeName` for type names, both of which the library uses deliberately. Writing
  `List<Dictionary<string, int>>` out of `GenericName`, `TypeArgumentList` and `PredefinedType`
  nodes buys nothing — a type name has no statements that could go wrong.
* New helpers belong in `SyntaxHelper`; the extension method on top of it stays a thin
  delegation. **`SyntaxHelper` itself never declares extension methods** — otherwise they leak
  out of the `Eshava.CodeAnalysis` namespace and the split above stops holding.
* **`Add…` appends, `With…` replaces.** Roslyn's own convention, and the reason
  `AddParameter(a).AddParameter(b)` keeps both parameters instead of dropping the first.
* **No silent failure.** A modifier the library does not know is an `ArgumentException`, not a
  token that quietly disappears from the generated code.
* **Never modify what the caller passed in.** Helpers that add something to a statement list copy
  it first.
* Extension classes are named in the plural (`StringExtensions`), one file per receiver type.
* **No hard coded indentation.** Trivia that assumes a nesting depth breaks as soon as the
  element is generated one level deeper; the consumer calls `NormalizeWhitespace()`.
* Culture-independent conversions only (`ToLowerInvariant`, `Int32.Parse` with
  `CultureInfo.InvariantCulture`). This code runs inside the compiler process of whoever consumes
  it, on whatever locale their machine has.
* **Anything a consumer notices goes into [`CHANGELOG.md`](CHANGELOG.md)** under the version it is
  released in — renames, changed signatures, fixed behaviour. Written while the change is made, not
  reconstructed at release time.

## Roslyn Reference

`Microsoft.CodeAnalysis.CSharp` is referenced as an ordinary dependency and **flows on to the
consumers**. That is deliberate: something a dependency already provides should not have to be
declared again in every project that consumes it.

**The consequence is that the version declared here is a lower bound for every consumer.** NuGet
does not treat undercutting it as a warning but as `NU1605` — a restore error. Verified: a project
with a direct reference to an older Roslyn plus this package fails to restore, while the same
project builds cleanly against a package that declares no Roslyn dependency.

**So raise the version only when the library actually needs something newer**, and expect it to be
a coordinated step: every consuming generator that sits below the new version has to be raised with
it.

### When to follow a new Roslyn release, and when not to

The reference does not deliver anything — it states a requirement, and that makes it asymmetric:

* **Referencing an older Roslyn is harmless.** Newer compilers load older analyzers. Measured: the
  library compiles against `4.14.0`, so the version in use is well above what the code needs.
* **Referencing a newer one than the host compiler has stops the generator from running.** An
  analyzer needs a compiler whose Roslyn is at least the referenced version; otherwise it is
  skipped with `CS9057` — a *warning*. What arrives is not a clear error but a wall of `CS0246`,
  because the generated types are missing.

Follow a new release when, and only when:

1. an API the library needs only exists there — for this library that means a syntax node or
   `SyntaxKind` for a C# feature that is to be **generated** (raw strings needed 4.4, collection
   expressions 4.8);
2. a Roslyn bug affects the library — the workaround in
   `CreateInterpolatedRawStringExpression` exists because the formatter drops the trivia of the raw
   string start token, and dropping the workaround one day requires the version that fixes it;
3. generated code moves to a newer language version whose tree the old API cannot build.

**A new release on its own is not a reason.** What a newer Roslyn improves — performance,
diagnostics, language support — reaches the build through the SDK of the machine or the build agent,
not through this reference. The same goes for security fixes: the Roslyn that runs is always the
compiler's, never the one named here.

`Microsoft.CodeAnalysis.Analyzers` is a different case. It carries the `RS` rules that
`EnforceExtendedAnalyzerRules` switches on — those apply while *this* library is compiled and are
of no concern to anyone consuming it, which is why it is referenced with `PrivateAssets="all"`.
The two belong together: either both are there or neither is.

**Raising that one forces nobody.** It does not appear in the package at all — the nuspec lists
`Microsoft.CodeAnalysis.CSharp` as the single dependency. A consumer does end up with
`Microsoft.CodeAnalysis.Analyzers`, but through Roslyn's own chain (`CSharp` → `Common` →
`Analyzers`) and at the version that chain asks for. Move it whenever the `RS` rules are worth it,
in practice together with the Roslyn version above.

**The rule behind all of this:** what binds consumers is a reference *without* `PrivateAssets`. In
this project that is exactly one — `Microsoft.CodeAnalysis.CSharp`.

## Dependants

`Eshava.DomainDrivenDesign.CodeAnalysis` consumes this package. A breaking change there is felt
only once the package version is raised in that repository — the repositories are coupled by
NuGet version, never by project reference.

**What changed per version, and what a migration costs, is in [`CHANGELOG.md`](CHANGELOG.md).**
Renames belong there, not here — this file describes how the library is built, not its history. Nothing but the call sites has to be touched.
