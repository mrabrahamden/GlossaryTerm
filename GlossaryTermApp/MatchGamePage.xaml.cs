using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MatchGameLib;
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
            //double height = 0;
            //схема такая: у нас есть большаая стэк панель. в ней лежат док панели - по количеству слов
            //в каждой док панели слева - прямоугольник, куда можно будет термин перетащить
            //а справа - врап панель
            //и так как у нас стоит lastChildFill==true то при resize окна всё будет все равно чётко
            foreach (var term in MatchGame.TermList)
            {
                StackPanel termStackPanel=new StackPanel()
                {
                    Orientation = Orientation.Horizontal
                };
                DockPanel dockPanel = new DockPanel()
                {
                    LastChildFill = true
                };
                WrapPanel wrapPanel = new WrapPanel();
                TextBlock descriptionTextBlock = new TextBlock()
                {
                    Text = term.Description,
                    FontSize = 18,
                    Tag = term,
                    Background = (SolidColorBrush) new BrushConverter().ConvertFromString("#F9F7A8"),
                    TextWrapping = TextWrapping.Wrap,
                    Padding = new Thickness(5, 2, 5, 2),
                    Margin = new Thickness(10, 10, 5, 0)
                    //Width = 550
                };
                Rectangle placeForWordRectangle = new Rectangle()
                {
                    AllowDrop = true,
                    Tag = term,
                    Fill = new SolidColorBrush(Color.FromRgb(202, 207, 210)),
                    Margin = new Thickness(30, 10, 10, 0),
                    Width = 200
                };
                
                wrapPanel.Children.Add(descriptionTextBlock);
                termStackPanel.Children.Add(placeForWordRectangle);
                dockPanel.Children.Add(termStackPanel);
                dockPanel.Children.Add(wrapPanel);
                WordsCanvas.Children.Add(dockPanel);
                //descriptionTextBlock.UpdateLayout();
                //WordsCanvas.UpdateLayout();
                //descriptionTextBlock.Measure(new System.Windows.Size(Double.PositiveInfinity, Double.PositiveInfinity));
                //var desiredSizeNew = descriptionTextBlock.DesiredSize;
                //height += desiredSizeNew.Height;

                TextBlock wordTextBlock = new TextBlock()
                {
                    Text = term.Word,
                    FontSize = 18,
                    Tag = term,
                    Background = (SolidColorBrush) new BrushConverter().ConvertFromString("#F08080"),
                    Padding = new Thickness(5, 1, 5, 1)
                };

                wordTextBlock.MouseDown += WordTextBlock_MouseDown;
                wordTextBlock.MouseUp += WordTextBlock_MouseUp;
                wordTextBlock.MouseMove += WordTextBlock_MouseMove;

                Border forTextBlock = new Border()
                {
                    BorderThickness = new Thickness(2),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(202, 207, 210)),
                    Margin = new Thickness(10, 10, 0, 0),
                    Child = wordTextBlock
                };
                
                WordsWrapPanel.Children.Add(forTextBlock);
            }
        }

        private void WordTextBlock_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void WordTextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
        }

        private void WordTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
        }
    }
}
