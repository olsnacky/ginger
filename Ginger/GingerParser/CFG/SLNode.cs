using GingerUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GingerParser
{
    public partial class StatementList
    {
        private Node _entry;

        public Node entry
        {
            get { return _entry; }
            set { _entry = value; }
        }
    }

    public partial class Statement
    {
        private List<Statement> _cfgSuccessors = new List<Statement>();
        private List<Statement> _cfgPredecessors = new List<Statement>();

        public List<Statement> cfgSuccessors
        {
            get { return _cfgSuccessors; }
        }

        public List<Statement> cfgPredecessors
        {
            get { return _cfgPredecessors; }
        }

        public void addSuccessor(Statement s)
        {
            _cfgSuccessors.Add(s);
            s.cfgPredecessors.Add(this);
        }
    }
}
