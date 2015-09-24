using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GingerParser;
using System.IO;
using System.Collections.Generic;

namespace GingerParserTest
{
    public class TestVisitor : NodeVisitor
    {
        public List<Node> visitedNodes;

        public TestVisitor(StatementList sl)
        {
            visitedNodes = new List<Node>();
            sl.accept(this);
        }

        public void visitAssign(Assign a)
        {
            visitCollection(a);
        }

        public void visitBinaryOperation(BinaryOperation bo)
        {
            visitCollection(bo);
        }

        public void visitBoolean(GingerParser.Boolean b)
        {
            add(b);
        }

        public void visitBranch(Branch b)
        {
            visitCollection(b);
        }

        public void visitCompare(Compare c)
        {
            visitCollection(c);
        }

        public void visitDeclaration(Declaration d)
        {
            visitCollection(d);
        }

        public void visitIdentifier(Identifier i)
        {
            add(i);
        }

        public void visitInteger(Integer i)
        {
            add(i);
        }

        public void visitLiteral(Literal l)
        {
            add(l);
        }

        public void visitStatementList(StatementList sl)
        {
            visitCollection(sl);
        }

        public void visitWhile(While w)
        {
            visitCollection(w);
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
    public class ParserTest
    {
        [TestMethod]
        public void Simple()
        {
            const int COUNT = 31;
            List<Node> nodes = new List<Node>();
            Parser parser = new Parser(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\parser.gngr"));
            parser.parse();
            TestVisitor tv = new TestVisitor(parser.ast);

            for (int i = 0; i < COUNT; i++)
            {
                switch (i)
                {
                    case 0:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
                        break;
                    case 1:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Declaration), $"{i}");
                        break;
                    case 2:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Integer), $"{i}");
                        break;
                    case 3:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 4:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Assign), $"{i}");
                        break;
                    case 5:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 6:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal), $"{i}");
                        break;
                    case 7:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Declaration), $"{i}");
                        break;
                    case 8:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(GingerParser.Boolean), $"{i}");
                        break;
                    case 9:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 10:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Branch), $"{i}");
                        break;
                    case 11:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Compare), $"{i}");
                        break;
                    case 12:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 13:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal), $"{i}");
                        break;
                    case 14:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
                        break;
                    case 15:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(While), $"{i}");
                        break;
                    case 16:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Compare), $"{i}");
                        break;
                    case 17:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 18:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal), $"{i}");
                        break;
                    case 19:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
                        break;
                    case 20:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Assign), $"{i}");
                        break;
                    case 21:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 22:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(BinaryOperation), $"{i}");
                        break;
                    case 23:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 24:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal), $"{i}");
                        break;
                    case 25:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Declaration), $"{i}");
                        break;
                    case 26:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Integer), $"{i}");
                        break;
                    case 27:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 28:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Assign), $"{i}");
                        break;
                    case 29:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 30:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            }
        }

        [TestMethod]
        public void Precedence()
        {
            const int COUNT = 13;
            List<Node> nodes = new List<Node>();
            Parser parser = new Parser(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\precedence.gngr"));
            parser.parse();
            TestVisitor tv = new TestVisitor(parser.ast);

            for (int i = 0; i < COUNT; i++)
            {
                switch (i)
                {
                    case 0:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
                        break;
                    case 1:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Branch), $"{i}");
                        break;
                    case 2:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Compare), $"{i}");
                        break;
                    case 3:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(BinaryOperation), $"{i}");
                        break;
                    case 4:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 5:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal), $"{i}");
                        break;
                    case 6:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(BinaryOperation), $"{i}");
                        break;
                    case 7:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 8:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal), $"{i}");
                        break;
                    case 9:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
                        break;
                    case 10:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Assign), $"{i}");
                        break;
                    case 11:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 12:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void NotOpenStatement()
        {
            Parser parser = new Parser(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\NotOpenStatement.gngr"));
            parser.parse();
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void NoIdentifierAfterType()
        {
            Parser parser = new Parser(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\IdentifierDoesNotFollowType.gngr"));
            parser.parse();
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void NoExpressionAfterAssignment()
        {
            Parser parser = new Parser(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\ExpressionDoesNotFollowAssignment.gngr"));
            parser.parse();
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void NotAStatement()
        {
            Parser parser = new Parser(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\ExpressionInsteadOfStatement.gngr"));
            parser.parse();
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void ExpressionNotClosed()
        {
            Parser parser = new Parser(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\ExpressionNotClosed.gngr"));
            parser.parse();
        }

        [TestMethod]
        [ExpectedException(typeof(ParseException))]
        public void ExpressionNotFound()
        {
            Parser parser = new Parser(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\ExpressionNotFound.gngr"));
            parser.parse();
        }
    }
}
