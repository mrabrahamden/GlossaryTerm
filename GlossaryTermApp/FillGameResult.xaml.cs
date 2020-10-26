using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GlossaryTermApp
{
    /// <summary>
    /// Логика взаимодействия для FillGameResult.xaml
    /// </summary>
    public partial class FillGameResult : Window
    {
        public FillGameResult(int numofErrors,int numOfWords)
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
