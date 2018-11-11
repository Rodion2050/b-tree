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
    public partial class TestForm : Form
    {
        private AppDbContext AppDb = new AppDbContext();
        private List<Test> tests = new List<Test>();
        private RadioButton[][] answersRadioButtons;
        private RichTextBox[] askTextBoxes;
        public TestForm()
        {
            InitializeComponent();

            //AppDb.Tests.Add(new Test() { Ask = "Что такое Б-дерево", Answer1 = "1", Answer2 = "2", Answer3 = "3", Answer4 = "4", CorrectAnswerIndex = 1 });
            //AppDb.Tests.Add(new Test() { Ask = "1", Answer1 = "1", Answer2 = "2", Answer3 = "3", Answer4 = "4", CorrectAnswerIndex = 1 });
            //AppDb.Tests.Add(new Test() { Ask = "2", Answer1 = "1", Answer2 = "2", Answer3 = "3", Answer4 = "4", CorrectAnswerIndex = 1 });
            //AppDb.Tests.Add(new Test() { Ask = "3", Answer1 = "1", Answer2 = "2", Answer3 = "3", Answer4 = "4", CorrectAnswerIndex = 1 });
            //AppDb.Tests.Add(new Test() { Ask = "4", Answer1 = "1", Answer2 = "2", Answer3 = "3", Answer4 = "4", CorrectAnswerIndex = 1 });
            //AppDb.Tests.Add(new Test() { Ask = "5", Answer1 = "1", Answer2 = "2", Answer3 = "3", Answer4 = "4", CorrectAnswerIndex = 1 });
            //AppDb.Tests.Add(new Test() { Ask = "6", Answer1 = "1", Answer2 = "2", Answer3 = "3", Answer4 = "4", CorrectAnswerIndex = 1 });
            //AppDb.Tests.Add(new Test() { Ask = "7", Answer1 = "1", Answer2 = "2", Answer3 = "3", Answer4 = "4", CorrectAnswerIndex = 1 });
            //AppDb.Tests.Add(new Test() { Ask = "8", Answer1 = "1", Answer2 = "2", Answer3 = "3", Answer4 = "4", CorrectAnswerIndex = 1 });
            //AppDb.Tests.Add(new Test() { Ask = "9", Answer1 = "1", Answer2 = "2", Answer3 = "3", Answer4 = "4", CorrectAnswerIndex = 1 });
            //AppDb.Tests.Add(new Test() { Ask = "10", Answer1 = "1", Answer2 = "2", Answer3 = "3", Answer4 = "4", CorrectAnswerIndex = 1 });
            //AppDb.Tests.Add(new Test() { Ask = "11", Answer1 = "1", Answer2 = "2", Answer3 = "3", Answer4 = "4", CorrectAnswerIndex = 1 });
            //AppDb.Tests.Add(new Test() { Ask = "12", Answer1 = "1", Answer2 = "2", Answer3 = "3", Answer4 = "4", CorrectAnswerIndex = 1 });
            //AppDb.Tests.Add(new Test() { Ask = "13", Answer1 = "1", Answer2 = "2", Answer3 = "3", Answer4 = "4", CorrectAnswerIndex = 1 });
            //AppDb.Tests.Add(new Test() { Ask = "14", Answer1 = "1", Answer2 = "2", Answer3 = "3", Answer4 = "4", CorrectAnswerIndex = 1 });
            //AppDb.Tests.Add(new Test() { Ask = "15", Answer1 = "1", Answer2 = "2", Answer3 = "3", Answer4 = "4", CorrectAnswerIndex = 1 });
            //AppDb.Tests.Add(new Test() { Ask = "16", Answer1 = "1", Answer2 = "2", Answer3 = "3", Answer4 = "4", CorrectAnswerIndex = 1 });
            //AppDb.Tests.Add(new Test() { Ask = "17", Answer1 = "1", Answer2 = "2", Answer3 = "3", Answer4 = "4", CorrectAnswerIndex = 1 });
            //AppDb.Tests.Add(new Test() { Ask = "18", Answer1 = "1", Answer2 = "2", Answer3 = "3", Answer4 = "4", CorrectAnswerIndex = 1 });
            //AppDb.Tests.Add(new Test() { Ask = "19", Answer1 = "1", Answer2 = "2", Answer3 = "3", Answer4 = "4", CorrectAnswerIndex = 1 });

            AppDb.SaveChanges();

            var testsQuery = from test in AppDb.Tests
                        select test;
            var testsList = testsQuery.ToList();
            //foreach(var t in testsList)
            //{
            //    AppDb.Tests.Remove(t);
            //}
            //AppDb.SaveChanges();

            var rand = new Random();
            const int asks = 5;
            var indices = new int[asks];
            int i = 0;
            while (i < asks)
            {
                indices[i] = -1;
                int index = rand.Next(testsQuery.Count());
                if (!indices.Contains(index))
                {
                    indices[i] = index;
                    i++;
                }
            }

            askTextBoxes = new RichTextBox[asks] { askTextBox1, askTextBox2, askTextBox3, askTextBox4, askTextBox5 };
            answersRadioButtons = new RadioButton[][] {new RadioButton[]{ask1RadioButton1, ask1RadioButton2, ask1RadioButton3, ask1RadioButton4 },
                                        new RadioButton[]{ask2RadioButton1, ask2RadioButton2, ask2RadioButton3, ask2RadioButton4 },
                                        new RadioButton[]{ask3RadioButton1, ask3RadioButton2, ask3RadioButton3, ask3RadioButton4},
                                        new RadioButton[]{ask4RadioButton1, ask4RadioButton2, ask4RadioButton3, ask4RadioButton4},
                                        new RadioButton[]{ask5RadioButton1, ask5RadioButton2, ask5RadioButton3, ask5RadioButton4} };
            i = 0;
            foreach(var index in indices)
            {
                askTextBoxes[i].Text = testsList[index].Ask;
                answersRadioButtons[i][0].Text = testsList[index].Answer1;
                answersRadioButtons[i][1].Text = testsList[index].Answer2;
                answersRadioButtons[i][2].Text = testsList[index].Answer3;
                answersRadioButtons[i][3].Text = testsList[index].Answer4;
                tests.Add(testsList[index]);
                i++;
            }
        }

        private void tryButton_Click(object sender, EventArgs e)
        {
            int correct = 0;
            for(int i = 0; i < answersRadioButtons.Count(); i++)
            {
                int checkedButton = -1;
                for(int j = 0; j < answersRadioButtons[i].Count(); j++)
                {
                    if (answersRadioButtons[i][j].Checked)
                    {
                        checkedButton = j;
                        break;
                    }
                }
                answersRadioButtons[i][tests[i].CorrectAnswerIndex].ForeColor = Color.Green;
                if(tests[i].CorrectAnswerIndex == checkedButton)
                {
                    correct++;
                }
                else if(checkedButton >= 0)
                {
                    answersRadioButtons[i][checkedButton].ForeColor = Color.Red;
                }
            }
            resultTextBox.Text = "Правильных ответов: " + correct;
            tryButton.Enabled = false;
            SaveRecord(correct);
        }
        private void SaveRecord(int nAnswers)
        {
            using(AppDbContext context = new AppDbContext())
            {
                var userQuery = from u in context.Users
                           where u.Login == LoginForm.CurrentUser.Login
                           select u;
                var userList = userQuery.ToList();
                if(userList.Count != 0)
                {
                    var user = userQuery.ToList()[0];
                    if(user.RecordAnswersCount < nAnswers)
                    {
                        user.RecordAnswersCount = nAnswers;
                        context.SaveChanges();
                    }
                }

            }
        }
    }
}
