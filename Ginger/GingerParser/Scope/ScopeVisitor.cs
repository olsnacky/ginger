using GingerUtil;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GingerParser.Scope
{
    public class ScopeVisitor : SLVisitor
    {
        private Scope _currentScope;
        private List<ParseException> _errors;
        private List<Variable> _formalArgs;
        private bool _inFunction = false;
        private bool _inVariableList = false;

        public List<ParseException> errors
        {
            get { return _errors; }
        }

        public ScopeVisitor(StatementList ast)
        {
            _errors = new List<ParseException>();
            _formalArgs = new List<Variable>();
            ast.accept(this);
        }

        public void visitStatementList(StatementList sl)
        {
            Scope parentScope = _currentScope;
            // create and assign a new scope to the statement list
            if (parentScope != null)
            {
                _currentScope = new Scope(parentScope);
            }
            else
            {
                _currentScope = new Scope();
            }

            sl.scope = _currentScope;

            // if there are formal args in play, assume they should be added to this scope
            while (_formalArgs.Count > 0)
            {
                Variable v = _formalArgs[0];
                _formalArgs.RemoveAt(0);
                visitVariable(v);
            }

            visitChildren(sl);

            // now that we've finished with the statement list return to the previous scope
            _currentScope = parentScope;
        }

        public void visitIdentifier(Identifier i)
        {
            i.declaration = _currentScope.find(i);
        }

        public void visitVariable(Variable d)
        {
            if (capturingFormalArgs())
            {
                _formalArgs.Add(d);
            }
            else
            {
                _currentScope.add(d);
                visitChildren(d);
            }
            //_currentScope.add(d);
            //    visitChildren(d);
        }

        //public void visitWhile(While w)
        //{
        //    visitChildren(w);
        //}

        //public void visitBranch(If b)
        //{
        //    visitChildren(b);
        //}

        //public void visitInequalityOperation(InequalityOperation c)
        //{
        //    visitChildren(c);
        //}

        public void visitBinaryOperation(BinaryOperation bo)
        {
            visitChildren(bo);
        }

        public void visitAssign(Assign a)
        {
            visitChildren(a);

            //if (!(a.identifier.canAssign((Typeable)a.expression))) {
            //    throw new TypeException(a.identifier.row, a.identifier.col, "This value cannot be assigned.");
            //}
        }

        //public void visitInteger(Integer i)
        //{
        //    return;
        //}

        //public void visitBoolean(Boolean b)
        //{
        //    return;
        //}

        private void visitChildren(NodeCollection nc)
        {
            foreach (Node n in nc)
            {
                try
                {
                    n.accept(this);
                }
                catch (ParseException pe)
                {
                    _errors.Add(pe);
                }
            }
        }

        public void visitLiteral<T>(Literal<T> l) where T : Typeable
        {
            return;
        }

        public void visitReturn(Return r)
        {
            visitChildren(r);
        }

        public void visitFunction(Function f)
        {
            _inFunction = true;
            visitChildren(f);
            _inFunction = false;
        }

        public void visitVariableList(VarList vl)
        {
            _inVariableList = true;
            visitChildren(vl);
            _inVariableList = false;
        }

        //public void visitVoid(Void v)
        //{
        //    throw new NotImplementedException();
        //}

        //public void visitComponent(Component c)
        //{
        //    throw new NotImplementedException();
        //}

        //public void visitContract(Contract c)
        //{
        //    throw new NotImplementedException();
        //}

        //public void visitImplementation(Implementation i)
        //{
        //    throw new NotImplementedException();
        //}

        public void visitExpressionList(ExpressionList el)
        {
            visitChildren(el);
        }

        public void visitInvocation(Invocation i)
        {
            _currentScope.add(i);
            visitChildren(i);
        }

        public void visitInteger(Integer i)
        {
            throw new NotImplementedException();
        }

        private bool capturingFormalArgs()
        {
            return _inFunction && _inVariableList;
        }

        public void visitSource(Source s)
        {
            return;
        }

        public void visitSink(Sink s)
        {
            visitChildren(s);
        }
    }
}
