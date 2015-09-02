//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using GingerScanner;

//namespace GingerScannerTest
//{
//    [TestClass]
//    public class ScannerTest
//    {
//        [TestMethod]
//        public void TestConstructor()
//        {
//            string test = "David";
//            Scanner gs = new Scanner(test);

//            Assert.AreEqual(test, gs.ReadToEnd());
//        }

//        [TestMethod]
//        public void TestNextSingle()
//        {
//            string test = "test";
//            Scanner gs = new Scanner(test);

//            Assert.AreEqual(new Token(test), gs.next());
//        }

//        [TestMethod]
//        public void TestNextCouple()
//        {
//            string first = "test";
//            string second = "david";
//            string test = $"{first} {second}";
//            Scanner s = new Scanner(test);

//            Assert.AreEqual(new Token(first), s.next());
//            Assert.AreEqual(new Token(second), s.next());
//        }

//        [TestMethod]
//        public void TestNextMulti()
//        {
//            string first = "test";
//            string second = "david";
//            string third = "hello";
//            string test = $"{first} {second} {third}";
//            Scanner s = new Scanner(test);

//            Assert.AreEqual(new Token(first), s.next());
//            Assert.AreEqual(new Token(second), s.next());
//            Assert.AreEqual(new Token(third), s.next());
//        }
//    }
//}
