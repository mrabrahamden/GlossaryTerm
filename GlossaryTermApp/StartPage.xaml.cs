using SerializerLib;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;

namespace GlossaryTermApp
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Window
    {
        internal int grade;
        internal string subject;
        internal Serializer serializer = new Serializer();
        private List<string> listOfSubjects = new List<string>();
        private List<string> standartListOfSubjects = new List<string>()
        {
            "Астрономия",
            "Английский язык",
            "Биология",
            "География",
            "Изобразительное искусство",
            "Информатика",
            "История",
            "Литература",
            "Математика",
            "Музыка",
            "МХК",
            "ОБЖ",
            "Обществознание",
            "Право",
            "Русский язык",
            "Технология",
            "Физика",
            "Французский язык",
            "Химия"
        };
        public StartPage()
        {

            InitializeComponent();
            serializer.GetSettings();
            if (serializer.Settings.ListOfSubjects.Count == 0)
            {
                WelcomeWindow welcomeWindow = new WelcomeWindow();
                welcomeWindow.ShowDialog();
                if (welcomeWindow.checkedList.Count > 0)
                {
                    serializer.Settings.ListOfSubjects=new List<string>(welcomeWindow.checkedList);
                    listOfSubjects = welcomeWindow.checkedList;
                    serializer.SaveSettings();
                }
                else
                {
                    listOfSubjects = standartListOfSubjects;
                }
            }
            else
            {
                listOfSubjects = serializer.Settings.ListOfSubjects;
            }

            foreach (var subject in listOfSubjects)
            {
                ComboBoxSubject.Items.Add(subject);
            }
            if (serializer.Settings.Class != 0)
            {
                ComboBoxGrade.SelectedIndex = serializer.Settings.Class - 1;
            }
            if (ComboBoxSubject.Items.Contains(serializer.Settings.Subject))
            {
                ComboBoxSubject.SelectedIndex = ComboBoxSubject.Items.IndexOf(serializer.Settings.Subject);
            }
        }

        private void ButtonStart_OnClick(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(ComboBoxGrade.Text) && !String.IsNullOrEmpty(ComboBoxSubject.Text))
            {
                bool okayToContinue = false;
                while (okayToContinue == false)
                {
                    okayToContinue = true;
                    string input = ComboBoxGrade.Text;
                    Regex regex = new Regex(@"(\d)+\D");
                    grade = int.Parse(regex.Match(input).ToString());
                    subject = ComboBoxSubject.Text;
                }

                serializer.Settings.Class = grade;
                serializer.Settings.Subject = subject;
                serializer.SaveSettings();
                serializer.Deserialize();
                MainWindow mainWindow = new MainWindow(serializer);
                this.Hide();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Выберите класс и предмет.");
            }
        }

    }
}
