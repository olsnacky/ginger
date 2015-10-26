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

    public abstract class Operation : SLNodeCollection
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

        public override bool Equals(object obj)
        {
            Operation op2 = obj as Operation;
            if (op2 == null)
            {
                return false;
            }

            return this.op == op2.op && this.lhs == op2.lhs && this.rhs == op2.rhs;
        }

        public static bool operator ==(Operation binOp1, Operation binOp2)
        {
            return binOp1.Equals(binOp2);
        }

        public static bool operator !=(Operation binOp1, Operation binOp2)
        {
            return !binOp1.Equals(binOp2);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int HASH_MULTIPLIER = 23;
                int hash = 17;
                hash = hash * HASH_MULTIPLIER + op.GetHashCode();
                hash = hash * HASH_MULTIPLIER + lhs.GetHashCode();
                hash = hash * HASH_MULTIPLIER + rhs.GetHashCode();
                return hash;
            }
        }
    }

    public class InequalityOperation : Operation
    {
        public InequalityOperation(GingerToken inequalityOp, Node lhs, Node rhs) : base(inequalityOp, lhs, rhs)
        {
            return;
        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitInequalityOperation(this);
        }
    }

    public class BinaryOperation : Operation
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

        public override bool Equals(object obj)
        {
            Declaration dec2 = obj as Declaration;
            if (dec2 == null)
            {
                return false;
            }

            // declaration part of scope partial
            return this.type == dec2.type && this.identifier == dec2.identifier && this.scope == dec2.scope;
        }

        public static bool operator ==(Declaration dec1, Declaration dec2)
        {
            if (System.Object.ReferenceEquals(dec1, dec2))
            {
                return true;
            }

            if (((object)dec1 == null) || ((object)dec2 == null))
            {
                return false;
            }

            return dec1.Equals(dec2);
        }

        public static bool operator !=(Declaration dec1, Declaration dec2)
        {
            return !(dec1 == dec2);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int HASH_MULTIPLIER = 23;
                int hash = 17;
                hash = hash * HASH_MULTIPLIER + type.GetHashCode();
                hash = hash * HASH_MULTIPLIER + identifier.GetHashCode();
                hash = hash * HASH_MULTIPLIER + scope.GetHashCode();
                return hash;
            }
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

        public override bool Equals(object obj)
        {
            Assign assign2 = obj as Assign;
            if (assign2 == null)
            {
                return false;
            }

            return this.identifier == assign2.identifier && this.expression == assign2.expression;
        }

        public static bool operator ==(Assign assign1, Assign assign2)
        {
            return assign1.Equals(assign2);
        }

        public static bool operator !=(Assign assign1, Assign assign2)
        {
            return !assign1.Equals(assign2);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                const int HASH_MULTIPLIER = 23;
                int hash = 17;
                hash = hash * HASH_MULTIPLIER + identifier.GetHashCode();
                hash = hash * HASH_MULTIPLIER + expression.GetHashCode();
                return hash;
            }
        }
    }

    public class Integer : Node
    {
        string _value;

        public string value
        {
            get { return _value; }
        }

        public Integer(string value) : base()
        {
            this._value = value;
        }

        public Integer() : base()
        {
            return;
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
                hash = hash * HASH_MULTIPLIER + value.GetHashCode();
                return hash;
            }
        }
    }

    public class Boolean : Node
    {
        public Boolean() : base()
        {
            return;
        }

        public override void accept(NodeVisitor v)
        {
            ((SLVisitor)v).visitBoolean(this);
        }
    }

    public partial class Identifier : Node
    {
        private string _name;

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
                hash = hash * HASH_MULTIPLIER + _name.GetHashCode();

                if (_declaration != null)
                {
                    hash = hash * HASH_MULTIPLIER + _declaration.GetHashCode();
                }
                
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
