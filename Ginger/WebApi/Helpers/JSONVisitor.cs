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
        AST,
        ControlFlow
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

        public bool edgeExists(GraphNode source, GraphNode target, EdgeRelation relation)
        {
            foreach(GraphEdge edge in _edges)
            {
                if (edge.source == source.id && edge.target == target.id && edge.relation == relation)
                {
                    return true;
                }
            }

            return false;
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

    public class MappingComparer : IEqualityComparer<Node>
    {
        public bool Equals(Node obj1, Node obj2)
        {
            return Object.ReferenceEquals(obj1, obj2);
        }

        public int GetHashCode(Node obj)
        {
            return obj.GetHashCode();
        }
    }

    class JSONVisitor : SLVisitor
    {
        private const string DEFAULT_ID = "-1";
        private Graph _graph;
        private Stack<GraphNode> _nodeStack;
        private Dictionary<Node, GraphNode> _mappings;

        public JSONVisitor(StatementList graph)
        {
            _mappings = new Dictionary<Node, GraphNode>(new MappingComparer());
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
            _mappings.Add(a, assignment);
            _graph.add(assignment);
            addASTEdge(assignment, "");
            addCFGEdges(a);

            visitChildren(assignment, a);
        }

        public void visitBinaryOperation(BinaryOperation bo)
        {
            string operation = bo.op == Ginger.GingerToken.Addition ? "+" : "-";
            GraphNode binaryOp = new GraphNode(getNewId(), NodeType.BinaryOperation, $"Binary Operation ({operation})");
            _mappings.Add(bo, binaryOp);
            _graph.add(binaryOp);
            addASTEdge(binaryOp, "");

            visitChildren(binaryOp, bo);
        }

        public void visitBoolean(GingerParser.Boolean b)
        {
            GraphNode boolean = new GraphNode(getNewId(), NodeType.Boolean, "Boolean");
            _mappings.Add(b, boolean);
            _graph.add(boolean);
            addASTEdge(boolean, "");
        }

        public void visitBranch(If b)
        {
            GraphNode branch = new GraphNode(getNewId(), NodeType.Branch, "If");
            _mappings.Add(b, branch);
            _graph.add(branch);
            addASTEdge(branch, "");
            addCFGEdges(b);

            visitChildren(branch, b);
        }

        public void visitDeclaration(Declaration d)
        {
            GraphNode declaration = new GraphNode(getNewId(), NodeType.Declaration, "Declaration");
            _mappings.Add(d, declaration);
            _graph.add(declaration);
            addASTEdge(declaration, "");

            visitChildren(declaration, d);
        }

        public void visitIdentifier(Identifier i)
        {
            GraphNode identifier = new GraphNode(getNewId(), NodeType.Identifier, i.name);
            _mappings.Add(i, identifier);
            _graph.add(identifier);
            addASTEdge(identifier, "");
        }

        public void visitInequalityOperation(InequalityOperation c)
        {
            string operation = c.op == Ginger.GingerToken.LessThan ? "<" : ">";
            GraphNode inequalityOp = new GraphNode(getNewId(), NodeType.InequalityOperation, $"Inequality Operation ({operation})");
            _mappings.Add(c, inequalityOp);
            _graph.add(inequalityOp);
            addASTEdge(inequalityOp, "");

            visitChildren(inequalityOp, c);
        }

        public void visitInteger(Integer i)
        {
            GraphNode integer = new GraphNode(getNewId(), NodeType.Integer, "Integer");
            _mappings.Add(i, integer);
            _graph.add(integer);
            addASTEdge(integer, "");
        }

        public void visitLiteral<T>(Literal<T> l) where T : Typeable
        {
            GraphNode literal = new GraphNode(getNewId(), NodeType.Literal, ((ILiteral)l.value).value);
            _mappings.Add(l, literal);
            _graph.add(literal);
            addASTEdge(literal, "");
        }

        public void visitStatementList(StatementList sl)
        {
            GraphNode statementList = new GraphNode(getNewId(), NodeType.StatementList, "Statement List");
            _mappings.Add(sl, statementList);
            _graph.add(statementList);

            if (_nodeStack.Count > 0)
            {
                addASTEdge(statementList, "");
            }

            visitChildren(statementList, sl);
        }

        public void visitWhile(While w)
        {
            GraphNode whileNode = new GraphNode(getNewId(), NodeType.While, "While");
            _mappings.Add(w, whileNode);
            _graph.add(whileNode);
            addASTEdge(whileNode, "");
            addCFGEdges(w);

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

        private void addCFGEdge(GraphNode source, GraphNode target)
        {
            GraphEdge edge = new GraphEdge(source, target, EdgeRelation.ControlFlow, "CF", true);
            _graph.add(edge);
        }

        private void addCFGEdges(Statement statement)
        {
            addCFGEdges(statement, true, statement.cfgSuccessors);
            addCFGEdges(statement, false, statement.cfgPredecessors);
        }

        private void addCFGEdges(Statement statement, bool source, List<Statement> linkedStatements)
        {
            foreach (Statement linkedStatement in linkedStatements)
            {
                GraphNode firstNode;
                GraphNode secondNode;

                if (_mappings.TryGetValue(statement, out firstNode) && _mappings.TryGetValue(linkedStatement, out secondNode))
                {
                    GraphNode sourceNode;
                    GraphNode targetNode;

                    if (source)
                    {
                        sourceNode = firstNode;
                        targetNode = secondNode;
                    }
                    else
                    {
                        sourceNode = secondNode;
                        targetNode = firstNode;
                    }

                    if (!_graph.edgeExists(sourceNode, targetNode, EdgeRelation.ControlFlow))
                    {
                        addCFGEdge(sourceNode, targetNode);
                    }
                }
            }
        }
    }
}
