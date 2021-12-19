using System;
using System.IO;
using System.Text;

namespace BinarySearchTreeD_Example
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
        int _count;
        public BinarySearchTree()
        {
            this.Root = null;
            _count = 0;
        }
        public void Insert(string x)
        {
            this.Root = Insert(x, this.Root);
        }
        public bool Contains(string x)
        {
            return Contains(x, this.Root);
        }
        public bool FileContains(FileStream fs, string x)
        {
            return FileContains(fs, x, 0, this.Root.Element.Length);
        }
        public void Print()
        {
            Print(this.Root);
        }
        public void WriteToFile(string filename, int n)
        {
            var bufTree = new byte[n][];
            BuildBufTree(bufTree, this.Root, this.Root.Element.Length);
            if (File.Exists(filename)) File.Delete(filename);
            try
            {
                using (var writer = new BinaryWriter(File.Open(filename,
                    FileMode.Create)))
                {
                    foreach (var item in bufTree)
                    {
                        for (var j = 0; j < item.Length; j++)
                            writer.Write(item[j]);
                    }
                }
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        private void BuildBufTree(byte[][] bufTree, TreeNode t, int k)
        {
            var mn = -1;
            if (t == null)
            {
                return;
            }
            else
            {
                BuildBufTree(bufTree, t.Left, k);
                var i = t.ElementNum;
                bufTree[i] = new byte[k + 8];
                if (t.Left != null)
                    BitConverter.GetBytes(t.Left.ElementNum).CopyTo(bufTree[i],
                        0);
                else
                    BitConverter.GetBytes(mn).CopyTo(bufTree[i], 0);
                Encoding.ASCII.GetBytes(t.Element).CopyTo(bufTree[i], 4);
                if (t.Right != null)
                    BitConverter.GetBytes(t.Right.ElementNum).CopyTo(bufTree[i],
                        k + 4);
                else
                    BitConverter.GetBytes(mn).CopyTo(bufTree[i], k + 4);
                BuildBufTree(bufTree, t.Right, k);
            }
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
        private bool FileContains(FileStream fs, string x, int t, int k)
        {
            var ak = k + 8;
            var data = new byte[ak];
            while (t >= 0)
            {
                fs.Seek(t * ak, SeekOrigin.Begin);
                fs.Read(data, 0, ak);
                var tLeft = BitConverter.ToInt32(data, 0);
                var element = Encoding.ASCII.GetString(data, 4, k);
                var tRight = BitConverter.ToInt32(data, k + 4);
                if ((x as IComparable).CompareTo(element) < 0)
                {
                    t = tLeft;
                }
                else if ((x as IComparable).CompareTo(element) > 0)
                {
                    t = tRight;
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
                t = new TreeNode(x, _count++);
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
