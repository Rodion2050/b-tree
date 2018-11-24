using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace Kursach2
{
    class TreeControl : Control
    {
        private B_Tree<ComparableInt> bTree;

        private Pen rectanglePen;
        private Font stringFont = new Font("Consolas", 14, new FontStyle(), GraphicsUnit.Pixel);
        private Brush stringBrush = new SolidBrush(Color.Black);
        private Brush foundedElementBrush = new SolidBrush(Color.Red);
        private Pen foundedElementPen;

        private ComparableInt foundedKey = null;
        private B_Tree_Node<ComparableInt> foundedNode = null;
        private int oneKeyWidth = 35;
        private int delimiterSize = 1;
        private int oneNodeHeight = 20;
        public TreeControl(B_Tree<ComparableInt> b_Tree)
        {
            Brush brush = new SolidBrush(Color.Black);
            rectanglePen = new Pen(brush);
            foundedElementPen =  new Pen(foundedElementBrush);
            //foundedElementPen.
            Size = new Size(100, 100);
            this.bTree = b_Tree;
        }
       
        public void updateTree(B_Tree<ComparableInt> b_Tree)
        {
            bTree = b_Tree;
            Invalidate();
            foundedKey = null;
            foundedNode = null;
        }

        public void updateTreeWithFoundedElement(B_Tree<ComparableInt> b_Tree, ComparableInt key, B_Tree_Node<ComparableInt> inNode)
        {
            foundedKey = key;
            foundedNode = inNode;
            bTree = b_Tree;
            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            DrawTree_r(bTree.Root, 0, 0, e);

        }





        class NodeSize
        {
            public int ChildrenSize;
            public int currentNodeSize;
            public int currentNodeXBegin;
        }


        private NodeSize DrawTree_r(B_Tree_Node<ComparableInt> node, int x_beg, int y_beg, PaintEventArgs e)
        {
            int x = x_beg, y = y_beg + oneNodeHeight + 20;
            int size = 0;
            List<NodeSize> sizes = new List<NodeSize>();
            foreach (var i in node.Pointers)
            {
                NodeSize nodeSize = DrawTree_r(i, x, y, e);
                size = nodeSize.ChildrenSize;
                sizes.Add(nodeSize);
                x += size + 20;
            }
            if(x == x_beg)//Is leaf
            {
                size = DrawNode(node, x_beg, y_beg, e);
                return new NodeSize() { ChildrenSize = size, currentNodeSize = size, currentNodeXBegin = x_beg };
            }
            if (x > Size.Width)
            {
                Size = new Size(x + 2*oneKeyWidth, Size.Height);
            }
            if (y > Size.Height)
            {
                Size = new Size(Size.Width, 2*y);
            }
            int element_begin = (x_beg + x - size - 20) / 2;

            int element_size = DrawNode(node, element_begin, y_beg, e);
            for(int pointerIndex = 0; pointerIndex < sizes.Count; pointerIndex++)
            {
                e.Graphics.DrawLine(rectanglePen, element_begin + element_size/(sizes.Count-1)*pointerIndex, y - 20, sizes[pointerIndex].currentNodeXBegin +  sizes[pointerIndex].currentNodeSize / 2, y);

            }
            return new NodeSize() { ChildrenSize = x - x_beg, currentNodeSize = element_size, currentNodeXBegin = element_begin };
        }

        private int DrawNode(B_Tree_Node<ComparableInt> node, int x_beg, int y_beg, PaintEventArgs e)
        {
            int nodeSize = node.Keys.Count * oneKeyWidth + (node.Keys.Count - 1) * delimiterSize;
            e.Graphics.DrawRectangle(rectanglePen, x_beg, y_beg, nodeSize, oneNodeHeight);
            int currX = x_beg + delimiterSize, currY = y_beg + delimiterSize;
            foreach(var i in node.Keys)
            {
                if(foundedKey != null && i.Value == foundedKey.Value)
                {
                    e.Graphics.FillRectangle(foundedElementBrush, currX, currY, oneKeyWidth, oneNodeHeight);
                }
                e.Graphics.DrawString(i.ToString(), stringFont, stringBrush, currX, currY);
                currX += oneKeyWidth;
                e.Graphics.DrawLine(rectanglePen, currX, currY, currX,currY+oneNodeHeight);
                currX += delimiterSize;
            }
            return nodeSize;
        }
    }
}
