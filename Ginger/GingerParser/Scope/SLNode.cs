using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GingerParser
{
    public partial class StatementList
    {
        private Scope _scope;

        public Scope scope
        {
            get { return _scope; }
            set { _scope = value; }
        }
    }

    public partial class Identifier
    {
        private Declaration _declaration;

        // the original declaration for this identifier
        public Declaration declaration
        {
            get { return _declaration; }
            set { _declaration = value; }
        }
    }
}
