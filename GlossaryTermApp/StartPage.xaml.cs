using System;
using System.Windows;

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

        private void ButtonStart_OnClick(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow=new MainWindow();
            this.Hide();
            mainWindow.Show();
            this.Close();
        }

    }
}
