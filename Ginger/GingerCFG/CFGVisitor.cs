using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GingerUtil;

namespace GingerCFG
{
    public interface CFGVisitor : NodeVisitor
    {
        void visitEntry(CFGEntry s);
        void visitBasicBlock(CFGBasicBlock bb);
        void visitExit(CFGExit e);
    }
}
