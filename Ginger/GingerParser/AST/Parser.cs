using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ginger;
using GingerScanner;
using GingerUtil;

namespace GingerParser
{
    public class Parser
    {
        private const int MIN_PRECEDENCE = 0;
        // operator precedence mapping
        private Dictionary<GingerToken, Precedence> OPERATOR_PRECEDENCE = new Dictionary<GingerToken, Precedence>
        {
            [GingerToken.Addition] = new Precedence(1, false),
            [GingerToken.LessThan] = new Precedence(0, false)
        };

        private struct Precedence
        {
            public int precedence;
            public bool rightAssociated;

            public Precedence(int precedence, bool rightAssociated)
            {
                this.precedence = precedence;
                this.rightAssociated = rightAssociated;
            }
        }

        private Scanner scanner;
        private GingerToken currentScannerToken;
        private GingerToken? spiedScannerToken;
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
                Compare condition = (Compare)parseExpression();

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
                    nc = new Declaration(type, new Identifier(new string(scanner.tokenValue)));
                }
            }
            // statement = identifier, "=", expression
            else if (currentScannerToken == GingerToken.Identifier)
            {
                Identifier identifier = new Identifier(new string(scanner.tokenValue));
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

        private Node parseExpression()
        {
            return parseExpression(parseTerminalExpression(), MIN_PRECEDENCE);
        }

        private Node parseTerminalExpression()
        {
            Node n;
            if (currentScannerToken == GingerToken.Identifier || currentScannerToken == GingerToken.IntegerLiteral)
            {
                if (currentScannerToken == GingerToken.Identifier)
                {
                    n = new Identifier(new string(scanner.tokenValue));
                }
                else
                {
                    n = new Literal<Integer>(new Integer(new string(scanner.tokenValue)));
                }
            }
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

        // precedence-climbing method
        // https://en.wikipedia.org/wiki/Operator-precedence_parser#Precedence_climbing_method
        private Node parseExpression(Node lhs, int minPrecedence)
        {
            spyScannerToken();
            // the spied token is an operator and has precedence >= the minimumum precedence
            while (isOperator(spiedScannerToken.GetValueOrDefault()) && OPERATOR_PRECEDENCE[spiedScannerToken.GetValueOrDefault()].precedence >= minPrecedence)
            {
                GingerToken op = spiedScannerToken.GetValueOrDefault();
                // move to spied position
                nextScannerToken();

                nextScannerToken();
                Node rhs = parseTerminalExpression();

                spyScannerToken();
                // the spied token is an operator, and its precedence is greater than the previous op's precedence OR it is a right associated operator with precedence equal to op's precedence
                while (isOperator(spiedScannerToken.GetValueOrDefault()) && (OPERATOR_PRECEDENCE[spiedScannerToken.GetValueOrDefault()].precedence > OPERATOR_PRECEDENCE[op].precedence || (OPERATOR_PRECEDENCE[spiedScannerToken.GetValueOrDefault()].rightAssociated && OPERATOR_PRECEDENCE[spiedScannerToken.GetValueOrDefault()].precedence == OPERATOR_PRECEDENCE[op].precedence)))
                {
                    rhs = parseExpression(rhs, OPERATOR_PRECEDENCE[spiedScannerToken.GetValueOrDefault()].precedence);
                    spyScannerToken();
                }

                if (Grammar.isBinaryOperator(op))
                {
                    lhs = new BinaryOperation(op, lhs, rhs);
                }
                else if (Grammar.isConditionOperator(op))
                {
                    lhs = new Compare(op, lhs, rhs);
                }
                else
                {
                    throw new ParseException();
                }
            }

            return lhs;
        }

        private bool isOperator(GingerToken token)
        {
            return Grammar.isBinaryOperator(token) || Grammar.isConditionOperator(token);
        }

        private void nextScannerToken()
        {
            currentScannerToken = scanner.next();
        }

        private void spyScannerToken()
        {
            spiedScannerToken = scanner.spy();
        }
    }

    public class ParseException : Exception
    {
        public ParseException() : base()
        {

        }
    }
}
