using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GingerParser;
using GingerUtil;

namespace GingerCFG
{
    public class ASTVisitor : SLVisitor
    {
        private CFGEntry _cfg;
        private Node currentNode;
        //private Node previousNode;

        public CFGEntry cfg
        {
            get { return _cfg; }
        }

        //private BasicBlock currentBasicBlock
        //{
        //    get { return this.currentNode as BasicBlock; }
        //}

        public ASTVisitor(StatementList ast)
        {
            this._cfg = new CFGEntry();
            this.currentNode = this._cfg;
            ast.accept(this);
            //this.currentBasicBlock = null;
            //this.previousNode = this._cfg;
        }

        public void visitAssign(Assign a)
        {
            addStatement(a);
        }

        public void visitBinaryOperation(BinaryOperation bo)
        {
            // do nothing
        }

        public void visitBoolean(GingerParser.Boolean b)
        {
            // do nothing
        }

        public void visitBranch(Branch b)
        {
            addStatement(b);
            //currentBasicBlock.addStatement(b);
            // we're going to use this for the no path
            //BasicBlock branchBlock = this.currentBasicBlock;

            // if meet condition
            //prepareForNewBasicBlock();
            visitChildren(b);

            // if don't meet condition
            //prepareForNewBasicBlock();
        }

        public void visitCompare(Compare c)
        {
            // do nothing
        }

        public void visitDeclaration(Declaration d)
        {
            addStatement(d);
        }

        public void visitIdentifier(Identifier i)
        {
            // do nothing
        }

        public void visitInteger(Integer i)
        {
            // do nothing
        }

        public void visitLiteral(Literal l)
        {
            // do nothing
        }

        public void visitStatementList(StatementList sl)
        {
            if (sl.Count() > 0)
            {
                // the basic block representing the current point in the statement list
                CFGBasicBlock slbb = new CFGBasicBlock();
                this.currentNode.add(slbb);
                this.currentNode = slbb;

                for (int i = 0; i < sl.Count(); i++)
                {
                    // this statement list has been broken
                    // create a new basic block and link it
                    if (this.currentNode != slbb)
                    {
                        // if there are more statements to come
                        CFGBasicBlock bb = new CFGBasicBlock();
                        currentNode.add(bb);
                        slbb.add(bb);
                        slbb = bb;
                        this.currentNode = bb;
                    }

                    sl.get(i).accept(this);
                }
            }
        }

        public void visitWhile(While w)
        {
            addStatement(w);
            visitChildren(w);
        }

        private void visitChildren(NodeCollection nc)
        {
            foreach (Node n in nc)
            {
                n.accept(this);
            }
        }

        private void addStatement(Node statement)
        {
            CFGBasicBlock currentBasicBlock = currentNode as CFGBasicBlock;
            currentBasicBlock.addStatement(statement);
        }
    }
}
