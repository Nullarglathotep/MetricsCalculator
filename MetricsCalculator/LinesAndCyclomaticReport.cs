using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;

namespace MetricsCalculator
{
    // Use strategy pattern / interface to generate report
    // Use template pattern to allow changing of output format via subclassing. Base class handles running the
    // tree, subclass handles formatting the data by overriding the addToReport method.
    public class LinesAndCyclomaticReport<OutputType> : InOrderReporter, IReport<OutputType>
    {
        int depth = -1;
        ILinesAndCyclomaticReportFormatter<OutputType> formatter;
        public LinesAndCyclomaticReport(ILinesAndCyclomaticReportFormatter<OutputType> formatter)
        {
            this.formatter = formatter;
        }
        public OutputType GenerateReport(MetricsAccumulator metrics)
        {
            Visit(metrics.GetRoot());
            return this.formatter.GetFormattedOutput();
        }

        public override void Visit(MetricsAccumulationNode node)
        {
            depth++;
            SyntaxKind nodeKind = node.nodeReference.Kind();

            int loc, tLoc, complexity, tComplexity;
            node.TryGetDataItem<int>(LinesOfCodeStrategy.LinesOfCodeData, out loc);

            node.TryGetDataItem<int>(LinesOfCodeStrategy.TotalLinesOfCodeData, out tLoc);

            node.TryGetDataItem<int>(CyclomaticComplexityStrategy.CyclomaticComplexityeData, out complexity);

            node.TryGetDataItem<int>(CyclomaticComplexityStrategy.TotalCyclomaticComplexityData, out tComplexity);

            formatter.AddItem(depth, node.Name, loc, tLoc, complexity, tComplexity);

            base.Visit(node);
            depth--;
        }
    }
}
