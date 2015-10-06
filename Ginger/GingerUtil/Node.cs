using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GingerUtil
{
    public abstract class Node
    {
        private List<Node> _parents;

        public List<Node> parents
        {
            get { return _parents; }
        }

        public Node()
        {
            this._parents = new List<Node>();
        }

        public virtual void add(Node n)
        {
            throw new NotImplementedException();
        }

        public virtual void remove(Node n)
        {
            throw new NotImplementedException();
        }

        public virtual Node get(int index)
        {
            throw new NotImplementedException();
        }

        public abstract void accept(NodeVisitor v);
    }

    public abstract class NodeCollection : Node, IEnumerable<Node>
    {
        private List<Node> _children;

        public List<Node> children
        {
            get { return _children; }
        }

        public NodeCollection()
        {
            _children = new List<Node>();
        }

        public override void add(Node n)
        {
            n.parents.Add(this);
            _children.Add(n);
        }

        public override void remove(Node n)
        {
            _children.Remove(n);
        }

        public override Node get(int index)
        {
            return _children[index];
        }

        public IEnumerator<Node> GetEnumerator()
        {
            return _children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
