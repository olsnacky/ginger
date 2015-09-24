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
        private StatementList statementList;

        public StatementList ast
        {
            get
            {
                return statementList;
            }
        }

        public Parser(string source)
        {
            scanner = new Scanner(source);
        }

        public void parse()
        {
            nextScannerToken();
            statementList = parseStatementList(GingerToken.EndOfFile);
        }

        private StatementList parseStatementList(GingerToken endToken)
        {
            StatementList sl = new StatementList();
            do
            {
                sl.add(parseStatement());
                nextScannerToken();

                //if (currentScannerToken != GingerToken.EndOfLine)
                //{
                //    throw new ParseException();
                //}

                //nextScannerToken();
            } while (currentScannerToken != endToken);

            return sl;
        }

        private NodeCollection parseStatement()
        {
            NodeCollection nc;
            // statement = ("if" | "while"), expression, "{", statement-list, "}"
            if (Grammar.isControl(currentScannerToken))
            {
                StatementList sl;
                GingerToken controlToken = currentScannerToken;

                nextScannerToken();
                Compare condition = parseCondition();

                nextScannerToken();
                if (currentScannerToken == GingerToken.OpenStatementList)
                {
                    nextScannerToken();
                    sl = parseStatementList(GingerToken.CloseStatementList);
                }
                else
                {
                    throw new ParseException();
                }

                if (controlToken == GingerToken.While)
                {
                    nc = new While(condition, sl);
                }
                else
                {
                    nc = new Branch(condition, sl);
                }
            }
            // statement = type, identifier
            else if (Grammar.isType(currentScannerToken))
            {
                Node type;
                if (currentScannerToken == GingerToken.Int)
                {
                    type = new Integer();
                }
                else
                {
                    type = new Boolean();
                }


                nextScannerToken();
                if (currentScannerToken != GingerToken.Identifier)
                {
                    throw new ParseException();
                }
                else
                {
                    nc = new Declaration(type, new Identifier());
                }
            }
            // statement = identifier, "=", expression
            else if (currentScannerToken == GingerToken.Identifier)
            {
                Identifier identifier = new Identifier();
                nextScannerToken();
                if (currentScannerToken == GingerToken.Assignment)
                {
                    nextScannerToken();
                    nc = new Assign(identifier, parseExpression());
                }
                else
                {
                    throw new ParseException();
                }
            }
            else
            {
                throw new ParseException();
            }

            return nc;
        }

        private Compare parseCondition()
        {
            // expression, "<", expression
            Node leftExpression = parseExpression();

            nextScannerToken();
            if (Grammar.isCompareOperator(currentScannerToken))
            {
                GingerToken compareOp = currentScannerToken;
                nextScannerToken();
                return new Compare(compareOp, leftExpression, parseExpression());
            }
            else
            {
                throw new ParseException();
            }
        }

        private Node parseExpression()
        {
            Node n;
            // expression = identifier | expression, "+", expression | integer
            if (currentScannerToken == GingerToken.Identifier || currentScannerToken == GingerToken.IntegerLiteral)
            {
                Node value;
                if (currentScannerToken == GingerToken.Identifier)
                {
                    value = new Identifier();
                }
                else
                {
                    value = new Literal();
                }

                n = value;

                spyScannerToken();
                if (Grammar.isBinaryOperator(currentScannerToken))
                {
                    //NodeCollection nc;
                    GingerToken opToken;

                    // move to the spied position
                    nextScannerToken();
                    opToken = currentScannerToken;

                    // move to the next position
                    nextScannerToken();

                    n = new BinaryOperation(currentScannerToken, value, parseExpression());
                   // if (Grammar.isBinaryOperator(opToken))
                    //{
                        
                    //}
                    //else
                    //{
                      //  nc = new Compare(currentScannerToken, value, parseExpression());
                    //}

                    //n = nc;
                }
            }
            // expression = "(", expression, ")"
            else if (currentScannerToken == GingerToken.OpenPrecedent)
            {
                nextScannerToken();
                n = parseExpression();

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

            return n;
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
