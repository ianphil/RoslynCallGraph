# Roslyn Method Call Analyzer

## Overview
This project is a code analysis tool built using the .NET Compiler Platform (Roslyn). It analyzes C# projects to identify method calls and their callers, helping developers understand dependencies between methods in their codebase and creates a call graph.

## Purpose
The analyzer serves several purposes:
- Identify all method calls in a codebase
- Map the relationships between caller and callee methods 
- Help understand dependencies and the impact of code changes
- Assist in code refactoring by visualizing method usage

## Requirements
- .NET 6.0 SDK or newer
- The project you want to analyze must be buildable with MSBuild

## How to Run

1. Update the project path in `Program.cs`:
   ```csharp
   var projectPath = @"/path/to/your/project.csproj";
   ```

2. Build the analyzer:
   ```
   cd /home/ianphil/src/tmp/roslyndemo/RoslynAnalyzer
   dotnet build
   ```

3. Run the analyzer:
   ```
   dotnet run
   ```

4. Examine the console output showing the method call relationships

## Output
The analyzer will output information in the following format:
```
Project loaded: [ProjectName]
[Class1].[Method1] is called by [Class2].[Method2]
...
Analysis complete!
```

## Customization
You can modify the analyzer to extract different information by:
- Changing the symbol types being analyzed (currently focused on methods)
- Extracting different properties from the semantic model
- Adding filters to focus on specific namespaces or classes
- Changing the output format or exporting to a file

## Troubleshooting
- If the compilation is null, verify the project path is correct
- Ensure all project references and NuGet packages can be resolved
- For large projects, you may need to increase memory allocation
