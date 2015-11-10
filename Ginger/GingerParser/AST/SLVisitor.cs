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
        void visitBranch(If b);
        void visitInequalityOperation(InequalityOperation c);
        void visitBinaryOperation(BinaryOperation bo);
        void visitDeclaration(Declaration d);
        void visitAssign(Assign a);
        void visitInteger(Integer i);
        void visitBoolean(Boolean b);
        void visitIdentifier(Identifier i);
        void visitLiteral<T>(Literal<T> l) where T : Typeable;
    }
}