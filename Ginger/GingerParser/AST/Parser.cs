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
            [GingerToken.Subtraction] = new Precedence(1, false),
            [GingerToken.LessThan] = new Precedence(0, false),
            [GingerToken.GreaterThan] = new Precedence(0, false)
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
        private List<ParseException> _errors;

        public List<ParseException> errors
        {
            get { return _errors; }
        }

        public StatementList ast
        {
            get
            {
                return statementList;
            }
        }

        public Parser(string source)
        {
            _errors = new List<ParseException>();
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
                try
                {
                    sl.add(parseStatement());
                }
                catch (ParseException pe)
                {
                    _errors.Add(pe);
                }
                
                nextScannerToken();

                if (currentScannerToken != endToken && currentScannerToken == GingerToken.EndOfFile)
                {
                    _errors.Add(new ParseException(scanner.row, scanner.col, $"Expected '{endToken.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR));
                }
            } while (currentScannerToken != endToken && currentScannerToken != GingerToken.EndOfFile);

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

                Node conditionExpression = parseExpression();

                nextScannerToken();
                if (currentScannerToken == GingerToken.OpenStatementList)
                {
                    nextScannerToken();
                    sl = parseStatementList(GingerToken.CloseStatementList);
                }
                else
                {
                    throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.OpenStatementList.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
                }

                if (controlToken == GingerToken.While)
                {
                    nc = new While(conditionExpression, sl);
                }
                else
                {
                    nc = new If(conditionExpression, sl);
                }
            }
            // statement = type, identifier
            else if (Grammar.isType(currentScannerToken))
            {
                Node type;
                if (currentScannerToken == GingerToken.Int)
                {
                    type = new Integer(scanner.row, scanner.col);
                }
                else
                {
                    type = new Boolean(scanner.row, scanner.col);
                }


                nextScannerToken();
                if (currentScannerToken != GingerToken.Identifier)
                {
                    throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.Identifier.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
                }
                else
                {
                    nc = new Declaration(type, new Identifier(scanner.row, scanner.col, new string(scanner.tokenValue)));
                }
            }
            // statement = identifier, "=", expression
            else if (currentScannerToken == GingerToken.Identifier)
            {
                Identifier identifier = new Identifier(scanner.row, scanner.col, new string(scanner.tokenValue));
                nextScannerToken();
                if (currentScannerToken == GingerToken.Assignment)
                {
                    nextScannerToken();
                    nc = new Assign(identifier, parseExpression());
                }
                else
                {
                    throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.Assignment.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
                }
            }
            else
            {
                throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.If.ToString()}', '{GingerToken.While.ToString()}', '{GingerToken.Int.ToString()}', '{GingerToken.Bool.ToString()}', or '{GingerToken.Identifier.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
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
            if (currentScannerToken == GingerToken.Identifier || currentScannerToken == GingerToken.Number || currentScannerToken == GingerToken.BooleanLiteral || currentScannerToken == GingerToken.Subtraction)
            {
                if (currentScannerToken == GingerToken.Identifier)
                {
                    n = new Identifier(scanner.row, scanner.col, new string(scanner.tokenValue));
                }
                else
                {
                    if (currentScannerToken == GingerToken.Subtraction)
                    {
                        spyScannerToken();
                        if (spiedScannerToken.GetValueOrDefault() == GingerToken.Number)
                        {
                            nextScannerToken();
                            string value = Lexicon.SUBTRACTION + new string(scanner.tokenValue);
                            n = new Literal<Integer>(new Integer(scanner.row, scanner.col, value));
                        }
                        else
                        {
                            throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.Number.ToString()}', found '{spiedScannerToken.GetValueOrDefault().ToString()}'", ExceptionLevel.ERROR);
                        }
                    }
                    else if (currentScannerToken == GingerToken.Number)
                    {
                        n = new Literal<Integer>(new Integer(scanner.row, scanner.col, new string(scanner.tokenValue)));
                    }
                    else
                    {
                        n = new Literal<Boolean>(new Boolean(scanner.row, scanner.col, new string(scanner.tokenValue)));
                    }
                }
            }
            else if (currentScannerToken == GingerToken.OpenPrecedent)
            {
                nextScannerToken();
                n = parseExpression();

                nextScannerToken();
                if (currentScannerToken != GingerToken.ClosePrecedent)
                {
                    throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.ClosePrecedent.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
                }
            }
            else
            {
                throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.Identifier.ToString()}', '{GingerToken.Number.ToString()}', or '{GingerToken.OpenPrecedent.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
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
                    lhs = new InequalityOperation(op, lhs, rhs);
                }
                else
                {
                    throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.Addition.ToString()}', or '{GingerToken.LessThan.ToString()}',  found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
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

    public enum ExceptionLevel
    {
        INFO = 0,
        WARNING = 1,
        ERROR = 2
    }

    public class ParseException : Exception
    {
        private int _row;
        private int _column;
        private string _reason;
        private ExceptionLevel _level;

        public int row
        {
            get { return _row; }
        }

        public int column
        {
            get { return _column; }
        }

        public string reason
        {
            get { return _reason; }
        }

        public ExceptionLevel level
        {
            get { return _level; }
        }

        public ParseException(int row, int column, string reason, ExceptionLevel level) : base()
        {
            this._row = row;
            this._column = column;
            this._reason = reason;
            this._level = level;
        }
    }
}
