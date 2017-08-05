using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetricsCalculator
{
    public class InOrderReporter
    {
        public virtual void Visit(MetricsAccumulationNode node)
        {
            foreach (MetricsAccumulationNode child in node.children)
            {
                Visit(child);
            }
        }
    }
}
