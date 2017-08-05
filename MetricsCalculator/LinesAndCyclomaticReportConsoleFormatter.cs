using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsCalculator
{
    class LinesAndCyclomaticReportConsoleFormatter : ILinesAndCyclomaticReportFormatter<string>
    {
        StringBuilder sb;
        bool outputDelievered;
        public LinesAndCyclomaticReportConsoleFormatter()
        {
            sb = new StringBuilder();
            outputDelievered = false;
        }
        public void AddItem(int depth, string name, int lines, int totalLines, int complexity, int totalComplexity)
        {
            if (outputDelievered)
            {
                return;
            }

            var indents = new String('\t', depth);

            sb.Append(indents + name);
            sb.Append("\tloc:" + lines);
            sb.Append("\ttLOC:" + totalLines);
            sb.Append("\tCC:" + complexity);
            sb.Append("\tCC:" + totalComplexity);
            sb.AppendLine();
        }

        public string GetFormattedOutput()
        {
            outputDelievered = true;
            return sb.ToString();
        }
    }
}
