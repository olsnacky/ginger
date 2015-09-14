using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ginger;
using GingerScanner;

namespace GingerParser
{
    public class Parser
    {
        private Scanner scanner;
        private GingerToken currentScannerToken;

        public Parser(string source)
        {
            scanner = new Scanner(source);
        }

        public void parse()
        {
            nextScannerToken();
            parseStatementList(GingerToken.EndOfFile);
        }

        private void parseStatementList(GingerToken endToken)
        {
            do
            {
                parseStatement();
                nextScannerToken();

                //if (currentScannerToken != GingerToken.EndOfLine)
                //{
                //    throw new ParseException();
                //}

                //nextScannerToken();
            } while (currentScannerToken != endToken);
        }

        private void parseStatement()
        {
            // statement = ("if" | "while"), expression, "{", statement-list, "}"
            if (Grammar.isControl(currentScannerToken))
            {
                nextScannerToken();
                parseExpression();

                nextScannerToken();
                if (currentScannerToken == GingerToken.OpenStatementList)
                {
                    nextScannerToken();
                    parseStatementList(GingerToken.CloseStatementList);
                }
                else
                {
                    throw new ParseException();
                }
            }
            // statement = type, identifier
            else if (Grammar.isType(currentScannerToken))
            {
                nextScannerToken();
                if (currentScannerToken != GingerToken.Identifier)
                {
                    throw new ParseException();
                }
            }
            // statement = identifier, "=", expression
            else if (currentScannerToken == GingerToken.Identifier)
            {
                nextScannerToken();
                if (currentScannerToken == GingerToken.Assignment)
                {
                    nextScannerToken();
                    parseExpression();
                }
            }
            else
            {
                throw new ParseException();
            }
        }

        private void parseExpression()
        {
            // expression = identifier | expression("+" | "<") expression | integer
            if (currentScannerToken == GingerToken.Identifier || currentScannerToken == GingerToken.IntegerLiteral)
            {
                spyScannerToken();
                if (Grammar.isBinaryOperator(currentScannerToken))
                {
                    // move to the spied position
                    nextScannerToken();

                    // move to the next position
                    nextScannerToken();
                    parseExpression();
                }
            }
            // expression = "(", expression, ")"
            else if (currentScannerToken == GingerToken.OpenPrecedent)
            {
                nextScannerToken();
                parseExpression();

                nextScannerToken();
                if (currentScannerToken != GingerToken.ClosePrecedent)
                {
                    throw new ParseException();
                }
            }
            else
            {
                throw new ParseException();
            }
        }

        private void nextScannerToken()
        {
            currentScannerToken = scanner.next();
        }

        private void spyScannerToken()
        {
            currentScannerToken = scanner.spy();
        }
    }

    public class ParseException : Exception
    {
        public ParseException() : base()
        {

        }
    }
}
