using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using MatchGameLib;
using Brushes = System.Drawing.Brushes;
using Color = System.Windows.Media.Color;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace GlossaryTermApp
{
    /// <summary>
    /// Логика взаимодействия для MatchGamePage.xaml
    /// </summary>
    public partial class MatchGamePage : Window
    {
        public MatchGame MatchGame;
        public MatchGamePage(MatchGame matchGame)
        {
            InitializeComponent();
            MatchGame = matchGame;
            PrepareForm();
        }

        private void PrepareForm()
        {
            double height = 0;
            foreach (var term in MatchGame.TermList)
            {
                StackPanel termStackPanel=new StackPanel()
                {
                    Orientation = Orientation.Horizontal
                };

                TextBlock descriptionTextBlock = new TextBlock()
                {
                    Text = term.Description,
                    FontSize = 18,
                    Tag = term,
                    Background = System.Windows.Media.Brushes.Brown,
                    TextWrapping = TextWrapping.Wrap
                };
                descriptionTextBlock.Margin=new Thickness(10,10,10,0);

                Rectangle placeForWordRectangle = new Rectangle()
                {
                    AllowDrop = true,
                    Tag = term
                };
                placeForWordRectangle.Fill = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                placeForWordRectangle.Margin = new Thickness(10, 10, 10, 0);
                placeForWordRectangle.Width = 200;

                Canvas.SetLeft(placeForWordRectangle,5);
                Canvas.SetTop(placeForWordRectangle,height);
                WordsCanvas.Children.Add(placeForWordRectangle);
                Canvas.SetLeft(descriptionTextBlock, placeForWordRectangle.Width+5);
                Canvas.SetTop(descriptionTextBlock, height);
                WordsCanvas.Children.Add(descriptionTextBlock);

               // height+=

                TextBlock wordTextBlock = new TextBlock()
                {
                    Text = term.Word,
                    FontSize = 18,
                    Tag = term,
                    Background = System.Windows.Media.Brushes.Brown
                };
                wordTextBlock.Margin = new Thickness(10, 10, 0, 0);
                WordsWrapPanel.Children.Add(wordTextBlock);
            }
        }
    }
}
