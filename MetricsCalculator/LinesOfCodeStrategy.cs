using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace MetricsCalculator
{
    [Serializable]
    internal class LinesOfCodeStrategy : IMetricsStrategy
    {
        public static readonly string LinesOfCodeData = "LinesOfCodeCount";
        public static readonly string TotalLinesOfCodeData = "TotalLinesOfCodeCount";
        private static readonly int InitialLinesOfCode = 0;

        IDataStoreProvider dataProvider;

        SortedSet<SyntaxKind> linesToCount;


        public LinesOfCodeStrategy()
        {

            // linesToCount.Add(SyntaxKind.);
            linesToCount = new SortedSet<SyntaxKind>();
            linesToCount.Add(SyntaxKind.BreakStatement);
            linesToCount.Add(SyntaxKind.CatchClause);
            linesToCount.Add(SyntaxKind.CheckedStatement);
            linesToCount.Add(SyntaxKind.ClassDeclaration);
            linesToCount.Add(SyntaxKind.ConstructorDeclaration);
            linesToCount.Add(SyntaxKind.ContinueStatement);
            linesToCount.Add(SyntaxKind.DelegateDeclaration);
            linesToCount.Add(SyntaxKind.DoStatement);
            linesToCount.Add(SyntaxKind.ElseClause);
            linesToCount.Add(SyntaxKind.EmptyStatement);
            linesToCount.Add(SyntaxKind.EnumDeclaration);
            linesToCount.Add(SyntaxKind.ExpressionStatement);
            linesToCount.Add(SyntaxKind.FieldDeclaration);
            linesToCount.Add(SyntaxKind.FinallyClause);
            linesToCount.Add(SyntaxKind.FixedStatement);
            linesToCount.Add(SyntaxKind.ForStatement);
            linesToCount.Add(SyntaxKind.ForEachStatement);
            linesToCount.Add(SyntaxKind.GotoStatement);
            linesToCount.Add(SyntaxKind.GotoCaseStatement);
            linesToCount.Add(SyntaxKind.IfStatement);
            linesToCount.Add(SyntaxKind.InterfaceDeclaration);
            linesToCount.Add(SyntaxKind.LabeledStatement);
            linesToCount.Add(SyntaxKind.LocalDeclarationStatement);
            linesToCount.Add(SyntaxKind.LockStatement);
            linesToCount.Add(SyntaxKind.MethodDeclaration);
            linesToCount.Add(SyntaxKind.NamespaceDeclaration);
            linesToCount.Add(SyntaxKind.ReturnStatement);
            linesToCount.Add(SyntaxKind.StructDeclaration);
            linesToCount.Add(SyntaxKind.SwitchSection);
            linesToCount.Add(SyntaxKind.SwitchStatement);
            linesToCount.Add(SyntaxKind.ThrowStatement);
            linesToCount.Add(SyntaxKind.TryStatement);
            linesToCount.Add(SyntaxKind.UncheckedStatement);
            linesToCount.Add(SyntaxKind.UnsafeStatement);
            linesToCount.Add(SyntaxKind.UsingDirective);
            linesToCount.Add(SyntaxKind.UsingStatement);
            linesToCount.Add(SyntaxKind.WhileStatement);
            linesToCount.Add(SyntaxKind.YieldBreakStatement);
            linesToCount.Add(SyntaxKind.YieldReturnStatement);
        }

        public void InitializeSelf()
        {
            IMetricsDataStore storage = dataProvider.GetDataStore();
            storage.SetDataItem<int>(LinesOfCodeData, LinesOfCodeStrategy.InitialLinesOfCode);
        }
        public void FinalizeSelf(MetricsAccumulationNode node)
        {
            int totalLinesOfCode = 0;

            node.TryGetDataItem<int>(LinesOfCodeData, out totalLinesOfCode);
            foreach (MetricsAccumulationNode child in node.children)
            {
                int childLinesOfCode = 0;
                if (child.TryGetDataItem<int>(TotalLinesOfCodeData, out childLinesOfCode) ||
                    child.TryGetDataItem<int>(LinesOfCodeData, out childLinesOfCode))
                {
                    totalLinesOfCode += childLinesOfCode;
                }
                continue;
            }
            node.SetDataItem<int>(TotalLinesOfCodeData, totalLinesOfCode);
        }

        public void Process(SyntaxNode node)
        {
            IMetricsDataStore storage = dataProvider.GetDataStore();
            if (linesToCount.Contains(node.Kind()))
            {
                if (node.Kind() == SyntaxKind.IfStatement &&
                    node.Parent != null && node.Parent.Kind() == SyntaxKind.ElseClause)
                {
                    return; // Only count the 'else' in an 'else if'.
                }
                int currentLOC;
                storage.TryGetDataItem<int>(LinesOfCodeData, out currentLOC);
                storage.SetDataItem<int>(LinesOfCodeData, ++currentLOC);
            }
        }

        public void SetDataStoreProvider(IDataStoreProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }
    }
}