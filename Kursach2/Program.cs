using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Kursach2
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            string login = ReadCurrentUser();
            if(login != null && login != "")
            {
                LoginForm.CurrentUser = new utils.User() { Login = login };
                Application.Run(new SelectModeForm());
            }
            else
            {
                Application.Run(new LoginForm());
            }
        }
        private static string ReadCurrentUser()
        {
            string result = "";
            using (StreamReader reader = new StreamReader("user.txt"))
            {
                result = reader.ReadLine();
                reader.Close();
            }
            return result;
        }
    }
}
