using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GingerParser.Scope
{
    //public class ImportIdentifierComparer : IEqualityComparer<Identifier>
    //{
    //    public bool Equals(Identifier x, Identifier y)
    //    {
    //        return x.name.Equals(y.name);
    //    }

    //    public int GetHashCode(Identifier obj)
    //    {
    //        return obj.name.GetHashCode();
    //    }
    //}

    public class Scope
    {
        private Scope _parent;
        private Dictionary<Identifier, Variable> _locals;
        private List<Scope> _children;
        private Dictionary<Identifier, int> _invocationCount;
        private Dictionary<Identifier, Scope> _imports;
        private Dictionary<Variable, Scope> _components;
        //private Dictionary<Identifier, Scope> _componentScopes;

        public Scope parent
        {
            get { return _parent; }
        }

        // global scope
        public Scope()
        {
            initialise();
            _components = new Dictionary<Variable, Scope>();
        }

        public Scope(Scope parent)
        {
            this._parent = parent;
            initialise();
        }

        //public void add(Component c)
        //{
        //    _componentScopes.Add(c.identifier, c.scope);
        //}

        public void add(Invocation invc)
        {
            Identifier funcName = invc.identifier;
            if (_invocationCount.ContainsKey(funcName))
            {
                _invocationCount[funcName] = _invocationCount[funcName] + 1;
            }
            else
            {
                _invocationCount[funcName] = 1;
            }

            invc.invocationCount = _invocationCount[funcName];
            invc.identifier.invocationCount = _invocationCount[funcName];
        }

        public void add(Variable declaration)
        {
            // an identifier cannot be declared more than once in the same scope
            if (_locals.ContainsKey(declaration.identifier)) {
                throw new ScopeException(declaration.identifier.row, declaration.identifier.col, $"Identifier {declaration.identifier.name} cannot be used twice within the same scope.");
            }

            _locals.Add(declaration.identifier, declaration);
        }

        public void add(Component c)
        {
            _components.Add(c.variable, c.scope);
        }

        public void add(Import i)
        {
            Scope componentScope = findComponentScope(i.identifier);
            _imports.Add(i.identifier, componentScope);
        }

        public void add(Scope scope)
        {
            _children.Add(scope);
        }

        private Scope findComponentScope(Identifier i)
        {
            if (_components != null)
            {
                Scope s;
                bool found = false;
                try
                {
                    found = _components.TryGetValue(i.declaration, out s);
                }
                catch
                { 
                    throw new ScopeException(i.row, i.col, $"{i.name} is not a component");
                }

                if (found)
                {
                    return s;
                }
                else
                {
                    throw new ScopeException(i.row, i.col, $"{i.name} is not a component");
                }
            }
            else
            {
                return _parent.findComponentScope(i);
            }
        }

        private Scope findImportScope(Identifier i)
        {
            if (_imports.ContainsKey(i))
            {
                return _imports[i];
            }
            else if (hasParent())
            {
                return _parent.findImportScope(i);
            }
            else
            {
                throw new ScopeException(i.row, i.col, $"{i.name} could not be found - did you forget to import it?");
            }
        }

        public Variable find(Identifier i)
        {
            if (i.type == IdentifierType.Simple)
            {
                if (_locals.ContainsKey(i))
                {
                    return _locals[i];
                }
                else if (hasParent())
                {
                    return _parent.find(i);
                }
                //else if (_componentScopes.ContainsKey(identifier))
                //{

                //}
                else
                {
                    throw new ScopeException(i.row, i.col, $"{i.name} has not been declared");
                }
            }
            else if (i.type == IdentifierType.Compound)
            {
                List<Identifier> parts = i.parts;
                // can only currently handle components
                if (parts.Count == 2)
                {
                    Scope componentScope = findImportScope(parts[0]);
                    return componentScope.find(parts[1]);
                }
                else
                {
                    throw new ArgumentException();
                }

                //Scope activeScope = this;
                //Variable result = null;
                //while (parts.Count > 0)
                //{
                //    Identifier part = parts[0];
                //    parts.RemoveAt(0);

                //    Variable v = activeScope.find(part);
                //    if (parts.Count == 0)
                //    {
                //        result = v;
                //        break;
                //    }
                //    else
                //    {
                //        activeScope = v.scope;
                //    }                    
                //}

                //if (result != null)
                //{
                //    return result;
                //}
                //else
                //{
                //    throw new ScopeException(identifier.row, identifier.col, $"{identifier.name} has not been declared.");
                //}
            }
            else
            {
                throw new ArgumentException();
            }
        }

        public bool hasParent()
        {
            return _parent != null;
        }

        private void initialise()
        {
            _children = new List<Scope>();
            _locals = new Dictionary<Identifier, Variable>();
            _invocationCount = new Dictionary<Identifier, int>();
            _imports = new Dictionary<Identifier, Scope>(new ImportIdentifierComparer());
        }
    }

    public class ScopeException : ParseException
    {
        public ScopeException(int row, int col, string reason) : base(row, col, reason, ExceptionLevel.ERROR)
        {

        }
    }
}
