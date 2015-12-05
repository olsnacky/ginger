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
        private char ASSIGNMENT = '=';
        private char SECURITY_ASSIGNMENT = ':';
        private char OPEN_PRECEDENT = '(';
        private char CLOSE_PRECEDENT = ')';
        private char OPEN_STATEMENT = '{';
        private char CLOSE_STATEMENT = '}';
        private char WHITESPACE = ' ';
        private char[] LOWER_ALPHA = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        private char[] UPPER_ALPHA = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private char[] DIGITS = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private char ADDITION = '+';
        private char SUBTRACTION = '-';
        private char LESS_THAN = '<';
        private char GREATER_THAN = '>';
        private char END_OF_LINE = ';';
        private char[] INT = { 'i', 'n', 't' };
        private char[] BOOL = { 'b', 'o', 'o', 'l' };
        private char[] BOOLEAN_TRUE = { 't', 'r', 'u', 'e' };
        private char[] BOOLEAN_FALSE = { 'f', 'a', 'l', 's', 'e' };
        private char UNDERSCORE = '_';
        //private char NEGATE = '-';

        private char[] IF = { 'i', 'f' };
        private char[] WHILE = { 'w', 'h', 'i', 'l', 'e' };
        
        private char EMPTY = '\0';
        private char PERIOD = '.';
        private char UNKNOWN = '@';

        private char[] MIXED_ALPHA = new char[] { 'a', 'B', 'c', 'D' };
        private char[] MIXED_ALL = new char[] { 'a', 'B', '1', '_' };
        private char[] INTEGER = new char[] { '1', '2', '3', '4' };
        private char[] NEGATIVE_INTEGER = new char[] { '-', '5', '6', '7' };
        private char[] EMPTY_CHARS = new char[] { };
        private char[] SINGLE = new char[] { 's' };

        private char[] testChars;
        private char[][] testCharArrays;

        private char[] validChars;

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
            tempc.Add(SECURITY_ASSIGNMENT);
            tempc.Add(OPEN_PRECEDENT);
            tempc.Add(CLOSE_PRECEDENT);
            tempc.Add(WHITESPACE);
            tempc.Add(END_OF_LINE);
            tempc.Add(ADDITION);
            tempc.Add(SUBTRACTION);
            tempc.Add(LESS_THAN);
            tempc.Add(GREATER_THAN);
            tempc.Add(EMPTY);
            tempc.Add(UNKNOWN);
            tempc.Add(PERIOD);
            tempc.Add(OPEN_STATEMENT);
            tempc.Add(CLOSE_STATEMENT);
            tempc.Add(UNDERSCORE);
            //tempc.Add(NEGATE);
            return tempc.ToArray();
        }

        private char[][] getTestCharArrays()
        {
            List<char[]> tempca = new List<char[]>();
            tempca.Add(BOOL);
            tempca.Add(INT);
            tempca.Add(LOWER_ALPHA);
            tempca.Add(UPPER_ALPHA);
            tempca.Add(DIGITS);
            tempca.Add(EMPTY_CHARS);
            tempca.Add(SINGLE);
            tempca.Add(IF);
            tempca.Add(WHILE);
            tempca.Add(INTEGER);
            tempca.Add(NEGATIVE_INTEGER);
            tempca.Add(BOOLEAN_FALSE);
            tempca.Add(BOOLEAN_TRUE);
            return tempca.ToArray();
        }

        private char[] getValidChars()
        {
            List<char> tempc = new List<char>();
            tempc.AddRange(getAlpha());
            tempc.AddRange(DIGITS);

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
        public void HasSecurityAssignment()
        {
            Assert.AreEqual(Lexicon.SECURITY_ASSIGNMENT, SECURITY_ASSIGNMENT);
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
        public void HasBooleanTrue()
        {
            CollectionAssert.AreEqual(Lexicon.BOOLEAN_TRUE, BOOLEAN_TRUE);
        }

        [TestMethod]
        public void hasBooleanFalse()
        {
            CollectionAssert.AreEqual(Lexicon.BOOLEAN_FALSE, BOOLEAN_FALSE);
        }

        [TestMethod]
        public void HasEndOfLine()
        {
            Assert.AreEqual(Lexicon.END_OF_LINE, END_OF_LINE);
        }

        [TestMethod]
        public void HasAddition()
        {
            Assert.AreEqual(Lexicon.ADDITION, ADDITION);
        }

        [TestMethod]
        public void HasSubtraction()
        {
            Assert.AreEqual(Lexicon.SUBTRACTION, SUBTRACTION);
        }

        [TestMethod]
        public void HasGreaterThan()
        {
            Assert.AreEqual(Lexicon.GREATER_THAN, GREATER_THAN);
        }

        [TestMethod]
        public void HasLessThan()
        {
            Assert.AreEqual(Lexicon.LESS_THAN, LESS_THAN);
        }

        [TestMethod]
        public void HasOpenStatementList()
        {
            Assert.AreEqual(Lexicon.OPEN_STATEMENT_LIST, OPEN_STATEMENT);
        }

        [TestMethod]
        public void HasCloseStatementList()
        {
            Assert.AreEqual(Lexicon.CLOSE_STATEMENT_LIST, CLOSE_STATEMENT);
        }

        [TestMethod]
        public void HasInt()
        {
            CollectionAssert.AreEqual(Lexicon.INT, INT);
        }

        [TestMethod]
        public void HasBool()
        {
            CollectionAssert.AreEqual(Lexicon.BOOL, BOOL);
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
        public void StartIntegerChar()
        {
            char[] allowed = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            foreach (char c in testChars)
            {
                if (allowed.Contains(c))
                {
                    Assert.IsTrue(Lexicon.isStartNumberChar(c), $"{c}");
                }
                else
                {
                    Assert.IsFalse(Lexicon.isStartNumberChar(c), $"{c}");
                }
            }
        }

        [TestMethod]
        public void StartKeywordOrIdentifierChar()
        {
            foreach (char c in testChars)
            {
                if (LOWER_ALPHA.Contains(c) || UPPER_ALPHA.Contains(c) || c == UNDERSCORE)
                {
                    Assert.IsTrue(Lexicon.isStartKeywordOrIdentifierChar(c), $"{c}");
                }
                else
                {
                    Assert.IsFalse(Lexicon.isStartKeywordOrIdentifierChar(c), $"{c}");
                }
            }
        }

        [TestMethod]
        public void KeywordOrIdentifierChar()
        {
            foreach (char c in testChars)
            {
                if (DIGITS.Contains(c) || LOWER_ALPHA.Contains(c) || UPPER_ALPHA.Contains(c) || c == UNDERSCORE)
                {
                    Assert.IsTrue(Lexicon.isKeywordOrIdentifierChar(c), $"{c}");
                }
                else
                {
                    Assert.IsFalse(Lexicon.isKeywordOrIdentifierChar(c), $"{c}");
                }
            }
        }
    }
}
