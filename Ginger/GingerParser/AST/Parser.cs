using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ginger;
using GingerScanner;
using GingerUtil;
using System.IO;
using System.Reflection;

namespace GingerParser
{
    public class Parser
    {
        private const int MIN_PRECEDENCE = 0;
        // operator precedence mapping
        private Dictionary<GingerToken, Precedence> OPERATOR_PRECEDENCE = new Dictionary<GingerToken, Precedence>
        {
            [GingerToken.Addition] = new Precedence(1, false)
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
        //private StatementList statementList;
        private ComponentList _componentList;
        private List<ParseException> _errors;
        //private Component _entry;

        public List<ParseException> errors
        {
            get { return _errors; }
        }

        //public StatementList ast
        //{
        //    get
        //    {
        //        return statementList;
        //    }
        //}

        public ComponentList ast => _componentList;
        //public Component entry => _entry;

        public Parser(string source)
        {
            _errors = new List<ParseException>();
            scanner = new Scanner(source);
        }

        public void parse()
        {
            nextScannerToken();
            //statementList = parseStatementList(GingerToken.EndOfFile, false);
            _componentList = parseComponentList();
        }

        private ComponentList parseComponentList()
        {
            ComponentList cl = new ComponentList();

            do
            {
                Component c = null;

                try
                {
                    c = parseComponent();
                }
                catch (ParseException pe)
                {
                    _errors.Add(pe);
                }

                //if (c != null)
                //{
                //    if (c.variable.identifier.name.Equals("app"))
                //    {
                //        if (_entry == null)
                //        {
                //            _entry = c;
                //        }
                //        else
                //        {
                //            _errors.Add(new ParseException(scanner.row, scanner.col, $"Found more than one entry component", ExceptionLevel.ERROR));
                //        }
                //    }
                //}
                if (c != null)
                {
                    cl.add(c);
                }

                nextScannerToken();
            } while (currentScannerToken != GingerToken.EndOfFile);

            //if (_entry == null)
            //{
            //    _errors.Add(new ParseException(0, 0, $"Did not find entry component 'app'", ExceptionLevel.ERROR));
            //}

            return cl;
        }

        private StatementList parseStatementList()
        {
            StatementList sl = new StatementList();
            if (currentScannerToken != GingerToken.CloseList)
            {
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
                } while (currentScannerToken != GingerToken.CloseList);
            }

            return sl;
        }

        private FunctionList parseFunctionList()
        {
            FunctionList fl = new FunctionList();

            do
            {
                Function f = null;
                try
                {
                    f = parseFunction();
                }
                catch (ParseException pe)
                {
                    _errors.Add(pe);
                }

                if (f != null)
                {
                    fl.add(f);
                    //if (f.name.identifier.name.Equals("main"))
                    //{
                    //    if (_entry == null)
                    //    {
                    //        _entry = f;
                    //    }
                    //    else
                    //    {
                    //        _errors.Add(new ParseException(scanner.row, scanner.col, $"Found more than one entry point", ExceptionLevel.ERROR));
                    //    }
                    //}
                }

                nextScannerToken();
            } while (currentScannerToken != GingerToken.CloseList);

            return fl;
        }

        private Identifier parseIdentifier(bool compoundAllowed)
        {
            string name = "";
            int row;
            int col;

            if (currentScannerToken == GingerToken.Identifier)
            {
                row = scanner.row;
                col = scanner.col;
            }
            else
            {
                throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.Identifier.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
            }

            do
            {
                name += new string(scanner.tokenValue);

                //nextScannerToken();
                spyScannerToken();
                if (spiedScannerToken == GingerToken.IdentifierSeparator)
                {
                    if (compoundAllowed)
                    {
                        nextScannerToken();
                        name += Lexicon.IDENT_SEPARATOR;

                        nextScannerToken();
                        if (currentScannerToken != GingerToken.Identifier)
                        {
                            throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.Identifier.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
                        }
                    }
                    else
                    {
                        throw new ParseException(scanner.row, scanner.col, $"Found compound identifier, where one is not allowed", ExceptionLevel.ERROR);
                    }
                }
                else
                {
                    break;
                }
            } while (currentScannerToken == GingerToken.Identifier);

            return new Identifier(row, col, name);
        }

        private Function parseFunction()
        {
            Function f;
            Variable name;
            VarList formalArgsList = new VarList();
            StatementList sl;

            if (currentScannerToken == GingerToken.Function)
            {
                nextScannerToken();
                if (currentScannerToken == GingerToken.Identifier)
                {
                    //name = new Variable(new Identifier(scanner.row, scanner.col, new string(scanner.tokenValue)));
                    name = new Variable(parseIdentifier(false));

                    // open-precedent
                    nextScannerToken();
                    if (currentScannerToken == GingerToken.OpenPrecedent)
                    {
                        // variable-list
                        formalArgsList = parseVarList();

                        // close-precedent
                        if (currentScannerToken == GingerToken.ClosePrecedent)
                        {
                            nextScannerToken();
                        }
                        else
                        {
                            throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.ClosePrecedent.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
                        }

                        // statement list
                        if (currentScannerToken == GingerToken.OpenList)
                        {
                            nextScannerToken();

                            sl = parseStatementList();
                            //nc = new Function(functionName, functionVariableList, functionReturnType, sl);
                            f = new Function(name, formalArgsList, sl);
                        }
                        else
                        {
                            throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.OpenList.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
                        }
                    }
                    else
                    {
                        throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.OpenPrecedent.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
                    }
                }
                else
                {
                    throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.Identifier.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
                }
            }
            else
            {
                throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.Function.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
            }

            return f;
        }

        private ImportList parseImportList()
        {
            ImportList il = new ImportList();
            while (currentScannerToken == GingerToken.Import)
            {
                nextScannerToken();
                if (currentScannerToken == GingerToken.Identifier)
                {
                    //il.add(new Import(new Identifier(scanner.row, scanner.col, new string(scanner.tokenValue))));
                    il.add(new Import(parseIdentifier(false)));
                }
                else
                {
                    throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.Identifier.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
                }

                nextScannerToken();
            }

            return il;
        }

        private Component parseComponent()
        {
            Component c;
            GingerToken type;
            Variable name;
            FunctionList fl;
            ImportList il;

            if (Grammar.isComponent(currentScannerToken))
            {
                type = currentScannerToken;
                nextScannerToken();

                if (currentScannerToken == GingerToken.Identifier)
                {
                    //name = new Variable(new Identifier(scanner.row, scanner.col, new string(scanner.tokenValue)));
                    name = new Variable(parseIdentifier(false));
                    nextScannerToken();

                    if (currentScannerToken == GingerToken.OpenList)
                    {
                        nextScannerToken();
                        il = parseImportList();
                        if (currentScannerToken == GingerToken.Function)
                        {
                            fl = parseFunctionList();
                        }
                        else
                        {
                            fl = new FunctionList();
                        }

                        c = new Component(type, name, il, fl);
                    }
                    else
                    {
                        throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.OpenList.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
                    }
                }
                else
                {
                    throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.Identifier.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
                }
            }
            else
            {
                throw new ParseException(scanner.row, scanner.col, $"Expected component type, found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
            }

            return c;
        }

        private NodeCollection parseStatement()
        {
            NodeCollection nc;
            // statement = "if", expression, statement-list
            //if (Grammar.isControl(currentScannerToken))
            //{
            //    StatementList sl;
            //    GingerToken controlToken = currentScannerToken;

            //    nextScannerToken();

            //    Node conditionExpression = parseExpression();

            //    nextScannerToken();
            //    if (currentScannerToken == GingerToken.OpenStatementList)
            //    {
            //        nextScannerToken();
            //        sl = parseStatementList(GingerToken.CloseStatementList);
            //    }
            //    else
            //    {
            //        throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.OpenStatementList.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
            //    }

            //    nc = new If(conditionExpression, sl);
            //}
            // statement = variable
            if (Grammar.isType(currentScannerToken))
            {
                nc = parseVariableDeclaration();
            }
            // statement = identifier, assignment, expression
            else if (currentScannerToken == GingerToken.Identifier)
            {
                //Identifier identifier = new Identifier(scanner.row, scanner.col, new string(scanner.tokenValue));
                Identifier identifier = parseIdentifier(true);
                nextScannerToken();
                if (currentScannerToken == GingerToken.Assignment)
                {
                    nextScannerToken();
                    nc = new Assign(identifier, parseExpression());
                }
                else if (currentScannerToken == GingerToken.OpenPrecedent)
                {
                    ExpressionList exprList = parseExpressionList();

                    // close-precedent
                    if (currentScannerToken == GingerToken.ClosePrecedent)
                    {
                        nc = new Invocation(identifier, exprList);
                    }
                    else
                    {
                        throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.ClosePrecedent.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
                    }
                }
                else
                {
                    throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.Assignment.ToString()} or {GingerToken.OpenPrecedent.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
                }
            }
            // statement = return [expression]
            else if (currentScannerToken == GingerToken.Return)
            {
                Node expression = null;
                spyScannerToken();
                if (spiedScannerToken.GetValueOrDefault() != GingerToken.CloseList)
                {
                    nextScannerToken();
                    expression = parseExpression();
                }

                nc = new Return(expression);
            }
            // component = (cont | imp), identifdier, {, function-decs ,}
            //else if (Grammar.isComponent(currentScannerToken))
            //{
            //    Component comp;
            //    Variable componentName;

            //    GingerToken componentType = currentScannerToken;
            //    nextScannerToken();

            //    if (currentScannerToken == GingerToken.Identifier)
            //    {
            //        componentName = new Variable(new Identifier(scanner.row, scanner.col, new string(scanner.tokenValue)));

            //        nextScannerToken();
            //        if (currentScannerToken == GingerToken.OpenList)
            //        {

            //        }
            //        else
            //        {
            //            throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.Identifier.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
            //        }
            //    }
            //    else
            //    {
            //        throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.Identifier.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
            //    }

            //}
            // statement = function-declaration = function, type, identifier, open-precedent, variable-list, close-precedent, statement-list
            //else if (currentScannerToken == GingerToken.Function)
            //{
            //    if (inFunction)
            //    {
            //        throw new ParseException(scanner.row, scanner.col, "Cannot declare function in function", ExceptionLevel.ERROR);
            //    }

            //    Variable functionName;
            //    VarList formalArgsList = new VarList();
            //    //Type functionReturnType;
            //    StatementList sl;

            //    // returns
            //    //nextScannerToken();
            //    //if (Grammar.isFunctionType(currentScannerToken))
            //    //{
            //    //    functionReturnType = parseType();
            //    //}
            //    //else
            //    //{
            //    //    throw new ParseException(scanner.row, scanner.col, $"Expected type, found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
            //    //}

            //    // identifier
            //    nextScannerToken();
            //    if (currentScannerToken == GingerToken.Identifier)
            //    {
            //        functionName = new Variable(new Identifier(scanner.row, scanner.col, new string(scanner.tokenValue)));

            //        // open-precedent
            //        nextScannerToken();
            //        if (currentScannerToken == GingerToken.OpenPrecedent)
            //        {
            //            // variable-list
            //            formalArgsList = parseVarList();

            //            // close-precedent
            //            if (currentScannerToken == GingerToken.ClosePrecedent)
            //            {
            //                nextScannerToken();
            //            }
            //            else
            //            {
            //                throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.ClosePrecedent.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
            //            }

            //            // statement list
            //            if (currentScannerToken == GingerToken.OpenList)
            //            {
            //                nextScannerToken();
            //                sl = parseStatementList(GingerToken.CloseList, true);
            //                //nc = new Function(functionName, functionVariableList, functionReturnType, sl);
            //                nc = new Function(functionName, formalArgsList, sl);
            //            }
            //            else
            //            {
            //                throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.OpenList.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
            //            }
            //        }
            //        else
            //        {
            //            throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.OpenPrecedent.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
            //        }
            //    }
            //    else
            //    {
            //        throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.Identifier.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
            //    }
            //}
            else if (currentScannerToken == GingerToken.Sink)
            {
                nextScannerToken();
                if (currentScannerToken == GingerToken.OpenPrecedent)
                {
                    Node expr;

                    nextScannerToken();
                    expr = parseExpression();
                    nextScannerToken();
                    if (currentScannerToken == GingerToken.ClosePrecedent)
                    {
                        if (currentScannerToken == GingerToken.ClosePrecedent)
                        {
                            nc = new Sink(parseAnnotation(GingerToken.Low), expr);
                        }
                        else
                        {
                            throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.ClosePrecedent.ToString()} or {GingerToken.Sink.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
                        }
                    }
                    else
                    {
                        throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.ClosePrecedent.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
                    }
                }
                else
                {
                    throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.OpenList.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
                }
            }
            // statement = component = comp, identifier, as, component-type, statement-list
            //else if (currentScannerToken == GingerToken.Component)
            //{
            //    Identifier compName;
            //    Type compType;
            //    StatementList sl;

            //    // identifier
            //    nextScannerToken();
            //    if (currentScannerToken == GingerToken.Identifier)
            //    {
            //        compName = new Identifier(scanner.row, scanner.col, new string(scanner.tokenValue));
            //    }
            //    else
            //    {
            //        throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.Identifier.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
            //    }

            //    // component type
            //    nextScannerToken();
            //    if (currentScannerToken == GingerToken.As)
            //    {
            //        nextScannerToken();
            //        if (Grammar.isComponentType(currentScannerToken))
            //        {
            //            compType = parseType();
            //        }
            //        else
            //        {
            //            throw new ParseException(scanner.row, scanner.col, $"Expected component type, found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
            //        }
            //    }
            //    else
            //    {
            //        throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.As.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
            //    }

            //    // statement list
            //    nextScannerToken();
            //    if (currentScannerToken == GingerToken.OpenStatementList)
            //    {
            //        nextScannerToken();
            //        sl = parseStatementList(GingerToken.CloseStatementList);
            //        nc = new Component(compName, compType, sl);
            //    }
            //    else
            //    {
            //        throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.OpenStatementList.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
            //    }
            //}
            else
            {
                //throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.If.ToString()}', '{GingerToken.Int.ToString()}', or '{GingerToken.Identifier.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
                spyScannerToken();
                throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.Identifier.ToString()}', found '{currentScannerToken.ToString()}' near {spiedScannerToken.ToString()}", ExceptionLevel.ERROR);
            }

            return nc;
        }

        //private Type parseType()
        //{
        //    Type type;
        //    if (currentScannerToken == GingerToken.Int)
        //    {
        //        type = new Integer(scanner.row, scanner.col);
        //    }
        //    else if (currentScannerToken == GingerToken.Bool)
        //    {
        //        type = new Boolean(scanner.row, scanner.col);
        //    }
        //    else if (currentScannerToken == GingerToken.Void)
        //    {
        //        type = new Void(scanner.row, scanner.col);
        //    }
        //    else if (currentScannerToken == GingerToken.Contract)
        //    {
        //        type = new Contract(scanner.row, scanner.col);
        //    }
        //    else if (currentScannerToken == GingerToken.Implementation)
        //    {
        //        type = new Implementation(scanner.row, scanner.col);
        //    }
        //    else
        //    {
        //        throw new NotImplementedException();
        //    }

        //    return type;
        //}

        private GingerToken parseAnnotation(GingerToken defaultAnnotation)
        {
            spyScannerToken();
            if (spiedScannerToken == GingerToken.OpenAnnotation)
            {
                nextScannerToken();
                nextScannerToken();
                if (currentScannerToken == GingerToken.Annotation)
                {
                    nextScannerToken();
                    return currentScannerToken;
                }
                else
                {
                    throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.Annotation.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
                }
            }
            else
            {
                return defaultAnnotation;
            }
        }

        private VarList parseVarList()
        {
            VarList formArgsList = new VarList();

            do
            {
                nextScannerToken();
                if (currentScannerToken != GingerToken.ClosePrecedent)
                {
                    //if (Grammar.isType(currentScannerToken) || currentScannerToken == GingerToken.Ref)
                    if (Grammar.isType(currentScannerToken))
                    {
                        Variable v = (Variable)parseVariableDeclaration();
                        formArgsList.add(v);
                        nextScannerToken();
                    }
                    else
                    {
                        throw new ParseException(scanner.row, scanner.col, $"Expected identifier', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);

                    }
                }
            } while (currentScannerToken == GingerToken.ListSeparator);

            return formArgsList;
        }

        private ExpressionList parseExpressionList()
        {
            ExpressionList actArgsList = new ExpressionList();

            do
            {
                nextScannerToken();
                if (currentScannerToken != GingerToken.ClosePrecedent)
                {
                    //    if (Grammar.isType(currentScannerToken))
                    //    {
                    Node e = parseExpression();
                    actArgsList.add(e);
                    nextScannerToken();
                    //}
                    //else
                    //{
                    //    throw new ParseException(scanner.row, scanner.col, $"Expected type', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);

                    //}
                }
            } while (currentScannerToken == GingerToken.ListSeparator);

            return actArgsList;
        }

        private NodeCollection parseVariableDeclaration()
        {
            //Type type = parseType();
            //bool isReference = false;
            //if (currentScannerToken == GingerToken.Ref)
            //{
            //    isReference = true;
            //    // before moving, assume on .Ref token
            //    nextScannerToken();
            //}

            // before moving, assume on .Var token
            nextScannerToken();
            if (currentScannerToken != GingerToken.Identifier)
            {
                throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.Identifier.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
            }
            else
            {
                //return new Variable(new Identifier(scanner.row, scanner.col, new string(scanner.tokenValue)), isReference);
                //return new Variable(new Identifier(scanner.row, scanner.col, new string(scanner.tokenValue)));
                return new Variable(parseIdentifier(false));
            }
        }

        private Node parseExpression()
        {
            return parseExpression(parseTerminalExpression(), MIN_PRECEDENCE);
        }

        private Node parseTerminalExpression()
        {
            Node n;
            //if (currentScannerToken == GingerToken.Identifier || currentScannerToken == GingerToken.Number || currentScannerToken == GingerToken.BooleanLiteral)
            if (currentScannerToken == GingerToken.Identifier)
            {
                //Identifier ident = new Identifier(scanner.row, scanner.col, new string(scanner.tokenValue));
                Identifier ident = parseIdentifier(true);

                // let's see it this is a function call or a variable
                spyScannerToken();
                if (spiedScannerToken.GetValueOrDefault() == GingerToken.OpenPrecedent)
                {
                    // it's a function
                    // variable-list
                    nextScannerToken();
                    ExpressionList exprList = parseExpressionList();

                    // close-precedent
                    if (currentScannerToken == GingerToken.ClosePrecedent)
                    {
                        n = new Invocation(ident, exprList);
                    }
                    else
                    {
                        throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.ClosePrecedent.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
                    }
                }
                else
                {
                    n = ident;
                }
            }
            else if (currentScannerToken == GingerToken.Number)
            {
                //if (currentScannerToken == GingerToken.Number)
                //{
                //    n = new Literal<Integer>(new Integer(scanner.row, scanner.col, new string(scanner.tokenValue)));
                //}
                //else
                //{
                //    n = new Literal<Boolean>(new Boolean(scanner.row, scanner.col, new string(scanner.tokenValue)));
                //}
                n = new Literal<Integer>(new Integer(scanner.row, scanner.col, new string(scanner.tokenValue)));
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
            else if (currentScannerToken == GingerToken.Source)
            {
                nextScannerToken();
                if (currentScannerToken == GingerToken.OpenPrecedent)
                {
                    nextScannerToken();
                    if (currentScannerToken == GingerToken.ClosePrecedent)
                    {
                        n = new Source(parseAnnotation(GingerToken.High));
                    }
                    else
                    {
                        throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.ClosePrecedent.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
                    }
                }
                else
                {
                    throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.OpenPrecedent.ToString()}', found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
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
                //else if (Grammar.isConditionOperator(op))
                //{
                //    lhs = new InequalityOperation(op, lhs, rhs);
                //}
                else
                {
                    throw new ParseException(scanner.row, scanner.col, $"Expected '{GingerToken.Addition.ToString()}',  found '{currentScannerToken.ToString()}'", ExceptionLevel.ERROR);
                }
            }

            return lhs;
        }

        private bool isOperator(GingerToken token)
        {
            return Grammar.isBinaryOperator(token);
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
