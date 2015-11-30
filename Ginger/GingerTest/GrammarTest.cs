using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ginger;

namespace GingerTest
{
    [TestClass]
    public class GrammarTest
    {
        [TestMethod]
        public void Type()
        {
            foreach (GingerToken token in Enum.GetValues(typeof(GingerToken)))
            {
                if (token == GingerToken.Int || token == GingerToken.Bool)
                {
                    Assert.IsTrue(Grammar.isType(token), token.ToString());
                }
                else
                {
                    Assert.IsFalse(Grammar.isType(token), token.ToString());
                }
            }
        }

        [TestMethod]
        public void Control()
        {
            foreach (GingerToken token in Enum.GetValues(typeof(GingerToken)))
            {
                if (token == GingerToken.If || token == GingerToken.While)
                {
                    Assert.IsTrue(Grammar.isControl(token), token.ToString());
                }
                else
                {
                    Assert.IsFalse(Grammar.isControl(token), token.ToString());
                }
            }
        }

        [TestMethod]
        public void BinaryOperator()
        {
            foreach (GingerToken token in Enum.GetValues(typeof(GingerToken)))
            {
                if (token == GingerToken.Addition || token == GingerToken.Subtraction)
                {
                    Assert.IsTrue(Grammar.isBinaryOperator(token), token.ToString());
                }
                else
                {
                    Assert.IsFalse(Grammar.isBinaryOperator(token), token.ToString());
                }
            }
        }

        [TestMethod]
        public void CompareOperator()
        {
            foreach (GingerToken token in Enum.GetValues(typeof(GingerToken)))
            {
                if (token == GingerToken.LessThan || token == GingerToken.GreaterThan)
                {
                    Assert.IsTrue(Grammar.isConditionOperator(token), token.ToString());
                }
                else
                {
                    Assert.IsFalse(Grammar.isConditionOperator(token), token.ToString());
                }
            }
        }
    }
}
