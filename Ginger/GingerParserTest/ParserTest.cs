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

        public void visitBinaryOperation(BinaryOperation bo)
        {
            visitCollection(bo);
        }

        public void visitBoolean(GingerParser.Boolean b)
        {
            add(b);
        }

        public void visitBranch(If b)
        {
            visitCollection(b);
        }

        public void visitInequalityOperation(InequalityOperation c)
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

        public void visitLiteral<T>(Literal<T> l) where T : Typeable
        {
            add(l);
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
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\AST\parser.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

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
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(If), $"{i}");
                        break;
                    case 11:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(InequalityOperation), $"{i}");
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
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(InequalityOperation), $"{i}");
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
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\AST\precedence.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

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
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(If), $"{i}");
                        break;
                    case 2:
                        Assert.IsInstanceOfType(tv.visitedNodes[i], typeof(InequalityOperation), $"{i}");
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
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Scope\scope.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();
            ScopeVisitor sv = new ScopeVisitor(parser.ast);
            TestVisitor tv = new TestVisitor(parser.ast);

            // empty declaration to fool compiler
            Declaration george = new Declaration(new Integer(0, 0), new Identifier(0, 0, "geoerge1Test"));
            Declaration kim = new Declaration(new Integer(0, 0), new Identifier(0, 0, "kim1Test"));
            Declaration subKim = new Declaration(new Integer(0, 0), new Identifier(0, 0, "kim2Test"));

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
        public void DoubleDeclaration()
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Scope\DoubleDeclaration.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();
            ScopeVisitor sv = new ScopeVisitor(parser.ast);

            Assert.IsInstanceOfType(sv.errors[0], typeof(ScopeException));
        }

        [TestMethod]
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

        [TestMethod]
        public void UseOfIdentifierInFirstStatement()
        {
            Parser parser = new Parser("int x int y if y < x { int z }");
            parser.parse();

            CFGVisitor cfgv = new CFGVisitor(parser.ast);
            ScopeVisitor sv = new ScopeVisitor(parser.ast);
            DDGVisitor ddgv = new DDGVisitor(parser.ast);

            Assert.IsInstanceOfType(ddgv.errors[0], typeof(AccessException));
        }

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

        [TestMethod]
        public void AssignBoolVarToIntVar()
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Scope\AssignBoolVarToIntVar.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();
            ScopeVisitor sv = new ScopeVisitor(parser.ast);
            Assert.IsInstanceOfType(sv.errors[0], typeof(TypeException));
        }

        [TestMethod]
        public void AssignIntLiteralToBoolVar()
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Scope\AssignIntLiteralToBoolVar.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();
            ScopeVisitor sv = new ScopeVisitor(parser.ast);
            Assert.IsInstanceOfType(sv.errors[0], typeof(TypeException));
        }

        [TestMethod]
        public void AssignBinaryOpToIntVar()
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Scope\AssignBinaryOpToIntVar.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();
            ScopeVisitor sv = new ScopeVisitor(parser.ast);
        }

        [TestMethod]
        public void AssignInequalityToBoolVar()
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Scope\AssignInequalityToBoolVar.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());
            parser.parse();
            ScopeVisitor sv = new ScopeVisitor(parser.ast);
        }

        [TestMethod]
        public void AssignBinaryOpToBoolVar()
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Scope\AssignBinaryOpToBoolVar.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());
            parser.parse();
            ScopeVisitor sv = new ScopeVisitor(parser.ast);

            Assert.IsInstanceOfType(sv.errors[0], typeof(TypeException));
        }

        [TestMethod]
        public void AssignInequalityToIntVar()
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\Scope\AssignInequalityToIntVar.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());
            parser.parse();
            ScopeVisitor sv = new ScopeVisitor(parser.ast);

            Assert.IsInstanceOfType(sv.errors[0], typeof(TypeException));
        }

        [TestMethod]
        public void BlockSurroundedIf() {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\CFG\BlockSurroundedIf.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();
            CFGVisitor sv = new CFGVisitor(parser.ast);

            Statement ass1 = (Statement)parser.ast.entry;
            Assert.IsTrue(ass1.cfgSuccessors.Count == 1, "1");

            Statement ass2 = ass1.cfgSuccessors[0];
            Assert.IsTrue(ass2.cfgSuccessors.Count == 1, "2");

            Statement if1 = ass2.cfgSuccessors[0];
            Assert.IsTrue(if1.cfgSuccessors.Count == 2, "3");

            Statement ass4 = if1.cfgSuccessors[0];
            Assert.IsTrue(ass4.cfgSuccessors.Count == 1, "4");

            Statement if2 = ass4.cfgSuccessors[0];
            Assert.IsTrue(if2.cfgSuccessors.Count == 2, "5");

            Statement ass5 = if2.cfgSuccessors[0];
            Assert.IsTrue(ass5.cfgSuccessors.Count == 1, "6");

            Statement ass6 = ass5.cfgSuccessors[0];
            Assert.IsTrue(ass6.cfgSuccessors.Count == 1, "7");
            Assert.AreEqual(ass6, if2.cfgSuccessors[1], "8");

            Statement ass7 = ass6.cfgSuccessors[0];
            Assert.IsTrue(ass7.cfgSuccessors.Count == 0, "9");
            Assert.AreEqual(ass7, if1.cfgSuccessors[1], "10");
        }

        [TestMethod]
        public void CFG()
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\CFG\cfg.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();
            CFGVisitor sv = new CFGVisitor(parser.ast);

            Statement ass1 = (Statement)parser.ast.entry;
            Statement if1 = ass1.cfgSuccessors[0];
            Statement ass2 = if1.cfgSuccessors[0];
            Statement while1 = ass2.cfgSuccessors[0];
            Assert.AreEqual(while1, if1.cfgSuccessors[1]);
            Statement ass3 = while1.cfgSuccessors[0];
            Assert.AreEqual(while1, ass3.cfgSuccessors[0]);
        }

        [TestMethod]
        public void NestedIfNoStatements()
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\CFG\NestedIfNoStatements.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();
            CFGVisitor sv = new CFGVisitor(parser.ast);

            Statement ass1 = (Statement)parser.ast.entry;
            Statement ass2 = ass1.cfgSuccessors[0];
            Statement if1 = ass2.cfgSuccessors[0];
            Statement if2 = if1.cfgSuccessors[0];
            Statement ass3 = if2.cfgSuccessors[0];
        }

        [TestMethod]
        public void NestedIfWithStatements()
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\CFG\NestedIfWithStatements.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();
            CFGVisitor sv = new CFGVisitor(parser.ast);

            Statement ass1 = (Statement)parser.ast.entry;
            Statement ass2 = ass1.cfgSuccessors[0];
            Statement if1 = ass2.cfgSuccessors[0];
            Statement if2 = if1.cfgSuccessors[0];
            Statement ass3 = if2.cfgSuccessors[0];
            Statement ass4 = ass3.cfgSuccessors[0];
            Assert.AreEqual(ass4, if2.cfgSuccessors[1]);
            Statement ass5 = ass4.cfgSuccessors[0];
            Assert.AreEqual(ass5, if1.cfgSuccessors[1]);
        }

        [TestMethod]
        public void While()
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\CFG\while.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();
            CFGVisitor sv = new CFGVisitor(parser.ast);

            Statement ass1 = (Statement)parser.ast.entry;
            Statement if1 = ass1.cfgSuccessors[0];
            Statement ass2 = if1.cfgSuccessors[0];
            Statement while1 = ass2.cfgSuccessors[0];
            Statement ass3 = while1.cfgSuccessors[0];
            Assert.AreEqual(while1, ass3.cfgSuccessors[0]);
            Statement ass4 = ass3.cfgSuccessors[1];
            Assert.AreEqual(ass4, while1.cfgSuccessors[1]);
            Statement ass5 = ass4.cfgSuccessors[0];
            Assert.AreEqual(ass5, if1.cfgSuccessors[1]);
        }

        [TestMethod]
        public void DDG()
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\DDG\ddg.gngr"));
            Parser parser = new Parser(reader.ReadToEnd());

            parser.parse();
            CFGVisitor cfgv = new CFGVisitor(parser.ast);
            ScopeVisitor sv = new ScopeVisitor(parser.ast);
            DDGVisitor ddgv = new DDGVisitor(parser.ast);

            Assign ass1 = (Assign)parser.ast.entry; // x = 5
            Assign ass2 = (Assign)ass1.cfgSuccessors[0]; // y = x + 1
            Statement if1 = (Statement)ass2.cfgSuccessors[0]; // if y < x
            Assign ass3 = (Assign)if1.cfgSuccessors[0]; // x = 5
            Assign ass4 = (Assign)ass3.cfgSuccessors[0]; // z = x
            Statement if2 = (Statement)if1.cfgSuccessors[1]; // if y < x
            Statement while1 = (Statement)if2.cfgSuccessors[0]; // while x < y + 8
            Assign ass5 = (Assign)while1.cfgSuccessors[0]; // y = x
            Statement if3 = (Statement)ass5.cfgSuccessors[0]; // if y < x
            Assign ass6 = (Assign)if3.cfgSuccessors[0]; // b = x
            Assign ass7 = (Assign)if3.cfgSuccessors[1]; // x = y
            Assign ass8 = (Assign)while1.cfgSuccessors[1]; // j = y + x
            Assign ass9 = (Assign)ass8.cfgSuccessors[0]; // x = 9 + 10
            Assign ass10 = (Assign)ass9.cfgSuccessors[0]; // x = x + y

            // ass 1
            Assert.IsTrue(ass1.dataDependencies.Count == 8, "1");
            Assert.IsTrue(ass1.dataDependencies.Contains(ass8), "2");
            Assert.IsTrue(ass1.dataDependencies.Contains(if2), "3");
            Assert.IsTrue(ass1.dataDependencies.Contains(ass2), "4");
            Assert.IsTrue(ass1.dataDependencies.Contains(if1), "5");
            Assert.IsTrue(ass1.dataDependencies.Contains(while1), "6");
            Assert.IsTrue(ass1.dataDependencies.Contains(ass5), "7");
            Assert.IsTrue(ass1.dataDependencies.Contains(if3), "8");
            Assert.IsTrue(ass1.dataDependencies.Contains(ass6), "9");

            // ass 2
            Assert.IsTrue(ass2.dataDependencies.Count == 5, "10");
            Assert.IsTrue(ass2.dataDependencies.Contains(ass8), "11");
            Assert.IsTrue(ass2.dataDependencies.Contains(if2), "12");
            Assert.IsTrue(ass2.dataDependencies.Contains(if1), "13");
            Assert.IsTrue(ass2.dataDependencies.Contains(while1), "14");
            Assert.IsTrue(ass2.dataDependencies.Contains(ass10), "15");

            // ass 3
            Assert.IsTrue(ass3.dataDependencies.Count == 1, "16");
            Assert.IsTrue(ass3.dataDependencies.Contains(ass4), "17");

            // ass 4
            Assert.IsTrue(ass4.dataDependencies.Count == 0, "18");

            // ass 5
            Assert.IsTrue(ass5.dataDependencies.Count == 2, "19");
            Assert.IsTrue(ass5.dataDependencies.Contains(if3), "20");
            Assert.IsTrue(ass5.dataDependencies.Contains(ass7), "21");

            // ass 6
            Assert.IsTrue(ass6.dataDependencies.Count == 0, "22");

            // ass 7
            Assert.IsTrue(ass7.dataDependencies.Count == 5, "23");
            Assert.IsTrue(ass7.dataDependencies.Contains(while1), "24");
            Assert.IsTrue(ass7.dataDependencies.Contains(ass5), "25");
            Assert.IsTrue(ass7.dataDependencies.Contains(if3), "26");
            Assert.IsTrue(ass7.dataDependencies.Contains(ass6), "27");
            Assert.IsTrue(ass7.dataDependencies.Contains(ass8), "28");

            // ass 8
            Assert.IsTrue(ass8.dataDependencies.Count == 0, "29");

            // ass 9
            Assert.IsTrue(ass9.dataDependencies.Count == 1, "30");
            Assert.IsTrue(ass9.dataDependencies.Contains(ass10), "31");

            // ass 10
            Assert.IsTrue(ass10.dataDependencies.Count == 0, "32");
        }
    }
}
