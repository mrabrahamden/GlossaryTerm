using System.Windows;
using System.Windows.Media;

namespace GlossaryTermApp
{
    public partial class GameResult : Window
    {
        public GameResult(int numofErrors,int numOfWords)
        {
            InitializeComponent();

            if (numofErrors == 0)
            {
                Result.Foreground = Brushes.Green;
                Result.Text = "Молодец!";
                NumOfRightAnswers.Text = "Всё верно!";
                NumOfWrongAnswers.Text = "";
            }
            else
            {
                Result.Text = "Есть ошибки!";
                Result.Foreground = Brushes.Red;
                NumOfRightAnswers.Text = "Верно : "+(numOfWords-numofErrors).ToString();
                NumOfWrongAnswers.Text = "Ошибок : " + numofErrors.ToString();
            }

        }
    }
}
