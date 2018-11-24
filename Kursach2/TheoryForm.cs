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
    public partial class TheoryForm : Form
    {
        public TheoryForm()
        {
            InitializeComponent();
            string curDir = Directory.GetCurrentDirectory();
            this.webBrowser1.Url = new Uri(String.Format("file:///{0}/all.htm", curDir));;
        }
    }
}
