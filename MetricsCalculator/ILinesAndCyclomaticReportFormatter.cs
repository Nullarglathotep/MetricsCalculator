namespace MetricsCalculator
{
    public interface ILinesAndCyclomaticReportFormatter<OutputType>
    {
        void AddItem(int depth, string name, int lines, int totalLines, int complexity, int totalComplexity);

        OutputType GetFormattedOutput();
    }
}