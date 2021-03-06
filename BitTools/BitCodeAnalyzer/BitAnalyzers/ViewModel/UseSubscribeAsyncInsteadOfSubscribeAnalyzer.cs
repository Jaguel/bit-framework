﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using System.Collections.Immutable;

namespace BitCodeAnalyzer.BitAnalyzers.ViewModel
{
    [DiagnosticAnalyzer(LanguageNames.CSharp)]
    public class UseSubscribeAsyncInsteadOfSubscribeAnalyzer : DiagnosticAnalyzer
    {
        public const string DiagnosticId = nameof(UseSubscribeAsyncInsteadOfSubscribeAnalyzer);

        public const string Title = "Use SubscribeAsync instead of Subscribe";
        public const string Message = Title;
        public const string Description = Title;
        private const string Category = "Bit";

        private static readonly DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, Message, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

        public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

        public override void Initialize(AnalysisContext context)
        {
            context.EnableConcurrentExecution();
            context.RegisterSyntaxNodeAction(AnalyzeSyntax, SyntaxKind.InvocationExpression);
        }

        private void AnalyzeSyntax(SyntaxNodeAnalysisContext context)
        {
            SyntaxNode root = context.Node;

            if (!(root is InvocationExpressionSyntax))
                return;

            InvocationExpressionSyntax invocation = (InvocationExpressionSyntax)root;

            IMethodSymbol symbol = (IMethodSymbol)context.SemanticModel.GetSymbolInfo(invocation).Symbol;

            if (symbol == null || symbol.Name != "Subscribe")
                return;

            string symbolName = symbol.ContainingType.ToDisplayString();

            if (symbolName.StartsWith("Prism.Events.PubSubEvent"))
            {
                Diagnostic diagn = Diagnostic.Create(Rule, root.GetLocation(), Message);

                context.ReportDiagnostic(diagn);
            }
        }
    }
}
