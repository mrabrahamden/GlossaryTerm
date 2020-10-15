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
            bool okayToContinue = false;
            while (okayToContinue==false)
            {
                okayToContinue = true;
                string input = ComboBoxGrade.Text;
                Regex regex = new Regex(@"(\d)+\D");
                grade = int.Parse(regex.Match(input).ToString());
                subject = ComboBoxSubject.Text;
            }
            MainWindow mainWindow=new MainWindow();
            this.Hide();
            mainWindow.Show();
            this.Close();
        }

        private void SelectionCheck()
        {}
    }
}
