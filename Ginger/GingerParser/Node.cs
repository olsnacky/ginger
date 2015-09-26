using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ginger;

namespace GingerParser
{
    public abstract class Node
    {
        private Node _parent;

        public Node parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public virtual void add(Node n)
        {
            throw new NotImplementedException();
        }

        public virtual void remove(Node n)
        {
            throw new NotImplementedException();
        }

        public virtual Node get(int index)
        {
            throw new NotImplementedException();
        }

        public abstract void accept(NodeVisitor v);
    }

    public abstract class NodeCollection : Node, IEnumerable<Node>
    {
        private List<Node> children;

        public NodeCollection()
        {
            children = new List<Node>();
        }

        public override void add(Node n)
        {
            n.parent = this;
            children.Add(n);
        }

        public override void remove(Node n)
        {
            children.Remove(n);
        }

        public override Node get(int index)
        {
            return children[index];
        }

        public IEnumerator<Node> GetEnumerator()
        {
            return children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class StatementList : NodeCollection
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
            v.visitStatementList(this);
        }
    }

    public class While : NodeCollection
    {
        public While(Compare condition, StatementList body) : base()
        {
            this.add(condition);
            this.add(body);
        }

        public override void accept(NodeVisitor v)
        {
            v.visitWhile(this);
        }
    }

    public class Branch : NodeCollection
    {
        public Branch(Compare condition, StatementList body) : base()
        {
            this.add(condition);
            this.add(body);
        }

        public override void accept(NodeVisitor v)
        {
            v.visitBranch(this);
        }
    }

    public class Compare : NodeCollection
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
            v.visitCompare(this);
        }
    }

    public class BinaryOperation : NodeCollection
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
            v.visitBinaryOperation(this);
        }
    }

    public class Declaration : NodeCollection
    {
        private const int IDENTIFIER_INDEX = 1;
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
            v.visitDeclaration(this);
        }

        //public override bool Equals(object obj)
        //{
        //    Declaration dec2 = obj as Declaration;
        //    if (dec2 == null)
        //    {
        //        return false;
        //    }

        //    return identifier == dec2.identifier && this.parent == dec2.parent;
        //}

        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}
    }

    public class Assign : NodeCollection
    {
        public Assign(Identifier identifier, Node expression)
        {
            this.add(identifier);
            this.add(expression);
        }

        public override void accept(NodeVisitor v)
        {
            v.visitAssign(this);
        }
    }

    public class Integer : Node
    {
        public Integer() : base()
        {

        }

        public override void accept(NodeVisitor v)
        {
            v.visitInteger(this);
        }
    }

    public class Boolean : Node
    {
        public Boolean() : base()
        {

        }

        public override void accept(NodeVisitor v)
        {
            v.visitBoolean(this);
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
            v.visitIdentifier(this);
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

    public class Literal : Node
    {
        public Literal() : base() {

        }

        public override void accept(NodeVisitor v)
        {
            v.visitLiteral(this);
        }
    }
}
