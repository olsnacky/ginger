using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ginger;
using GingerUtil;

namespace GingerParser
{
    public interface ISourcePosition
    {
        int row
        {
            get;
        }

        int col
        {
            get;
        }
    }

    public interface ILiteral
    {
        string value
        {
            get;
        }
    }

    public abstract class SLNodeCollection : NodeCollection, ISourcePosition
    {
        public virtual int col
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public virtual int row
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override void add(Node n)
        {
            n.parents.Clear();
            n.parents.Add(this);
            children.Add(n);
        }
    }

    public partial class StatementList : SLNodeCollection
    {
        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitStatementList(this);
        }
    }

    public class FunctionList : SLNodeCollection
    {
        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitFunctionList(this);
        }
    }

    public partial class Statement : SLNodeCollection
    {
        public override void accept(NodeVisitor v)
        {
            throw new NotImplementedException();
        }
    }

    public partial class Declaration : SLNodeCollection
    {
        public override void accept(NodeVisitor v)
        {
            throw new NotImplementedException();
        }
    }

    //public partial class While : Statement
    //{
    //    private const int CONDITION_INDEX = 0;
    //    private const int BODY_INDEX = 1;

    //    public Node condition
    //    {
    //        get { return this.get(CONDITION_INDEX); }
    //    }

    //    public StatementList body
    //    {
    //        get { return (StatementList)this.get(BODY_INDEX); }
    //    }

    //    public While(Node condition, StatementList body) : base()
    //    {
    //        this.add(condition);
    //        this.add(body);
    //    }

    //    //public override void accept(NodeVisitor v)
    //    //{
    //    //    ((SLVisitor)v).visitWhile(this);
    //    //}
    //}

    //public partial class If : Statement
    //{
    //    private const int CONDITION_INDEX = 0;
    //    private const int BODY_INDEX = 1;

    //    public Node condition
    //    {
    //        get { return this.get(CONDITION_INDEX); }
    //    }

    //    public StatementList body
    //    {
    //        get { return (StatementList)this.get(BODY_INDEX); }
    //    }

    //    public If(Node condition, StatementList body) : base()
    //    {
    //        this.add(condition);
    //        this.add(body);
    //    }

    //    public override void accept(NodeVisitor v)
    //    {
    //        ((SLVisitor)v).visitBranch(this);
    //    }
    //}

    public class ExpressionList : NodeCollection
    {
        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitExpressionList(this);
        }
    }

    public class VarList : SLNodeCollection
    {
        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitVariableList(this);
        }

        public VarList()
        {

        }
    }

    public partial class Component : SLNodeCollection
    {
        private const int VARIABLE_INDEX = 0;
        private const int IMPORT_LIST_INDEX = 1;
        private const int FUNCTION_LIST_INDEX = 2;
        private const int EXTENDS_INDEX = 3;
        private GingerToken _type;

        public Variable variable
        { 
            get
            {
                return (Variable)get(VARIABLE_INDEX);
            }
        }

        public ImportList importList
        {
            get
            {
                return (ImportList)get(IMPORT_LIST_INDEX);
            }
        }

        public FunctionList functionList
        {
            get
            {
                return (FunctionList)get(FUNCTION_LIST_INDEX);
            }
        }

        public Function entry
        {
            get
            {
                Function result = null;
                if (variable.identifier.name.Equals("app"))
                {
                    foreach (Function f in functionList)
                    {
                        if (f.name.identifier.name.Equals("main"))
                        {
                            result = f;
                            break;
                        }
                    }
                }

                return result;
            }
        }

        public Identifier extends
        {
            get
            {
                return (Identifier)get(EXTENDS_INDEX);
            }
        }

        public GingerToken type => _type;        

        public Component(GingerToken type, Variable name, ImportList il, FunctionList fl) : base()
        {
            initialise(type, name, il, fl);
        }

        public Component(GingerToken type, Variable name, ImportList il, FunctionList fl, Identifier extends) : base()
        {
            initialise(type, name, il, fl);
            add(extends);
        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitComponent(this);
        }

        private void initialise(GingerToken type, Variable name, ImportList il, FunctionList fl)
        {
            _type = type;
            add(name);
            add(il);
            add(fl);
        }
    }

    public partial class ComponentList : SLNodeCollection
    {
        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitComponentList(this);
        }
    }

    public class Function : Declaration
    {
        //private const int TYPE_INDEX = 0;
        private const int IDENTIFIER_INDEX = 0;
        private const int VARIABLE_LIST_INDEX = 1;
        private const int STATEMENT_LIST_INDEX = 2;

        public StatementList body
        {
            get { return (StatementList)get(STATEMENT_LIST_INDEX); }
        }

        public VarList formalParams
        {
            get { return (VarList)get(VARIABLE_LIST_INDEX); }
        }

        public Variable name
        {
            get { return ((Variable)get(IDENTIFIER_INDEX)); }
        }

        public Function(Variable name, VarList variableList, StatementList body) : base()
        {
            //this.add(returnType);
            this.add(name);
            this.add(variableList);
            this.add(body);
        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitFunction(this);
        }
    }

    public partial class Invocation : SLNodeCollection
    {
        private const int IDENTIFIER_INDEX = 0;
        private const int EXPRESSION_LIST_INDEX = 1;

        public ExpressionList expressionList
        {
            get { return (ExpressionList)this.get(EXPRESSION_LIST_INDEX); }
        }

        public Identifier identifier
        {
            get { return (Identifier)this.get(IDENTIFIER_INDEX); }
        }

        public Invocation(Identifier ident, ExpressionList exprlist)
        {
            this.add(ident);
            this.add(exprlist);
        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitInvocation(this);
        }
    }

    //public class Component : SLNodeCollection
    //{
    //    public Component(Identifier name, Type componentType, StatementList body)
    //    {
    //        this.add(name);
    //        this.add(componentType);
    //        this.add(body);
    //    }

    //    public override void accept(NodeVisitor v)
    //    {
    //        ((SLVisitor)v).visitComponent(this);
    //    }
    //}

    public class Return : Statement
    {
        private const int EXPRESSION_INDEX = 0;
        public Node expression
        {
            get
            {
                try
                {
                    return this.get(EXPRESSION_INDEX);
                }
                catch
                {
                    return null;
                }
            }
        }

        public Return(Node expression = null) : base()
        {
            if (expression != null)
            {
                this.add(expression);
            }
        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitReturn(this);
        }
    }

    public abstract partial class Operation : SLNodeCollection
    {
        private const int LHS_INDEX = 0;
        private const int RHS_INDEX = 1;
        private GingerToken _op;

        public GingerToken op
        {
            get { return _op; }
        }

        public Node lhs
        {
            get { return this.get(LHS_INDEX); }
        }

        public Node rhs
        {
            get { return this.get(RHS_INDEX); }
        }

        public Operation(GingerToken op, Node lhs, Node rhs) : base()
        {
            this._op = op;
            this.add(lhs);
            this.add(rhs);
        }
    }

    public abstract class InformationEndpoint : SLNodeCollection
    {
        private GingerToken _securityLevel;

        public GingerToken securityLevel
        {
            get { return this._securityLevel; }
        }

        public InformationEndpoint(GingerToken securityLevel)
        {
            this._securityLevel = securityLevel;
        }
    }

    public class Source : InformationEndpoint
    {
        public Source(GingerToken securityLevel) : base(securityLevel)
        {

        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitSource(this);
        }
    }

    public class Sink : InformationEndpoint
    {
        private const int EXPR_INDEX = 0;

        public Node expr
        {
            get { return this.get(EXPR_INDEX); }
        }

        public Sink(GingerToken securityLevel, Node expr) : base(securityLevel)
        {
            this.add(expr);
        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitSink(this);
        }
    }

    //public partial class InequalityOperation : Operation
    //{
    //    public override int row
    //    {
    //        get
    //        {
    //            return ((ISourcePosition)lhs).row;
    //        }
    //    }

    //    public override int col
    //    {
    //        get
    //        {
    //            return ((ISourcePosition)lhs).col;
    //        }
    //    }
    //    public InequalityOperation(GingerToken inequalityOp, Node lhs, Node rhs) : base(inequalityOp, lhs, rhs)
    //    {
    //        return;
    //    }

    //    public override void accept(NodeVisitor v)
    //    {
    //        ((SLVisitor)v).visitInequalityOperation(this);
    //    }
    //}

    public partial class BinaryOperation : Operation
    {
        public BinaryOperation(GingerToken binaryOp, Node lhs, Node rhs) : base(binaryOp, lhs, rhs)
        {
            return;
        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitBinaryOperation(this);
        }
    }

    public class ImportList : SLNodeCollection
    {
        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitImportList(this);
        }
    }

    public class Import : Declaration
    {
        private const int IDENTIFIER_INDEX = 0;

        public Identifier identifier
        {
            get { return (Identifier)get(IDENTIFIER_INDEX); }
        }

        public Import(Identifier i)
        {
            add(i);
        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitImport(this);
        }
    }

    public class Variable : Declaration
    {
        //private const int TYPE_INDEX = 0;
        private const int IDENTIFIER_INDEX = 0;
        private const int STATEMENT_LIST_INDEX = 0;
        //private bool _passByReference;

        //public override int row
        //{
        //    get
        //    {
        //        return ((ISourcePosition)type).row;
        //    }
        //}

        //public override int col
        //{
        //    get
        //    {
        //        return ((ISourcePosition)type).col;
        //    }
        //}

        //public Node type
        //{
        //    get { return this.get(TYPE_INDEX); }
        //}

        public Identifier identifier
        {
            get { return (Identifier)this.get(IDENTIFIER_INDEX); }
        }

        //public bool passByReference
        //{
        //    get { return this._passByReference; }
        //}

        public Scope.Scope scope
        {
            get { return ((Component)this.parents[STATEMENT_LIST_INDEX]).scope; }
        }

        //public Variable(Identifier identifier, bool passByReference = false)
        public Variable(Identifier identifier)
        {
            //this.add(type);
            this.add(identifier);
            //this._passByReference = passByReference;
        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitVariable(this);
        }
    }

    public partial class Assign : Statement
    {
        private const int IDENTIFIER_INDEX = 0;
        private const int EXPRESSION_INDEX = 1;

        public Identifier identifier
        {
            get { return (Identifier)this.get(IDENTIFIER_INDEX); }
        }

        public Node expression
        {
            get { return this.get(EXPRESSION_INDEX); }
        }

        public Assign(Identifier identifier, Node expression)
        {
            this.add(identifier);
            this.add(expression);
        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitAssign(this);
        }
    }

    public abstract class Type : Node, ISourcePosition
    {
        private int _row;
        private int _col;

        public Type(int row, int col) : base()
        {
            this._row = row;
            this._col = col;
        }

        public int col
        {
            get
            {
                return _col;
            }
        }

        public int row
        {
            get
            {
                return _row;
            }
        }

        public override void accept(NodeVisitor v)
        {
            throw new NotImplementedException();
        }
    }

    public partial class Integer : Type, ILiteral
    {
        private string _value;

        public string value
        {
            get { return _value; }
        }

        public Integer(int row, int col, string value) : base(row, col)
        {
            this._value = value;
        }

        public Integer(int row, int col) : base(row, col)
        {

        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitInteger(this);
        }

        public override bool Equals(object obj)
        {
            Integer integer2 = obj as Integer;
            if (integer2 == null)
            {
                return false;
            }

            return this.value == integer2.value;
        }

        public static bool operator ==(Integer integer1, Integer integer2)
        {
            return integer1.Equals(integer2);
        }

        public static bool operator !=(Integer integer1, Integer integer2)
        {
            return !integer1.Equals(integer2);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int HASH_MULTIPLIER = 23;
                int hash = 17;

                if (value != null)
                {
                    hash = hash * HASH_MULTIPLIER + value.GetHashCode();
                }

                return hash;
            }
        }
    }

    //public partial class Boolean : Type, ILiteral
    //{
    //    private string _value;

    //    public string value
    //    {
    //        get
    //        {
    //            return _value;
    //        }
    //    }

    //    public Boolean(int row, int col) : base(row, col)
    //    {

    //    }

    //    public Boolean(int row, int col, string value) : base(row, col)
    //    {
    //        this._value = value;
    //    }

    //    public override void accept(NodeVisitor v)
    //    {
    //        ((SLVisitor)v).visitBoolean(this);
    //    }
    //}

    //public partial class Void : Type
    //{
    //    public Void(int row, int col) : base(row, col)
    //    {

    //    }

    //    public override void accept(NodeVisitor v)
    //    {
    //        ((SLVisitor)v).visitVoid(this);
    //    }
    //}

    //public partial class Contract : Type
    //{
    //    public Contract(int row, int col) : base(row, col)
    //    {

    //    }

    //    public override void accept(NodeVisitor v)
    //    {
    //        ((SLVisitor)v).visitContract(this);
    //    }
    //}

    //public partial class Implementation : Type
    //{
    //    public Implementation(int row, int col) : base(row, col)
    //    {

    //    }

    //    public override void accept(NodeVisitor v)
    //    {
    //        ((SLVisitor)v).visitImplementation(this);
    //    }
    //}

        public enum IdentifierType
    {
        Simple,
        Compound
    }

    public class ImportIdentifierComparer : IEqualityComparer<Identifier>
    {
        public bool Equals(Identifier x, Identifier y)
        {
            return x.name.Equals(y.name);
        }

        public int GetHashCode(Identifier obj)
        {
            return obj.name.GetHashCode();
        }
    }

    public partial class Identifier : Node, ISourcePosition
    {
        private string _name;
        private int _row;
        private int _col;

        public string name
        {
            get { return _name; }
        }

        public int row
        {
            get
            {
                return _row;
            }
        }

        public int col
        {
            get
            {
                return _col;
            }
        }

        public List<Identifier> parts
        {
            get
            {
                List<Identifier> result = new List<Identifier>();
                string[] tokens = _name.Split(Lexicon.IDENT_SEPARATOR);
                if (tokens.Length == 1)
                {
                    result.Add(this);
                }
                else if (tokens.Length > 1)
                {
                    foreach (string token in tokens)
                    {
                        result.Add(new Identifier(_row, _col, token));
                    }
                }
                else
                {
                    throw new ArgumentException();
                }

                return result;
            }
        }

        public IdentifierType type
        {
            get
            {
                
                if (parts.Count > 1)
                {
                    return IdentifierType.Compound;
                }
                else if (parts.Count == 1)
                {
                    return IdentifierType.Simple;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
        }

        public Identifier(int row, int col, string name) : base()
        {
            this._row = row;
            this._col = col;
            this._name = name;
        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitIdentifier(this);
        }

        public override bool Equals(object obj)
        {
            Identifier ident2 = obj as Identifier;
            if (ident2 == null)
            {
                return false;
            }

            // declaration part of scope partial
            return this.name == ident2.name;
        }

        public static bool operator ==(Identifier ident1, Identifier ident2)
        {
            if (System.Object.ReferenceEquals(ident1, ident2))
            {
                return true;
            }

            if (((object)ident1 == null) || ((object)ident2 == null))
            {
                return false;
            }

            return ident1.Equals(ident2);
        }

        public static bool operator !=(Identifier ident1, Identifier ident2)
        {
            if (ident1 == null)
            {
                return false;
            }

            return !ident1.Equals(ident2);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int HASH_MULTIPLIER = 23;
                int hash = 17;
                // hash = hash * HASH_MULTIPLIER + _name.GetHashCode();

                if (_declaration != null)
                {
                    hash = hash * HASH_MULTIPLIER + _declaration.GetHashCode();
                }

                return hash;
            }
        }
    }

    public partial class Literal<T> : Node, ISourcePosition
    {
        T _value;

        public T value
        {
            get { return _value; }
        }

        public int row
        {
            get
            {
                return ((ISourcePosition)value).row;
            }
        }

        public int col
        {
            get
            {
                return ((ISourcePosition)value).col;
            }
        }

        public Literal(T value) : base()
        {
            this._value = value;
        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitLiteral(this);
        }
    }
}
