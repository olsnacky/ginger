using GingerUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GingerParser.DDG
{
    class DDGVisitor : SLVisitor
    {
        Scope.Scope _currentScope;
        Statement _currentStatement;

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

        public void visitBranch(If b)
        {
            _currentStatement = b;
            b.condition.accept(this);
            b.body.accept(this);
        }

        public void visitDeclaration(Declaration d)
        {
            return;
        }

        public void visitIdentifier(Identifier i)
        {
            HashSet<Assign> assignments = _currentStatement.findAssignment(i);
            foreach (Assign assignment in assignments)
            {
                assignment.dataDependencies.Add(_currentStatement);
            }
        }

        public void visitInequalityOperation(InequalityOperation c)
        {
            c.lhs.accept(this);
            c.rhs.accept(this);
        }

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
                    statement.accept(this);
                }
            }
        }

        public void visitWhile(While w)
        {
            _currentStatement = w;
            w.condition.accept(this);
            w.body.accept(this);
        }
    }
}
