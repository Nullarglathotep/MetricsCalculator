using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace MetricsCalculator
{
    public class MetricsTreeWalker : CSharpSyntaxWalker, IDataStoreProvider
    {
        int Tabs = 0;

        private MetricsStrategies strategies;
        private MetricsAccumulator currentAccumulator;
        private Dictionary<string,MetricsAccumulator> accumulatedMetrics;

        public MetricsTreeWalker()
        {
            strategies = new MetricsStrategies();
            accumulatedMetrics = new Dictionary<string, MetricsAccumulator>();
        }

        public void Initialize()
        {
            strategies.CreateStrategies(this);
        }
        public MetricsAccumulator GetMetricsForFile(string fileName)
        {
            try
            {

                string line;
                // Open the text file using a stream reader.
                using (StreamReader sr = new StreamReader(fileName))
                {
                    // Read the stream to a string
                    line = sr.ReadToEnd();
                }
                //Console.WriteLine(line);
                SyntaxTree tree = CSharpSyntaxTree.ParseText(line);

                CreateNewAccumulator(fileName);
                AnalyzeTree(tree);

                return currentAccumulator;
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
                throw;
            }
        }

        private void CreateNewAccumulator(string title)
        {
            currentAccumulator = new MetricsAccumulator();
            currentAccumulator.Title = title;
            accumulatedMetrics.Add(title, currentAccumulator);
        }
        private void AnalyzeTree(SyntaxTree tree)
        {
            this.Visit(tree.GetRoot());
            strategies.FinalizeAllStrategies(currentAccumulator.GetRoot());
        }

        public override void Visit(SyntaxNode node)
        {

            CSharpSyntaxNode csNode = node as CSharpSyntaxNode;
            if (currentAccumulator.PushIfNeccessary(csNode))
            {
                strategies.InitializeStrategyDataForNewNode();
            }
            strategies.ProcessAllStrategies(csNode);
            base.Visit(node);
            currentAccumulator.PopIfNecessary(csNode);
        }

        public IMetricsDataStore GetDataStore()
        {
            return currentAccumulator.GetDataStore();
        }

        public MetricsAccumulator GetMetrics(string name)
        {
            if (accumulatedMetrics.ContainsKey(name))
            {
                return accumulatedMetrics[name];
            }
            return null;
        }
    }
}
