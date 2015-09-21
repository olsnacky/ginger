using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GingerParser
{
    public interface NodeVisitor
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
}
