using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GingerParser;
using System.IO;
using GingerCFG;
using System.Collections.Generic;
using GingerUtil;
using System.Linq;

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
                // check to see if we've visited this node, else we might get in an infinite loop
                if (!visitedNodes.Any(node => node == n)) {
                    n.accept(this);
                }
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

            // create new instance to fool compiler
            CFGEntry entry = new CFGEntry();
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
                        entry = (CFGEntry)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(entry, typeof(CFGEntry), $"{i}/1");
                        break;
                    case 1:
                        b1 = (CFGBasicBlock)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(b1, typeof(CFGBasicBlock), $"{i}/1");
                        Assert.AreEqual(entry, b1.parents[0], $"{i}/1");
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

        [TestMethod]
        public void NestedIfNoStatements()
        {
            const int COUNT = 5;
            Parser parser = new Parser(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\NestedIfNoStatements.gngr"));
            parser.parse();
            ASTVisitor astv = new ASTVisitor(parser.ast);
            TestVisitor tv = new TestVisitor(astv.cfg);

            // create new instance to fool cmpiler
            CFGEntry entry = new CFGEntry();
            CFGBasicBlock b1 = new CFGBasicBlock();
            CFGBasicBlock b2 = new CFGBasicBlock();
            CFGBasicBlock b3 = new CFGBasicBlock();

            for (int i = 0; i < COUNT; i++)
            {
                switch (i)
                {
                    case 0:
                        entry = (CFGEntry)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(entry, typeof(CFGEntry), $"{i}/1");
                        break;
                    case 1:
                        b1 = (CFGBasicBlock)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(b1, typeof(CFGBasicBlock), $"{i}/1");
                        Assert.AreEqual(entry, b1.parents[0], $"{i}/1");
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
                        CFGExit exit = (CFGExit)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(exit, typeof(CFGExit), $"{i}/1");
                        Assert.AreEqual(b3, exit.parents[0], $"{i}/2");
                        Assert.AreEqual(b2, exit.parents[1], $"{i}/3");
                        Assert.AreEqual(b1, exit.parents[2], $"{i}/4");
                        break;
                }
            }
        }

        [TestMethod]
        public void BlockSurroundedIf()
        {
            const int COUNT = 7;
            Parser parser = new Parser(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\BlockSurroundedIf.gngr"));
            parser.parse();
            ASTVisitor astv = new ASTVisitor(parser.ast);
            TestVisitor tv = new TestVisitor(astv.cfg);

            // create new instance to fool compiler
            CFGEntry entry = new CFGEntry();
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
                        entry = (CFGEntry)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(entry, typeof(CFGEntry), $"{i}/1");
                        break;
                    case 1:
                        b1 = (CFGBasicBlock)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(b1, typeof(CFGBasicBlock), $"{i}/1");
                        Assert.AreEqual(entry, b1.parents[0], $"{i}/1");
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

        [TestMethod]
        public void CFG()
        {
            const int COUNT = 6;
            Parser parser = new Parser(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\cfg.gngr"));
            parser.parse();
            ASTVisitor astv = new ASTVisitor(parser.ast);
            TestVisitor tv = new TestVisitor(astv.cfg);

            // create new instance to fool compiler
            CFGEntry entry = new CFGEntry();
            CFGBasicBlock b1 = new CFGBasicBlock();
            CFGBasicBlock b2 = new CFGBasicBlock();
            CFGBasicBlock b3 = new CFGBasicBlock();
            CFGBasicBlock b4 = new CFGBasicBlock();

            for (int i = 0; i < COUNT; i++)
            {
                switch (i)
                {
                    case 0:
                        entry = (CFGEntry)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(entry, typeof(CFGEntry), $"{i}/1");
                        break;
                    case 1:
                        b1 = (CFGBasicBlock)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(b1, typeof(CFGBasicBlock), $"{i}/1");
                        Assert.AreEqual(entry, b1.parents[0], $"{i}/1");
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
                        Assert.AreEqual(b1, b3.parents[1], $"{i}/3");
                        break;
                    case 4:
                        b4 = (CFGBasicBlock)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(b4, typeof(CFGBasicBlock), $"{i}/1");
                        Assert.AreEqual(b3, b4.parents[0], $"{i}/2");
                        break;
                    case 6:
                        CFGExit exit = (CFGExit)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(exit, typeof(CFGExit), $"{i}/1");
                        Assert.AreEqual(b4, exit.parents[0], $"{i}/2");
                        Assert.AreEqual(b3, exit.parents[1], $"{i}/2");
                        break;
                }
            }
        }

        [TestMethod]
        public void While()
        {
            const int COUNT = 8;
            Parser parser = new Parser(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\while.gngr"));
            parser.parse();
            ASTVisitor astv = new ASTVisitor(parser.ast);
            TestVisitor tv = new TestVisitor(astv.cfg);

            // create new instance to fool compiler
            CFGEntry entry = new CFGEntry();
            CFGBasicBlock b1 = new CFGBasicBlock();
            CFGBasicBlock b2 = new CFGBasicBlock();
            CFGBasicBlock b3 = new CFGBasicBlock();
            CFGBasicBlock b4 = new CFGBasicBlock();
            CFGBasicBlock b5 = new CFGBasicBlock();
            CFGBasicBlock b6 = new CFGBasicBlock();

            for (int i = 0; i < COUNT; i++)
            {
                switch (i)
                {
                    case 0:
                        entry = (CFGEntry)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(entry, typeof(CFGEntry), $"{i}/1");
                        break;
                    case 1:
                        b1 = (CFGBasicBlock)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(b1, typeof(CFGBasicBlock), $"{i}/1");
                        Assert.AreEqual(entry, b1.parents[0], $"{i}/1");
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
                        Assert.AreEqual(b4, b3.parents[1], $"{i}/3");
                        break;
                    case 5:
                        b5 = (CFGBasicBlock)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(b5, typeof(CFGBasicBlock), $"{i}/1");
                        Assert.AreEqual(b4, b5.parents[0], $"{i}/2");
                        Assert.AreEqual(b3, b5.parents[1], $"{i}/3");
                        break;
                    case 6:
                        b6 = (CFGBasicBlock)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(b6, typeof(CFGBasicBlock), $"{i}/1");
                        Assert.AreEqual(b5, b6.parents[0], $"{i}/2");
                        Assert.AreEqual(b1, b6.parents[1], $"{i}/3");
                        break;
                    case 7:
                        CFGExit exit = (CFGExit)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(exit, typeof(CFGExit), $"{i}/1");
                        Assert.AreEqual(b6, exit.parents[0], $"{i}/2");
                        break;
                }
            }
        }

        [TestMethod]
        public void PDG()
        {
            const int COUNT = 12;
            Parser parser = new Parser(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\pdg.gngr"));
            parser.parse();
            ASTVisitor astv = new ASTVisitor(parser.ast, false);
            TestVisitor tv = new TestVisitor(astv.cfg);

            // create new instance to fool compiler
            CFGEntry entry = new CFGEntry();
            CFGBasicBlock b1 = new CFGBasicBlock();
            CFGBasicBlock b2 = new CFGBasicBlock();
            CFGBasicBlock b3 = new CFGBasicBlock();
            CFGBasicBlock b4 = new CFGBasicBlock();
            CFGBasicBlock b5 = new CFGBasicBlock();
            CFGBasicBlock b6 = new CFGBasicBlock();
            CFGBasicBlock b7 = new CFGBasicBlock();
            CFGBasicBlock b8 = new CFGBasicBlock();
            CFGBasicBlock b9 = new CFGBasicBlock();
            CFGBasicBlock b10 = new CFGBasicBlock();

            for (int i = 0; i < COUNT; i++)
            {
                switch (i)
                {
                    case 0:
                        entry = (CFGEntry)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(entry, typeof(CFGEntry), $"{i}/1");
                        break;
                    case 1:
                        b1 = (CFGBasicBlock)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(b1, typeof(CFGBasicBlock), $"{i}/1");
                        Assert.AreEqual(entry, b1.parents[0], $"{i}/1");
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
                        break;
                    case 5:
                        b5 = (CFGBasicBlock)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(b5, typeof(CFGBasicBlock), $"{i}/1");
                        Assert.AreEqual(b4, b5.parents[0], $"{i}/2");
                        break;
                    case 6:
                        b6 = (CFGBasicBlock)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(b6, typeof(CFGBasicBlock), $"{i}/1");
                        Assert.AreEqual(b5, b6.parents[0], $"{i}/2");
                        Assert.AreEqual(b6, b5.parents[1], $"{i}/3");
                        break;
                    case 7:
                        b7 = (CFGBasicBlock)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(b7, typeof(CFGBasicBlock), $"{i}/1");
                        Assert.AreEqual(b6, b7.parents[0], $"{i}/2");
                        Assert.AreEqual(b5, b5.parents[1], $"{i}/3");
                        break;
                    case 8:
                        b8 = (CFGBasicBlock)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(b8, typeof(CFGBasicBlock), $"{i}/1");
                        Assert.AreEqual(b7, b8.parents[0], $"{i}/2");
                        break;
                    case 9:
                        b9 = (CFGBasicBlock)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(b9, typeof(CFGBasicBlock), $"{i}/1");
                        Assert.AreEqual(b8, b9.parents[0], $"{i}/2");
                        break;
                    case 10:
                        b10 = (CFGBasicBlock)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(b10, typeof(CFGBasicBlock), $"{i}/1");
                        Assert.AreEqual(b9, b10.parents[0], $"{i}/2");
                        Assert.AreEqual(b8, b10.parents[1], $"{i}/3");
                        Assert.AreEqual(b3, b10.parents[2], $"{i}/4");
                        break;
                    case 11:
                        CFGExit exit = (CFGExit)tv.visitedNodes[i];
                        Assert.IsInstanceOfType(exit, typeof(CFGExit), $"{i}/1");
                        Assert.AreEqual(b10, exit.parents[0], $"{i}/2");
                        break;
                }
            }
        }
    }
}
