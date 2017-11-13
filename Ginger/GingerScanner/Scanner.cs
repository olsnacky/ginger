using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ginger;

namespace GingerScanner
{
    public class Scanner : System.IO.StringReader
    {
        private const char EOF = '\0';
        private const int NO_MORE_CHAR = -1;
        private char currentChar;
        private List<char> semanticValue;

        private int _col;
        private int _row;

        public int col
        {
            get { return _col; }
        }

        public int row
        {
            get { return _row; }
        }

        public char[] tokenValue
        {
            get
            {
                return semanticValue.ToArray();
            }
        }

        public Scanner(string source) : base(source)
        {
            _col = 0;
            _row = 0;
            clearSemanticValue();
            nextChar();
        }

        public GingerToken spy()
        {
            Scanner s = (Scanner)this.MemberwiseClone();
            return s.next();
        }

        public GingerToken next()
        {
            clearSemanticValue();

            // clear out any white space, we don't care about it
            while (Lexicon.isWhiteSpace(currentChar))
            {
                nextChar();
            }

            if (Lexicon.isStartNumberChar(currentChar))
            {
                do
                {
                    semanticValue.Add(currentChar);
                    nextChar();
                } while (Lexicon.isDigit(currentChar));

                return GingerToken.Number;
            }
            else if (Lexicon.isStartAssignmentChar(currentChar))
            {
                nextChar();
                if (currentChar == Lexicon.ASSIGNMENT[1])
                {
                    nextChar();
                    return GingerToken.Assignment;
                }
                else {
                    GingerToken spied = spy();
                    if (spied == GingerToken.Annotation)
                    {
                        return GingerToken.OpenAnnotation;
                    }
                }
            }
            else if (Lexicon.isStartKeywordOrIdentifierChar(currentChar))
            {
                do
                {
                    semanticValue.Add(currentChar);
                    nextChar();
                } while (Lexicon.isKeywordOrIdentifierChar(currentChar));

                //if (tokenValue.SequenceEqual(Lexicon.IF))
                //{
                //    return GingerToken.If;
                //}
                //else if (tokenValue.SequenceEqual(Lexicon.BOOLEAN_TRUE) || tokenValue.SequenceEqual(Lexicon.BOOLEAN_FALSE))
                //{
                //    return GingerToken.BooleanLiteral;
                //}
                //else if (tokenValue.SequenceEqual(Lexicon.INT))
                //{
                //    return GingerToken.Int;
                //}
                //else if (tokenValue.SequenceEqual(Lexicon.BOOL))
                //{
                //    return GingerToken.Bool;
                //}
                if (tokenValue.SequenceEqual(Lexicon.FUNCTION))
                {
                    return GingerToken.Function;
                }
                //else if (tokenValue.SequenceEqual(Lexicon.COMPONENT))
                //{
                //    return GingerToken.Component;
                //}
                else if (tokenValue.SequenceEqual(Lexicon.RETURN))
                {
                    return GingerToken.Return;
                }
                //else if (tokenValue.SequenceEqual(Lexicon.VOID))
                //{
                //    return GingerToken.Void;
                //}
                //else if (tokenValue.SequenceEqual(Lexicon.AS))
                //{
                //    return GingerToken.As;
                //}
                //else if (tokenValue.SequenceEqual(Lexicon.CONTRACT))
                //{
                //    return GingerToken.Contract;
                //}
                //else if (tokenValue.SequenceEqual(Lexicon.IMPLEMENTATION))
                //{
                //    return GingerToken.Implementation;
                //}
                else if (tokenValue.SequenceEqual(Lexicon.VAR_DECLARATION))
                {
                    return GingerToken.Var;
                }
                //else if(tokenValue.SequenceEqual(Lexicon.REF))
                //{
                //    return GingerToken.Ref;
                //}
                else if(tokenValue.SequenceEqual(Lexicon.SEC_LVL_HIGH))
                {
                    return GingerToken.High;
                }
                else if (tokenValue.SequenceEqual(Lexicon.SEC_LVL_LOW))
                {
                    return GingerToken.Low;
                }
                else if (tokenValue.SequenceEqual(Lexicon.SYS_READ))
                {
                    return GingerToken.Source;
                }
                else if (tokenValue.SequenceEqual(Lexicon.SYS_WRITE))
                {
                    return GingerToken.Sink;
                }
                else if (tokenValue.SequenceEqual(Lexicon.CONTRACT))
                {
                    return GingerToken.Contract;
                }
                else if (tokenValue.SequenceEqual(Lexicon.IMPLEMENTATION))
                {
                    return GingerToken.Implementation;
                }
                else if (tokenValue.SequenceEqual(Lexicon.IMPORT))
                {
                    return GingerToken.Import;
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
                return GingerToken.OpenList;
            }
            else if (currentChar == Lexicon.CLOSE_STATEMENT_LIST)
            {
                nextChar();
                return GingerToken.CloseList;
            }
            else if (currentChar == Lexicon.ANNOTATION)
            {
                nextChar();
                return GingerToken.Annotation;
            }
            //else if (currentChar == Lexicon.END_OF_LINE)
            //{
            //    nextChar();
            //    return GingerToken.EndOfLine;
            //}
            else if (currentChar == Lexicon.ADDITION)
            {
                nextChar();
                return GingerToken.Addition;
            }
            //else if (currentChar == Lexicon.SUBTRACTION)
            //{
            //    nextChar();
            //    return GingerToken.Subtraction;
            //}
            //else if (currentChar == Lexicon.LESS_THAN)
            //{
            //    nextChar();
            //    return GingerToken.LessThan;
            //}
            //else if (currentChar == Lexicon.GREATER_THAN)
            //{
            //    nextChar();
            //    return GingerToken.GreaterThan;
            //}
            //else if (currentChar == Lexicon.SECURITY_ASSIGNMENT)
            //{
            //    nextChar();
            //    return GingerToken.SecurityAssignment;
            //}
            else if (currentChar == EOF)
            {
                nextChar();
                return GingerToken.EndOfFile;
            }
            else if (currentChar == Lexicon.LIST_SEPARATOR)
            {
                nextChar();
                return GingerToken.ListSeparator;
            }
            else if (currentChar == Lexicon.IDENT_SEPARATOR)
            {
                nextChar();
                return GingerToken.IdentifierSeparator;
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

            // if this is a new line, or a carriage return not followed by a newline (the new line will
            // be picked up as the next value)
            if (currentChar == '\n' || (currentChar == '\r' && !(Convert.ToChar(this.Peek()) == '\n')))
            {
                _col = 0;
                _row++;
            }
            else
            {
                _col++;
            }
        }

        private void clearSemanticValue()
        {
            semanticValue = new List<char>();
        }
    }
}
