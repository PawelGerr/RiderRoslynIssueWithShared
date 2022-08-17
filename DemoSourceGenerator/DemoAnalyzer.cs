using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DemoSourceGenerator;

[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class DemoAnalyzer : DiagnosticAnalyzer
{
   public static readonly DiagnosticDescriptor DemoDiagnosticDescriptor
      = new("DEMO001", "Demo Error", "Demo Error", "DemoAnalyzer", DiagnosticSeverity.Error, true);

   public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get; }
      = ImmutableArray.Create(DemoDiagnosticDescriptor);

   public override void Initialize(AnalysisContext context)
   {
      context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
      context.EnableConcurrentExecution();

      context.RegisterSymbolAction(AnalyzeNamedType, SymbolKind.NamedType);
   }

   private static void AnalyzeNamedType(SymbolAnalysisContext context)
   {
      var type = (INamedTypeSymbol)context.Symbol;
      var node = type.DeclaringSyntaxReferences.FirstOrDefault()?.GetSyntax(context.CancellationToken);

      if (node is ClassDeclarationSyntax cds)
      {
         var error = Diagnostic.Create(DemoDiagnosticDescriptor,
                                       cds.Identifier.GetLocation(),
                                       type.Name);
         context.ReportDiagnostic(error);
      }
   }
}
