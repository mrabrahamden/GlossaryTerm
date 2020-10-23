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
using TermLib;

namespace GlossaryTermApp
{
    /// <summary>
    /// Логика взаимодействия для FillGameEditorPage.xaml
    /// </summary>
    public partial class FillGameEditorPage : Window
    {
        public FillGameEditorPage(List<SimpleTerm> list)
        {
            InitializeComponent();
            foreach (var term in list)
            {

                string wordAndDescription = term.Word + " -- ";
                TextBlock newWord = new TextBlock { Text = wordAndDescription, TextWrapping = TextWrapping.Wrap };
                WrapPanel panelForOneWord = new WrapPanel();
                panelForOneWord.Children.Add(newWord);
                foreach (var word in term.DescriptionWordsList)
                {
                    if (word.Length > 0)
                    {
                        if (Char.IsLetter(word[0]))
                        {
                            Button button = new Button()
                            {
                                Content = word
                            };
                            button.Click += ButtonOnClick;
                            panelForOneWord.Children.Add(button);
                        }
                        else
                        {
                            TextBlock split=new TextBlock()
                            {
                                Text = word
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

        }
    }
}
