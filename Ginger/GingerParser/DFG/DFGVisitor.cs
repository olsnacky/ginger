using Ginger;
using GingerUtil;
using System;
using System.Collections.Generic;

namespace GingerParser.DFG
{
    public struct DeferredInvocation
    {
        public Invocation invocation;
        public DFGNode invocationNode;
        public DFGVisitor visitor;

        public DeferredInvocation(Invocation i, DFGNode invNode, DFGVisitor dfgv)
        {
            visitor = dfgv;
            invocation = i;
            invocationNode = invNode;
        }
    }

    public class DFGVisitor : SLVisitor
    {
        private bool _addingSources;
        private StatementList _ast;
        //private bool _buildingExprList;
        private bool _capturingFormalParams;
        private DFG _dfg;
        private List<List<Identifier>> _exprList;
        private Dictionary<Identifier, DFG> _functionGraphs;
        private bool _isParent;
        //VarList _formalParams;
        //List<DFGNode> _formalParams;
        //private DFGNode _high;
        //private DFGNode _low;
        private List<Identifier> _sources;
        private List<DeferredInvocation> _deferredInvocations;

        public DFG dfg
        {
            get { return _dfg; }
        }

        public DFGVisitor(StatementList ast)
        {
            _isParent = true;
            initialise(ast, null, null, null, null, null);
        }

        public DFGVisitor(StatementList ast, VarList formalParams, DFGNode high, DFGNode low, Dictionary<Identifier, DFG> functionGraphs, List<DeferredInvocation> deferredInvocations)
        {
            _isParent = false;
            initialise(ast, functionGraphs, formalParams, high, low, deferredInvocations);
        }

        private void initialise(StatementList ast, Dictionary<Identifier, DFG> functionGraphs, VarList formalParams, DFGNode high, DFGNode low, List<DeferredInvocation> di)
        {
            _addingSources = false;
            //_buildingExprList = false;
            _capturingFormalParams = false;
            //_formalParams = formalParams;
            _sources = new List<Identifier>();
            _ast = ast;
            _dfg = new DFG(high, low);

            if (di == null)
            {
                _deferredInvocations = new List<DeferredInvocation>();
            }
            else
            {
                _deferredInvocations = di;
            }

            if (functionGraphs == null)
            {
                _functionGraphs = new Dictionary<Identifier, DFG>();
            }
            else
            {
                _functionGraphs = functionGraphs;
            }

            if (formalParams != null)
            {
                _capturingFormalParams = true;
                foreach (Variable v in formalParams)
                {
                    visitVariable(v);
                }
                _capturingFormalParams = false;
            }

            _ast.accept(this);
            if (_isParent)
            {
                foreach (DeferredInvocation defInv in _deferredInvocations)
                {
                    defInv.visitor._visitDeferredInvocation(defInv);
                    //_visitDeferredInvocation(defInv);
                }
            }
        }

        public void visitAssign(Assign a)
        {
            _addingSources = true;
            a.expression.accept(this);
            if (_sources.Count > 0)
            {
                _dfg.addAssign(_sources, a.identifier);
                _sources.Clear();
            }
            _addingSources = false;
        }

        public void visitBinaryOperation(BinaryOperation bo)
        {
            visitChildren(bo);
        }

        public void visitExpressionList(ExpressionList el)
        {
            _addingSources = true;
            _exprList = new List<List<Identifier>>();
            foreach (Node n in el)
            {
                n.accept(this);
                List<Identifier> paramSources = new List<Identifier>(_sources);
                _exprList.Add(paramSources);
                _sources.Clear();
            }
            _addingSources = false;
        }

        public void visitFunction(Function f)
        {
            DFGVisitor fv = new DFGVisitor(f.body, f.formalParams, _dfg.high, _dfg.low, _functionGraphs, _deferredInvocations);
            _functionGraphs.Add(f.name.identifier, fv.dfg);
        }

        public void visitIdentifier(Identifier i)
        {
            if (_addingSources)
            {
                _sources.Add(i);
            }
        }

        public void visitInteger(Integer i)
        {
            throw new NotImplementedException();
        }

        public void visitInvocation(GingerParser.Invocation i)
        {
            _deferredInvocations.Add(new DeferredInvocation(i, _dfg.addInvocation(i), this));
            //DFG fdfg = _functionGraphs[i.name].Copy();
            //fdfg.high = _dfg.high;
            //fdfg.low = _dfg.low;

            //foreach (DFGNode n in fdfg.nodes)
            //{
            //    if (n.parents.Contains(fdfg.high))
            //    {
            //        fdfg.high.addEdge(n);
            //    }
            //    else if (n.parents.Contains(fdfg.low))
            //    {
            //        fdfg.low.addEdge(n);
            //    }
            //}

            //fdfg.resetIds();
            //i.expressionList.accept(this);
            //_dfg.addInvocation(i, fdfg, _exprList);
            i.name.accept(this);
        }

         void _visitDeferredInvocation(DeferredInvocation di)
        {
            Invocation i = di.invocation;
            DFG fdfg = _functionGraphs[i.name].Copy();
            fdfg.high = _dfg.high;
            fdfg.low = _dfg.low;

            foreach (DFGNode n in fdfg.nodes)
            {
                if (n.parents.Contains(fdfg.high))
                {
                    fdfg.high.addEdge(n);
                }
                else if (n.parents.Contains(fdfg.low))
                {
                    fdfg.low.addEdge(n);
                }
            }

            fdfg.resetIds();
            i.expressionList.accept(this);
            _dfg.addDeferredInvocation(di, fdfg, _exprList);

        }

        public void visitLiteral<T>(Literal<T> l) where T : Typeable
        {
            return;
        }

        public void visitReturn(Return r)
        {
            _addingSources = true;
            if (r.expression != null)
            {
                r.expression.accept(this);
            }

            if (_sources.Count > 0)
            {
                _dfg.addReturn(_sources);
                _sources.Clear();
            }
            _addingSources = false;
        }

        public void visitSink(Sink s)
        {
            Identifier i = new Identifier(0, 0, s.securityLevel.ToString());
            Assign a = new Assign(i, s.expr);
            a.accept(this);
        }

        public void visitSource(Source s)
        {
            Identifier i = new Identifier(0, 0, s.securityLevel.ToString());
            i.accept(this);
        }

        public void visitStatementList(StatementList sl)
        {
            foreach (Node statement in sl)
            {
                statement.accept(this);
            }
        }

        public void visitVariable(Variable d)
        {
            DFGNode dfgn = new DFGNode(d);
            _dfg.addNode(dfgn);
            if (_capturingFormalParams)
            {
                _dfg.addFormalParam(dfgn);
            }
        }

        public void visitVariableList(VarList vl)
        {
            throw new NotImplementedException();
        }

        private DFGNode getSecurityNode(GingerToken securityLevel)
        {
            if (securityLevel == GingerToken.High)
            {
                return _dfg.high;
            }
            else if (securityLevel == GingerToken.Low)
            {
                return _dfg.low;
            }
            else
            {
                throw new ArgumentException();
            }
        }

        //private bool haveSecurityNode(GingerToken securityLevel)
        //{
        //    if (securityLevel == GingerToken.High)
        //    {
        //        return _high != null;
        //    }
        //    else if (securityLevel == GingerToken.Low)
        //    {
        //        return _low != null;
        //    }
        //    else
        //    {
        //        throw new ArgumentException();
        //    }
        //}

        //private void confirmSecurityLevel(GingerToken securityLevel, DFGNode n)
        //{
        //    if (securityLevel == GingerToken.High)
        //    {
        //        _high = n;
        //    }
        //    else if (securityLevel == GingerToken.Low)
        //    {
        //       _low = n;
        //    }
        //}

        //private Identifier processSecurityLevel(GingerToken securityLevel)
        //{
        //    Identifier i;
        //    if (!haveSecurityNode(securityLevel))
        //    {
        //        i = new Identifier(0, 0, securityLevel.ToString());
        //        Variable v = new Variable(i);
        //        DFGNode dfgn = new DFGNode(v);
        //        _dfg.addNode(dfgn);
        //        confirmSecurityLevel(securityLevel, dfgn);
        //    }
        //    else
        //    {
        //        DFGNode securityNode = getSecurityNode(securityLevel);
        //        i = securityNode.variable.identifier;
        //    }

        //    return i;
        //}

        private void visitChildren(NodeCollection nc)
        {
            foreach (Node n in nc)
            {
                n.accept(this);
            }
        }
    }
}
