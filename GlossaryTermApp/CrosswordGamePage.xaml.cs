using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using CrosswordLib;
using TermLib;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Orientation = System.Windows.Controls.Orientation;
using TextBox = System.Windows.Controls.TextBox;

namespace GlossaryTermApp
{
    public partial class CrosswordGamePage : Window
    {
        private CrosswordGame _crosswordGame;
        private SimpleTerm[] _crosswordTerms;
        private LetterFromWord[,] _matrix;
        private int _width;
        private int _height;
        private List<Border> listOfBorders=new List<Border>();
        private List<TextBlock> listOfLetters=new List<TextBlock>();
        private List<TextBlock> listOfMainWordLetters=new List<TextBlock>();
        private List<TextBox> placeForWordsList = new List<TextBox>();
        private object mainWordTag;
        public CrosswordGamePage(CrosswordGame crosswordGame)
        {
            InitializeComponent();
            this.Width = Screen.PrimaryScreen.Bounds.Width;
            this.Height = Screen.PrimaryScreen.Bounds.Height;
            this._crosswordGame = crosswordGame;
            _crosswordTerms = crosswordGame.CrossWordTerms;
            _matrix = crosswordGame.CrosswordMatrix;
            _width = _matrix.GetLength(1);
            _height = _matrix.GetLength(0);
            CrosswordCanvas.Width = this.Width;
            CrosswordCanvas.Height = this.Height * 0.6;
            mainWordTag = _crosswordTerms[0];
            PrepareForm();
        }

        private void PrepareForm()
        {
            PrepareCrossword();
            PrepareAnswers();
        }

        private void PrepareAnswers()
        {
            StackPanel _wordsStackPanel = new StackPanel();
            _wordsStackPanel.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#F2F3F4");
            AnswersScrollViewer.Content = _wordsStackPanel;
            bool firstHorizontalWord = true;
            for (int i = 0; i < _crosswordTerms.Length; i++)
            {
                var term = _crosswordTerms[i];
                if (term != null)
                {
                    if (i == 0)
                    {
                        _wordsStackPanel.Children.Add(new TextBlock() {Text = "По вертикали: ", FontSize = 20, Margin = new Thickness(10, 8, 0, 2) });
                    }
                    else if (firstHorizontalWord)
                    {
                        _wordsStackPanel.Children.Add(new TextBlock() { Text = "По горизонтали: ", FontSize = 20, Margin = new Thickness(10,8,0,2)});
                        firstHorizontalWord = false;
                    }

                    StackPanel termStackPanel = new StackPanel()
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
                        FontSize = 20,
                        Tag = term,
                        TextAlignment = TextAlignment.Left,
                        Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#F9F7A8"),
                        TextWrapping = TextWrapping.Wrap,
                        Padding = new Thickness(5, 2, 5, 2),
                        Margin = new Thickness(10, 10, 5, 0)
                    };
                    TextBox placeForWordTextBox = new TextBox()
                    {
                        FontSize = 18,
                        Tag = term,
                        Background = new SolidColorBrush(Color.FromRgb(202, 207, 210)),
                        Margin = new Thickness(15, 10, 10, 0),
                        Width = 200,
                        TextAlignment = TextAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    placeForWordTextBox.MaxLength = term.Word.Length;
                    placeForWordsList.Add(placeForWordTextBox);
                    placeForWordTextBox.TextChanged += PlaceForWordTextBox_TextChanged;
                    wrapPanel.Children.Add(descriptionTextBlock);
                    termStackPanel.Children.Add(placeForWordTextBox);
                    dockPanel.Children.Add(termStackPanel);
                    dockPanel.Children.Add(wrapPanel);
                    _wordsStackPanel.Children.Add(dockPanel);
                }
            }
        }

        private void PlaceForWordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox= (TextBox) sender;
            var term = (SimpleTerm) textBox.Tag;
            if (textBox.Text.ToUpper() == term.Word.ToUpper())
            {
                textBox.IsEnabled = false;
                if (textBox.Tag == mainWordTag)
                {
                    ShowMainWord();
                }
                else
                {
                    ShowRightWord(textBox.Tag);
                }
            }
        }

        private void ShowRightWord(object tag)
        {
            foreach (var textBlock in listOfLetters)
            {
                if (textBlock.Tag == tag)
                {
                    textBlock.Visibility = Visibility.Visible;
                }
            }
            CheckTaskComplete();
        }

        private void ShowMainWord()
        {
            foreach (var textBlock in listOfMainWordLetters)
            {
                textBlock.Visibility = Visibility.Visible;
            }
            CheckTaskComplete();
        }

        private void CheckTaskComplete()
        {
            bool IsTaskComplete = true;
            foreach (var textBox in placeForWordsList)
            {
                if (textBox.IsEnabled)
                    IsTaskComplete = false;
            }

            if (IsTaskComplete)
            {
                GameResult gameResult=new GameResult(0,0);
                gameResult.ShowDialog();
                this.Close();
            }
        }
        private void PrepareCrossword()
        {
            CrosswordCanvas.Background=new SolidColorBrush(Color.FromRgb(245, 238, 248));
            var borderWidth =  CrosswordCanvas.Width / _width;
            var borderHeight = CrosswordCanvas.Height / _height;
            if (borderHeight < borderWidth)
            {
                borderWidth = borderHeight;
            }
            else
            {
                borderHeight = borderWidth;
            }
            double x = 0, y = 0;
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    TextBlock letter = new TextBlock()
                    {
                        TextAlignment = TextAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        //Tag = _matrix[i,j].Term
                    };

                    Border border = new Border()
                    {
                        BorderThickness = new Thickness(2),
                        BorderBrush = new SolidColorBrush(Color.FromRgb(202, 207, 210)),
                        Margin = new Thickness(10, 10, 0, 0),
                        Child = letter,
                        Width = borderWidth,
                        Height = borderHeight,
                        Background = new SolidColorBrush(Color.FromRgb(255,255,255))
                    };
                    if (_matrix[i, j] != null)
                    {
                        letter.Tag = _matrix[i, j].Term;
                        border.Tag = letter.Tag;
                    }


                    if (j == _crosswordGame.MainWordHorizontalIndex)
                    {
                        border.Background= new SolidColorBrush(Color.FromRgb(250, 219, 216));
                        listOfMainWordLetters.Add(letter);
                    }
                    char ch = ' ';
                    if ((_matrix[i, j] != null ) && (_matrix[i, j].Letter != ch))
                    {
                        ch = _matrix[i, j].Letter;
                    }
                    else
                    {
                        border.Visibility = Visibility.Hidden;
                    }
                    letter.Text = ch.ToString();
                    letter.Visibility = Visibility.Hidden;
                    listOfLetters.Add(letter);
                    listOfBorders.Add(border);
                    Canvas.SetLeft(border,x);
                    Canvas.SetTop(border,y);
                    x += borderWidth-2;
                    CrosswordCanvas.Children.Add(border);
                }
                y += borderHeight-2;
                x = 0;
            }
        }

        private void CrosswordGamePage_OnClosing(object sender, CancelEventArgs e)
        {
            int errors = 0;
            foreach (var textBox in placeForWordsList)
            {
                if (textBox.IsEnabled)
                    errors++;
            }
            new GameResult(errors,placeForWordsList.Count).ShowDialog();
        }
    }
}
