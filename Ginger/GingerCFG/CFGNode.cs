using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GingerUtil;
using GingerParser;

namespace GingerCFG
{
    public class CFGEntry : NodeCollection
    {
        public CFGEntry()
        {

        }

        public override void accept(NodeVisitor v)
        {
            ((CFGVisitor)v).visitEntry(this);
        }
    }

    public class CFGExit : Node
    {
        public CFGExit()
        {

        }

        public override void accept(NodeVisitor v)
        {
            ((CFGVisitor)v).visitExit(this);
        }
    }

    public class CFGBasicBlock : NodeCollection
    {
        private StatementList _statementList;

        public StatementList statementList
        {
            get { return _statementList; }
        }

        public CFGBasicBlock()
        {
            _statementList = new StatementList(); 
        }

        public override void accept(NodeVisitor v)
        {
            ((CFGVisitor)v).visitBasicBlock(this);
        }

        public void addStatement(Node statement)
        {
            _statementList.add(statement);
        }
    }
}
