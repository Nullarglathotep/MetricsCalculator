using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;

namespace MetricsCalculator
{
    public class MetricsAccumulationNode : IMetricsDataStore
    {
        public CSharpSyntaxNode nodeReference;
        public string Name { get; set; }

        public List<MetricsAccumulationNode> children;
        public MetricsAccumulationNode parent;

        public Dictionary<string, Object> data;

        public MetricsAccumulationNode(CSharpSyntaxNode syntaxNode)
        {
            children = new List<MetricsAccumulationNode>();
            data = new Dictionary<string, object>();
            nodeReference = syntaxNode;            
        }

        public void Initialize(string defaultName)
        {
            string name = (from c in nodeReference.ChildTokens()
                        where c.Kind() == SyntaxKind.IdentifierToken
                        select c.Text).FirstOrDefault<string>();
            if (String.IsNullOrEmpty(name))
            {
                this.Name = defaultName;
            }
            else
            {
                this.Name = name;
            }
        }
        public bool TryGetDataItem<DataType>(string name, out DataType value) 
        {
            if (data.ContainsKey(name))
            {
                Object obValue = data[name];
                DataType checker = default(DataType);
                if ((obValue.GetType()) == checker.GetType())
                {
                    value = (DataType)obValue;
                    return true;
                }

            }
            value = default(DataType);
            return false;
        }

        public void SetDataItem<DataType> (string name, DataType value)
        {
            data[name] = value;
        }

    }
}
