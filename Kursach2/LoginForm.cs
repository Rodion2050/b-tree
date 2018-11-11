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
using Kursach2.utils;
namespace Kursach2
{
    public partial class LoginForm : Form
    {
        public static User CurrentUser;
        private AppDbContext usersDb = new AppDbContext();

        public LoginForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var login = loginUsernameTextBox.Text;
            var password = loginPasswordTextBox.Text;

            var result = from user in usersDb.Users
                         where user.Login == login && user.Password == password
                         select user;

            var list = result.ToList();
            if(list.Count == 0)
            {
                MessageBox.Show("Неверный логин или пароль");
            }
            else
            {
                //Hide();
                CurrentUser = new User() { Login = login };
                SaveCurrentUser(login);
                new SelectModeForm().Show();
                //Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var login = signUpLoginTextBox.Text;
            if(signUpPasswordTextBox1.Text != signUpPasswordTextBox2.Text)
            {
                MessageBox.Show("Пароли не совпадают");
                return;
            }
            var password = signUpPasswordTextBox1.Text;
            var result = from user in usersDb.Users
                         where user.Login == login
                         select user;
            if(result.ToList().Count != 0)
            {
                MessageBox.Show("Логин " + login + " уже занят");
                return;
            }
            usersDb.Users.Add(new User() { Login = login, Password = password, RecordAnswersCount = 0 });
            usersDb.SaveChanges();


            MessageBox.Show("Пользователь " + login + " успешно добавлен");
            CurrentUser = new User() { Login = login };
            SaveCurrentUser(login);
            //Hide();
            new SelectModeForm().Show();
        }

        public static async void SaveCurrentUser(string login)
        {
            using (StreamWriter writer = new StreamWriter("user.txt", false))
            {
                await writer.WriteAsync(login);
                await writer.FlushAsync();
                writer.Close();
            }
        }
    }
}
