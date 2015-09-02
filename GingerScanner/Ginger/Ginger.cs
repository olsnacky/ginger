using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger
{
    public static class Lexicon
    {
        private static char[] TRUE = { 't', 'r', 'u', 'e' };
        private static char[] FALSE = { 'f', 'a', 'l', 's', 'e' };

        public static char ASSIGNMENT = '=';
        public static char OPEN_PRECEDENT = '(';
        public static char CLOSE_PRECEDENT = ')';
        public static char[] IF = { 'i', 'f' };
        public static char[] WHILE = { 'w', 'h', 'i', 'l', 'e' };
        public static char[][] BOOLEAN = new char[][] { TRUE, FALSE };

        private static char ZERO = '0';
        private static char[] DIGITS = new char[] { ZERO, '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private static char[] UPPER_ALPHABETIC_CHAR = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private static char[] LOWER_ALPHABETIC_CHAR = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
        private static char[] OPERATIONS = new char[] { '+' };
        private static char UNDERSCORE = '_';
        private static char NEGATE = '-';
        private static char OPEN_STATEMENT_LIST = '{';
        private static char CLOSE_STATEMENT_LIST = '}';
        private static char[] INT = { 'i', 'n', 't' };
        private static char[] BOOL = { 'b', 'o', 'o', 'l' };
        private static char LESS_THAN = '<';

        public static bool isWhiteSpace(char c)
        {
            // TODO: imeplement own method
            return Char.IsWhiteSpace(c);
        }

        public static bool isUnderscore(char c)
        {
            return UNDERSCORE == c;
        }

        public static bool isNegate(char c)
        {
            return NEGATE == c;
        }

        public static bool isUpperAlpha(char c)
        {
            return UPPER_ALPHABETIC_CHAR.Contains(c);
        }

        public static bool isLowerAlpha(char c)
        {
            return LOWER_ALPHABETIC_CHAR.Contains(c);
        }

        public static bool isAlpha(char c)
        {
            return isUpperAlpha(c) || isLowerAlpha(c);
        }

        public static bool isDigit(char c)
        {
            return DIGITS.Contains(c);
        }

        public static bool isInteger(char[] chars)
        {
            bool result;
            if (isEmpty(chars))
            {
                result = false;
            }
            else
            {
                result = true;
                for (int i = 0; i < chars.Length; i++)
                {
                    // if it's not a digit, or the first digit is a 0 with subsequent digits
                    if (!isDigit(chars[i]) || (i == 0 && chars[i] == ZERO && chars.Length > 1))
                    {
                        // allow negative numbers
                        if (!(i == 0 && isNegate(chars[i]) && chars.Length > 1))
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }
            
            return result;
        }

        public static bool isBoolean(char[] chars)
        {
            bool result = false;

            foreach (char[] b in BOOLEAN)
            {
                if (b.SequenceEqual(chars))
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        public static bool isValidChar(char c)
        {
            return isAlpha(c) || isDigit(c) || isUnderscore(c) || isNegate(c);
        }

        public static bool isOperator(char c)
        {
            return OPERATIONS.Contains(c);
        }

        public static bool isEquality(char[] chars)
        {
            if (chars.Length == 1)
            {
                return isEquality(chars[0]);
            }

            return false;
        }

        public static bool isEquality(char c)
        {
            return LESS_THAN == c;
        }

        public static bool isAssignment(char c)
        {
            return ASSIGNMENT == c;
        }

        public static bool isOpenPrecedent(char c)
        {
            return OPEN_PRECEDENT == c;
        }

        public static bool isClosePrecedent(char c)
        {
            return CLOSE_PRECEDENT == c;
        }

        public static bool isOpenStatementList(char c)
        {
            return OPEN_STATEMENT_LIST == c;
        }

        public static bool isCloseStatementList(char c)
        {
            return CLOSE_STATEMENT_LIST == c;
        }

        public static bool isIf(char[] chars)
        {
            return IF.SequenceEqual(chars);
        }

        public static bool isWhile(char[] chars)
        {
            return WHILE.SequenceEqual(chars);
        }

        public static bool isType(char[] chars)
        {
            return isInt(chars) || isBool(chars);
        }

        public static bool isIdentifier(char[] chars)
        {
            bool result;

            if (isEmpty(chars)) {
                result = false;
            }
            else
            {
                result = true;
                for (var i = 0; i < chars.Length; i++)
                {
                    char c = chars[i];
                    if (!(isAlpha(c) || isUnderscore(c)))
                    {
                        // allow digits if not the first character
                        if (!(i != 0 && isDigit(c)))
                        {
                            result = false;
                            break;
                        }
                    }
                }
            }

            return result;
        }

        private static bool isInt(char[] chars)
        {
            return INT.SequenceEqual(chars);
        }

        private static bool isBool(char[] chars)
        {
            return BOOL.SequenceEqual(chars);
        }

        private static bool isEmpty(char[] chars)
        {
            return chars.Length == 0;
        }
    }
}
