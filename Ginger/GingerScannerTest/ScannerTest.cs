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
            Assert.AreEqual(GingerToken.Assignment, gs.next(), "assign 1");
            Assert.AreEqual(GingerToken.IntegerLiteral, gs.next(), "literal 1");
            Assert.AreEqual(GingerToken.EndOfLine, gs.next(), "eol 1");
            Assert.AreEqual(GingerToken.Bool, gs.next(), "bool");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 2");
            Assert.AreEqual(GingerToken.EndOfLine, gs.next(), "eol 2");
            Assert.AreEqual(GingerToken.If, gs.next(), "if");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 3");
            Assert.AreEqual(GingerToken.LessThan, gs.next(), "< 1");
            Assert.AreEqual(GingerToken.IntegerLiteral, gs.next(), "literal 2");
            Assert.AreEqual(GingerToken.OpenStatementList, gs.next(), "osl 1");
            Assert.AreEqual(GingerToken.While, gs.next(), "while");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 4");
            Assert.AreEqual(GingerToken.LessThan, gs.next(), "< 2");
            Assert.AreEqual(GingerToken.IntegerLiteral, gs.next(), "literal 3");
            Assert.AreEqual(GingerToken.OpenStatementList, gs.next(), "osl 2");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 5");
            Assert.AreEqual(GingerToken.Assignment, gs.next(), "assign 2");
            Assert.AreEqual(GingerToken.OpenPrecedent, gs.next(), "op");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 6");
            Assert.AreEqual(GingerToken.Addition, gs.next(), "+");
            Assert.AreEqual(GingerToken.IntegerLiteral, gs.next(), "literal 4");
            Assert.AreEqual(GingerToken.ClosePrecedent, gs.next(), "cp");
            Assert.AreEqual(GingerToken.EndOfLine, gs.next(), "eol 3");
            Assert.AreEqual(GingerToken.CloseStatementList, gs.next(), "csl 1");
            Assert.AreEqual(GingerToken.CloseStatementList, gs.next(), "csl 2");
            Assert.AreEqual(GingerToken.Int, gs.next(), "int 2");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 7");
            Assert.AreEqual(GingerToken.Assignment, gs.next(), "assign 3");
            Assert.AreEqual(GingerToken.IntegerLiteral, gs.next(), "literal 5");
            Assert.AreEqual(GingerToken.EndOfLine, gs.next(), "eol 4");
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
