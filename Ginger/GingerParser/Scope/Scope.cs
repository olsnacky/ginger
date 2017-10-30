using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GingerParser.Scope
{
    public class Scope
    {
        private Scope _parent;
        private Dictionary<Identifier, Variable> locals;
        private List<Scope> children;
        private Dictionary<Identifier, int> _invocationCount;

        public Scope parent
        {
            get { return _parent; }
        }

        // global scope
        public Scope()
        {
            initialise();
        }

        public Scope(Scope parent)
        {
            this._parent = parent;
            initialise();
        }

        public void add(Invocation invc)
        {
            Identifier funcName = invc.name;
            if (_invocationCount.ContainsKey(funcName))
            {
                _invocationCount[funcName] = _invocationCount[funcName] + 1;
            }
            else
            {
                _invocationCount[funcName] = 1;
            }

            invc.invocationCount = _invocationCount[funcName];
            invc.name.invocationCount = _invocationCount[funcName];
        }

        public void add(Variable declaration)
        {
            // an identifier cannot be declared more than once in the same scope
            if (locals.ContainsKey(declaration.identifier)) {
                throw new ScopeException(declaration.identifier.row, declaration.identifier.col, $"Identifier {declaration.identifier.name} cannot be used twice within the same scope.");
            }

            locals.Add(declaration.identifier, declaration);
        }

        public void add(Scope scope)
        {
            children.Add(scope);
        }

        public Variable find(Identifier identifier)
        {
            if (locals.ContainsKey(identifier))
            {
                return locals[identifier];
            }
            else if (hasParent())
            {
                return _parent.find(identifier);
            }
            else
            {
                throw new ScopeException(identifier.row, identifier.col, "This identifier has not been declared.");
            }
        }

        public bool hasParent()
        {
            return _parent != null;
        }

        private void initialise()
        {
            children = new List<Scope>();
            locals = new Dictionary<Identifier, Variable>();
            _invocationCount = new Dictionary<Identifier, int>();
        }
    }

    public class ScopeException : ParseException
    {
        public ScopeException(int row, int col, string reason) : base(row, col, reason, ExceptionLevel.ERROR)
        {

        }
    }
}
