using GingerUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GingerParser.CFG
{
    public class CFGVisitor : SLVisitor
    {
        Queue<Statement> _previousStatements;
        List<ParseException> _errors;

        public CFGVisitor(StatementList ast)
        {
            _previousStatements = new Queue<Statement>();
            _errors = new List<ParseException>();
            ast.accept(this);
        }

        public void visitAssign(Assign a)
        {
            linkPastStatements(_previousStatements, a);
        }

        public void visitBinaryOperation(BinaryOperation bo)
        {
            return;
        }

        public void visitBoolean(Boolean b)
        {
            return;
        }

        public void visitBranch(If b)
        {
            linkPastStatements(_previousStatements, b);
            _previousStatements.Enqueue(b);
            b.body.accept(this);
        }

        public void visitInequalityOperation(InequalityOperation c)
        {
            return;
        }

        public void visitDeclaration(Declaration d)
        {
            return;
        }

        public void visitIdentifier(Identifier i)
        {
            return;
        }

        public void visitInteger(Integer i)
        {
            return;
        }

        public void visitStatementList(StatementList sl)
        {
            foreach (Node statement in sl)
            {
                if (statement is Statement)
                {
                    if (sl.entry == null)
                    {
                        sl.entry = statement;
                    }

                    statement.accept(this); // link multiple previous statements here
                    _previousStatements.Enqueue((Statement)statement);
                }
            }
        }

        public void visitWhile(While w)
        {
            linkPastStatements(_previousStatements, w);
            _previousStatements.Enqueue(w);
            w.body.accept(this);

            // add the while header as a successor to the final statement in the body
            linkPastStatementWithoutConsuming(_previousStatements, w);
        }

        private void linkPastStatements(Queue<Statement> statements, Statement s)
        {
            while (statements.Count > 0)
            {
                Statement previousStatement = statements.Dequeue();
                previousStatement.addSuccessor(s);
            }
        }

        private void linkPastStatementWithoutConsuming(Queue<Statement> statements, Statement s)
        {
            Statement previousStatement = statements.Peek();
            previousStatement.addSuccessor(s);
        }

        public void visitLiteral<T>(Literal<T> l) where T : Typeable
        {
            return;
        }
    }
}
