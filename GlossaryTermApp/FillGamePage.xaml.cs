using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
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
using FillGameLib;

namespace GlossaryTermApp
{
    /// <summary>
    /// Логика взаимодействия для FillGamePage.xaml
    /// </summary>
    public partial class FillGamePage : Window
    {
        public FillGamePage(FillGame game)
        {
            InitializeComponent();
            if (game.List.Count > 0)
            {
                foreach (var gameWord in game.List)
                {
                    string word = gameWord.Word;                 //вывод самого термина
                    TextBlock newWord = new TextBlock { Text = word + " -- ", TextWrapping = TextWrapping.Wrap, FontSize = 20, FontWeight = FontWeights.Bold};
                    WrapPanel panelForOneWord = new WrapPanel();
                    panelForOneWord.VerticalAlignment = VerticalAlignment.Top;
                    panelForOneWord.Children.Add(newWord);                                       
                    foreach (var wordPart in gameWord.DescriptionWordsAndSplittersList) //печать слов из определения
                    {
                        if (wordPart.IsKeyWord)
                        {
                            
                            TextBox skippedWord = new TextBox()
                                {FontSize = 20, MinWidth=20};
                            if(game.FixedLength)
                            {
                                skippedWord.MaxLength = wordPart.Word.Length;
                            }
                            panelForOneWord.Children.Add(skippedWord);
                            
                        }
                        else
                        {
                            TextBlock notSkippedWord = new TextBlock()
                                {Text = wordPart.Word, FontSize = 20, TextWrapping = TextWrapping.Wrap};
                            panelForOneWord.Children.Add(notSkippedWord);
                        }
                    }
                    Separator separate = new Separator();
                    stackPanelOutput.Children.Add(panelForOneWord);
                    stackPanelOutput.Children.Add(separate);
                }
            }
        }
    }
    ////https://github.com/xceedsoftware/wpftoolkit/wiki/IntegerUpDown про UpDown
}
