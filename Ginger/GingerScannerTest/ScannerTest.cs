using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GingerScanner;
using Ginger;
using System.IO;

namespace GingerScannerTest
{
    [TestClass]
    public class ScannerTest
    {
        [TestMethod]
        public void TestSimple()
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\test.gngr"));

            Scanner gs = new Scanner(reader.ReadToEnd());
            Assert.AreEqual(GingerToken.Int, gs.next(), "int 1");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 1");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 2");
            Assert.AreEqual(GingerToken.Assignment, gs.next(), "assign 1");
            Assert.AreEqual(GingerToken.IntegerLiteral, gs.next(), "integer 1");
            //Assert.AreEqual(GingerToken.EndOfLine, gs.next(), "eol 1");
            Assert.AreEqual(GingerToken.Bool, gs.next(), "bool 1");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 3");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 4");
            Assert.AreEqual(GingerToken.Assignment, gs.next(), "assign 2");
            Assert.AreEqual(GingerToken.BooleanLiteral, gs.next(), "boolean 1");
            //Assert.AreEqual(GingerToken.EndOfLine, gs.next(), "eol 2");
            Assert.AreEqual(GingerToken.If, gs.next(), "if");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 5");
            Assert.AreEqual(GingerToken.LessThan, gs.next(), "< 1");
            Assert.AreEqual(GingerToken.IntegerLiteral, gs.next(), "integer 2");
            Assert.AreEqual(GingerToken.OpenStatementList, gs.next(), "osl 1");
            Assert.AreEqual(GingerToken.While, gs.next(), "while");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 6");
            Assert.AreEqual(GingerToken.GreaterThan, gs.next(), "> 1");
            Assert.AreEqual(GingerToken.IntegerLiteral, gs.next(), "integer 3");
            Assert.AreEqual(GingerToken.OpenStatementList, gs.next(), "osl 2");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 5");
            Assert.AreEqual(GingerToken.Assignment, gs.next(), "assign 3");
            Assert.AreEqual(GingerToken.OpenPrecedent, gs.next(), "op 1");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 6");
            Assert.AreEqual(GingerToken.Addition, gs.next(), "+ 1");
            Assert.AreEqual(GingerToken.IntegerLiteral, gs.next(), "integer 4");
            Assert.AreEqual(GingerToken.ClosePrecedent, gs.next(), "cp 1");
            //Assert.AreEqual(GingerToken.EndOfLine, gs.next(), "eol 3");
            Assert.AreEqual(GingerToken.CloseStatementList, gs.next(), "csl 1");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 7");
            Assert.AreEqual(GingerToken.Assignment, gs.next(), "assign 4");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 8");
            Assert.AreEqual(GingerToken.Subtraction, gs.next(), "- 1");
            Assert.AreEqual(GingerToken.IntegerLiteral, gs.next(), "integer 5");
            Assert.AreEqual(GingerToken.CloseStatementList, gs.next(), "csl 2");
            Assert.AreEqual(GingerToken.Int, gs.next(), "int 2");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 7");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 8");
            Assert.AreEqual(GingerToken.Assignment, gs.next(), "assign 5");
            Assert.AreEqual(GingerToken.IntegerLiteral, gs.next(), "integer 6");
            //Assert.AreEqual(GingerToken.EndOfLine, gs.next(), "eol 4");

            Assert.AreEqual(GingerToken.Bool, gs.next(), "bool 2");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 9");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 10");
            Assert.AreEqual(GingerToken.Assignment, gs.next(), "assign 6");
            Assert.AreEqual(GingerToken.BooleanLiteral, gs.next(), "boolean 2");


            Assert.AreEqual(GingerToken.EndOfFile, gs.next(), "eof 1");
        }

        [TestMethod]
        public void TestSpy()
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\test.gngr"));

            Scanner gs = new Scanner(reader.ReadToEnd());
            Assert.AreEqual(GingerToken.Int, gs.next(), "int 1");
            Assert.AreEqual(GingerToken.Identifier, gs.spy(), "ident 1/1");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 2/1");
        }
    }
}
