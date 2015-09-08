using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ginger;

namespace GingerScanner
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
        EndOfFile
    }

    public class Scanner : System.IO.StringReader
    {
        private const char EOF = '\0';
        private const int NO_MORE_CHAR = -1;
        private char currentChar;
        private List<char> semanticValue;

        public char[] tokenValue
        {
            get
            {
                return semanticValue.ToArray();
            }
        }

        public Scanner(string source) : base(source)
        {
            clearSemanticValue();
            nextChar();
        }

        public GingerToken next()
        {
            clearSemanticValue();

            // clear out any white space, we don't care about it
            while (Lexicon.isWhiteSpace(currentChar))
            {
                nextChar();
            }

            if (Lexicon.isStartIntegerChar(currentChar))
            {
                do
                {
                    semanticValue.Add(currentChar);
                    nextChar();
                } while (Lexicon.isDigit(currentChar));

                return GingerToken.IntegerLiteral;
            }
            else if (Lexicon.isStartKeywordOrIdentifierChar(currentChar))
            {
                do
                {
                    semanticValue.Add(currentChar);
                    nextChar();
                } while (Lexicon.isKeywordOrIdentifierChar(currentChar));

                if (tokenValue.SequenceEqual(Lexicon.IF)) {
                    return GingerToken.If;
                }
                else if (tokenValue.SequenceEqual(Lexicon.WHILE))
                {
                    return GingerToken.While;
                }
                else if (tokenValue.SequenceEqual(Lexicon.INT))
                {
                    return GingerToken.Int;
                }
                else if (tokenValue.SequenceEqual(Lexicon.BOOL))
                {
                    return GingerToken.Bool;
                }
                else
                {
                    return GingerToken.Identifier;
                }
            }
            else if (currentChar == Lexicon.OPEN_PRECEDENT)
            {
                nextChar();
                return GingerToken.OpenPrecedent;
            }
            else if (currentChar == Lexicon.CLOSE_PRECEDENT)
            {
                nextChar();
                return GingerToken.ClosePrecedent;
            }
            else if (currentChar == Lexicon.OPEN_STATEMENT_LIST)
            {
                nextChar();
                return GingerToken.OpenStatementList;
            }
            else if (currentChar == Lexicon.CLOSE_STATEMENT_LIST)
            {
                nextChar();
                return GingerToken.CloseStatementList;
            }
            else if (currentChar == Lexicon.END_OF_LINE)
            {
                nextChar();
                return GingerToken.EndOfLine;
            }
            else if (currentChar == Lexicon.ADDITION)
            {
                nextChar();
                return GingerToken.Addition;
            }
            else if (currentChar == Lexicon.LESS_THAN)
            {
                nextChar();
                return GingerToken.LessThan;
            }
            else if (currentChar == Lexicon.ASSIGNMENT)
            {
                nextChar();
                return GingerToken.Assignment;
            } else if (currentChar == EOF)
            {
                nextChar();
                return GingerToken.EndOfFile;
            }

            nextChar();
            return GingerToken.Unknown;
        }

        private void nextChar()
        {
            char c;
            int next = this.Read();
            if (next == NO_MORE_CHAR)
            {
                c = EOF;
            }
            else
            {
                c = Convert.ToChar(next);
            }

            currentChar = c;
        }

        private void clearSemanticValue()
        {
            semanticValue = new List<char>();
        }
    }
}
