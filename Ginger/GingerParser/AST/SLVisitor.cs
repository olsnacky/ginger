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
        void visitAssign(Assign a);
        //void visitBoolean(Boolean b);
        void visitBinaryOperation(BinaryOperation bo);
        //void visitBranch(If b);
        //void visitComponent(Component c);
        //void visitContract(Contract c);
        void visitVariable(Variable d);
        void visitExpressionList(ExpressionList el);
        void visitFunction(Function f);
        void visitIdentifier(Identifier i);
        //void visitImplementation(Implementation i);
        void visitInteger(Integer i);
        void visitInvocation(Invocation i);
        void visitLiteral<T>(Literal<T> l) where T : Typeable;
        void visitReturn(Return r);
        void visitSink(Sink s);
        void visitSource(Source s);
        void visitStatementList(StatementList sl);
        void visitVariableList(VarList vl);
        //void visitVoid(Void v);
    }
}