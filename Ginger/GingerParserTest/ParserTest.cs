using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GingerParser;
using System.IO;
using System.Collections.Generic;
using GingerUtil;
using GingerParser.Scope;
using GingerParser.CFG;
using GingerParser.DDG;

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

        public void visitReturn(Return r)
        {
            visitCollection(r);
        }

        public void visitBinaryOperation(BinaryOperation bo)
        {
            visitCollection(bo);
        }

        //public void visitBoolean(GingerParser.Boolean b)
        //{
        //    add(b);
        //}

        //public void visitBranch(If b)
        //{
        //    visitCollection(b);
        //}

        public void visitVariable(Variable d)
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

        public void visitStatementList(StatementList sl)
        {
            visitCollection(sl);
        }

        //public void visitWhile(While w)
        //{
        //    visitCollection(w);
        //}

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

        public void visitLiteral<T>(Literal<T> l) where T : Typeable
        {
            add(l);
        }

        public void visitFunction(Function f)
        {
            visitCollection(f);
        }

        public void visitVariableList(VarList vl)
        {
            visitCollection(vl);
        }

        public void visitExpressionList(ExpressionList el)
        {
            visitCollection(el);
        }

        public void visitInvocation(Invocation i)
        {
            visitCollection(i);
        }

        public void visitSource(Source s)
        {
            add(s);
        }

        public void visitSink(Sink s)
        {
            visitCollection(s);
        }

        //public void visitVoid(GingerParser.Void v)
        //{
        //    add(v);
        //}

        //public void visitComponent(Component c)
        //{
        //    visitCollection(c);
        //}

        //public void visitContract(Contract c)
        //{
        //    add(c);
        //}

        //public void visitImplementation(Implementation i)
        //{
        //    add(i);
        //}
    }

    [TestClass]
    public class ParserTest
    {
        //[TestMethod]
        //public void IsInUnitTest()
        //{
        //    Assert.IsTrue(UnitTestDetector.IsInUnitTest,
        //        "Should detect that we are running inside a unit test."); // lol
        //}

        //[TestMethod]
        //public void SystemCode()
        //{
        //    const int COUNT = 32;
        //    List<Node> nodes = new List<Node>();
        //    //Parser parser = new Parser("");

        //    StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\AST\VariableDeclaration.gngr"));
        //    Parser parser = new Parser(reader.ReadToEnd());

        //    parser.parse();
        //    TestVisitor tv = new TestVisitor(parser.ast);
        //    Assert.IsTrue(COUNT == tv.visitedNodes.Count, $"Expected {COUNT} nodes, found {tv.visitedNodes.Count}");

        //    for (int i = 0; i < COUNT; i++)
        //    {
        //        switch (i)
        //        {
        //            case 0:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
        //                break;
        //            case 1:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
        //                break;
        //            case 2:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 3:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
        //                break;
        //            case 4:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 5:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Assign), $"{i}");
        //                break;
        //            case 6:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 7:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal<Integer>), $"{i}");
        //                break;
        //            case 8:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Assign), $"{i}");
        //                break;
        //            case 9:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 10:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal<Integer>), $"{i}");
        //                break;
        //            case 11:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Function), $"{i}");
        //                break;
        //            case 12:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
        //                break;
        //            case 13:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 14:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(VariableList), $"{i}");
        //                break;
        //            case 15:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
        //                break;
        //            case 16:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 17:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
        //                break;
        //            case 18:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Return), $"{i}");
        //                break;
        //            case 19:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 20:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Function), $"{i}");
        //                break;
        //            case 21:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
        //                break;
        //            case 22:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 23:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(VariableList), $"{i}");
        //                break;
        //            case 24:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
        //                break;
        //            case 25:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 26:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
        //                break;
        //            case 27:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 28:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
        //                break;
        //            case 29:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Assign), $"{i}");
        //                break;
        //            case 30:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 31:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            default:
        //                Assert.Fail();
        //                break;
        //        }
        //    }
        //}

        [TestMethod]
        public void VariableDeclaration()
        {
            const int COUNT = 3;
            List<Node> nodes = new List<Node>();
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\AST\VariableDeclaration.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();
            TestVisitor tv = new TestVisitor(parser.ast);
            Assert.IsTrue(COUNT == tv.visitedNodes.Count, $"Expected {COUNT} nodes, found {tv.visitedNodes.Count}");

            for (int i = 0; i < COUNT; i++)
            {
                switch (i)
                {
                    case 0:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
                        break;
                    case 1:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
                        break;
                    case 2:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            }
        }

        [TestMethod]
        public void SourceSink()
        {
            const int COUNT = 15;
            List<Node> nodes = new List<Node>();
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\AST\SourceSink.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();
            TestVisitor tv = new TestVisitor(parser.ast);
            Assert.IsTrue(COUNT == tv.visitedNodes.Count, $"Expected {COUNT} nodes, found {tv.visitedNodes.Count}");

            for (int i = 0; i < COUNT; i++)
            {
                switch (i)
                {
                    case 0:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
                        break;
                    case 1:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
                        break;
                    case 2:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 3:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Assign), $"{i}");
                        break;
                    case 4:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 5:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Source), $"{i}");
                        Assert.IsTrue(((Source)tv.visitedNodes[i]).securityLevel == Ginger.GingerToken.Low, $"{i}");
                        break;
                    case 6:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Sink), $"{i}");
                        Assert.IsTrue(((Sink)tv.visitedNodes[i]).securityLevel == Ginger.GingerToken.High, $"{i}");
                        break;
                    case 7:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 8:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
                        break;
                    case 9:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 10:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Assign), $"{i}");
                        break;
                    case 11:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 12:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Source), $"{i}");
                        Assert.IsTrue(((Source)tv.visitedNodes[i]).securityLevel == Ginger.GingerToken.High, $"{i}");
                        break;
                    case 13:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Sink), $"{i}");
                        Assert.IsTrue(((Sink)tv.visitedNodes[i]).securityLevel == Ginger.GingerToken.Low, $"{i}");
                        break;
                    case 14:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            }
        }

        //[TestMethod]
        //public void ReferenceValues()
        //{
        //    const int COUNT = 25;
        //    List<Node> nodes = new List<Node>();
        //    StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\AST\ReferenceValues.gngr"));
        //    Parser parser = new Parser(reader.ReadToEnd());

        //    parser.parse();
        //    TestVisitor tv = new TestVisitor(parser.ast);
        //    Assert.IsTrue(COUNT == tv.visitedNodes.Count, $"Expected {COUNT} nodes, found {tv.visitedNodes.Count}");

        //    for (int i = 0; i < COUNT; i++)
        //    {
        //        switch (i)
        //        {
        //            case 0:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
        //                break;
        //            case 1:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Function), $"{i}");
        //                break;
        //            case 2:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
        //                break;
        //            case 3:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 4:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(VarList), $"{i}");
        //                break;
        //            case 5:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
        //                Assert.IsTrue(((Variable)tv.visitedNodes[i]).passByReference, $"{i} is not a reference");
        //                break;
        //            case 6:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 7:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
        //                break;
        //            case 8:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 9:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
        //                break;
        //            case 10:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Assign), $"{i}");
        //                break;
        //            case 11:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 12:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 13:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
        //                break;
        //            case 14:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 15:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
        //                break;
        //            case 16:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 17:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Assign), $"{i}");
        //                break;
        //            case 18:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 19:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal<Integer>), $"{i}");
        //                break;
        //            case 20:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Invocation), $"{i}");
        //                break;
        //            case 21:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 22:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(ExpressionList), $"{i}");
        //                break;
        //            case 23:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 24:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            default:
        //                Assert.Fail();
        //                break;
        //        }
        //    }
        //}

        //[TestMethod]
        //public void ExpressionAsStatement()
        //{
        //    const int COUNT = 25;
        //    List<Node> nodes = new List<Node>();
        //    StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\AST\ExpressionAsStatement.gngr"));
        //    Parser parser = new Parser(reader.ReadToEnd());

        //    parser.parse();
        //    TestVisitor tv = new TestVisitor(parser.ast);
        //    Assert.IsTrue(COUNT == tv.visitedNodes.Count, $"Expected {COUNT} nodes, found {tv.visitedNodes.Count}");

        //    for (int i = 0; i < COUNT; i++)
        //    {
        //        switch (i)
        //        {
        //            case 0:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
        //                break;
        //            case 1:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Function), $"{i}");
        //                break;
        //            case 2:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
        //                break;
        //            case 3:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 4:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(VarList), $"{i}");
        //                break;
        //            case 5:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
        //                break;
        //            case 6:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 7:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
        //                break;
        //            case 8:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 9:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
        //                break;
        //            case 10:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Assign), $"{i}");
        //                break;
        //            case 11:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 12:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 13:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
        //                break;
        //            case 14:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 15:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
        //                break;
        //            case 16:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 17:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Assign), $"{i}");
        //                break;
        //            case 18:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 19:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal<Integer>), $"{i}");
        //                break;
        //            case 20:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Invocation), $"{i}");
        //                break;
        //            case 21:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 22:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(ExpressionList), $"{i}");
        //                break;
        //            case 23:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 24:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            default:
        //                Assert.Fail();
        //                break;
        //        }
        //    }
        //}

        [TestMethod]
        public void FunctionFull()
        {
            const int COUNT = 14;
            List<Node> nodes = new List<Node>();
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\AST\FunctionWithParamsAndReturnType.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();
            TestVisitor tv = new TestVisitor(parser.ast);
            Assert.IsTrue(COUNT == tv.visitedNodes.Count, $"Expected {COUNT} nodes, found {tv.visitedNodes.Count}");

            for (int i = 0; i < COUNT; i++)
            {
                switch (i)
                {
                    case 0:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
                        break;
                    case 1:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Function), $"{i}");
                        break;
                    case 2:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
                        break;
                    case 3:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 4:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(VarList), $"{i}");
                        break;
                    case 5:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
                        break;
                    case 6:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 7:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
                        break;
                    case 8:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 9:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
                        break;
                    case 10:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Return), $"{i}");
                        break;
                    case 11:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(BinaryOperation), $"{i}");
                        break;
                    case 12:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 13:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            }
        }

        [TestMethod]
        public void FunctionVoidNoParams()
        {
            const int COUNT = 11;
            List<Node> nodes = new List<Node>();
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\AST\FunctionNoParamsVoid.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();
            TestVisitor tv = new TestVisitor(parser.ast);
            Assert.IsTrue(COUNT == tv.visitedNodes.Count, $"Expected {COUNT} nodes, found {tv.visitedNodes.Count}");

            for (int i = 0; i < COUNT; i++)
            {
                switch (i)
                {
                    case 0:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
                        break;
                    case 1:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Function), $"{i}");
                        break;
                    case 2:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
                        break;
                    case 3:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 4:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(VarList), $"{i}");
                        break;
                    case 5:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
                        break;
                    case 6:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
                        break;
                    case 7:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 8:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Assign), $"{i}");
                        break;
                    case 9:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 10:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal<Integer>), $"{i}");
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            }
        }

        //[TestMethod]
        //public void Component()
        //{
        //    const int COUNT = 9;
        //    List<Node> nodes = new List<Node>();
        //    StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\AST\ComponentContract.gngr"));
        //    Parser parser = new Parser(reader.ReadToEnd());

        //    parser.parse();
        //    TestVisitor tv = new TestVisitor(parser.ast);
        //    Assert.IsTrue(COUNT == tv.visitedNodes.Count, $"Expected {COUNT} nodes, found {tv.visitedNodes.Count}");

        //    for (int i = 0; i < COUNT; i++)
        //    {
        //        switch (i)
        //        {
        //            case 0:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
        //                break;
        //            case 1:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Component), $"{i}");
        //                break;
        //            case 2:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 3:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Contract), $"{i}");
        //                break;
        //            case 4:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
        //                break;
        //            case 5:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Component), $"{i}");
        //                break;
        //            case 6:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 7:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Implementation), $"{i}");
        //                break;
        //            case 8:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
        //                break;
        //            default:
        //                Assert.Fail();
        //                break;
        //        }
        //    }
        //}

        [TestMethod]
        public void Assignment()
        {
            const int COUNT = 40;
            List<Node> nodes = new List<Node>();
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\AST\Assignment.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();
            TestVisitor tv = new TestVisitor(parser.ast);
            Assert.IsTrue(COUNT == tv.visitedNodes.Count, $"Expected {COUNT} nodes, found {tv.visitedNodes.Count}");

            for (int i = 0; i < COUNT; i++)
            {
                switch (i)
                {
                    case 0:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
                        break;
                    case 1:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
                        break;
                    case 2:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 3:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
                        break;
                    case 4:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 5:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Function), $"{i}");
                        break;
                    case 6:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
                        break;
                    case 7:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 8:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(VarList), $"{i}");
                        break;
                    case 9:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
                        break;
                    case 10:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 11:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
                        break;
                    case 12:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Return), $"{i}");
                        break;
                    case 13:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 14:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Assign), $"{i}");
                        break;
                    case 15:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 16:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal<Integer>), $"{i}");
                        break;
                    case 17:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Assign), $"{i}");
                        break;
                    case 18:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 19:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
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
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Assign), $"{i}");
                        break;
                    case 26:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 27:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(BinaryOperation), $"{i}");
                        break;
                    case 28:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(BinaryOperation), $"{i}");
                        break;
                    case 29:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(BinaryOperation), $"{i}");
                        break;
                    case 30:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 31:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal<Integer>), $"{i}");
                        break;
                    case 32:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 33:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal<Integer>), $"{i}");
                        break;
                    case 34:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Assign), $"{i}");
                        break;
                    case 35:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 36:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Invocation), $"{i}");
                        break;
                    case 37:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    case 38:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(ExpressionList), $"{i}");
                        break;
                    case 39:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            }
        }

        //[TestMethod]
        //public void Simple()
        //{
        //    const int COUNT = 48;
        //    List<Node> nodes = new List<Node>();
        //    StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\AST\parser.gngr"));
        //    Parser parser = new Parser(reader.ReadToEnd());

        //    parser.parse();
        //    TestVisitor tv = new TestVisitor(parser.ast);
        //    Assert.IsTrue(COUNT == tv.visitedNodes.Count, $"Expected {COUNT} nodes, found {tv.visitedNodes.Count}");

        //    for (int i = 0; i < COUNT; i++)
        //    {
        //        switch (i)
        //        {
        //            case 0:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
        //                break;
        //            // var george
        //            case 1:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
        //                break;
        //            case 2:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            // var bob
        //            case 3:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
        //                break;
        //            case 4:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            // var alice
        //            case 5:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
        //                break;
        //            case 6:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            // var foo
        //            case 7:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
        //                break;
        //            case 8:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            // george := 20
        //            case 9:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Assign), $"{i}");
        //                break;
        //            case 10:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 11:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal<Integer>), $"{i}");
        //                break;
        //            // def baz
        //            case 12:
        //                Assert.IsNotInstanceOfType(tv.visitedNodes[i], typeof(Function), $"{i}");
        //                break;
        //            case 13:
        //                Assert.IsNotInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 14:
        //                Assert.IsNotInstanceOfType(tv.visitedNodes[i], typeof(VariableList), $"{i}");
        //                break;
        //            case 15:
        //                Assert.IsNotInstanceOfType(tv.visitedNodes[i], typeof(VariableList), $"{i}");
        //                break;

        //            case 2:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Integer), $"{i}");
        //                break;
        //            case 3:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
                    
        //            case 7:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
        //                break;
        //            case 8:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(GingerParser.Boolean), $"{i}");
        //                break;
        //            case 9:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 10:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Assign), $"{i}");
        //                break;
        //            case 11:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 12:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal<GingerParser.Boolean>), $"{i}");
        //                break;
        //            case 13:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(If), $"{i}");
        //                break;
        //            //case 14:
        //            //    Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(InequalityOperation), $"{i}");
        //            //    break;
        //            case 15:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 16:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal<Integer>), $"{i}");
        //                break;
        //            case 17:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
        //                break;
        //            case 18:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(While), $"{i}");
        //                break;
        //            //case 19:
        //            //    Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(InequalityOperation), $"{i}");
        //            //    break;
        //            case 20:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal<Integer>), $"{i}");
        //                break;
        //            case 21:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 22:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
        //                break;
        //            case 23:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Assign), $"{i}");
        //                break;
        //            case 24:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 25:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(BinaryOperation), $"{i}");
        //                break;
        //            case 26:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 27:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal<Integer>), $"{i}");
        //                break;
        //            case 28:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(If), $"{i}");
        //                break;
        //            case 29:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 30:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(StatementList), $"{i}");
        //                break;
        //            case 31:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Assign), $"{i}");
        //                break;
        //            case 32:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 33:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(BinaryOperation), $"{i}");
        //                break;
        //            case 34:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 35:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal<Integer>), $"{i}");
        //                break;
        //            case 36:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
        //                break;
        //            case 37:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Integer), $"{i}");
        //                break;
        //            case 38:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 39:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Assign), $"{i}");
        //                break;
        //            case 40:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 41:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 42:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Variable), $"{i}");
        //                break;
        //            case 43:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(GingerParser.Boolean), $"{i}");
        //                break;
        //            case 44:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 45:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Assign), $"{i}");
        //                break;
        //            case 46:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Identifier), $"{i}");
        //                break;
        //            case 47:
        //                Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(Literal<GingerParser.Boolean>), $"{i}");
        //                break;
        //            default:
        //                Assert.Fail();
        //                break;
        //        }
        //    }
        //}

        //[TestMethod]
        public void SimpleScope()
        {
            const int COUNT = 33;
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Scope\scope.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();
            ScopeVisitor sv = new ScopeVisitor(parser.ast);
            TestVisitor tv = new TestVisitor(parser.ast);

            // empty declaration to fool compiler
            Variable george = new Variable( new Identifier(0, 0, "geoerge1Test"));
            Variable kim = new Variable(new Identifier(0, 0, "kim1Test"));
            Variable subKim = new Variable(new Identifier(0, 0, "kim2Test"));

            for (int i = 0; i < COUNT; i++)
            {
                switch (i)
                {
                    case 1:
                        george = tv.visitedNodes[i] as Variable;
                        break;
                    case 3:
                        Identifier ident = tv.visitedNodes[i] as Identifier;
                        Assert.AreEqual(george, ident.declaration, $"{i}");
                        break;
                    case 4:
                        kim = tv.visitedNodes[i] as Variable;
                        break;
                    case 6:
                        Identifier ident2 = tv.visitedNodes[i] as Identifier;
                        Assert.AreEqual(kim, ident2.declaration, $"{i}");
                        break;
                    case 11:
                        Identifier ident3 = tv.visitedNodes[i] as Identifier;
                        Assert.AreEqual(george, ident3.declaration, $"{i}");
                        break;
                    case 14:
                        Identifier ident4 = tv.visitedNodes[i] as Identifier;
                        Assert.AreEqual(kim, ident4.declaration, $"{i}");
                        break;
                    case 16:
                        Identifier ident5 = tv.visitedNodes[i] as Identifier;
                        Assert.AreEqual(george, ident5.declaration, $"{i}");
                        break;
                    case 24:
                        subKim = tv.visitedNodes[i] as Variable;
                        break;
                    case 28:
                        Identifier ident8 = tv.visitedNodes[i] as Identifier;
                        Assert.AreEqual(subKim, ident8.declaration, $"{i}");
                        break;
                    case 31:
                        Identifier ident10 = tv.visitedNodes[i] as Identifier;
                        Assert.AreEqual(george, ident10.declaration, $"{i}");
                        break;
                    case 32:
                        Identifier ident11 = tv.visitedNodes[i] as Identifier;
                        Assert.AreEqual(subKim, ident11.declaration, $"{i}");
                        break;
                    default:
                        break;
                }
            }
        }

        //[TestMethod]
        public void DoubleDeclaration()
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Scope\DoubleDeclaration.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();
            ScopeVisitor sv = new ScopeVisitor(parser.ast);

            Assert.IsInstanceOfType(sv.errors[0], typeof(ScopeException));
        }

        //[TestMethod]
        [ExpectedException(typeof(ScopeException))]
        public void IdentifierNotInScope()
        {
            Scope scope = new Scope();
            Identifier ident = new Identifier(0, 0, "david");
            scope.find(ident);
        }

        [TestMethod]
        public void NotOpenStatement()
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\AST\NotOpenStatement.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();
            Assert.IsInstanceOfType(parser.errors[0], typeof(ParseException));
        }

        [TestMethod]
        public void NoIdentifierAfterType()
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\AST\IdentifierDoesNotFollowType.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();
            Assert.IsInstanceOfType(parser.errors[0], typeof(ParseException));
        }

        [TestMethod]
        public void NoCloseToStatementListAtEnd()
        {
            Parser parser = new Parser("int x x = 1 int y y = 0 if y < x {x = x + 1 ");

            parser.parse();
            Assert.IsInstanceOfType(parser.errors[0], typeof(ParseException));
        }

        //[TestMethod]
        //public void UseOfIdentifierInFirstStatement()
        //{
        //    Parser parser = new Parser("var x var y y = x");
        //    parser.parse();

        //    CFGVisitor cfgv = new CFGVisitor(parser.ast);
        //    ScopeVisitor sv = new ScopeVisitor(parser.ast);
        //    DDGVisitor ddgv = new DDGVisitor(parser.ast);

        //    Assert.IsInstanceOfType(ddgv.errors[0], typeof(AccessException));
        //}

        [TestMethod]
        public void NoExpressionAfterAssignment()
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\AST\ExpressionDoesNotFollowAssignment.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();
            Assert.IsInstanceOfType(parser.errors[0], typeof(ParseException));
        }

        [TestMethod]
        public void NotAStatement()
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\AST\ExpressionInsteadOfStatement.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();
            Assert.IsInstanceOfType(parser.errors[0], typeof(ParseException));
        }

        [TestMethod]
        public void ExpressionNotClosed()
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\AST\ExpressionNotClosed.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();

            Assert.IsInstanceOfType(parser.errors[0], typeof(ParseException));
        }

        [TestMethod]
        public void ExpressionNotFound()
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\AST\ExpressionNotFound.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();

            Assert.IsInstanceOfType(parser.errors[0], typeof(ParseException));
        }

        [TestMethod]
        public void TypeChecking()
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Scope\type.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();
            ScopeVisitor sv = new ScopeVisitor(parser.ast);
        }

        //[TestMethod]
        //public void AssignBoolVarToIntVar()
        //{
        //    StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Scope\AssignBoolVarToIntVar.gngr"));
        //    Parser parser = new Parser(reader.ReadToEnd());

        //    parser.parse();
        //    ScopeVisitor sv = new ScopeVisitor(parser.ast);
        //    Assert.IsInstanceOfType(sv.errors[0], typeof(TypeException));
        //}

        //[TestMethod]
        //public void AssignIntLiteralToBoolVar()
        //{
        //    StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Scope\AssignIntLiteralToBoolVar.gngr"));
        //    Parser parser = new Parser(reader.ReadToEnd());

        //    parser.parse();
        //    ScopeVisitor sv = new ScopeVisitor(parser.ast);
        //    Assert.IsInstanceOfType(sv.errors[0], typeof(TypeException));
        //}

        [TestMethod]
        public void AssignBinaryOpToIntVar()
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Scope\AssignBinaryOpToIntVar.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();
            ScopeVisitor sv = new ScopeVisitor(parser.ast);
        }

        [TestMethod]
        public void FormalArgsInFunctionScope()
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Scope\FormalArgsInFunction.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();
            ScopeVisitor sv = new ScopeVisitor(parser.ast);
        }

        //[TestMethod]
        //public void AssignInequalityToBoolVar()
        //{
        //    StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Scope\AssignInequalityToBoolVar.gngr"));
        //    Parser parser = new Parser(reader.ReadToEnd());
        //    parser.parse();
        //    ScopeVisitor sv = new ScopeVisitor(parser.ast);
        //}

        //[TestMethod]
        //public void AssignBinaryOpToBoolVar()
        //{
        //    StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Scope\AssignBinaryOpToBoolVar.gngr"));
        //    Parser parser = new Parser(reader.ReadToEnd());
        //    parser.parse();
        //    ScopeVisitor sv = new ScopeVisitor(parser.ast);

        //    Assert.IsInstanceOfType(sv.errors[0], typeof(TypeException));
        //}

        //[TestMethod]
        //public void BlockSurroundedIf()
        //{
        //    StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\CFG\BlockSurroundedIf.gngr"));
        //    Parser parser = new Parser(reader.ReadToEnd());

        //    parser.parse();
        //    CFGVisitor sv = new CFGVisitor(parser.ast);

        //    Statement ass1 = (Statement)parser.ast.entry;
        //    Assert.IsTrue(ass1.cfgSuccessors.Count == 1, "1");

        //    Statement ass2 = ass1.cfgSuccessors[0];
        //    Assert.IsTrue(ass2.cfgSuccessors.Count == 1, "2");

        //    Statement ass3 = ass2.cfgSuccessors[0];
        //    Assert.IsTrue(ass3.cfgSuccessors.Count == 1, "3");

        //    Statement if1 = ass3.cfgSuccessors[0];
        //    Assert.IsTrue(if1.cfgSuccessors.Count == 2, "4");

        //    Statement ass4 = if1.cfgSuccessors[0];
        //    Assert.IsTrue(ass4.cfgSuccessors.Count == 1, "5");

        //    Statement if2 = ass4.cfgSuccessors[0];
        //    Assert.IsTrue(if2.cfgSuccessors.Count == 2, "6");

        //    Statement ass5 = if2.cfgSuccessors[0];
        //    Assert.IsTrue(ass5.cfgSuccessors.Count == 1, "7");

        //    Statement ass6 = ass5.cfgSuccessors[0];
        //    Assert.IsTrue(ass6.cfgSuccessors.Count == 1, "8");
        //    Assert.AreEqual(ass6, if2.cfgSuccessors[1], "9");

        //    Statement ass7 = ass6.cfgSuccessors[0];
        //    Assert.IsTrue(ass7.cfgSuccessors.Count == 0, "10");
        //    Assert.AreEqual(ass7, if1.cfgSuccessors[1], "11");
        //}

        //[TestMethod]
        //public void CFG()
        //{
        //    StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\CFG\cfg.gngr"));
        //    Parser parser = new Parser(reader.ReadToEnd());

        //    parser.parse();
        //    CFGVisitor sv = new CFGVisitor(parser.ast);

        //    Statement ass1 = (Statement)parser.ast.entry;
        //    Statement ass2 = ass1.cfgSuccessors[0];
        //    Statement ass3 = if1.cfgSuccessors[0];
        //}

        //[TestMethod]
        //public void NestedIfNoStatements()
        //{
        //    StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\CFG\NestedIfNoStatements.gngr"));
        //    Parser parser = new Parser(reader.ReadToEnd());

        //    parser.parse();
        //    CFGVisitor sv = new CFGVisitor(parser.ast);

        //    Statement ass1 = (Statement)parser.ast.entry;
        //    Statement ass2 = ass1.cfgSuccessors[0];
        //    Statement if1 = ass2.cfgSuccessors[0];
        //    Statement if2 = if1.cfgSuccessors[0];
        //    Statement ass3 = if2.cfgSuccessors[0];
        //}

        //[TestMethod]
        //public void NestedIfWithStatements()
        //{
        //    StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\CFG\NestedIfWithStatements.gngr"));
        //    Parser parser = new Parser(reader.ReadToEnd());

        //    parser.parse();
        //    CFGVisitor sv = new CFGVisitor(parser.ast);

        //    Statement ass1 = (Statement)parser.ast.entry;
        //    Statement ass2 = ass1.cfgSuccessors[0];
        //    Statement ass3 = ass2.cfgSuccessors[0];
        //    Statement if1 = ass3.cfgSuccessors[0];
        //    Statement if2 = if1.cfgSuccessors[0];
        //    Statement ass4 = if2.cfgSuccessors[0];
        //    Statement ass5 = ass4.cfgSuccessors[0];
        //    Assert.AreEqual(ass5, if2.cfgSuccessors[1]);
        //    Statement ass6 = ass5.cfgSuccessors[0];
        //    Assert.AreEqual(ass6, if1.cfgSuccessors[1]);
        //}

        //[TestMethod]
        //public void DDG()
        //{
        //    StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\DDG\ddg.gngr"));
        //    Parser parser = new Parser(reader.ReadToEnd());

        //    parser.parse();
        //    CFGVisitor cfgv = new CFGVisitor(parser.ast);
        //    ScopeVisitor sv = new ScopeVisitor(parser.ast);
        //    DDGVisitor ddgv = new DDGVisitor(parser.ast);

        //    Assign ass1 = (Assign)parser.ast.entry; // x = 5
        //    Assign ass2 = (Assign)ass1.cfgSuccessors[0]; // y = x + 1
        //    Assign ass3 = (Assign)ass2.cfgSuccessors[0]; // a = true
        //    Statement if1 = (Statement)ass3.cfgSuccessors[0]; // if a
        //    Assign ass4 = (Assign)if1.cfgSuccessors[0]; // x = 5
        //    Assign ass5 = (Assign)ass4.cfgSuccessors[0]; // z = x
        //    Assign ass6 = (Assign)ass5.cfgSuccessors[0]; // x = 9 + 10
        //    Assign ass7 = (Assign)ass6.cfgSuccessors[0]; // x = x + y

        //    // ass 1
        //    Assert.IsTrue(ass1.dataDependencies.Count == 1, "1");
        //    Assert.IsTrue(ass1.dataDependencies.Contains(ass2), "2");

        //    // ass 2
        //    Assert.IsTrue(ass2.dataDependencies.Count == 1, "3");
        //    Assert.IsTrue(ass2.dataDependencies.Contains(ass7), "4");

        //    // ass 3
        //    Assert.IsTrue(ass3.dataDependencies.Count == 1, "5");
        //    Assert.IsTrue(ass3.dataDependencies.Contains(if1), "6");

        //    // ass 4
        //    Assert.IsTrue(ass4.dataDependencies.Count == 1, "7");
        //    Assert.IsTrue(ass4.dataDependencies.Contains(ass5), "8");

        //    // ass 5
        //    Assert.IsTrue(ass5.dataDependencies.Count == 0, "9");

        //    // ass 6
        //    Assert.IsTrue(ass6.dataDependencies.Count == 1, "10");
        //    Assert.IsTrue(ass6.dataDependencies.Contains(ass7), "11");

        //    // ass 7
        //    Assert.IsTrue(ass7.dataDependencies.Count == 0, "12");
        //}
    }
}
