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
            Assert.AreEqual(GingerToken.Var, gs.next(), "var 1");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 1");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 2");
            Assert.AreEqual(GingerToken.Assignment, gs.next(), "assign 1");
            Assert.AreEqual(GingerToken.Number, gs.next(), "int 1");
            Assert.AreEqual(GingerToken.Var, gs.next(), "var 2");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 3");
            Assert.AreEqual(GingerToken.Function, gs.next(), "func 1");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 4");
            Assert.AreEqual(GingerToken.OpenPrecedent, gs.next(), "op 1");
            Assert.AreEqual(GingerToken.Var, gs.next(), "var 3");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 5");
            Assert.AreEqual(GingerToken.ClosePrecedent, gs.next(), "cp 1");
            Assert.AreEqual(GingerToken.OpenList, gs.next(), "osl 1");
            Assert.AreEqual(GingerToken.Return, gs.next(), "ret 1");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 14");
            Assert.AreEqual(GingerToken.CloseList, gs.next(), "csl 1");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 6");
            Assert.AreEqual(GingerToken.Assignment, gs.next(), "assign 2");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 7");
            Assert.AreEqual(GingerToken.OpenPrecedent, gs.next(), "op 2");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 8");
            Assert.AreEqual(GingerToken.Addition, gs.next(), "add 1");
            Assert.AreEqual(GingerToken.Number, gs.next(), "int 2");
            Assert.AreEqual(GingerToken.ClosePrecedent, gs.next(), "cp 2");
            Assert.AreEqual(GingerToken.Addition, gs.next(), "add 2");
            Assert.AreEqual(GingerToken.Source, gs.next(), "read 1");
            Assert.AreEqual(GingerToken.OpenPrecedent, gs.next(), "op 3");
            //Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 10");
            Assert.AreEqual(GingerToken.ClosePrecedent, gs.next(), "cp 3");
            Assert.AreEqual(GingerToken.OpenAnnotation, gs.next(), "oa 1");
            Assert.AreEqual(GingerToken.Annotation, gs.next(), "annot 1");
            Assert.AreEqual(GingerToken.Low, gs.next(), "low 1");
            Assert.AreEqual(GingerToken.Sink, gs.next(), "write 1");
            Assert.AreEqual(GingerToken.OpenPrecedent, gs.next(), "op 4");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 15");
            Assert.AreEqual(GingerToken.ClosePrecedent, gs.next(), "cp 4");
            Assert.AreEqual(GingerToken.OpenAnnotation, gs.next(), "oa 2");
            Assert.AreEqual(GingerToken.Annotation, gs.next(), "annot 2");
            Assert.AreEqual(GingerToken.High, gs.next(), "high 1");
        }

        [TestMethod]
        public void TestSpy()
        {
            StreamReader reader = new StreamReader(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\test.gngr"));

            Scanner gs = new Scanner(reader.ReadToEnd());
            Assert.AreEqual(GingerToken.Var, gs.next(), "var 1");
            Assert.AreEqual(GingerToken.Identifier, gs.spy(), "ident 1/2");
            Assert.AreEqual(GingerToken.Identifier, gs.next(), "ident 2/2");
        }
    }
}
