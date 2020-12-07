using GameLib;
using SerializerLib;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Orientation = System.Windows.Controls.Orientation;
using TextBox = System.Windows.Controls.TextBox;

namespace TeacherryApp
{
    public partial class CrosswordGamePage : Window
    {
        private CrosswordGame _crosswordGame;
        private SimpleTerm[] _crosswordTerms;
        private LetterFromWord[,] _matrix;
        private int _width;
        private int _height;
        private List<TextBlock> _listOfLetters = new List<TextBlock>();
        private List<TextBlock> _listOfMainWordLetters = new List<TextBlock>();
        private List<TextBox> _placeForWordsList = new List<TextBox>();
        private object _mainWordTag;
        public CrosswordGamePage(CrosswordGame crosswordGame)
        {
            InitializeComponent();
            this.Width = SystemParameters.PrimaryScreenWidth;
            this.Height = SystemParameters.PrimaryScreenHeight;
            this._crosswordGame = crosswordGame;
            _crosswordTerms = crosswordGame.CrossWordTerms;
            _matrix = crosswordGame.CrosswordMatrix;
            _width = _matrix.GetLength(1);
            _height = _matrix.GetLength(0);
            CrosswordCanvas.Width = this.Width - 30;
            CrosswordCanvas.Height = (this.Height - 30) * 0.6;
            _mainWordTag = _crosswordTerms[0];
            _writtenMainWordLetters = new bool[_height];
            PrepareForm();
        }

        public void PrepareForm()
        {
            PrepareCrossword();
            PrepareAnswers();
        }

        private void PrepareAnswers()
        {
            StackPanel wordsStackPanel = new StackPanel();
            wordsStackPanel.Background = (SolidColorBrush)new BrushConverter().ConvertFromString("#F2F3F4");
            AnswersScrollViewer.Content = wordsStackPanel;
            bool firstHorizontalWord = true;
            for (int i = 0; i < _crosswordTerms.Length; i++)
            {
                var term = _crosswordTerms[i];
                if (term != null)
                {
                    if (i == 0)
                    {
                        wordsStackPanel.Children.Add(new TextBlock() { Text = "По вертикали: ", FontSize = 20, Margin = new Thickness(10, 8, 0, 2) });
                    }
                    else if (firstHorizontalWord)
                    {
                        wordsStackPanel.Children.Add(new TextBlock() { Text = "По горизонтали: ", FontSize = 20, Margin = new Thickness(10, 8, 0, 2) });
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
                    _placeForWordsList.Add(placeForWordTextBox);
                    placeForWordTextBox.TextChanged += PlaceForWordTextBox_TextChanged;
                    placeForWordTextBox.GotKeyboardFocus += PlaceForWordTextBox_GotKeyboardFocus;
                    placeForWordTextBox.LostKeyboardFocus += PlaceForWordTextBox_LostKeyboardFocus;
                    wrapPanel.Children.Add(descriptionTextBlock);
                    termStackPanel.Children.Add(placeForWordTextBox);
                    dockPanel.Children.Add(termStackPanel);
                    dockPanel.Children.Add(wrapPanel);
                    dockPanel.Tag = placeForWordTextBox;
                    dockPanel.MouseEnter += DockPanel_MouseEnter;
                    dockPanel.MouseLeave += DockPanel_MouseLeave;
                    wordsStackPanel.Children.Add(dockPanel);
                }
            }

        }

        private void PlaceForWordTextBox_LostKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (textBox.Tag != _mainWordTag)
            {
                foreach (var textBlock in _listOfLetters)
                {
                    if (textBox.Tag == textBlock.Tag)
                    {
                        var border = (Border) textBlock.Parent;
                        if (!_listOfMainWordLetters.Contains(textBlock))
                            border.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                        else
                            border.Background = new SolidColorBrush(Color.FromRgb(250, 219, 216));
                    }
                }
            }
        }
        private void PlaceForWordTextBox_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            if (textBox.Tag != _mainWordTag)
            {
                foreach (var textBlock in _listOfLetters)
                {
                    if (textBox.Tag == textBlock.Tag)
                    {
                        var border = (Border)textBlock.Parent;
                        border.Background = new SolidColorBrush(Color.FromRgb(208, 236, 231));
                    }
                }
            }
        }

        private void DockPanel_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var textBox = (TextBox)((DockPanel)sender).Tag;
            if (textBox.Tag != _mainWordTag)
            {
                foreach (var textBlock in _listOfLetters)
                {
                    if (textBox.Tag == textBlock.Tag)
                    {
                        var border = (Border) textBlock.Parent;
                        if (!_listOfMainWordLetters.Contains(textBlock))
                            border.Background = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                        else
                            border.Background = new SolidColorBrush(Color.FromRgb(250, 219, 216));
                    }
                }
            }
        }

        private void DockPanel_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            var textBox = (TextBox)((DockPanel)sender).Tag;
            if (textBox.Tag != _mainWordTag)
            {
                foreach (var textBlock in _listOfLetters)
                {
                    if (textBox.Tag == textBlock.Tag)
                    {
                        var border = (Border) textBlock.Parent;
                        border.Background = new SolidColorBrush(Color.FromRgb(208, 236, 231));
                    }
                }
            }
        }

        private bool[] _writtenMainWordLetters;
        private void PlaceForWordTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            var term = (SimpleTerm)textBox.Tag;
            textBox.Background = new SolidColorBrush(Color.FromRgb(202, 207, 210));
            List<TextBlock> listOfTermTextBlocks = new List<TextBlock>();
            if (textBox.Tag == _mainWordTag)
            {
                listOfTermTextBlocks = _listOfMainWordLetters;
            }
            else
            {
                listOfTermTextBlocks =
                    (from t in _listOfLetters where (t.Tag != null) && ((SimpleTerm)t.Tag) == term select t).ToList();
            }
            string textToUpper = textBox.Text.ToUpper();

            for (int i = 0; i < listOfTermTextBlocks.Count; i++)
            {
                if (i < textToUpper.Length)
                    if (textBox.Tag != _mainWordTag)
                    {
                        if ((listOfTermTextBlocks[i].Text.Length > 0) && (listOfTermTextBlocks[i].Text[0] != textToUpper[i]) && (listOfTermTextBlocks[i].IsEnabled == false))
                        {
                            textBox.Background = Brushes.LightCoral;
                        }
                        else
                        {
                            if (listOfTermTextBlocks[i].IsEnabled)
                            {
                                listOfTermTextBlocks[i].Text = textToUpper[i].ToString();
                            }
                        }
                    }
                    else
                    {
                        if ((listOfTermTextBlocks[i].Text.Length > 0) && (listOfTermTextBlocks[i].Text[0] != textToUpper[i]) && (listOfTermTextBlocks[i].IsEnabled == false))
                        {
                            textBox.Background = Brushes.LightCoral;
                        }
                        else
                        {
                            if (listOfTermTextBlocks[i].IsEnabled)
                            {
                                listOfTermTextBlocks[i].Text = textToUpper[i].ToString();
                                _writtenMainWordLetters[i] = true;
                            }
                        }
                    }
                else
                {
                    if (textBox.Tag != _mainWordTag)
                    {
                        if (listOfTermTextBlocks[i].IsEnabled)
                            listOfTermTextBlocks[i].Text = "";
                    }
                    else
                    {
                        if (listOfTermTextBlocks[i].IsEnabled)
                            if (_writtenMainWordLetters[i])
                            {
                                listOfTermTextBlocks[i].Text = " ";
                                _writtenMainWordLetters[i] = false;
                            }
                    }
                }
            }


            if (textBox.Text.ToUpper() == term.Word.ToUpper())
            {
                textBox.IsEnabled = false;
                foreach (var tb in listOfTermTextBlocks)
                {
                    tb.IsEnabled = false;
                }
                CheckTaskComplete();
            }
        }

        private void CheckTaskComplete()
        {
            bool isTaskComplete = true;
            foreach (var textBox in _placeForWordsList)
            {
                if (textBox.IsEnabled)
                    isTaskComplete = false;
            }

            if (isTaskComplete)
            {
                GameResult gameResult = new GameResult(0, 0);
                gameResult.ShowDialog();
                this.Close();
            }
        }
        private void PrepareCrossword()
        {
            CrosswordCanvas.Background = new SolidColorBrush(Color.FromRgb(245, 238, 248));
            var borderWidth = CrosswordCanvas.Width / _width;
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
                        Background = new SolidColorBrush(Color.FromRgb(255, 255, 255))
                    };
                    if (_matrix[i, j] != null)
                    {
                        letter.Tag = _matrix[i, j].Term;
                        border.Tag = letter.Tag;
                    }


                    if (j == _crosswordGame.MainWordHorizontalIndex)
                    {
                        border.Background = new SolidColorBrush(Color.FromRgb(250, 219, 216));
                        _listOfMainWordLetters.Add(letter);
                    }
                    char ch = ' ';
                    if ((_matrix[i, j] != null) && (_matrix[i, j].Letter != ch))
                    {
                        ch = _matrix[i, j].Letter;
                    }
                    else
                    {
                        border.Visibility = Visibility.Hidden;
                    }
                    //letter.Text = ch.ToString();
                    //letter.Visibility = Visibility.Hidden;
                    _listOfLetters.Add(letter);
                    Canvas.SetLeft(border, x);
                    Canvas.SetTop(border, y);
                    x += borderWidth - 2;
                    CrosswordCanvas.Children.Add(border);
                }
                y += borderHeight - 2;
                x = 0;
            }
        }

        private void CrosswordGamePage_OnClosing(object sender, CancelEventArgs e)
        {
            int errors = 0;
            foreach (var textBox in _placeForWordsList)
            {
                if (textBox.IsEnabled)
                    errors++;
            }
            new GameResult(errors, _placeForWordsList.Count).ShowDialog();
        }
    }
}
