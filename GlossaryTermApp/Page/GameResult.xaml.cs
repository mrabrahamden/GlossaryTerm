using System.Windows;
using System.Windows.Media;

namespace GlossaryTermApp
{
    public partial class GameResult : Window
    {
        public GameResult(int numOfErrors, int numOfWords)
        {
            InitializeComponent();

            if (numOfErrors == 0)
            {
                Result.Foreground = Brushes.SeaGreen;
                Result.Text = "Молодец!";
                NumOfRightAnswers.Text = "Всё верно!";
                NumOfWrongAnswers.Text = "";
            }
            else
            {
                Result.Text = "Есть ошибки!";
                Result.Foreground = Brushes.IndianRed;
                NumOfRightAnswers.Text = "Верно : " + (numOfWords - numOfErrors);
                NumOfWrongAnswers.Text = "Ошибок : " + numOfErrors;
            }

        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
