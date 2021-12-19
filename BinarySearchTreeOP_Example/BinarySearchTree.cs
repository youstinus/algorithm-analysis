using System;

namespace BinarySearchTreeOP_Example
{
    class TreeNode
    {
        public string Element { get; set; }
        public TreeNode Left { get; set; }
        public TreeNode Right { get; set; }
        public int ElementNum { get; set; }
        public TreeNode(string element, int num)
        {
            this.Element = element;
            this.ElementNum = num;
        }
    }
    class BinarySearchTree
    {
        public TreeNode Root { get; set; }
        int count;
        public BinarySearchTree()
        {
            this.Root = null;
            count = 0;
        }
        public void Insert(string x)
        {
            this.Root = Insert(x, this.Root);
        }
        public bool Contains(string x)
        {
            return Contains(x, this.Root);
        }
        public void Print()
        {
            Print(this.Root);
        }
        private bool Contains(string x, TreeNode t)
        {
            while (t != null)
            {
                if ((x as IComparable).CompareTo(t.Element) < 0)
                {
                    t = t.Left;
                }
                else if ((x as IComparable).CompareTo(t.Element) > 0)
                {
                    t = t.Right;
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
        protected TreeNode Insert(string x, TreeNode t)
        {
            if (t == null)
            {
                t = new TreeNode(x, count++);
            }
            else if ((x as IComparable).CompareTo(t.Element) < 0)
            {
                t.Left = Insert(x, t.Left);
            }
            else if ((x as IComparable).CompareTo(t.Element) > 0)
            {
                t.Right = Insert(x, t.Right);
            }
            else
            {
                // throw new Exception("Duplicate item");
            }
            return t;
        }
        private void Print(TreeNode t)
        {
            if (t == null)
            {
                return;
            }
            else
            {
                Print(t.Left);
                if (t.Left != null) Console.Write("{0,3:N0} <<- ",
                    t.Left.ElementNum);
                else Console.Write(" ");
                Console.Write("{0,3:N0} {1} ", t.ElementNum, t.Element);
                if (t.Right != null) Console.WriteLine(" ->> {0,3:N0}",
                    t.Right.ElementNum);
                else Console.WriteLine(" ");
                Print(t.Right);
            }
        }
    }
}
