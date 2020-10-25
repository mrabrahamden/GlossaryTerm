using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using SerializerLib;
using TermLib;

namespace GlossaryTermApp
{
    public partial class FillGameEditorPage : Window
    {
        public FillGameEditorPage(List<SimpleTerm> list)
        {
            InitializeComponent();
            foreach (var term in list)
            {
                string wordAndDescription = term.Word + " -- ";
                TextBlock newWord = new TextBlock { Text = wordAndDescription, TextWrapping = TextWrapping.Wrap, FontSize = 20};
                WrapPanel panelForOneWord = new WrapPanel();
                panelForOneWord.Children.Add(newWord);
                foreach (var descriptionWord in term.DescriptionWordsAndSplittersList)
                {
                    var word = descriptionWord.Word;
                    if (word.Length > 0)
                    {
                        if (!descriptionWord.IsSplitter)
                        {
                            Button button = new Button()
                            {
                                Content = word, FontSize = 20
                            };
                            if (descriptionWord.IsKeyWord)
                                button.Background = Brushes.LightGreen;
                            else
                                button.Background = Brushes.LightGray;
                            button.Click += ButtonOnClick;

                            panelForOneWord.Children.Add(button);
                        }
                        else
                        {
                            TextBlock split=new TextBlock()
                            {
                                Text = word, FontSize = 20
                            };
                            panelForOneWord.Children.Add(split);
                        }
                    }
                }
                Separator separate = new Separator();
                StackPanelForWords.Children.Add(panelForOneWord);
                StackPanelForWords.Children.Add(separate);
            }
        }

        private void ButtonOnClick(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button) sender;
            var panelForOneWord = (WrapPanel)clickedButton.Parent;
            var newWord = panelForOneWord.Children.OfType<TextBlock>().First();
            var wordAndDescr = newWord.Text;
            Regex regexForWord = new Regex(@"(\w)+");
            var termWord = regexForWord.Match(wordAndDescr).ToString();
            if (clickedButton.Background == Brushes.LightGreen)
            {
                
                clickedButton.Background = Brushes.LightGray;
            }
            else clickedButton.Background = Brushes.LightGreen;

            //дальше мы как-то используем полученное для игры
        }
    }
}
