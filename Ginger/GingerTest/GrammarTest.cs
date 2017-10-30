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
                //if (token == GingerToken.Int || token == GingerToken.Bool)
                //{
                //    Assert.IsTrue(Grammar.isType(token), token.ToString());
                //}
                if (token == GingerToken.Var)
                {
                    Assert.IsTrue(Grammar.isType(token), token.ToString());
                }
                else
                {
                    Assert.IsFalse(Grammar.isType(token), token.ToString());
                }
            }
        }

        //[TestMethod]
        //public void FunctionType()
        //{
        //    foreach (GingerToken token in Enum.GetValues(typeof(GingerToken)))
        //    {
        //        if (token == GingerToken.Int || token == GingerToken.Bool || token == GingerToken.Void)
        //        {
        //            Assert.IsTrue(Grammar.isFunctionType(token), token.ToString());
        //        }
        //        else
        //        {
        //            Assert.IsFalse(Grammar.isFunctionType(token), token.ToString());
        //        }
        //    }
        //}

        //[TestMethod]
        //public void ComponentType()
        //{
        //    foreach (GingerToken token in Enum.GetValues(typeof(GingerToken)))
        //    {
        //        if (token == GingerToken.Contract || token == GingerToken.Implementation)
        //        {
        //            Assert.IsTrue(Grammar.isComponentType(token), token.ToString());
        //        }
        //        else
        //        {
        //            Assert.IsFalse(Grammar.isComponentType(token), token.ToString());
        //        }
        //    }
        //}

        //[TestMethod]
        //public void Control()
        //{
        //    foreach (GingerToken token in Enum.GetValues(typeof(GingerToken)))
        //    {
        //        if (token == GingerToken.If)
        //        {
        //            Assert.IsTrue(Grammar.isControl(token), token.ToString());
        //        }
        //        else
        //        {
        //            Assert.IsFalse(Grammar.isControl(token), token.ToString());
        //        }
        //    }
        //}

        [TestMethod]
        public void BinaryOperator()
        {
            foreach (GingerToken token in Enum.GetValues(typeof(GingerToken)))
            {
                if (token == GingerToken.Addition)
                {
                    Assert.IsTrue(Grammar.isBinaryOperator(token), token.ToString());
                }
                else
                {
                    Assert.IsFalse(Grammar.isBinaryOperator(token), token.ToString());
                }
            }
        }
    }
}
