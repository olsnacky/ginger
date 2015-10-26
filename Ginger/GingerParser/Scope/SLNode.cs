namespace GingerParser
{
    public partial class StatementList
    {
        private Scope.Scope _scope;

        public Scope.Scope scope
        {
            get { return _scope; }
            set { _scope = value; }
        }
    }

    public partial class Identifier
    {
        private Declaration _declaration;

        // the original declaration for this identifier
        public Declaration declaration
        {
            get { return _declaration; }
            set { _declaration = value; }
        }
    }
}
