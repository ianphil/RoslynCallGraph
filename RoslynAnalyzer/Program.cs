// Path to the .csproj of your app you want to analyze
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.MSBuild;

var projectPath = @"/home/ianphil/src/tmp/roslyndemo/MyRoslynDemo/MyRoslynDemo.csproj";

// 1. Create a MSBuildWorkspace and open the project
using var workspace = MSBuildWorkspace.Create();
var project = await workspace.OpenProjectAsync(projectPath);

Console.WriteLine($"Project loaded: {project.Name}");

// 2. Get the compilation for the project
var compilation = await project.GetCompilationAsync();

// 3. For each syntax tree (each .cs file), get the semantic model
//    and find all the method symbols. Then, find references.
if (compilation == null)
{
    Console.WriteLine("Compilation is null. Check project path or references.");
    return;
}

var allMethods = compilation.SyntaxTrees
    .Select(tree => compilation.GetSemanticModel(tree))
    .SelectMany(model =>
    {
        // from the syntax tree, get all method declarations
        var methodDecls = model.SyntaxTree.GetRoot()
            .DescendantNodes()
            .OfType<Microsoft.CodeAnalysis.CSharp.Syntax.MethodDeclarationSyntax>();

        // convert them to symbols using the semantic model
        return methodDecls
            .Select(decl => model.GetDeclaredSymbol(decl))
            .OfType<IMethodSymbol>()
            .Where(symbol => symbol != null);
    })
    .ToList();

// 4. For each method, find references across the solution
foreach (var methodSymbol in allMethods)
{
    var references = await SymbolFinder.FindCallersAsync(methodSymbol, project.Solution);

    // 'references' returns a collection of 'CallerResult'
    // Each 'CallerResult' says "MethodSymbol is called by these locations"
    // We can then group or show them
    foreach (var caller in references)
    {
        // 'caller.CallingSymbol' is the symbol that calls our 'methodSymbol'
        // 'caller.Locations' are the specific usage locations
        var callerSymbol = caller.CallingSymbol as IMethodSymbol;
        if (callerSymbol != null)
        {
            Console.WriteLine(
                $"{methodSymbol.ContainingType.Name}.{methodSymbol.Name} is called by {callerSymbol.ContainingType.Name}.{callerSymbol.Name}");
        }
    }
}

Console.WriteLine("Analysis complete!");