using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ginger;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace GingerTest
{
    [TestClass]
    public class LexiconTest
    {
        char ASSIGNMENT = '=';
        char OPEN_PRECEDENT = '(';
        char CLOSE_PRECEDENT = ')';
        char OPEN_STATEMENT = '{';
        char CLOSE_STATEMENT = '}';
        char WHITESPACE = ' ';
        char UNDERSCORE = '_';
        char NEGATE = '-';
        char[] LOWER_ALPHA = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        char[] UPPER_ALPHA = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        char[] DIGITS = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        char[] OPERATORS = { '+' };
        char[] EQUALITIES = { '<' };

        char[] IF = { 'i', 'f' };
        char[] WHILE = { 'w', 'h', 'i', 'l', 'e' };
        char[][] BOOLS = { new char[] { 't', 'r', 'u', 'e' }, new char[] { 'f', 'a', 'l', 's', 'e' } };

        char EMPTY = '\0';
        char PERIOD = '.';
        char UNKNOWN = '@';
        int ZERO_IDX = 0;
        int TEST_DIGIT_IDX = 4;

        char[] MIXED_ALPHA = new char[] { 'a', 'B', 'c', 'D' };
        char[] MIXED_ALL = new char[] { 'a', 'B', '1', '_' };
        char[] INTEGER = new char[] { '1', '2', '3', '4' };
        char[] NEGATIVE_INTEGER = new char[] { '-', '5', '6', '7' };
        char[] EMPTY_CHARS = new char[] { };
        char[] SINGLE = new char[] { 's' };

        char[] testChars;
        char[][] testCharArrays;

        char[] validChars;

        [TestInitialize]
        public void Init()
        {
            testChars = getTestChars();
            testCharArrays = getTestCharArrays();
            validChars = getValidChars();
        }

        private char[] getTestChars()
        {
            List<char> tempc = new List<char>();
            tempc.AddRange(LOWER_ALPHA);
            tempc.AddRange(UPPER_ALPHA);
            tempc.AddRange(DIGITS);
            tempc.Add(ASSIGNMENT);
            tempc.Add(OPEN_PRECEDENT);
            tempc.Add(CLOSE_PRECEDENT);
            tempc.Add(WHITESPACE);
            tempc.Add(UNDERSCORE);
            tempc.Add(NEGATE);
            tempc.AddRange(OPERATORS);
            tempc.AddRange(EQUALITIES);
            tempc.Add(EMPTY);
            tempc.Add(UNKNOWN);
            tempc.Add(PERIOD);
            tempc.Add(OPEN_STATEMENT);
            tempc.Add(CLOSE_STATEMENT);
            return tempc.ToArray();
        }

        private char[][] getTestCharArrays()
        {
            List<char[]> tempca = new List<char[]>();
            tempca.AddRange(BOOLS);
            tempca.Add(LOWER_ALPHA);
            tempca.Add(UPPER_ALPHA);
            tempca.Add(DIGITS);
            tempca.Add(EMPTY_CHARS);
            tempca.Add(SINGLE);
            tempca.Add(IF);
            tempca.Add(WHILE);
            tempca.Add(INTEGER);
            tempca.Add(NEGATIVE_INTEGER);
            return tempca.ToArray();
        }

        private char[] getValidChars()
        {
            List<char> tempc = new List<char>();
            tempc.AddRange(getAlpha());
            tempc.AddRange(DIGITS);
            tempc.Add(UNDERSCORE);
            tempc.Add(NEGATE);
            return tempc.ToArray();
        }

        private char[] getAlpha()
        {
            List<char> tempc = new List<char>();
            tempc.AddRange(LOWER_ALPHA);
            tempc.AddRange(UPPER_ALPHA);

            return tempc.ToArray();
        }

        [TestMethod]
        public void HasAssignment()
        {
            Assert.AreEqual(Lexicon.ASSIGNMENT, ASSIGNMENT);
        }

        [TestMethod]
        public void HasOpenPrecedent()
        {
            Assert.AreEqual(Lexicon.OPEN_PRECEDENT, OPEN_PRECEDENT);
        }

        [TestMethod]
        public void HasClosePrecedent()
        {
            Assert.AreEqual(Lexicon.CLOSE_PRECEDENT, CLOSE_PRECEDENT);
        }

        [TestMethod]
        public void HasIf()
        {
            CollectionAssert.AreEqual(Lexicon.IF, IF);
        }

        [TestMethod]
        public void HasWhile()
        {
            CollectionAssert.AreEqual(Lexicon.WHILE, WHILE);
        }

        [TestMethod]
        public void HasBoolean()
        {
            for (var i = 0; i < Lexicon.BOOLEAN.Length; i++)
            {
                CollectionAssert.AreEqual(Lexicon.BOOLEAN[i], BOOLS[i]);
            }
        }

        [TestMethod]
        public void WhiteSpace()
        {
            foreach (char c in testChars)
            {
                if (c == WHITESPACE)
                {
                    Assert.IsTrue(Lexicon.isWhiteSpace(c), $"{c}");
                }
                else
                {
                    Assert.IsFalse(Lexicon.isWhiteSpace(c), $"{c}");
                }
            }
        }

        [TestMethod]
        public void Underscore()
        {
            foreach (char c in testChars)
            {
                if (c == UNDERSCORE)
                {
                    Assert.IsTrue(Lexicon.isUnderscore(c), $"{c}");
                }
                else
                {
                    Assert.IsFalse(Lexicon.isUnderscore(c), $"{c}");
                }
            }
        }

        [TestMethod]
        public void Negate()
        {
            foreach (char c in testChars)
            {
                if (c == NEGATE)
                {
                    Assert.IsTrue(Lexicon.isNegate(c), $"{c}");
                }
                else
                {
                    Assert.IsFalse(Lexicon.isNegate(c), $"{c}");
                }
            }
        }

        [TestMethod]
        public void UpperAlpha()
        {
            foreach (char c in testChars)
            {
                if (UPPER_ALPHA.Contains(c))
                {
                    Assert.IsTrue(Lexicon.isUpperAlpha(c), $"{c}");
                }
                else
                {
                    Assert.IsFalse(Lexicon.isUpperAlpha(c), $"{c}");
                }
            }
        }

        [TestMethod]
        public void LowerAlpha()
        {
            foreach (char c in testChars)
            {
                if (LOWER_ALPHA.Contains(c))
                {
                    Assert.IsTrue(Lexicon.isLowerAlpha(c), $"{c}");
                }
                else
                {
                    Assert.IsFalse(Lexicon.isLowerAlpha(c), $"{c}");
                }
            }
        }

        [TestMethod]
        public void Alpha()
        {
            char[] alpha = getAlpha();
            foreach (char c in testChars)
            {
                if (alpha.Contains(c))
                {
                    Assert.IsTrue(Lexicon.isAlpha(c), $"{c}");
                }
                else
                {
                    Assert.IsFalse(Lexicon.isAlpha(c), $"{c}");
                }
            }
        }

        [TestMethod]
        public void Digit()
        {
            foreach (char c in testChars)
            {
                if (DIGITS.Contains(c))
                {
                    Assert.IsTrue(Lexicon.isDigit(c), $"{c}");
                }
                else
                {
                    Assert.IsFalse(Lexicon.isDigit(c), $"{c}");
                }
            }
        }

        [TestMethod]
        public void Integer()
        {
            char testDigit = DIGITS[TEST_DIGIT_IDX];
            char zero = DIGITS[ZERO_IDX];
            Assert.IsTrue(Lexicon.isInteger(new char[] { testDigit }), "digit");
            Assert.IsFalse(Lexicon.isInteger(new char[] { NEGATE }), "negate without digits");
            Assert.IsTrue(Lexicon.isInteger(new char[] { NEGATE, testDigit }), "negate single");
            Assert.IsTrue(Lexicon.isInteger(new char[] { zero }), "zero");
            Assert.IsTrue(Lexicon.isInteger(new char[] { NEGATE, zero }), "negate zero");
            Assert.IsFalse(Lexicon.isInteger(new char[] { testDigit, testDigit, NEGATE, testDigit, testDigit }), "negate in middle"); 
            Assert.IsFalse(Lexicon.isInteger(new char[] { testDigit, testDigit, NEGATE }), "negate at end");

            char[][] cas = new char[][] { INTEGER, NEGATIVE_INTEGER };
            foreach (char[] ca in testCharArrays)
            {
                if (cas.Contains(ca))
                {
                    Assert.IsTrue(Lexicon.isInteger(ca), $"{ca}");
                }
                else
                {
                    Assert.IsFalse(Lexicon.isInteger(ca), $"{ca}");
                }
            }
        }

        [TestMethod]
        public void Boolean()
        {
            foreach (char[] ca in testCharArrays)
            {
                if (BOOLS.Contains(ca))
                {
                    Assert.IsTrue(Lexicon.isBoolean(ca), $"{ca}");
                }
                else
                {
                    Assert.IsFalse(Lexicon.isBoolean(ca), $"{ca}");
                }
            }
        }

        [TestMethod]
        public void Char()
        {
            foreach (char c in testChars)
            {
                if (validChars.Contains(c))
                {
                    Assert.IsTrue(Lexicon.isValidChar(c), $"{c}");
                }
                else
                {
                    Assert.IsFalse(Lexicon.isValidChar(c), $"{c}");
                }
            }
        }

        [TestMethod]
        public void Operator()
        {
            foreach (char c in testChars)
            {
                if (OPERATORS.Contains(c))
                {
                    Assert.IsTrue(Lexicon.isOperator(c), $"{c}");
                }
                else
                {
                    Assert.IsFalse(Lexicon.isOperator(c), $"{c}");
                }
            }
        }

        [TestMethod]
        public void Equality()
        {
            foreach (char c in testChars)
            {
                if (EQUALITIES.Contains(c))
                {
                    Assert.IsTrue(Lexicon.isEquality(c), $"{c}");
                }
                else
                {
                    Assert.IsFalse(Lexicon.isEquality(c), $"{c}");
                }
            }
        }

        [TestMethod]
        public void Assignment()
        {
            foreach (char c in testChars)
            {
                if (c == ASSIGNMENT) {
                    Assert.IsTrue(Lexicon.isAssignment(c), $"{c}");
                } else
                {
                    Assert.IsFalse(Lexicon.isAssignment(c), $"{c}");
                }
            }
        }

        [TestMethod]
        public void OpenPrecedent()
        {
            foreach (char c in testChars)
            {
                if (c == OPEN_PRECEDENT)
                {
                    Assert.IsTrue(Lexicon.isOpenPrecedent(c), $"{c}");
                }
                else
                {
                    Assert.IsFalse(Lexicon.isOpenPrecedent(c), $"{c}");
                }
            }
        }

        [TestMethod]
        public void ClosePrecedent()
        {
            foreach (char c in testChars)
            {
                if (c == CLOSE_PRECEDENT)
                {
                    Assert.IsTrue(Lexicon.isClosePrecedent(c), $"{c}");
                }
                else
                {
                    Assert.IsFalse(Lexicon.isClosePrecedent(c), $"{c}");
                }
            }
        }

        [TestMethod]
        public void OpenStatement()
        {
            foreach (char c in testChars)
            {
                if (c == OPEN_STATEMENT)
                {
                    Assert.IsTrue(Lexicon.isOpenStatementList(c), $"{c}");
                }
                else
                {
                    Assert.IsFalse(Lexicon.isOpenStatementList(c), $"{c}");
                }
            }
        }

        [TestMethod]
        public void CloseStatement()
        {
            foreach (char c in testChars)
            {
                if (c == CLOSE_STATEMENT)
                {
                    Assert.IsTrue(Lexicon.isCloseStatementList(c), $"{c}");
                }
                else
                {
                    Assert.IsFalse(Lexicon.isCloseStatementList(c), $"{c}");
                }
            }
        }

        [TestMethod]
        public void If()
        {
            foreach (char[] ca in testCharArrays)
            {
                if (ca.SequenceEqual(IF))
                {
                    Assert.IsTrue(Lexicon.isIf(ca), $"{ca}");
                }
                else
                {
                    Assert.IsFalse(Lexicon.isIf(ca), $"{ca}");
                }
            }
        }

        [TestMethod]
        public void While()
        {
            foreach (char[] ca in testCharArrays)
            {
                if (ca.SequenceEqual(WHILE))
                {
                    Assert.IsTrue(Lexicon.isWhile(ca), $"{ca}");
                }
                else
                {
                    Assert.IsFalse(Lexicon.isWhile(ca), $"{ca}");
                }
            }
        }

        [TestMethod]
        public void Identifier()
        {
            char[] id1 = { '_', 'h' };
            char[] id2 = { '1' };
            char[] id3 = { '1', 'h' };
            char[] id4 = { 'h' };
            char[] id5 = { 'h', 'e' };
            char[] id6 = { 'h', '2' };
            char[] id7 = { 'h', '_' };
            char[] id8 = { 'h', '_', '3' };
            char[] id9 = { 'H', 'A', 'L' };
            List<char[]> tempIdentifiers = new List<char[]>();
            tempIdentifiers.Add(id1);
            tempIdentifiers.Add(id2);
            tempIdentifiers.Add(id3);
            tempIdentifiers.Add(id4);
            tempIdentifiers.Add(id5);
            tempIdentifiers.Add(id6);
            tempIdentifiers.Add(id7);
            tempIdentifiers.Add(id8);
            tempIdentifiers.Add(id9);
            char[][] identifiers = tempIdentifiers.ToArray();


            foreach (char[] ca in identifiers)
            {
                if (!ca.SequenceEqual(id2) || !ca.SequenceEqual(id3))
                {
                    Assert.IsTrue(Lexicon.isIdentifier(ca), $"{ca}");
                }
                else
                {
                    Assert.IsFalse(Lexicon.isIdentifier(ca), $"{ca}");
                }
            }
        }
    }
}
