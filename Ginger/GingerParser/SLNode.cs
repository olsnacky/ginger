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
    public abstract class SLNodeCollection : NodeCollection
    {
        //public SLNodeCollection() : base()
        //{

        //}

        public override void add(Node n)
        {
            n.parents.Clear();
            n.parents.Add(this);
            children.Add(n);
        }
    }

    public class StatementList : SLNodeCollection
    {
        private Scope _scope;

        public Scope scope
        {
            get { return _scope; }
            set { _scope = value; }
        }

        public StatementList() : base()
        {
            
        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitStatementList(this);
        }
    }

    public class While : SLNodeCollection
    {
        public While(Compare condition, StatementList body) : base()
        {
            this.add(condition);
            this.add(body);
        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitWhile(this);
        }
    }

    public class Branch : SLNodeCollection
    {
        public Compare condition
        {
            get { return (Compare)this.get(0); }
        }

        public StatementList body
        {
            get { return (StatementList)this.get(1); }
        }

        public Branch(Compare condition, StatementList body) : base()
        {
            this.add(condition);
            this.add(body);
        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitBranch(this);
        }
    }

    public class Compare : SLNodeCollection
    {
        private GingerToken compareOp;
        public Compare(GingerToken compareOp, Node value1, Node value2) : base()
        {
            this.compareOp = compareOp;
            this.add(value1);
            this.add(value2);
        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitCompare(this);
        }
    }

    public class BinaryOperation : SLNodeCollection
    {
        private GingerToken binaryOp;
        public BinaryOperation(GingerToken binaryOp, Node value1, Node value2) : base()
        {
            this.binaryOp = binaryOp;
            this.add(value1);
            this.add(value2);
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

        public Node type
        {
            get { return this.get(TYPE_INDEX); }
        }

        public Identifier identifier
        {
            get { return (Identifier)this.get(IDENTIFIER_INDEX); }
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

    public class Assign : SLNodeCollection
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

    public class Integer : Node
    {
        string _value;

        public Integer(string value) : base()
        {
            this._value = value;
        }

        public Integer() : base()
        {

        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitInteger(this);
        }
    }

    public class Boolean : Node
    {
        public Boolean() : base()
        {

        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitBoolean(this);
        }
    }

    public class Identifier : Node
    {
        private Declaration _declaration;
        private string _name;

        // the original declaration for this identifier
        public Declaration declaration
        {
            get { return _declaration; }
            set { _declaration = value; }
        }

        public string name
        {
            get { return _name; }
        }

        public Identifier(string name) : base() {
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

            return this.name == ident2.name;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int HASH_MULTIPLIER = 23;
                int hash = 17;
                hash = hash * HASH_MULTIPLIER + _name.GetHashCode();
                return hash;
            }
        }
    }

    public class Literal<T> : Node
    {
        T _value;

        public T value
        {
            get { return _value; }
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
