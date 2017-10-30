using GingerParser;
using GingerParser.DFG;
using GingerUtil;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Helpers
{
    enum EdgeRelation
    {
        // AST
        AST,
        // DFG
        Direct
    }

    enum NodeType
    {
        // AST
        StatementList,
        Declaration,
        Integer,
        Identifier,
        Assignment,
        Literal,
        Branch,
        While,
        InequalityOperation,
        BinaryOperation,
        Boolean,
        Function,
        VariableList,
        Return,
        Invocation,
        // DFG
        High,
        Low,
        Subexpression
    }

    struct JsonGraph
    {
        public Graph graph;
    }

    struct Graph
    {
        private List<GraphNode> _nodes;
        private List<GraphEdge> _edges;

        public List<GraphNode> nodes
        {
            get
            {
                if (_nodes == null)
                {
                    _nodes = new List<GraphNode>();
                }

                return _nodes;
            }
        }

        public List<GraphEdge> edges
        {
            get
            {
                if (_edges == null)
                {
                    _edges = new List<GraphEdge>();
                }

                return _edges;
            }
        }

        public void add(GraphNode node)
        {
            if (_nodes == null)
            {
                _nodes = new List<GraphNode>();
            }

            _nodes.Add(node);
        }

        public void add(GraphEdge edge)
        {
            if (_edges == null)
            {
                _edges = new List<GraphEdge>();
            }

            _edges.Add(edge);
        }

        public GraphNode? findNode(string id)
        {
            if (_nodes == null)
            {
                return null;
            }

            try
            {
                return _nodes.Where(n => n.id.Equals(id)).First();
            }
            catch
            {
                return null;
            }
        }
    }

    struct GraphEdge
    {
        private GraphNode _source;
        private GraphNode _target;
        private EdgeRelation _relation;
        private bool _directed;
        private string _label;
        private List<MetaData> _metadata;

        public string source
        {
            get
            {
                return _source.id;
            }
        }

        public string target
        {
            get
            {
                return _target.id;
            }
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public EdgeRelation relation
        {
            get
            {
                return _relation;
            }
        }

        public string label
        {
            get
            {
                return _label;
            }
        }

        public bool directed
        {
            get
            {
                return _directed;
            }
        }

        public List<MetaData> metadata
        {
            get
            {
                return _metadata;
            }
        }

        public GraphEdge(GraphNode source, GraphNode target, EdgeRelation relation, string label, bool directed)
        {
            this._metadata = new List<MetaData>();
            this._source = source;
            this._target = target;
            this._relation = relation;
            this._label = label;
            this._directed = directed;
        }

        public void add(MetaData metadata)
        {
            this._metadata.Add(metadata);
        }

        public void add(List<MetaData> metadata)
        {
            this._metadata.AddRange(metadata);
        }
    }

    struct GraphNode
    {
        private string _id;
        private NodeType _type;
        private string _label;
        private List<MetaData> _metadata;
        private string _groupId;

        public string id
        {
            get
            {
                return _id;
            }
        }

        public string groupId => _groupId;

        [JsonConverter(typeof(StringEnumConverter))]
        public NodeType type
        {
            get
            {
                return _type;
            }
        }

        public string label
        {
            get
            {
                return _label;
            }
        }

        public List<MetaData> metadata
        {
            get
            {
                return _metadata;
            }
        }

        public GraphNode(string id, NodeType type, string label, string groupId = null)
        {
            this._metadata = new List<MetaData>();
            this._id = id;
            this._type = type;
            this._label = label;

            if (groupId == null)
            {
                _groupId = new Guid().ToString();
            }
            else
            {
                _groupId = groupId;
            }

        }

        public void add(MetaData metadata)
        {
            this._metadata.Add(metadata);
        }

        public void add(List<MetaData> metadata)
        {
            this._metadata.AddRange(metadata);
        }
    }

    struct MetaData
    {

    }

    class DfgJsonConverter
    {
        private DFG _dfg;
        private Graph _graph;

        public JsonGraph graph
        {
            get
            {
                JsonGraph g = new JsonGraph();
                g.graph = _graph;
                return g;
            }
        }

        public DfgJsonConverter(DFG dfg)
        {
            _dfg = dfg;
            _graph = new Graph();
            processNodes();
        }

        private void processNodes()
        {
            foreach (DFGNode n in _dfg.nodes)
            {
                GraphNode? source = processNode(n);

                if (source != null)
                {
                    foreach (DFGNode an in n.adjacencyList)
                    {
                        GraphNode? target = processNode(an);
                        if (target != null)
                        {
                            GraphEdge ge = new GraphEdge(source.Value, target.Value, EdgeRelation.Direct, "", true);
                            _graph.add(ge);
                        }
                    }
                }
            }
        }

        private GraphNode? processNode(DFGNode n)
        {
            GraphNode? gn;
            NodeType nt;
            string label;
            if (n.type == DFGNodeType.Subexpression)
            {
                //if 
                //gn = _graph.findNode(n.variable.identifier.name);
                //if (gn == null)
                //{
                //    gn = new GraphNode(n.variable.identifier.name, NodeType.Subexpression, n.variable.identifier.name, n.subGraphId.ToString());
                //    _graph.add(gn.Value);
                //}
                //nodeIdentifier = n.variable.identifier.name;
                nt = NodeType.Subexpression;
                label = n.variable.identifier.name;
            }
            else if (n.type == DFGNodeType.Return)
            {
                //gn = _graph.findNode(n.id.ToString());
                //if (gn == null)
                //{
                //    gn = new GraphNode(n.id.ToString(), NodeType.Return, n.label, n.subGraphId.ToString());
                //    _graph.add(gn.Value);
                //}
                nt = NodeType.Return;
                label = n.label;
                if (n.adjacencyList.Count == 0)
                {
                    return null;
                }
            }
            else if (n.type == DFGNodeType.Invocation)
            {
                //gn = new GraphNode(n.id.ToString(), NodeType.Invocation, n.label, n.subGraphId.ToString());
                //_graph.add(gn.Value);
                nt = NodeType.Invocation;
                label = n.label;
            }
            else if (n.type == DFGNodeType.High)
            {
                nt = NodeType.High;
                label = n.label;
                if (n.adjacencyList.Count == 0 && n.parents.Count == 0)
                {
                    return null;
                }
            }
            else if (n.type == DFGNodeType.Low)
            {
                nt = NodeType.Low;
                label = n.label;
                if (n.adjacencyList.Count == 0 && n.parents.Count == 0)
                {
                    return null;
                }
            }
            else
            {
                throw new ArgumentException();
            }

            gn = _graph.findNode(n.id.ToString());
            if (gn == null)
            {
                gn = new GraphNode(n.id.ToString(), nt, label, n.subGraphId.ToString());
                _graph.add(gn.Value);
            }


            return gn.Value;
        }

        private string getNewId()
        {
            return Guid.NewGuid().ToString();
        }
    }

    class AstJsonVisitor : SLVisitor
    {
        private const string DEFAULT_ID = "-1";
        private Graph _graph;
        private Stack<GraphNode> _nodeStack;

        public AstJsonVisitor(StatementList graph)
        {
            _nodeStack = new Stack<GraphNode>();
            _graph = new Graph();
            graph.accept(this);
        }

        public JsonGraph graph
        {
            get
            {
                JsonGraph g = new JsonGraph();
                g.graph = _graph;
                return g;
            }
        }

        public void visitAssign(Assign a)
        {
            GraphNode assignment = new GraphNode(getNewId(), NodeType.Assignment, "Assign");
            _graph.add(assignment);
            addASTEdge(assignment, "");

            visitChildren(assignment, a);
        }

        public void visitBinaryOperation(BinaryOperation bo)
        {
            string operation = bo.op == Ginger.GingerToken.Addition ? "+" : "-";
            GraphNode binaryOp = new GraphNode(getNewId(), NodeType.BinaryOperation, $"Binary Operation ({operation})");
            _graph.add(binaryOp);
            addASTEdge(binaryOp, "");

            visitChildren(binaryOp, bo);
        }

        //public void visitBoolean(GingerParser.Boolean b)
        //{
        //    GraphNode boolean = new GraphNode(getNewId(), NodeType.Boolean, "Boolean");
        //    _graph.add(boolean);
        //    addASTEdge(boolean, "");
        //}

        //public void visitBranch(If b)
        //{
        //    GraphNode branch = new GraphNode(getNewId(), NodeType.Branch, "If");
        //    _graph.add(branch);
        //    addASTEdge(branch, "");

        //    visitChildren(branch, b);
        //}

        public void visitVariable(Variable d)
        {
            GraphNode declaration = new GraphNode(getNewId(), NodeType.Declaration, "Declaration");
            _graph.add(declaration);
            addASTEdge(declaration, "");

            visitChildren(declaration, d);
        }

        public void visitIdentifier(Identifier i)
        {
            GraphNode identifier = new GraphNode(getNewId(), NodeType.Identifier, i.name);
            _graph.add(identifier);
            addASTEdge(identifier, "");
        }

        //public void visitInequalityOperation(InequalityOperation c)
        //{
        //    string operation = c.op == Ginger.GingerToken.LessThan ? "<" : ">";
        //    GraphNode inequalityOp = new GraphNode(getNewId(), NodeType.InequalityOperation, $"Inequality Operation ({operation})");
        //    _graph.add(inequalityOp);
        //    addASTEdge(inequalityOp, "");

        //    visitChildren(inequalityOp, c);
        //}

        public void visitInteger(Integer i)
        {
            GraphNode integer = new GraphNode(getNewId(), NodeType.Integer, "Integer");
            _graph.add(integer);
            addASTEdge(integer, "");
        }

        public void visitLiteral<T>(Literal<T> l) where T : Typeable
        {
            GraphNode literal = new GraphNode(getNewId(), NodeType.Literal, ((ILiteral)l.value).value);
            _graph.add(literal);
            addASTEdge(literal, "");
        }

        public void visitStatementList(StatementList sl)
        {
            GraphNode statementList = new GraphNode(getNewId(), NodeType.StatementList, "Statement List");
            _graph.add(statementList);

            if (_nodeStack.Count > 0)
            {
                addASTEdge(statementList, "");
            }

            visitChildren(statementList, sl);
        }

        //public void visitWhile(While w)
        //{
        //    GraphNode whileNode = new GraphNode(getNewId(), NodeType.While, "While");
        //    _graph.add(whileNode);
        //    addASTEdge(whileNode, "");

        //    visitChildren(whileNode, w);
        //}

        private void visitChildren(GraphNode gn, NodeCollection nc)
        {
            _nodeStack.Push(gn);
            foreach (Node n in nc)
            {
                n.accept(this);
            }
            _nodeStack.Pop();
        }

        private string getNewId()
        {
            return _graph.nodes.Count.ToString();
        }

        private void addASTEdge(GraphNode node, string label)
        {
            GraphEdge edge = new GraphEdge(_nodeStack.Peek(), node, EdgeRelation.AST, label, false);
            _graph.add(edge);
        }

        public void visitReturn(Return r)
        {
            GraphNode ret = new GraphNode(getNewId(), NodeType.Return, "Return");
            _graph.add(ret);
            addASTEdge(ret, "");

            visitChildren(ret, r);
        }

        public void visitFunction(Function f)
        {
            GraphNode function = new GraphNode(getNewId(), NodeType.Function, "Function");
            _graph.add(function);
            addASTEdge(function, "");

            visitChildren(function, f);
        }

        public void visitVariableList(VarList vl)
        {
            GraphNode varList = new GraphNode(getNewId(), NodeType.VariableList, "Variable List");
            _graph.add(varList);
            addASTEdge(varList, "");

            visitChildren(varList, vl);
        }

        //public void visitComponent(Component c)
        //{
        //    throw new NotImplementedException();
        //}

        //public void visitContract(Contract c)
        //{
        //    throw new NotImplementedException();
        //}

        //public void visitImplementation(Implementation i)
        //{
        //    throw new NotImplementedException();
        //}

        //public void visitVoid(GingerParser.Void v)
        //{
        //    throw new NotImplementedException();
        //}

        public void visitExpressionList(ExpressionList el)
        {
            GraphNode exprList = new GraphNode(getNewId(), NodeType.VariableList, "Expression List");
            _graph.add(exprList);
            addASTEdge(exprList, "");

            visitChildren(exprList, el);
        }

        public void visitInvocation(GingerParser.Invocation i)
        {
            GraphNode invocation = new GraphNode(getNewId(), NodeType.VariableList, "Invocation");
            _graph.add(invocation);
            addASTEdge(invocation, "");

            visitChildren(invocation, i);
        }

        public void visitSink(Sink s)
        {
            GraphNode sink = new GraphNode(getNewId(), NodeType.VariableList, $"Sink: {s.securityLevel.ToString()}");
            _graph.add(sink);
            addASTEdge(sink, "");

            visitChildren(sink, s);
        }

        public void visitSource(Source s)
        {
            GraphNode source = new GraphNode(getNewId(), NodeType.VariableList, $"Source: {s.securityLevel.ToString()}");
            _graph.add(source);
            addASTEdge(source, "");
        }
    }
}
