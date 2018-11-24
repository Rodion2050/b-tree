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
    public partial class TreeForm : Form
    {
        private TreeControl treeControl;
        private B_Tree<ComparableInt> b_Tree = new B_Tree<ComparableInt>(2, ComparableInt.FromStr);
        public TreeForm()
        {
            this.treeControl = new TreeControl(b_Tree);
            this.treeControl.Location = new System.Drawing.Point(40, 200);
           
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int t = Convert.ToInt32(textBox2.Text);
                if(t <= 1)
                {
                    throw new FormatException();
                }
                B_Tree<ComparableInt> newTree = new B_Tree<ComparableInt>(t, ComparableInt.FromStr);
                var list = b_Tree.ToList();
                foreach(var i in list)
                {
                    newTree.Insert(i);
                }
                b_Tree = newTree;
                treeControl.updateTree(b_Tree);
            }catch(FormatException ex)
            {
                MessageBox.Show("Уровень ветвистости задается целым числом > 1");
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int k = Convert.ToInt32(textBox1.Text);
                var res = b_Tree.Search(new ComparableInt(k));
                if(res == null)
                {
                    MessageBox.Show(k + " не найдено");
                    treeControl.updateTree(b_Tree);
                    return;
                }
                treeControl.updateTreeWithFoundedElement(b_Tree, new ComparableInt(k), res.node);
            }catch(FormatException ex)
            {
                MessageBox.Show("Неверный формат");

            }
        }
    }
}
