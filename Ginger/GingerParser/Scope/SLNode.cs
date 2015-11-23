using GingerUtil;
using System;

namespace GingerParser
{
    public interface Typeable
    {
        System.Type evaluateType();
        bool canAssign(Typeable target);
        bool canCompare(Typeable target);
    }

    public partial class StatementList
    {
        private Scope.Scope _scope;

        public Scope.Scope scope
        {
            get { return _scope; }
            set { _scope = value; }
        }
    }

    public abstract partial class Operation : Typeable
    {
        public bool canAssign(Typeable target)
        {
            return ((Typeable)this.evaluateType()).canAssign(target);
        }

        public bool canCompare(Typeable target)
        {
            return ((Typeable)this.evaluateType()).canCompare(target);
        }

        public System.Type evaluateType()
        {
            throw new NotImplementedException();
        }
    }

    public partial class InequalityOperation : Typeable
    {
        public new System.Type evaluateType()
        {
            if (((Typeable)this.lhs).canCompare((Typeable)this.rhs))
            {
                return typeof(Boolean);
            }

            throw new TypeException(((ISourcePosition)this.lhs).row, ((ISourcePosition)this.lhs).col, "This value cannot be compared.");
        }
    }

    public partial class BinaryOperation : Typeable
    {
        public new System.Type evaluateType()
        {
            if (((Typeable)this.lhs).canAssign((Typeable)this.rhs))
            {
                return ((Typeable)this.lhs).evaluateType();
            }

            throw new TypeException(((ISourcePosition)this.lhs).row, ((ISourcePosition)this.lhs).col, "This value cannot be assigned." );
        }
    }

    public partial class Identifier : Typeable
    {
        private Declaration _declaration;

        // the original declaration for this identifier
        public Declaration declaration
        {
            get { return _declaration; }
            set { _declaration = value; }
        }

        public bool canAssign(Typeable target)
        {
            return ((Typeable)declaration.type).canAssign(target);
        }

        public bool canCompare(Typeable target)
        {
            return ((Typeable)declaration.type).canCompare(target);
        }

        public System.Type evaluateType()
        {
            return ((Typeable)declaration.type).evaluateType();
        }
    }

    public partial class Integer : Typeable
    {
        public bool canAssign(Typeable target)
        {
            return target.evaluateType() == typeof(Integer);
        }

        public bool canCompare(Typeable target)
        {
            return this.canAssign(target);
        }

        public System.Type evaluateType()
        {
            return this.GetType();
        }
    }

    public partial class Boolean : Typeable
    {
        public bool canAssign(Typeable target)
        {
            return target.evaluateType() == typeof(Boolean);
        }

        public bool canCompare(Typeable target)
        {
            return this.canAssign(target);
        }

        public System.Type evaluateType()
        {
            return this.GetType();
        }
    }

    public partial class Literal<T> : Typeable where T : Typeable
    {
        public bool canAssign(Typeable target)
        {
            return this.value.canAssign(target);
        }

        public bool canCompare(Typeable target)
        {
            return this.value.canCompare(target);
        }

        public System.Type evaluateType()
        {
            return this.value.evaluateType();
        }
    }

    public class TypeException : ParseException
    {
        public TypeException(int row, int col, string reason) : base(row, col, reason, ExceptionLevel.ERROR)
        {

        }
    }
}
