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
            var testsQuery = from test in AppDb.Tests
                             select test;
            var testsList = testsQuery.ToList();
            //foreach (var t in testsList)
            //{
            //    AppDb.Tests.Remove(t);
            //}

            if(testsList.Count == 0)
            {
                AppDb.Tests.Add(new Test() { Ask = "Что такое Б-дерево", Answer1 = "алгоритм", Answer2 = "структура данных", Answer3 = "объект", Answer4 = "функция", CorrectAnswerIndex = 2 });
                AppDb.Tests.Add(new Test() { Ask = "Если t-степень B-дерева, то корень должен содержать количество ключей", Answer1 = "t", Answer2 = "t-1", Answer3 = "от t-1 до 2t-1", Answer4 = "от 1 до 2t-1", CorrectAnswerIndex = 4 });
                AppDb.Tests.Add(new Test() { Ask = "Если t-степень B-дерева и node-не корень, то node должен содержать количество ключей", Answer1 = "t", Answer2 = "t-1", Answer3 = "от t-1 до 2t-1", Answer4 = "от 1 до 2t-1", CorrectAnswerIndex = 3 });
                AppDb.Tests.Add(new Test() { Ask = "Сколько потомков у листьев?", Answer1 = "t-1", Answer2 = "от t-1 до 2t-1", Answer3 = "0", Answer4 = "от 1 до 2t-1", CorrectAnswerIndex = 3 });
                AppDb.Tests.Add(new Test() { Ask = "Чему должна быть равна степень дерева(t)", Answer1 = "t > 1", Answer2 = "t=1", Answer3 = "t > 0", Answer4 = "0<t<10", CorrectAnswerIndex = 1 });
                AppDb.Tests.Add(new Test() { Ask = "В-деревья используются для:", Answer1 = "организации индексов в СУБД", Answer2 = "хранения данных в ОЗУ", Answer3 = "алгоритмы на графах", Answer4 = "сортировка по произвольному ключу", CorrectAnswerIndex = 1 });
                AppDb.Tests.Add(new Test() { Ask = "Сложность поиска в B-дереве", Answer1 = "O(N^3)", Answer2 = "O(N)", Answer3 = "O(N^2)", Answer4 = "O(tlog(t, N))", CorrectAnswerIndex = 4 });
                AppDb.Tests.Add(new Test() { Ask = "Если узел содержит n ключей, то количество потомков у него:", Answer1 = "n+1", Answer2 = "n-1", Answer3 = "n", Answer4 = "2n", CorrectAnswerIndex = 1 });
                AppDb.Tests.Add(new Test() { Ask = "Первый потомок узла с первым ключом K[0] содержит ключи из интервала", Answer1 = "(K[1], K[2])", Answer2 = "(-inf,K[0])", Answer3 = "(K[i - 1], K[n])", Answer4 = "(K[1], K[n])", CorrectAnswerIndex = 2 });
                AppDb.Tests.Add(new Test() { Ask = "Для 0<i<n, i-й потомок и все его потомки содержат ключи из интервала", Answer1 = "(K[i-1],K[i])", Answer2 = "(-inf,K[i])", Answer3 = "(K[i - 1], K[n])", Answer4 = "(K[i], K[n])", CorrectAnswerIndex = 1 });
                AppDb.Tests.Add(new Test() { Ask = "n-й потомок содержит ключи из интервала", Answer1 = "(K[i-1],K[i])", Answer2 = "(-inf,K[i])", Answer3 = "(K[i - 1], K[n])", Answer4 = "(K[n-1], +inf)", CorrectAnswerIndex = 4 });
                AppDb.Tests.Add(new Test()
                {
                    Ask = "Сбалансированность дерева означает",
                    Answer1 = "длина всех путей от корня до листьев различается не более, чем на 1",
                    Answer2 = "длина всех путей от корня до листьев различается не более, чем на 2",
                    Answer3 = "длина всех путей от корня до листьев различается не менее, чем на 1",
                    Answer4 = "длина всех путей от корня до листьев различается не менее, чем на 2",
                    CorrectAnswerIndex = 1
                });
                AppDb.Tests.Add(new Test() { Ask = "В сильно ветвистом дереве каждый узел ссылается на следующее количество потомков", Answer1 = "< 2", Answer2 = "> 2", Answer3 = "> 1", Answer4 = "< 1", CorrectAnswerIndex = 2 });
                AppDb.Tests.Add(new Test() { Ask = "В каком году было предложено использовать B-деревья", Answer1 = "1980", Answer2 = "1965", Answer3 = "1875", Answer4 = "1971", CorrectAnswerIndex = 3 });
                AppDb.Tests.Add(new Test() { Ask = "Кто предложил использовать B-деревья?", Answer1 = "Н.Вирт", Answer2 = "Р.Бэйер и Е.МакКрейт", Answer3 = "Р.Хоар", Answer4 = "Э.В.Дейкстра", CorrectAnswerIndex = 2 });
                AppDb.Tests.Add(new Test() { Ask = "Ключи в каждом узле упорядочены по:", Answer1 = "убыванию", Answer2 = "неубыванию", Answer3 = "в порядке добавления", Answer4 = "в случайном порядке", CorrectAnswerIndex = 2 });
                AppDb.Tests.Add(new Test() { Ask = "В каком случае необходимо разбивать узел для вставки?", Answer1 = "узел содержит t+1 ключей", Answer2 = "узел содержит t-1 ключей", Answer3 = "узел содержит 2t+1 ключей", Answer4 = "узел содержит 2t-1 ключей", CorrectAnswerIndex = 4 });
                AppDb.Tests.Add(new Test() { Ask = "В каком случае необходимо выполнить слияние или перемещение при удалениии?", Answer1 = "узел содержит t+1 ключей", Answer2 = "узел содержит t-1 ключей", Answer3 = "узел содержит 2t+1 ключей", Answer4 = "узел содержит 2t-1 ключей", CorrectAnswerIndex = 2 });
                AppDb.Tests.Add(new Test() { Ask = "Какая вариация дерева используется в файловых системах HFS и Reiser4?", Answer1 = "B+ дерево", Answer2 = "B*-дерево", Answer3 = "B-дерево", Answer4 = "2, 3-дерево", CorrectAnswerIndex = 2 });
                AppDb.Tests.Add(new Test() { Ask = "В чем особенность B*-дерева?", Answer1 = "каждый узел должен быть заполнен как минимум на 2/3", Answer2 = "каждый узел имеет либо 2,либо 3 ребенка", Answer3 = "вся сопутствующая информация хранится в листьях", Answer4 = "каждый узел имеет либо 4, либо 5 ключей", CorrectAnswerIndex = 1 });
            }


            AppDb.SaveChanges();

            testsQuery = from test in AppDb.Tests
                        select test;


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
                answersRadioButtons[i][tests[i].CorrectAnswerIndex - 1].ForeColor = Color.Green;
                if(tests[i].CorrectAnswerIndex - 1 == checkedButton)
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
