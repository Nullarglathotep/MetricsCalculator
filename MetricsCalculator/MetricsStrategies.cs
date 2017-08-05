using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace MetricsCalculator
{
    internal class MetricsStrategies
    {
        private IList<IMetricsStrategy> _strategies;

        public void CreateStrategies(IDataStoreProvider provider)
        {
            _strategies = new List<IMetricsStrategy>();
            _strategies.Add(new LinesOfCodeStrategy());
            _strategies.Add(new CyclomaticComplexityStrategy());

            foreach (IMetricsStrategy strategy in _strategies)
            {
                strategy.SetDataStoreProvider(provider);
            }
        }

        public void InitializeStrategyDataForNewNode()
        {
            foreach (IMetricsStrategy strategy in _strategies)
            {
                strategy.InitializeSelf();
            }
        }
        public void ProcessAllStrategies(SyntaxNode node)
        {
            foreach (IMetricsStrategy strategy in _strategies)
            {
                strategy.Process(node);
            }
        }

        public void FinalizeAllStrategies(MetricsAccumulationNode node)
        {
            foreach (MetricsAccumulationNode child in node.children)
            {
                FinalizeAllStrategies(child);
            }
            foreach (IMetricsStrategy strategy in _strategies)
            {
                strategy.FinalizeSelf(node);
            }
        }
    }
}