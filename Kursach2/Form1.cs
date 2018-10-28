using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Kursach2
{
    public partial class Form1 : Form
    {
        private TreeControl treeControl;
        private B_Tree<ParcebleInt> b_Tree = new B_Tree<ParcebleInt>(2);
        public Form1()
        {

  
            this.treeControl = new TreeControl(b_Tree);
            this.treeControl.Location = new System.Drawing.Point(20, 100);
            this.AutoScroll = true;
            this.Controls.Add(treeControl);
            InitializeComponent();
           
            
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            b_Tree.Insert(new ParcebleInt(Convert.ToInt32(textBox1.Text)));
            treeControl.updateTree(b_Tree);
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
                b_Tree.Remove(new ParcebleInt(Convert.ToInt32(textBox1.Text)));
                treeControl.updateTree(b_Tree);
            
        }
    }
}
