using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GingerParser;
using System.IO;
using System.Collections.Generic;
using GingerUtil;

namespace GingerParserTest
{
    public class TestVisitor : SLVisitor
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

        public void visitLiteral<T>(Literal<T> l)
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
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal<Integer>), $"{i}");
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
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal<Integer>), $"{i}");
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
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal<Integer>), $"{i}");
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
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal<Integer>), $"{i}");
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
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal<Integer>), $"{i}");
                        break;
                    case 6:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(BinaryOperation), $"{i}");
                        break;
                    case 7:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 8:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal<Integer>), $"{i}");
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
        public void SimpleScope()
        {
            const int COUNT = 29;
            Parser parser = new Parser(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\scope.gngr"));
            parser.parse();
            ScopeVisitor sv = new ScopeVisitor(parser.ast);
            TestVisitor tv = new TestVisitor(parser.ast);

            // empty declaration to fool compiler
            Declaration george = new Declaration(new Integer(), new Identifier("geoerge1Test"));
            Declaration kim = new Declaration(new Integer(), new Identifier("kim1Test"));
            Declaration subKim = new Declaration(new Integer(), new Identifier("kim2Test"));

            for (int i = 0; i < COUNT; i++)
            {
                switch (i)
                {
                    case 1:
                        george = tv.visitedNodes[i] as Declaration;
                        break;
                    case 3:
                        Identifier ident = tv.visitedNodes[i] as Identifier;
                        Assert.AreEqual(george, ident.declaration, $"{i}");
                        break;
                    case 4:
                        kim = tv.visitedNodes[i] as Declaration;
                        break;
                    case 6:
                        Identifier ident2 = tv.visitedNodes[i] as Identifier;
                        Assert.AreEqual(kim, ident2.declaration, $"{i}");
                        break;
                    case 8:
                        Identifier ident3 = tv.visitedNodes[i] as Identifier;
                        Assert.AreEqual(george, ident3.declaration, $"{i}");
                        break;
                    case 11:
                        Identifier ident4 = tv.visitedNodes[i] as Identifier;
                        Assert.AreEqual(kim, ident4.declaration, $"{i}");
                        break;
                    case 13:
                        Identifier ident5 = tv.visitedNodes[i] as Identifier;
                        Assert.AreEqual(george, ident5.declaration, $"{i}");
                        break;
                    case 17:
                        Identifier ident6 = tv.visitedNodes[i] as Identifier;
                        Assert.AreEqual(george, ident6.declaration, $"{i}");
                        break;
                    case 18:
                        Identifier ident7 = tv.visitedNodes[i] as Identifier;
                        Assert.AreEqual(kim, ident7.declaration, $"{i}");
                        break;
                    case 20:
                        subKim = tv.visitedNodes[i] as Declaration;
                        break;
                    case 22:
                        Identifier ident8 = tv.visitedNodes[i] as Identifier;
                        Assert.AreEqual(subKim, ident8.declaration, $"{i}");
                        break;
                    case 24:
                        Identifier ident9 = tv.visitedNodes[i] as Identifier;
                        Assert.AreEqual(subKim, ident9.declaration, $"{i}");
                        break;
                    case 27:
                        Identifier ident10 = tv.visitedNodes[i] as Identifier;
                        Assert.AreEqual(george, ident10.declaration, $"{i}");
                        break;
                    case 28:
                        Identifier ident11 = tv.visitedNodes[i] as Identifier;
                        Assert.AreEqual(subKim, ident11.declaration, $"{i}");
                        break;
                    default:
                        break;
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ScopeException))]
        public void DoubleDeclaration()
        {
            Parser parser = new Parser(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\DoubleDeclaration.gngr"));
            parser.parse();
            ScopeVisitor sv = new ScopeVisitor(parser.ast);
        }

        [TestMethod]
        [ExpectedException(typeof(ScopeException))]
        public void IdentifierNotInScope()
        {
            Scope scope = new Scope();
            Identifier ident = new Identifier("david");
            scope.find(ident);
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

        [TestMethod]
        public void TypeChecking()
        {
            Parser parser = new Parser(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\type.gngr"));
            parser.parse();
            ScopeVisitor sv = new ScopeVisitor(parser.ast);
        }

        [TestMethod]
        [ExpectedException(typeof(TypeException))]
        public void AssignBoolVarToIntVar()
        {
            Parser parser = new Parser(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\AssignBoolVarToIntVar.gngr"));
            parser.parse();
            ScopeVisitor sv = new ScopeVisitor(parser.ast);
        }
        
        [TestMethod]
        [ExpectedException(typeof(TypeException))]
        public void AssignIntLiteralToBoolVar()
        {
            Parser parser = new Parser(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\AssignIntLiteralToBoolVar.gngr"));
            parser.parse();
            ScopeVisitor sv = new ScopeVisitor(parser.ast);
        }
    }
}
