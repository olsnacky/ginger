using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ginger
{
    public enum GingerToken
    {
        // default value
        Null = 0,
        Identifier,
        //If,
        Number,
        //BooleanLiteral,
        Assignment,
        OpenPrecedent,
        ClosePrecedent,
        OpenList,
        CloseList,
        Addition,
        Contract,
        Implementation,
        //Bool,
        //Int,
        Unknown,
        EndOfFile,
        Statement,
        StatementList,
        Expression,
        Function,
        //Component,
        Return,
        ListSeparator,
        //Void,
        //As,
        Var,
        //Ref,
        Source,
        Sink,
        High,
        Low,
        OpenAnnotation,
        Annotation,
        Import,
        IdentifierSeparator
    }

    public static class Grammar
    {
        public static bool isBinaryOperator(GingerToken token)
        {
            return token == GingerToken.Addition;
        }

        //public static bool isControl(GingerToken token)
        //{
        //    return token == GingerToken.If;
        //}

        public static bool isType(GingerToken token)
        {
            //return token == GingerToken.Int || token == GingerToken.Bool;
            return token == GingerToken.Var;
        }

        public static bool isInformationEndpoint(GingerToken token)
        {
            return token == GingerToken.Source || token == GingerToken.Sink;
        }

        public static bool isComponent(GingerToken token)
        {
            return token == GingerToken.Contract || token == GingerToken.Implementation;
        }

        //public static bool isComponentType(GingerToken token)
        //{
        //    return token == GingerToken.Contract || token == GingerToken.Implementation;
        //}

        //public static bool isFunctionType(GingerToken token)
        //{
        //    return isType(token) || token == GingerToken.Void;
        //}
    }

    public static class Lexicon
    {
        public static char[] ASSIGNMENT = { ':', '=' };
        public static char OPEN_PRECEDENT = '(';
        public static char CLOSE_PRECEDENT = ')';
        //public static char[] AS = { 'a', 's' };
        //public static char[] IF = { 'i', 'f' };
        public static char[] FUNCTION = { 'f', 'u', 'n', 'c', 't', 'i', 'o', 'n' };
        //public static char[] COMPONENT = { 'c', 'o', 'm', 'p' };
        public static char[] RETURN = { 'r', 'e', 't', 'u', 'r', 'n' };
        public static char ADDITION = '+';
        public static char OPEN_STATEMENT_LIST = '{';
        public static char CLOSE_STATEMENT_LIST = '}';
        public static char OPEN_ANNOTATION = ':';
        public static char ANNOTATION = '@';
        public static char IDENT_SEPARATOR = '.';
        public static char[] VAR_DECLARATION = { 'v', 'a', 'r' };
        //public static char[] INT = { 'i', 'n', 't' };
        //public static char[] VOID = { 'v', 'o', 'i', 'd' };
        //public static char[] BOOL = { 'b', 'o', 'o', 'l' };
        public static char[] CONTRACT = { 'c', 'o', 'n', 't', 'r', 'a', 'c', 't' };
        public static char[] IMPLEMENTATION = { 'i', 'm', 'p', 'l', 'e', 'm', 'e', 'n', 't', 'a', 't', 'i', 'o', 'n' };
        //public static char[] BOOLEAN_TRUE = { 't', 'r', 'u', 'e' };
        //public static char[] BOOLEAN_FALSE = { 'f', 'a', 'l', 's', 'e' };
        //public static char ZERO = '0';
        //public static char[] REF = { 'r', 'e', 'f' };
        public static char LIST_SEPARATOR = ',';
        public static char[] SEC_LVL_HIGH = { 'h', 'i', 'g', 'h' };
        public static char[] SEC_LVL_LOW = { 'l', 'o', 'w' };
        public static char[] SYS_READ = { 'r', 'e', 'a', 'd' };
        public static char[] SYS_WRITE = { 'w', 'r', 'i', 't', 'e' };
        public static char[] IMPORT = { 'i', 'm', 'p', 'o', 'r', 't' };

        private static char[] DIGITS = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        //private static char[] UPPER_ALPHABETIC_CHAR = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        private static char[] LOWER_ALPHABETIC_CHAR = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };

        public static bool isWhiteSpace(char c)
        {
            // TODO: implement own method
            return Char.IsWhiteSpace(c);
        }

        public static bool isDigit(char c)
        {
            return DIGITS.Contains(c);
        }

        public static bool isStartAssignmentChar(char c)
        {
            return ASSIGNMENT[0] == c;
        }

        public static bool isStartNumberChar(char c)
        {
            return isDigit(c);
        }

        public static bool isStartKeywordOrIdentifierChar(char c)
        {
            //return UPPER_ALPHABETIC_CHAR.Contains(c) || LOWER_ALPHABETIC_CHAR.Contains(c);
            return LOWER_ALPHABETIC_CHAR.Contains(c);
        }

        public static bool isKeywordOrIdentifierChar(char c)
        {
            //return isStartKeywordOrIdentifierChar(c) || isDigit(c);
            return isStartKeywordOrIdentifierChar(c);
        }
    }
}
