using SerializerLib;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;

namespace TeacherryApp
{
    public partial class StartPage : Window
    {
        internal int Grade;
        internal string Subject;
        internal Serializer Serializer;
        private List<string> _listOfSubjects = new List<string>();
        private List<string> _standartListOfSubjects = new List<string>()
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
            Serializer = new Serializer();
            InitializeComponent();
            Serializer.GetSettings();
            Serializer.TermList.Clear();
            if (Serializer.Settings.ListOfSubjects.Count == 0)
            {
                WelcomeWindow welcomeWindow = new WelcomeWindow();
                welcomeWindow.ShowDialog();
                GetSubjList(welcomeWindow);
            }
            else
            {
                _listOfSubjects = Serializer.Settings.ListOfSubjects;
            }

            foreach (var subject in _listOfSubjects)
            {
                ComboBoxSubject.Items.Add(subject);
            }
            if (Serializer.Settings.Class != 0)
            {
                ComboBoxGrade.SelectedIndex = Serializer.Settings.Class - 1;
            }
            if (ComboBoxSubject.Items.Contains(Serializer.Settings.Subject))
            {
                ComboBoxSubject.SelectedIndex = ComboBoxSubject.Items.IndexOf(Serializer.Settings.Subject);
            }
        }

        private void GetSubjList(WelcomeWindow welcomeWindow)
        {
            if (welcomeWindow.CheckedList.Count > 0)
            {
                Serializer.Settings.ListOfSubjects = new List<string>(welcomeWindow.CheckedList);
                _listOfSubjects = welcomeWindow.CheckedList;
                Serializer.SaveSettings();
            }
            else
            {
                _listOfSubjects = _standartListOfSubjects;
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
                    Grade = int.Parse(regex.Match(input).ToString());
                    Subject = ComboBoxSubject.Text;
                }

                Serializer.Settings.Class = Grade;
                Serializer.Settings.Subject = Subject;
                Serializer.SaveSettings();
                Serializer.Deserialize();
                MainWindow mainWindow = new MainWindow(Serializer);
                this.Hide();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Выберите класс и предмет.");
            }
        }

        private void ButtonEditSubjList_OnClick(object sender, RoutedEventArgs e)
        {
            WelcomeWindow changeSubjWelcomeWindow = new WelcomeWindow();
            changeSubjWelcomeWindow.lblFirstTime.Content = "Выберите нужные предметы";
            changeSubjWelcomeWindow.lblFirstTime.HorizontalContentAlignment = HorizontalAlignment.Center;
            changeSubjWelcomeWindow.imgCherry.Visibility = Visibility.Hidden;
            changeSubjWelcomeWindow.lblAppName.Visibility = Visibility.Hidden;
            changeSubjWelcomeWindow.lblQMark.Visibility = Visibility.Hidden;
            changeSubjWelcomeWindow.ShowDialog();
            GetSubjList(changeSubjWelcomeWindow);
            ComboBoxSubject.Items.Clear();
            foreach (var subject in _listOfSubjects)
            {
                ComboBoxSubject.Items.Add(subject);
            }
        }
    }
}
