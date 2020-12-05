using System.Windows;
using System.Windows.Media;

namespace GlossaryTermApp
{
    public partial class GameResult : Window
    {
        public GameResult(int numofErrors, int numOfWords)
        {
            InitializeComponent();

            if (numofErrors == 0)
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
                NumOfRightAnswers.Text = "Верно : " + (numOfWords - numofErrors);
                NumOfWrongAnswers.Text = "Ошибок : " + numofErrors;
            }

        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
