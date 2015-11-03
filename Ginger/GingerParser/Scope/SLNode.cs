using GingerUtil;
using System;

namespace GingerParser
{
    public interface Typeable
    {
        System.Type evaluateType();
        bool canAssign(Typeable target);
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

    public partial class Identifier
    {
        private Declaration _declaration;

        // the original declaration for this identifier
        public Declaration declaration
        {
            get { return _declaration; }
            set { _declaration = value; }
        }
    }

    public partial class Integer : Typeable
    {
        public bool canAssign(Typeable target)
        {
            return target.evaluateType() == typeof(Integer);
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

        public System.Type evaluateType()
        {
            return this.GetType();
        }
    }
}
