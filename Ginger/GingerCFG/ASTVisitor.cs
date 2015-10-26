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

        public void visitBranch(If b)
        {
            addStatement(b);
            visitChildren(b);
        }

        public void visitInequalityOperation(InequalityOperation c)
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
                    // if this statement list has been broken
                    // create a new basic block and link it
                    if (this.currentNode != slbb)
                    {
                        // do not link this block to the successer of the statement list
                        link = false;
                        // if there are more statements to come
                        CFGBasicBlock bb = new CFGBasicBlock();

                        // we've just exited out of some kind of branch, make this new block a child of it
                        this.currentNode.add(bb);
                        // now make this new block a child of the original statement list
                        slbb.add(bb);
                        // now the new block becomes the representative statement list
                        slbb = bb;

                        this.currentNode = bb;

                        // why does this work?????
                        clearLinkNodes();
                    }

                    // handle the next statement
                    Node nextStatement = sl.get(i);

                    if (nextStatement.GetType() == typeof(While))
                    {
                        // a while statement needs a basic block of its own
                        CFGBasicBlock bb = new CFGBasicBlock();
                        this.currentNode.add(bb);
                        slbb = bb;
                        this.currentNode = slbb;
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
            // assume the header is the current node
            CFGBasicBlock whileHeader = (CFGBasicBlock)this.currentNode;

            addStatement(w);
            visitChildren(w);

            // exiting the statement list
            // we have to link whatever is the current basic block
            // back to the head basic block for this while
            this.currentNode.add(whileHeader);
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
