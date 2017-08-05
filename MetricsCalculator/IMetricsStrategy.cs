using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;

namespace MetricsCalculator
{
    public interface IMetricsStrategy
    {
        void InitializeSelf();
        void Process(SyntaxNode node);
        void FinalizeSelf(MetricsAccumulationNode node);
        void SetDataStoreProvider(IDataStoreProvider dataProvider);
    }
}
