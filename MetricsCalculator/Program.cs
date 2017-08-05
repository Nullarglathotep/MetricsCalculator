using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;

namespace MetricsCalculator
{
    // todo: add all accumulating statements to lines of code strategy, need to go
    // through syntax kind class and look for statements.
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                MetricsTreeWalker analyzer = new MetricsTreeWalker();
                analyzer.Initialize();
                analyzer.GetMetricsForFile(args[0]);

                MetricsAccumulator metrics = analyzer.GetMetrics(args[0]);
                LinesAndCyclomaticReport<string> report = new LinesAndCyclomaticReport<string>(new LinesAndCyclomaticReportConsoleFormatter());
                Console.WriteLine(report.GenerateReport(metrics));
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("The metrics analyzer threw an exception.");
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.ReadLine();
            }
        }
    }
}
