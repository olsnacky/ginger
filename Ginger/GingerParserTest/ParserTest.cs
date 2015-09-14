using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GingerParser;
using System.IO;

namespace GingerParserTest
{
    [TestClass]
    public class ParserTest
    {
        [TestMethod]
        public void Simple()
        {
            Parser parser = new Parser(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Data\parser.gngr"));
            parser.parse();
        }
    }
}
