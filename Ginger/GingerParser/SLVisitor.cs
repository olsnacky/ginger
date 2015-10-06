using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GingerUtil;

namespace GingerParser
{
    public interface SLVisitor : NodeVisitor
    {
        void visitStatementList(StatementList sl);
        void visitWhile(While w);
        void visitBranch(Branch b);
        void visitCompare(Compare c);
        void visitBinaryOperation(BinaryOperation bo);
        void visitDeclaration(Declaration d);
        void visitAssign(Assign a);
        void visitInteger(Integer i);
        void visitBoolean(Boolean b);
        void visitIdentifier(Identifier i);
        void visitLiteral(Literal l);
    }

    public class ScopeVisitor : SLVisitor
    {
        private Scope currentScope;

        public ScopeVisitor(StatementList ast)
        {
            ast.accept(this);
        }

        public void visitStatementList(StatementList sl)
        {
            Scope parentScope = currentScope;
            // create and assign a new scope to the statement list
            if (parentScope != null)
            {
                currentScope = new Scope(parentScope);
            }
            else
            {
                currentScope = new Scope();
            }

            sl.scope = currentScope;

            visitChildren(sl);

            // now that we've finished with the statement list return to the previous scope
            currentScope = parentScope;
        }

        public void visitIdentifier(Identifier i)
        {
            i.declaration = currentScope.find(i);
        }

        public void visitDeclaration(Declaration d)
        {
            currentScope.add(d);
            visitChildren(d);
        }

        public void visitWhile(While w)
        {
            visitChildren(w);
        }

        public void visitBranch(Branch b)
        {
            visitChildren(b);
        }

        public void visitCompare(Compare c)
        {
            visitChildren(c);
        }

        public void visitBinaryOperation(BinaryOperation bo)
        {
            visitChildren(bo);
        }

        public void visitAssign(Assign a)
        {
            visitChildren(a);
        }

        public void visitInteger(Integer i)
        {
            return;
        }

        public void visitBoolean(Boolean b)
        {
            return;
        }

        public void visitLiteral(Literal l)
        {
            return;
        }

        private void visitChildren(NodeCollection nc)
        {
            foreach (Node n in nc)
            {
                n.accept(this);
            }
        }
    }
}
