using GingerUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GingerParser.DDG
{
    public class DDGVisitor : SLVisitor
    {
        Scope.Scope _currentScope;
        Statement _currentStatement;
        List<ParseException> _errors;

        public List<ParseException> errors
        {
            get { return _errors; }
        }

        public DDGVisitor(StatementList ast)
        {
            _errors = new List<ParseException>();
            ast.accept(this);
        }

        public void visitAssign(Assign a)
        {
            _currentStatement = a;
            a.expression.accept(this);
        }

        public void visitBinaryOperation(BinaryOperation bo)
        {
            bo.lhs.accept(this);
            bo.rhs.accept(this);
        }

        public void visitBoolean(Boolean b)
        {
            return;
        }

        //public void visitBranch(If b)
        //{
        //    _currentStatement = b;
        //    b.condition.accept(this);
        //    b.body.accept(this);
        //}

        public void visitVariable(Variable d)
        {
            return;
        }

        public void visitIdentifier(Identifier i)
        {
            HashSet<Assign> assignments = new HashSet<Assign>();
                
            if (_currentStatement.cfgPredecessors.Count > 0)
            {
                foreach (Statement predecessor in _currentStatement.cfgPredecessors)
                {
                    assignments.UnionWith(predecessor.findAssignment(i, new HashSet<Statement>()));
                }
            }
            else
            {
                throw new AccessException(i.row, i.col, "This value has not been initialised.");
            }
              
                
            foreach (Assign assignment in assignments)
            {
                assignment.dataDependencies.Add(_currentStatement);
            }
        }

        //public void visitInequalityOperation(InequalityOperation c)
        //{
        //    c.lhs.accept(this);
        //    c.rhs.accept(this);
        //}

        public void visitInteger(Integer i)
        {
            return;
        }

        public void visitLiteral<T>(Literal<T> l) where T : Typeable
        {
            return;
        }

        public void visitStatementList(StatementList sl)
        {
            foreach (Node statement in sl)
            {
                if (statement is Statement)
                {
                    _currentScope = sl.scope;

                    try
                    {
                        statement.accept(this);
                    }
                    catch (ParseException pe)
                    {
                        _errors.Add(pe);
                    }
                    
                }
            }
        }

        //public void visitWhile(While w)
        //{
        //    _currentStatement = w;
        //    w.condition.accept(this);
        //    w.body.accept(this);
        //}

        public void visitReturn(Return r)
        {
            return;
        }

        public void visitFunction(Function f)
        {
            throw new NotImplementedException();
        }

        public void visitVariableList(VarList vl)
        {
            throw new NotImplementedException();
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
            foreach (Node n in el)
            {
                n.accept(this);
            }
        }

        public void visitInvocation(Invocation i)
        {
            i.expressionList.accept(this);
        }

        public void visitSource(Source s)
        {
            throw new NotImplementedException();
        }

        public void visitSink(Sink s)
        {
            throw new NotImplementedException();
        }
    }
}
