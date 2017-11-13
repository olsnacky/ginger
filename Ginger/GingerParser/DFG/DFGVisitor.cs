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
        public Identifier inComponent;

        public DeferredInvocation(Invocation i, DFGNode invNode, DFGVisitor dfgv, Identifier inComp)
        {
            visitor = dfgv;
            invocation = i;
            invocationNode = invNode;
            inComponent = inComp;
        }
    }

    public class DFGVisitor : SLVisitor
    {
        private bool _addingSources;
        private SLNodeCollection _code;
        //private bool _buildingExprList;
        private bool _capturingFormalParams;
        private DFG _dfg;
        private List<List<Identifier>> _exprList;
        private Dictionary<Identifier, Dictionary<Identifier,DFG>> _componentFunctionGraphs;
        private bool _isParent;
        //VarList _formalParams;
        //List<DFGNode> _formalParams;
        //private DFGNode _high;
        //private DFGNode _low;
        private List<Identifier> _sources;
        private List<DeferredInvocation> _deferredInvocations;
        private bool _capturingComponent;
        private Identifier _currentComponent;
        private ComponentList _program;

        public DFG dfg
        {
            get { return _dfg; }
        }

        public DFGVisitor(ComponentList program)
        {
            _isParent = true;
            initialise(program, null, null, null, null, null, null);
            foreach (DeferredInvocation defInv in _deferredInvocations)
            {
                defInv.visitor._visitDeferredInvocation(defInv);
                //_visitDeferredInvocation(defInv);
            }
            //program.accept(this);
            cleanup();
        }

        private void cleanup()
        {
            foreach (DFGNode n in _dfg.nodes)
            {
                // remove connections between high and low
                // and stored graphs
                if (n.type == DFGNodeType.High || n.type == DFGNodeType.Low)
                {
                    n.adjacencyList.RemoveAll(nn => nn.subGraphId.Equals(new Guid()) && nn.type != DFGNodeType.High && nn.type != DFGNodeType.Low);
                }

                // cleanup return nodes that don't go anywhere
                n.adjacencyList.RemoveAll(nn => nn.type == DFGNodeType.Return && n.adjacencyList.Count == 0);
            }

            // cleanup return nodes that don't go anywhere
            _dfg.nodes.RemoveAll(n => n.type == DFGNodeType.Return && n.adjacencyList.Count == 0);
        }

        public DFGVisitor(StatementList functionCode, VarList formalParams, DFGNode high, DFGNode low, Dictionary<Identifier, Dictionary<Identifier,DFG>> componentFunctionGraphs, List<DeferredInvocation> deferredInvocations, Identifier currentComponent)
        {
            _isParent = false;
            initialise(functionCode, componentFunctionGraphs, formalParams, high, low, deferredInvocations, currentComponent);
        }

        private void initialise(SLNodeCollection code, Dictionary<Identifier, Dictionary<Identifier,DFG>> componentFunctionGraphs, VarList formalParams, DFGNode high, DFGNode low, List<DeferredInvocation> di, Identifier currentComponent)
        {
            _addingSources = false;
            //_buildingExprList = false;
            _capturingFormalParams = false;
            _capturingComponent = false;
            //_formalParams = formalParams;
            _sources = new List<Identifier>();
            _code = code;
            //_dfg = new DFG(high, low);
            _dfg = new DFG();
            _currentComponent = currentComponent;

            if (di == null)
            {
                _deferredInvocations = new List<DeferredInvocation>();
            }
            else
            {
                _deferredInvocations = di;
            }

            if (componentFunctionGraphs == null)
            {

                _componentFunctionGraphs = new Dictionary<Identifier, Dictionary<Identifier,DFG>>(new ImportIdentifierComparer());
            }
            else
            {
                _componentFunctionGraphs = componentFunctionGraphs;
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

            _code.accept(this);
            
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
            DFGVisitor fv = new DFGVisitor(f.body, f.formalParams, _dfg.high, _dfg.low, _componentFunctionGraphs, _deferredInvocations, _currentComponent);
            Dictionary<Identifier, DFG> functionList = _componentFunctionGraphs[_currentComponent];

            // if function has no return, add a ghost return
            if (fv.dfg.returnNode == null)
            {
                fv.dfg.addReturn();
            }

            functionList.Add(f.name.identifier, fv.dfg);
            if (_currentComponent.name.Equals("app") && f.name.identifier.name.Equals("main"))
            {
                Invocation main = new Invocation(f.name.identifier, new ExpressionList());
                main.accept(this);
            }
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

        public void visitInvocation(Invocation i)
        {
            _deferredInvocations.Add(new DeferredInvocation(i, _dfg.addInvocation(i), this, _currentComponent));
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
            i.identifier.accept(this);
        }

        private DFG _getGraph(Invocation i, Identifier component)
        {
            Identifier ident = i.identifier;
            if (ident.type == IdentifierType.Simple)
            {
                return _componentFunctionGraphs[component][ident].Copy();
            }
            else if (ident.type == IdentifierType.Compound)
            {
                List<Identifier> parts = ident.parts;
                if (parts.Count == 2)
                {
                    return _componentFunctionGraphs[parts[0]][parts[1]].Copy();
                }
                else
                {
                    throw new ArgumentException();
                }
                
            }
            else
            {
                throw new ArgumentException();
            }
        }

         void _visitDeferredInvocation(DeferredInvocation di)
        {
            Invocation i = di.invocation;
            DFG fdfg = _getGraph(i, di.inComponent);
            //fdfg.high = _dfg.high;
            //fdfg.low = _dfg.low;
            fdfg.replaceHigh(_dfg.high);
            fdfg.replaceLow(_dfg.low);

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
            else
            {
                _dfg.addReturn(null);
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

        public void visitComponent(Component c)
        {
            Identifier currentComponent = _currentComponent;
            _currentComponent = c.variable.identifier;
            _componentFunctionGraphs.Add(_currentComponent, new Dictionary<Identifier,DFG>(new ImportIdentifierComparer()));
            _capturingComponent = true;
            //visitChildren(c);
            c.importList.accept(this);
            c.functionList.accept(this);
            _capturingComponent = false;
            _currentComponent = currentComponent;
        }

        public void visitComponentList(ComponentList cl)
        {
            bool found = false;

            _program = cl;

            foreach (Component c in cl)
            {
                if (c.variable.identifier.name.Equals("app"))
                {
                    //initialise(c, null, null, null, null, null, null);
                    c.accept(this);
                    
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                throw new ParseException(0, 0, $"No entry component found - a component named 'app' must exist", ExceptionLevel.ERROR);
            }

            //if (_isParent)
            //{
                
            //}
        }

        public void visitFunctionList(FunctionList fl)
        {
            visitChildren(fl);
        }

        public void visitImport(Import i)
        {
            if (!_componentFunctionGraphs.ContainsKey(i.identifier))
            {
                foreach (Component c in _program)
                {
                    if (c.variable.identifier == i.identifier)
                    {
                        c.accept(this);
                    }
                }
            }
        }

        public void visitImportList(ImportList il)
        {
            visitChildren(il);
        }
    }
}
