using GingerParser;
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
        AST
    }

    enum NodeType
    {
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
        Boolean
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
            if (_nodes == null) {
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

        public string id
        {
            get
            {
                return _id;
            }
        }

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

        public GraphNode(string id, NodeType type, string label)
        {
            this._metadata = new List<MetaData>();
            this._id = id;
            this._type = type;
            this._label = label;
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

    class JSONVisitor : SLVisitor
    {
        private const string DEFAULT_ID = "-1";
        private Graph _graph;
        private Stack<GraphNode> _nodeStack;

        public JSONVisitor(StatementList graph)
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
            GraphNode assignment = new GraphNode(getNewId(), NodeType.Assignment, "");
            _graph.add(assignment);
            addASTEdge(assignment, "");

            visitChildren(assignment, a);
        }

        public void visitBinaryOperation(BinaryOperation bo)
        {
            GraphNode binaryOp = new GraphNode(getNewId(), NodeType.BinaryOperation, "");
            _graph.add(binaryOp);
            addASTEdge(binaryOp, "");

            visitChildren(binaryOp, bo);
        }

        public void visitBoolean(GingerParser.Boolean b)
        {
            GraphNode boolean = new GraphNode(getNewId(), NodeType.Boolean, "");
            _graph.add(boolean);
            addASTEdge(boolean, "");
        }

        public void visitBranch(If b)
        {
            GraphNode branch = new GraphNode(getNewId(), NodeType.Branch, "");
            _graph.add(branch);
            addASTEdge(branch, "");

            visitChildren(branch, b);
        }

        public void visitDeclaration(Declaration d)
        {
            GraphNode declaration = new GraphNode(getNewId(), NodeType.Declaration, "");
            _graph.add(declaration);
            addASTEdge(declaration, "");

            visitChildren(declaration, d);
        }

        public void visitIdentifier(Identifier i)
        {
            GraphNode identifier = new GraphNode(getNewId(), NodeType.Identifier, "");
            _graph.add(identifier);
            addASTEdge(identifier, "");
        }

        public void visitInequalityOperation(InequalityOperation c)
        {
            GraphNode inequalityOp = new GraphNode(getNewId(), NodeType.InequalityOperation, "");
            _graph.add(inequalityOp);
            addASTEdge(inequalityOp, "");

            visitChildren(inequalityOp, c);
        }

        public void visitInteger(Integer i)
        {
            GraphNode integer = new GraphNode(getNewId(), NodeType.Integer, "");
            _graph.add(integer);
            addASTEdge(integer, "");
        }

        public void visitLiteral<T>(Literal<T> l) where T : Typeable
        {
            GraphNode literal = new GraphNode(getNewId(), NodeType.Literal, "");
            _graph.add(literal);
            addASTEdge(literal, "");
        }

        public void visitStatementList(StatementList sl)
        {
            GraphNode statementList = new GraphNode(getNewId(), NodeType.StatementList, "");
            _graph.add(statementList);

            if (_nodeStack.Count > 0)
            {
                addASTEdge(statementList, "");
            }

            visitChildren(statementList, sl);
        }

        public void visitWhile(While w)
        {
            GraphNode whileNode = new GraphNode(getNewId(), NodeType.While, "");
            _graph.add(whileNode);
            addASTEdge(whileNode, "");

            visitChildren(whileNode, w);
        }

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
    }
}
