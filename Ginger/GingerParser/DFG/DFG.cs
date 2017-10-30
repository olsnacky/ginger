using Ginger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GingerParser.DFG
{
    public enum DFGNodeType
    {
        High,
        Invocation,
        Low,
        Return,
        Subexpression
    }

    public class DFGNode : IEquatable<DFGNode>
    {
        private List<DFGNode> _adjacencyList;
        private Guid _id;
        private Invocation _invocation;
        private string _label;
        private List<DFGNode> _parents;
        private Variable _variable;
        private DFGNodeType _type;

        public List<DFGNode> adjacencyList => _adjacencyList;
        public Guid id => _id;
        public Invocation invocation => _invocation;
        public List<DFGNode> parents => _parents;
        public Guid subGraphId;
        public string label
        {
            get
            {
                if (_type == DFGNodeType.Subexpression)
                {
                    return _variable.identifier.name;
                }
                else if (_type == DFGNodeType.Invocation)
                {
                    return _invocation.name.name;
                }
                else
                {
                    return _label;
                }
            }
        }
        public DFGNodeType type => _type;
        public Variable variable => _variable;

        public DFGNode(Variable variable)
        {
            _adjacencyList = new List<DFGNode>();
            _parents = new List<DFGNode>();
            _variable = variable;
            _type = DFGNodeType.Subexpression;

            setId();
        }

        public DFGNode(GingerParser.Invocation invocation)
        {
            _adjacencyList = new List<DFGNode>();
            _id = Guid.NewGuid();
            _invocation = invocation;
            _parents = new List<DFGNode>();
            _type = DFGNodeType.Invocation;
        }

        public DFGNode(DFGNodeType type, string label)
        {
            _adjacencyList = new List<DFGNode>();
            _id = Guid.NewGuid();
            _label = label;
            _parents = new List<DFGNode>();
            _type = type;
        }

        public void addEdge(DFGNode target)
        {
            _adjacencyList.Add(target);
            target.addParent(this);
        }

        public void addParent(DFGNode source)
        {
            _parents.Add(source);
        }

        public void setId()
        {
            _id = Guid.NewGuid();
        }

        public bool Equals(DFGNode other)
        {
            return _id == other.id;
        }
    }

    public class DFG
    {
        private List<DFGNode> _formalParams;
        private DFGNode _high;
        private DFGNode _low;
        private List<DFGNode> _nodes;
        private DFGNode _return;

        public DFGNode high
        {
            get { return _high; }
            set { _high = value; }
        }

        public DFGNode low
        {
            get { return _low; }
            set { _low = value; }
        }

        public List<DFGNode> nodes
        {
            get { return _nodes; }
        }

        public DFGNode returnNode => _return;

        public DFG(DFGNode high, DFGNode low)
        {
            _formalParams = new List<DFGNode>();
            _nodes = new List<DFGNode>();

            if (high == null)
            {
                _high = new DFGNode(DFGNodeType.High, DFGNodeType.High.ToString());
                addNode(_high);
            }
            else
            {
                _high = high;
            }

            if (low == null)
            {
                _low = new DFGNode(DFGNodeType.Low, DFGNodeType.Low.ToString());
                addNode(_low);
            }
            else
            {
                _low = low;
            }
        }

        public void addNode(DFGNode n)
        {
            _nodes.Add(n);
        }

        public void addAssign(List<Identifier> sources, Identifier sink)
        {
            DFGNode sinkNode = _findNode(sink);
            foreach (Identifier i in sources)
            {
                DFGNode source = _findNode(i);
                source.addEdge(sinkNode);
            }
        }

        public void addFormalParam(DFGNode formalParam)
        {
            _formalParams.Add(formalParam);
        }

        public DFGNode addInvocation(Invocation invc)
        {
            DFGNode dfgni = new DFGNode(invc);
            addNode(dfgni);
            return dfgni;
        }

        public void addDeferredInvocation(DeferredInvocation di, DFG functionGraph, List<List<Identifier>> sources)
        {
            // create invocation node
            //DFGNode dfgni = new DFGNode(invc);
            Guid subgraphId = Guid.NewGuid();
            //addNode(dfgni);

            // add the copied graph nodes to this graph
            foreach (DFGNode dfgn in functionGraph.nodes)
            {
                dfgn.subGraphId = subgraphId;
                addNode(dfgn);
            }

            // hook up the return node to the invocation node
            //functionGraph.returnNode.addEdge(dfgni);
            functionGraph.returnNode.addEdge(di.invocationNode);

            // hook up actual args to formal args
            for (int i = 0; i < sources.Count; i++)
            {
                List<Identifier> paramSources = sources[i];
                foreach (Identifier ident in paramSources)
                {
                    DFGNode source = _findNode(ident);
                    source.addEdge(functionGraph._formalParams[i]);
                }
            }
        }

        public void addReturn(List<Identifier> sources)
        {
            _return = new DFGNode(DFGNodeType.Return, "return");
            foreach (Identifier i in sources)
            {
                DFGNode source = _findNode(i);
                source.addEdge(_return);
            }
            _nodes.Add(_return);
        }

        public bool hasInterference()
        {
            return _bfs(_high, _low) != null;
        }

        public void resetIds()
        {
            foreach (DFGNode n in _nodes)
            {
                n.setId();
            }
        }

        private List<DFGNode> _bfs(DFGNode root, DFGNode dest)
        {
            Queue<DFGNode> q = new Queue<DFGNode>();
            List<DFGNode> result = new List<DFGNode>();
            List<DFGNode> visited = new List<DFGNode>();

            q.Enqueue(root);
            bool found = false;
            while (q.Count > 0)
            {
                DFGNode current = q.Dequeue();

                if (current == null)
                {
                    continue;
                }

                result.Add(current);
                visited.Add(current);

                foreach (DFGNode node in current.adjacencyList)
                {
                    if (!visited.Contains(node) && !q.Contains(node))
                    {
                        q.Enqueue(node);
                    }
                }

                if (dest == current)
                {
                    found = true;
                    break;
                }
            }

            if (found)
            {
                return result;
            }
            else
            {
                return null;
            }
        }

        private DFGNode _findNode(Identifier i)
        {
            // find non-security nodes
            DFGNode dfgn = _nodes.Find(n => n.subGraphId == new Guid() && (n.variable != null && n.variable.identifier == i) || (n.invocation != null && n.invocation.name == i && n.invocation.invocationCount == i.invocationCount));

            // find if security node
            if (dfgn == null)
            {
                if (i.name.Equals(GingerToken.High.ToString()))
                {
                    dfgn = _high;
                }
                else if (i.name.Equals(GingerToken.Low.ToString()))
                {
                    dfgn = _low;
                }
            }

            return dfgn;
        }

        private void _addEdge(DFGNode source, DFGNode target)
        {
            source.addEdge(target);
        }
    }
}
