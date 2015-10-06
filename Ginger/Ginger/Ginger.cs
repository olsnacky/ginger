using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger
{
    public enum GingerToken
    {
        Identifier,
        If,
        While,
        IntegerLiteral,
        Assignment,
        OpenPrecedent,
        ClosePrecedent,
        OpenStatementList,
        CloseStatementList,
        LessThan,
        Addition,
        Bool,
        Int,
        Unknown,
        EndOfLine,
        EndOfFile,
        Statement,
        StatementList,
        Expression
    }

    public static class Grammar
    {
        public static bool isType(GingerToken token)
        {
            return token == GingerToken.Int || token == GingerToken.Bool;
        }

        public static bool isControl(GingerToken token)
        {
            return token == GingerToken.If || token == GingerToken.While;
        }

        public static bool isBinaryOperator(GingerToken token)
        {
            return token == GingerToken.Addition;
        }

        public static bool isCompareOperator(GingerToken token)
        {
            return token == GingerToken.LessThan;
        }
    }

    public static class Lexicon
    {
        public static char ASSIGNMENT = '=';
        public static char OPEN_PRECEDENT = '(';
        public static char CLOSE_PRECEDENT = ')';
        public static char[] IF = { 'i', 'f' };
        public static char[] WHILE = { 'w', 'h', 'i', 'l', 'e' };
        public static char END_OF_LINE = ';';
        public static char ADDITION = '+';
        public static char LESS_THAN = '<';
        public static char OPEN_STATEMENT_LIST = '{';
        public static char CLOSE_STATEMENT_LIST = '}';
        public static char[] INT = { 'i', 'n', 't' };
        public static char[] BOOL = { 'b', 'o', 'o', 'l' };
        public static char ZERO = '0';

        private static char[] DIGITS = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private static char[] UPPER_ALPHABETIC_CHAR = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private static char[] LOWER_ALPHABETIC_CHAR = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        public static bool isWhiteSpace(char c)
        {
            // TODO: imeplement own method
            return Char.IsWhiteSpace(c);
        }

        public static bool isDigit(char c)
        {
            return DIGITS.Contains(c);
        }

        public static bool isStartIntegerChar(char c)
        {
            return isDigit(c) || c == '-';
        }

        public static bool isStartKeywordOrIdentifierChar(char c)
        {
            return UPPER_ALPHABETIC_CHAR.Contains(c) || LOWER_ALPHABETIC_CHAR.Contains(c) || c == '_';
        }

        public static bool isKeywordOrIdentifierChar(char c)
        {
            return isStartKeywordOrIdentifierChar(c) || isDigit(c);
        }
    }
}
