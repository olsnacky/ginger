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
        public StatementList() : base()
        {
            return;
        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitStatementList(this);
        }
    }

    public partial class Statement : SLNodeCollection
    {
        public override void accept(NodeVisitor v)
        {
            throw new NotImplementedException();
        }
    }

    public partial class While : Statement
    {
        private const int CONDITION_INDEX = 0;
        private const int BODY_INDEX = 1;

        public InequalityOperation condition
        {
            get { return (InequalityOperation)this.get(CONDITION_INDEX); }
        }

        public StatementList body
        {
            get { return (StatementList)this.get(BODY_INDEX); }
        }

        public While(InequalityOperation condition, StatementList body) : base()
        {
            this.add(condition);
            this.add(body);
        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitWhile(this);
        }
    }

    public partial class If : Statement
    {
        private const int CONDITION_INDEX = 0;
        private const int BODY_INDEX = 1;

        public InequalityOperation condition
        {
            get { return (InequalityOperation)this.get(CONDITION_INDEX); }
        }

        public StatementList body
        {
            get { return (StatementList)this.get(BODY_INDEX); }
        }

        public If(InequalityOperation condition, StatementList body) : base()
        {
            this.add(condition);
            this.add(body);
        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitBranch(this);
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

    public partial class InequalityOperation : Operation
    {
        public override int row
        {
            get
            {
                return ((ISourcePosition)lhs).row;
            }
        }

        public override int col
        {
            get
            {
                return ((ISourcePosition)lhs).col;
            }
        }
        public InequalityOperation(GingerToken inequalityOp, Node lhs, Node rhs) : base(inequalityOp, lhs, rhs)
        {
            return;
        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitInequalityOperation(this);
        }
    }

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

    public class Declaration : SLNodeCollection
    {
        private const int TYPE_INDEX = 0;
        private const int IDENTIFIER_INDEX = 1;
        private const int STATEMENT_LIST_INDEX = 0;

        public override int row
        {
            get
            {
                return ((ISourcePosition)type).row;
            }
        }

        public override int col
        {
            get
            {
                return ((ISourcePosition)type).col;
            }
        }

        public Node type
        {
            get { return this.get(TYPE_INDEX); }
        }

        public Identifier identifier
        {
            get { return (Identifier)this.get(IDENTIFIER_INDEX); }
        }

        public Scope.Scope scope
        {
            get { return ((StatementList)this.parents[STATEMENT_LIST_INDEX]).scope; }
        }

        public Declaration(Node type, Identifier identifier)
        {
            this.add(type);
            this.add(identifier);
        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitDeclaration(this);
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

    public class Type : Node, ISourcePosition
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

    public partial class Integer : Node, ISourcePosition
    {
        private string _value;
        private int _row;
        private int _col;

        public string value
        {
            get { return _value; }
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

        public Integer(int row, int col, string value) : base()
        {
            this._row = row;
            this._col = col;
            this._value = value;
        }

        public Integer(int row, int col) : base()
        {
            this._row = row;
            this._col = col;
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

    public partial class Boolean : Node, ISourcePosition
    {
        private int _row;
        private int _col;

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

        public Boolean(int row, int col) : base()
        {
            this._row = row;
            this._col = col;
        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitBoolean(this);
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

        public Identifier(int row, int col, string name) : base() {
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

        public Literal(T value) : base() {
            this._value = value;
        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitLiteral(this);
        }
    }
}
