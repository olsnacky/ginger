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
        private List<Node> linkNodes;
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
            this.linkNodes = new List<Node>();
            this._cfg = new CFGEntry();
            this.currentNode = this._cfg;
            ast.accept(this);

            CFGExit exit = new CFGExit();

            if (this.linkNodes.Count > 0)
            {
                this.currentNode = new CFGExit();
                linkParentNodes();
            }
            else
            {
                this.currentNode.add(exit);
            }
            
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
                bool link = true;
                Node linkNode;

                // the basic block representing the current point in the statement list
                CFGBasicBlock slbb = new CFGBasicBlock();
                linkNode = slbb;

                this.currentNode.add(slbb);
                this.currentNode = slbb;
                //linkParentNodes();

                for (int i = 0; i < sl.Count(); i++)
                {
                    // this statement list has been broken
                    // create a new basic block and link it
                    if (this.currentNode != slbb)
                    {
                        link = false;
                        // if there are more statements to come
                        CFGBasicBlock bb = new CFGBasicBlock();
                        //linkNode = bb;
                        this.currentNode.add(bb);
                        //linkParentNodes();
                        slbb.add(bb);
                        slbb = bb;
                        this.currentNode = bb;
                        //linkParentNodes();
                        clearParentNodes();
                    }

                    sl.get(i).accept(this);
                }

                if (link == true)
                {
                    linkNodes.Add(linkNode);
                }
            } 
        }

        public void visitWhile(While w)
        {
            addStatement(w);
            visitChildren(w);
        }

        private void linkParentNodes()
        {
            const int INDEX = 0;
            while (this.linkNodes.Count > 0)
            {
                Node n = this.linkNodes[INDEX];
                this.linkNodes.RemoveAt(INDEX);
                n.add(this.currentNode);
            }
        }

        private void clearParentNodes()
        {
            this.linkNodes.Clear();
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
