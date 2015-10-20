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
        }

        public void visitAssign(Assign a)
        {
            addStatement(a);
        }

        public void visitBinaryOperation(BinaryOperation bo)
        {
            return;
        }

        public void visitBoolean(GingerParser.Boolean b)
        {
            return;
        }

        public void visitBranch(Branch b)
        {
            addStatement(b);
            visitChildren(b);
        }

        public void visitCompare(Compare c)
        {
            return;
        }

        public void visitDeclaration(Declaration d)
        {
            addStatement(d);
        }

        public void visitIdentifier(Identifier i)
        {
            return;
        }

        public void visitInteger(Integer i)
        {
            return;
        }

        public void visitLiteral<T>(Literal<T> l)
        {
            return;
        }

        public void visitStatementList(StatementList sl)
        {
            if (sl.Count() > 0)
            {
                // use this to determine if we need to link the node succeeding
                // this statement list to be linked to it
                bool link = true;
                Node linkNode;

                // the basic block representing the current point in the statement list
                CFGBasicBlock slbb = new CFGBasicBlock();
                linkNode = slbb;

                // add this basic block as the child to the current node
                // and then set it as the current node
                this.currentNode.add(slbb);
                this.currentNode = slbb;

                for (int i = 0; i < sl.Count(); i++)
                {
                    // this statement list has been broken
                    // create a new basic block and link it
                    if (this.currentNode != slbb)
                    {
                        // do not link this block to the successer of the statement list
                        link = false;
                        // if there are more statements to come
                        CFGBasicBlock bb = new CFGBasicBlock();

                        // we've just exited out of some kinf of branch, make this ne wblock as child of it
                        this.currentNode.add(bb);
                        // now make this new block a child of the original statement list
                        slbb.add(bb);
                        // now the new block becomes the representative statement list
                        slbb = bb;
                        this.currentNode = bb;
                        // why does this work?????
                        clearLinkNodes();
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

        private void clearLinkNodes()
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
