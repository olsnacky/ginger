using GingerUtil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GingerParser
{
    public partial class Assign
    {
        private HashSet<Statement> _dataDependencies = new HashSet<Statement>();

        public HashSet<Statement> dataDependencies
        {
            get { return _dataDependencies; }
        }
    }

    public partial class Statement
    {
        public HashSet<Assign> findAssignment(Identifier i, HashSet<Statement> visitedNodes)
        {
            HashSet<Assign> assignStatements = new HashSet<Assign>();

            if (this is Assign && ((Assign)this).identifier.declaration == i.declaration)
            {
                assignStatements.Add((Assign)this);
                return assignStatements;
            }
            else if (this.cfgPredecessors.Count > 0)
            {
                foreach (Statement predecessor in this.cfgPredecessors)
                {
                    if (!(visitedNodes.Contains(predecessor)))
                    {
                        visitedNodes.Add(predecessor);
                        assignStatements.UnionWith(predecessor.findAssignment(i, visitedNodes));
                    }
                }

                return assignStatements;
            }
            else
            {
                throw new AccessException();
            }
        }
    }

    public class AccessException : Exception
    {
        public AccessException() : base()
        {

        }
    }
}
