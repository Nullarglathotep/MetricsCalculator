using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;

namespace MetricsCalculator
{
    public delegate void MetricsAccumulatorVisitor(MetricsAccumulationNode node);
    public class MetricsAccumulator : IDataStoreProvider
    {
        public string Title { get; set; }

        SortedSet<SyntaxKind> nodesToAccumulate;

        MetricsAccumulationNode rootNode;
        MetricsAccumulationNode currentNode;

        public MetricsAccumulationNode GetRoot()
        {
            return rootNode;
        }
        public MetricsAccumulator()
        {
            nodesToAccumulate = new SortedSet<SyntaxKind>();
            nodesToAccumulate.Add(SyntaxKind.CompilationUnit);
            nodesToAccumulate.Add(SyntaxKind.ClassDeclaration);
            nodesToAccumulate.Add(SyntaxKind.MethodDeclaration);
            nodesToAccumulate.Add(SyntaxKind.InterfaceDeclaration);
            // TODO: Add other "block creating" statements

        }

        public bool PushIfNeccessary(CSharpSyntaxNode node)
        {
            SyntaxKind nodeKind = node.Kind();
            if (nodesToAccumulate.Contains(nodeKind))
            {
                MetricsAccumulationNode newNode = new MetricsAccumulationNode(node);
                newNode.Initialize(this.Title);
                if (currentNode != null)
                {
                    currentNode.children.Add(newNode);
                    newNode.parent = currentNode;
                }
                else
                {
                    Debug.Assert(rootNode == null);
                    rootNode = newNode;
                }
                currentNode = newNode;
                return true;
            }
            return false;
        }

        public void PopIfNecessary(CSharpSyntaxNode node)
        {
            SyntaxKind nodeKind = node.Kind();
            if (nodesToAccumulate.Contains(nodeKind))
            {
                Debug.Assert(currentNode != null);
                currentNode = currentNode.parent;
            }

        }

        public IMetricsDataStore GetDataStore()
        {
            return (IMetricsDataStore)currentNode;
        }
    }
}
