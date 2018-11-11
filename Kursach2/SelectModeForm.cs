using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kursach2.utils;

namespace Kursach2
{
    public partial class SelectModeForm : Form
    {
        private AppDbContext usersDb = new AppDbContext();
        public SelectModeForm()
        {
            InitializeComponent();
            ShowRecordsTable();
            MessageBox.Show("Вы зашли как " + LoginForm.CurrentUser.Login);
            if(LoginForm.CurrentUser.Login == "admin")
            {
                button4.Visible = true;
            }
        }

        private void ShowRecordsTable()
        {
            var users = from p in usersDb.Users
                    select p;

            dataGridView1.Columns.Add("1", "Логин");
            
            dataGridView1.Columns.Add("1", "Рекорд");
            dataGridView1.Columns[0].Width = 190;
            dataGridView1.Columns[1].Width = 190;
            foreach (var u in users)
            {
                dataGridView1.Rows.Add(new string[] { u.Login, u.RecordAnswersCount.ToString() });
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            new Form1().Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            new TheoryForm().Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            new TestForm().Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var users = from p in usersDb.Users
                        where p.Login != "admin"
                        select p;
            foreach(var i in users)
            {
                usersDb.Users.Remove(i);
            }
            usersDb.SaveChanges();
            var u = from p in usersDb.Users
                    select p;
            dataGridView1.DataSource = u.ToList();
            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            LoginForm.SaveCurrentUser("");
            Close();
        }
    }
}
