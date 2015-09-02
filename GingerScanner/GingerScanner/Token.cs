using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ginger;

namespace GingerScanner
{
    public abstract class Token
    {
        protected char[] _chars;

        public Token(char[] chars)
        {
            this._chars = chars;
            validate();
        }

        public Token(char c)
        {
            this._chars = new char[] { c };
            validate();
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            return _chars.SequenceEqual(((Token)obj)._chars);
        }

        public override int GetHashCode()
        {
            return _chars.GetHashCode();
        }

        public static Token toToken(char[] chars)
        {
            if (chars.Length > 1)
            {
                if (Lexicon.isIf(chars))
                {
                    return new If();
                }
                else if (Lexicon.isWhile(chars))
                {
                    return new While();
                }
                else if (Lexicon.isInteger(chars))
                {
                    return new Integer(chars);
                }
            }
            else if (chars.Length == 1)
            {
                char c = chars[0];
                if (Lexicon.isOpenPrecedent(c))
                {
                    return new OpenPrecedent();
                }
                else if (Lexicon.isClosePrecedent(c))
                {
                    return new ClosePrecedent();
                }
                else if (Lexicon.isEquality(c))
                {
                    return new Equality(c);
                }
                else if (Lexicon.isOperator(c))
                {
                    return new Operator(c);
                }
            }

            throw new TokenParseException();
        }

        protected abstract void validate();
    }

    public class Identifier : Token
    {
        public Identifier(char[] name) : base(name)
        {

        }

        protected override void validate()
        {
            if (!Lexicon.isIdentifier(_chars))
            {
                throw new TokenParseException();
            }
        }
    }

    public class Integer : Token
    {
        public int value
        {
            get { return Convert.ToInt16(_chars); }
        }

        public Integer(char[] value) : base(value)
        {

        }

        protected override void validate()
        {
            if (!Lexicon.isInteger(_chars))
            {
                throw new TokenParseException();
            }
        }
    }

    public class If : Token
    {
        public If() : base(Lexicon.IF)
        {

        }

        protected override void validate()
        {
            if (!Lexicon.isIf(_chars))
            {
                throw new TokenParseException();
            }
        }
    }

    public class While : Token
    {
        public While() : base(Lexicon.WHILE)
        {

        }

        protected override void validate()
        {
            if (!Lexicon.isWhile(_chars))
            {
                throw new TokenParseException();
            }
        }
    }

    public class OpenPrecedent : Token
    {
        public OpenPrecedent() : base(Lexicon.OPEN_PRECEDENT)
        {

        }

        protected override void validate()
        {
            if (_chars.Length == 1)
            {
                if (!Lexicon.isOpenPrecedent(_chars[0]))
                {
                    throw new TokenParseException();
                }
            } else
            {
                throw new TokenParseException();
            }
        }
    }

    public class ClosePrecedent : Token
    {
        public ClosePrecedent() : base(Lexicon.CLOSE_PRECEDENT)
        {

        }

        protected override void validate()
        {
            if (_chars.Length == 1)
            {
                if (!Lexicon.isClosePrecedent(_chars[0]))
                {
                    throw new TokenParseException();
                }
            }
            else
            {
                throw new TokenParseException();
            }
        }
    }

    public class Equality : Token
    {
        public Equality(char[] equality) :  base(equality)
        {

        }

        public Equality(char equality) : base(equality)
        {

        }

        protected override void validate()
        {
            if (!Lexicon.isEquality(_chars))
            {
                throw new TokenParseException();
            }
        }
    }

    public class Operator : Token
    {
        public Operator(char op) : base(op)
        {

        }

        protected override void validate()
        {
            if (_chars.Length == 1)
            {
                if (!Lexicon.isOperator(_chars[0]))
                {
                    throw new TokenParseException();
                }
            }
            else
            {
                throw new TokenParseException();
            }
        }
    }

    public class Boolean : Token
    {
        public Boolean(char[] boolean) : base(boolean) {

        }

        protected override void validate()
        {
            if (!Lexicon.isBoolean(_chars))
            {
                throw new TokenParseException();
            }
        }
    }

    public class Assignment : Token
    {
        public Assignment() : base(Lexicon.ASSIGNMENT)
        {

        }

        protected override void validate()
        {
            if (_chars.Length == 1)
            {
                if (!Lexicon.isAssignment(_chars[0]))
                {
                    throw new TokenParseException();
                }
            }
            else
            {
                throw new TokenParseException();
            }
        }
    }

    public class TokenParseException : Exception
    {
        public TokenParseException() : base()
        {

        }
    }
}
