using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using FillGameLib;

namespace GlossaryTermApp
{
    /// <summary>
    /// Логика взаимодействия для FillGamePage.xaml
    /// </summary>
    public partial class FillGamePage : Window
    {
        private FillGame game;

        private int numOfSkipWords = 0;
        public FillGamePage(FillGame game)
        {
            InitializeComponent();
            this.game = game;
            var list = game.List.FindAll((term => term.ReadyForFillGame)).ToList();
            if (list.Count > 0)
            {
                foreach (var gameWord in list)
                {
                    string word = gameWord.Word;                 //вывод самого термина
                    var listOfSkippedWords = GetNumOfSkippedWords(game.Lvl,
                        gameWord.DescriptionWordsAndSplittersList
                            .FindAll((descriptionWord => descriptionWord.IsKeyWord)).Count);
                    numOfSkipWords += listOfSkippedWords.FindAll((b =>b==true )).Count;
                    TextBlock newWord = new TextBlock { Text = word + " ⸺ ", TextWrapping = TextWrapping.Wrap, FontSize = 20, FontWeight = FontWeights.Bold};
                    WrapPanel panelForOneWord = new WrapPanel();
                    panelForOneWord.VerticalAlignment = VerticalAlignment.Top;
                    panelForOneWord.Children.Add(newWord);                                       
                    foreach (var wordPart in gameWord.DescriptionWordsAndSplittersList) //печать слов из определения
                    {
                        if (wordPart.IsKeyWord&&listOfSkippedWords.Count>0)
                        {
                            if (listOfSkippedWords.First() == true)
                            {
                                TextBox skippedWord = new TextBox()
                                    {FontSize = 20, MinWidth = 20, Tag = wordPart.Word};
                                skippedWord.GotKeyboardFocus += SkippedWordOnGotKeyboardFocus;
                                if (game.FixedLength)
                                {
                                    skippedWord.MaxLength = wordPart.Word.Length;
                                }

                                panelForOneWord.Children.Add(skippedWord);
                            }
                            else
                            {
                                TextBlock skippedWord = new TextBlock()
                                    { Text = wordPart.Word, FontSize = 20, TextWrapping = TextWrapping.Wrap };
                                panelForOneWord.Children.Add(skippedWord);
                            }

                            listOfSkippedWords.RemoveAt(0);
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

        private void SkippedWordOnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.Background = default;
        }


        private List<bool> GetNumOfSkippedWords(int lvl, int num)
        {
            List<bool> listOfBools=new List<bool>();
            int numOfSkippedWords = 0;
            if (lvl == 1)
            {
                numOfSkippedWords = (int)(num * 0.3);
                if (numOfSkippedWords < 3)
                    numOfSkippedWords = 2;
            }
            else if (lvl == 2)
            {
                numOfSkippedWords = (int)(num * 0.5);
                if (numOfSkippedWords < 3)
                    numOfSkippedWords = 3;
            }
            else
            {
                numOfSkippedWords = (int)(num * 0.7);
                if (numOfSkippedWords < 3)
                    numOfSkippedWords = 4;
            }

            if (numOfSkippedWords > num)
            {
                numOfSkippedWords = num;
            }


            Random random=new Random();
            while (true)
            {
                if (num > 0)
                {
                    while ((listOfBools.FindAll((b => b == true)).Count < numOfSkippedWords)) 
                    {
                        var intNext = random.Next() % 2;
                        if (intNext == 1)
                        {
                            listOfBools.Add(true);
                        }
                        else
                        {
                            listOfBools.Add(false);
                        }
                    }
                
                    while (listOfBools.Count != num)
                    {
                        if(listOfBools.Count>num)
                            listOfBools.Remove(false);
                        if(listOfBools.Count<num)
                            listOfBools.Add(false);
                    }
                }

                return listOfBools;
            }

        }

        private int numOfErrors = 0;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var child in stackPanelOutput.Children)
            {
                if (child.GetType() == typeof(WrapPanel))
                {
                    var wrapPanel = (WrapPanel) child;
                    foreach (var child2 in wrapPanel.Children)
                    {
                        if (child2.GetType() == typeof(TextBox))
                        {
                            var textbox = (TextBox) child2;
                            if (textbox.Tag.ToString().ToLower() != textbox.Text.ToLower())
                            {
                                textbox.Background = Brushes.Red;
                                numOfErrors++;
                            }
                            else
                            {
                                textbox.Background = Brushes.LightGreen;
                            }
                        }
                    }
                }   
            }

            if (!game.TrainingMode)
            {
                GameResult gameResult=new GameResult(numOfErrors,numOfSkipWords);
                gameResult.ShowDialog();
                this.Close();
            }

            numOfErrors = 0;
        }
    }
    ////https://github.com/xceedsoftware/wpftoolkit/wiki/IntegerUpDown про UpDown
}
