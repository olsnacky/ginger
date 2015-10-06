using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GingerParser;
using System.IO;
using GingerCFG;
using System.Collections.Generic;
using GingerUtil;
using GingerCFG;

namespace GingerCFGTest
{
    public class TestVisitor : CFGVisitor
    {
        public List<Node> visitedNodes;

        public TestVisitor(CFGEntry cfg)
        {
            this.visitedNodes = new List<Node>();
            cfg.accept(this);
        }

        public void visitBasicBlock(CFGBasicBlock bb)
        {
            visitCollection(bb);
        }

        public void visitExit(CFGExit e)
        {
            add(e);
        }

        public void visitEntry(CFGEntry s)
        {
            visitCollection(s);
        }

        private void visitCollection(NodeCollection nc)
        {
            add(nc);
            visitChildren(nc);
        }

        private void visitChildren(NodeCollection nc)
        {
            foreach (Node n in nc)
            {
                n.accept(this);
            }
        }

        private void add(Node n)
        {
            visitedNodes.Add(n);
        }
    }

    [TestClass]
    public class GingerCFGTest
    {
        [TestMethod]
        public void NestedIfWithStatements()
        {
            const int COUNT = 7;
            Parser parser = new Parser(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\NestedIfWithStatements.gngr"));
            parser.parse();
            ASTVisitor astv = new ASTVisitor(parser.ast);
            TestVisitor tv = new TestVisitor(astv.cfg);

            // create new instance to fool cmpiler
            CFGBasicBlock b1 = new CFGBasicBlock();
            CFGBasicBlock b2 = new CFGBasicBlock();
            CFGBasicBlock b3 = new CFGBasicBlock();
            CFGBasicBlock b4 = new CFGBasicBlock();
            CFGBasicBlock b5 = new CFGBasicBlock();

            for (int i = 0; i < COUNT; i++)
            {
                switch (i)
                {
                    case 0:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(CFGEntry), $"{i}/1");
                        break;
                    case 1:
                        b1 = (CFGBasicBlock)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(b1, typeof(CFGBasicBlock), $"{i}/1");
                        break;
                    case 2:
                        b2 = (CFGBasicBlock)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(b2, typeof(CFGBasicBlock), $"{i}/1");
                        Assert.AreEqual(b1, b2.parents[0], $"{i}/2");
                        break;
                    case 3:
                        b3 = (CFGBasicBlock)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(b3, typeof(CFGBasicBlock), $"{i}/1");
                        Assert.AreEqual(b2, b3.parents[0], $"{i}/2");
                        break;
                    case 4:
                        b4 = (CFGBasicBlock)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(b4, typeof(CFGBasicBlock), $"{i}/1");
                        Assert.AreEqual(b3, b4.parents[0], $"{i}/2");
                        Assert.AreEqual(b2, b4.parents[1], $"{i}/3");
                        break;
                    case 5:
                        b5 = (CFGBasicBlock)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(b5, typeof(CFGBasicBlock), $"{i}/1");
                        Assert.AreEqual(b4, b5.parents[0], $"{i}/2");
                        Assert.AreEqual(b1, b5.parents[1], $"{i}/3");
                        break;
                    case 6:
                        CFGExit exit = (CFGExit)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(exit, typeof(CFGExit), $"{i}/1");
                        Assert.AreEqual(b5, exit.parents[0], $"{i}/2");
                        break;
                }
            }
        }
    }
}
