using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using SerializerLib;

namespace GlossaryTermApp
{
    /// <summary>
    /// Interaction logic for StartPage.xaml
    /// </summary>
    public partial class StartPage : Window
    {
        public StartPage()
        {
            InitializeComponent();
        }

        internal int grade;
        internal string subject;
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

                Serializer serializer = new Serializer(grade, subject);
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

        private void SelectionCheck()
        {}
    }
}
