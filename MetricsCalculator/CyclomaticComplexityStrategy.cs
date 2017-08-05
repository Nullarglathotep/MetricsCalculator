using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace MetricsCalculator
{
    class CyclomaticComplexityStrategy : IMetricsStrategy
    {
        public static readonly string CyclomaticComplexityeData = "CyclomaticComplexityCount";
        public static readonly string TotalCyclomaticComplexityData = "TotalCyclomaticComplexityCount";
        private static readonly int InitialComplexity = 1;

        IDataStoreProvider dataProvider;

        SortedSet<SyntaxKind> linesToCount;

        public CyclomaticComplexityStrategy()
        {
            linesToCount = new SortedSet<SyntaxKind>();
            linesToCount.Add(SyntaxKind.IfStatement);
            linesToCount.Add(SyntaxKind.WhileStatement);
            linesToCount.Add(SyntaxKind.DoStatement);
            linesToCount.Add(SyntaxKind.ForStatement);
            linesToCount.Add(SyntaxKind.ForEachStatement);
            linesToCount.Add(SyntaxKind.CaseSwitchLabel);
            linesToCount.Add(SyntaxKind.LogicalOrExpression);
            linesToCount.Add(SyntaxKind.LogicalAndExpression);
        }

        public void FinalizeSelf(MetricsAccumulationNode node)
        {
            int totalComplexity;
            node.TryGetDataItem<int>(CyclomaticComplexityeData, out totalComplexity);
            foreach (MetricsAccumulationNode child in node.children)
            {
                int childComplexity = 0;
                if (child.TryGetDataItem<int>(TotalCyclomaticComplexityData, out childComplexity) ||
                    child.TryGetDataItem<int>(CyclomaticComplexityeData, out childComplexity))
                {
                    totalComplexity += childComplexity;
                }
            }
            node.SetDataItem<int>(TotalCyclomaticComplexityData, totalComplexity);

        }

        public void Process(SyntaxNode node)
        {
            IMetricsDataStore storage = dataProvider.GetDataStore();
            if (linesToCount.Contains(node.Kind()))
            {
                int currentComplexity;
                storage.TryGetDataItem<int>(CyclomaticComplexityeData, out currentComplexity);
                storage.SetDataItem<int>(CyclomaticComplexityeData, ++currentComplexity);
            }
        }

        public void SetDataStoreProvider(IDataStoreProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }

        public void InitializeSelf()
        {
            IMetricsDataStore storage = dataProvider.GetDataStore();
            storage.SetDataItem<int>(CyclomaticComplexityeData, CyclomaticComplexityStrategy.InitialComplexity);
        }
    }
}

