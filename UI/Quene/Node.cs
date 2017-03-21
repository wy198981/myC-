using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace UI.Quene
{
    public class Node<T>
    {
        private ModelNode data;

        private Node<ModelNode> next;

        public Node(ModelNode data, Node<ModelNode> next)
        {
            this.data = data;
            this.next = next;
        }

        public Node(Node<ModelNode> next)
        {
            this.next = next;
            this.data = default(ModelNode);

        }

        public Node(ModelNode data)
        {
            this.data = data;
            this.next = null;
        }

        public Node()
        {
            this.data = default(ModelNode);
            this.next = null;
        }

        public ModelNode Data
        {
            get { return this.data; }
            set { this.data = value; }
        }

        public Node<ModelNode> Next
        {
            get { return next; }
            set { next = value; }
        }
    }
}
