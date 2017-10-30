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
        Dictionary<Identifier, StatementList> _functions;

        public Dictionary<Identifier, StatementList> functions
        {
            get { return _functions; }
        }

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

        //public void visitBranch(If b)
        //{
        //    linkPastStatements(_previousStatements, b);
        //    _previousStatements.Enqueue(b);
        //    b.body.accept(this);
        //}

        public void visitVariable(Variable d)
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
            foreach (Node n in sl)
            {
                n.accept(this);
            }
        }

        //public void visitWhile(While w)
        //{
        //    linkPastStatements(_previousStatements, w);
        //    _previousStatements.Enqueue(w);
        //    w.body.accept(this);

        //    // add the while header as a successor to the final statement in the body
        //    linkPastStatementWithoutConsuming(_previousStatements, w);
        //}

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

        public void visitReturn(Return r)
        {
            return;
        }

        public void visitFunction(Function f)
        {
            f.body.accept(this);
            _functions.Add(f.name.identifier, f.body);
        }

        public void visitVariableList(VarList vl)
        {
            throw new NotImplementedException();
        }

        public void visitExpressionList(ExpressionList el)
        {
            throw new NotImplementedException();
        }

        public void visitInvocation(Invocation i)
        {
            
        }

        public void visitSource(Source s)
        {
            throw new NotImplementedException();
        }

        public void visitSink(Sink s)
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
    }
}
