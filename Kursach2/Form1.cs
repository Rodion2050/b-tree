using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Kursach2
{
    public partial class Form1 : Form
    {
        private TreeControl treeControl;
        private B_Tree<ComparableInt> b_Tree = new B_Tree<ComparableInt>(2, ComparableInt.FromStr);
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
            b_Tree.Insert(new ComparableInt(Convert.ToInt32(textBox1.Text)));
            treeControl.updateTree(b_Tree);
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
                b_Tree.Remove(new ComparableInt(Convert.ToInt32(textBox1.Text)));
                treeControl.updateTree(b_Tree);
            
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            //FolderBrowserDialog dialog = new FolderBrowserDialog();
            //if(dialog.ShowDialog() == DialogResult.OK)
            //{
            string path = "tree";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            b_Tree.Save(path);
            //}
        }

        private void readButton_Click(object sender, EventArgs e)
        {
            string path = "tree";
            if (!Directory.Exists(path))
            {
                
            }
            b_Tree.ReadFromDir(path);
            treeControl.updateTree(b_Tree);
        }
    }
}
