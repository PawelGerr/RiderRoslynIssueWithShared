using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;

namespace DemoSourceGenerator;

[ExportCodeFixProvider(LanguageNames.CSharp)]
[Shared]
public class DemoCodeFixProvider : CodeFixProvider
{
   public override ImmutableArray<string> FixableDiagnosticIds { get; }
      = ImmutableArray.Create(DemoAnalyzer.DemoDiagnosticDescriptor.Id);

   public override Task RegisterCodeFixesAsync(CodeFixContext context)
   {
      var diagnostic = context.Diagnostics.FirstOrDefault();

      if (diagnostic is not null)
      {
         var action = CodeAction.Create("Demo Fix",
                                        _ => Task.FromResult(context.Document),
                                        "Demo Fix");
         context.RegisterCodeFix(action, diagnostic);
      }

      return Task.CompletedTask;
   }
}
