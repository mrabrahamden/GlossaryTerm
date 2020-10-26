using System.Windows;
using PdfSaver;
using SerializerLib;
using System.Windows.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using TermLib;
using FillGameLib;
using Brushes = System.Windows.Media.Brushes;

namespace GlossaryTermApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {

        public Serializer Serializer;

        public MainWindow(Serializer ser)
        {
            Serializer = ser;
            Serializer.DeleteSimilarTerms();
            Serializer.SortList();
            InitializeComponent();
        }

       
        private void ClearWorkPlace()
        {
            foreach (UIElement workPlaceChild in WorkPlace.Children)
            {
                if (workPlaceChild.Visibility == Visibility.Visible) workPlaceChild.Visibility = Visibility.Hidden;
            }
            SearchTB.Clear();
        }

        private bool _searchMode = false;
        private ImageBrush GetBrushFromImage(string path)
        {
            var image = System.Drawing.Image.FromFile(path);
            var bitmap = new System.Drawing.Bitmap(image);
            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            );
            bitmap.Dispose();
            var brush = new ImageBrush(bitmapSource);
            return brush;
        }

        private void CrosswordItem_OnSelected(object sender, RoutedEventArgs e)
        {
            ClearWorkPlace();
            Export export = new Export();
            export.CaptureScreen();
        }

        private void DictionaryItem_Selected(object sender, RoutedEventArgs e)
        {
            _searchMode = false;
            PerformDictionaryPrint(Serializer.TermList);
        }

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var btnPanel = (StackPanel)button.Parent;
            var panelForOneWord = (DockPanel)btnPanel.Parent;
            var newWord = (TextBlock)panelForOneWord.Children.OfType<TextBlock>().First();
            MenuEditBTN.IsSelected = true;
            var wordAndDescr = newWord.Text;
            _editMode = true;
            EditBTN.Content = "Изменить";
            ClearWorkPlace();
            ReadyToAddAWord(sender, e);
            TermTB.Text = null;
            DescriptionTB.Text = null;
            EditStackPanel.Visibility = Visibility.Visible;
            TermTB.Text = Serializer.GetTermNameByString(wordAndDescr);
            DescriptionTB.Text = Serializer.GetTermDescriptionByString(wordAndDescr);
            _editedTerm = Serializer.GetTermByString(wordAndDescr);
        }

        private bool _editMode = false;
        private SimpleTerm _editedTerm;
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button) sender;
            var btnPanel =(StackPanel) button.Parent;
            var panelForOneWord = (DockPanel) btnPanel.Parent;
            var newWord = (TextBlock) panelForOneWord.Children.OfType<TextBlock>().First();
            panelForOneWord.Visibility = Visibility.Hidden; 
            Serializer.DeleteTermByString(newWord.Text);
            DictionaryItem_Selected(null, null);
            Serializer.Serialize();
        }

        private void HomeItem_Selected(object sender, RoutedEventArgs e)
        {
            ClearWorkPlace();
            instructions.Visibility = Visibility.Visible;
        }

        private void ExitItem_OnSelected(object sender, RoutedEventArgs e)
        {
            this.Hide();
            StartPage startPage=new StartPage();
            startPage.Show();
            this.Close();
        }

        private void EditItem_Selected(object sender, RoutedEventArgs e)
        {
            _editMode = false;
            EditBTN.Content = "Добавить";
            ClearWorkPlace();
            ReadyToAddAWord(sender, e);
            TermTB.Text = null;
            DescriptionTB.Text = null;
            EditStackPanel.Visibility = Visibility.Visible;
        }

        private void EditBTN_Click(object sender, RoutedEventArgs e)
        {
            bool editingSuccess = false;

            if (!_editMode)
            {
                if((!(string.IsNullOrEmpty(TermTB.Text)|| string.IsNullOrEmpty(DescriptionTB.Text))))
                {
                    try
                    {
                        var term = new SimpleTerm(TermTB.Text, DescriptionTB.Text);
                        Serializer.TermList.Add(new SimpleTerm(TermTB.Text, DescriptionTB.Text));
                        if (Serializer.DeleteSimilarTerms())
                        {
                            MessageBox.Show("Данный термин уже внесён в словарь!");
                        }
                        else
                        {
                            editingSuccess = true;
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Не удалось добавить новый термин.");
                    }

                }
                else
                {
                    MessageBox.Show("Нужно заполнить поля термин и описание перед добавлением.");
                }
            }
            else
            {
                _editedTerm.Word = TermTB.Text;
                _editedTerm.Description = DescriptionTB.Text;
                editingSuccess = true;
            }

            if (editingSuccess)
            {
                Serializer.SortList();
                Serializer.Serialize();
                EditBTN.FontFamily = new FontFamily("Segoe MDL2 Assets");
                EditBTN.Foreground = Brushes.MediumSeaGreen;
                EditBTN.FontWeight = FontWeights.Bold;
                EditBTN.Content = "\xE73E" + " ";
                EditBTN.IsEnabled = false;
            }
            else
            {
                EditBTN.FontFamily = new FontFamily("Segoe MDL2 Assets");
                EditBTN.Foreground = Brushes.Red;
                EditBTN.FontWeight = FontWeights.Bold;
                EditBTN.Content = "\xE711" + " ";
                EditBTN.IsEnabled = false;
            }
        }

        private void ClearBTN_Click(object sender, RoutedEventArgs e)
        {
            TermTB.Clear();
            DescriptionTB.Clear();
        }

        private void ReadyToAddAWord(object sender, RoutedEventArgs e)
        {
            EditBTN.FontFamily = new FontFamily("Segoe UI");
            if (_editMode)
            {
                EditBTN.Content = "Изменить";
            }
            else
            {
                EditBTN.Content = "Добавить";
            }
            EditBTN.IsEnabled = true;
            EditBTN.Foreground = Brushes.Black;
            EditBTN.FontWeight = FontWeights.Regular;
        }

        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchTB.Text))
            {
                var request = SearchTB.Text;
                var result = Serializer.LookForAWord(request);
                _searchMode = true;
                PerformDictionaryPrint(result);
                SearchTB.Text = request;
            }
        }

        private void PerformDictionaryPrint(List<SimpleTerm> list)
        {
            ClearWorkPlace();
            DictionaryStackPanel.Visibility = Visibility.Visible;
            ScrollDictionary.Visibility = Visibility.Visible;
            StackPanelForWords.Visibility = Visibility.Visible;
            ScrollDictionary.Height = 350;
            ScrollDictionary.Width = 715;
            StackPanelForWords.Children.Clear();
            if (list.Count > 0)
            {
                foreach (var term in list)
                {
                    string wordAndDescription = term.ToString();
                    TextBlock newWord = new TextBlock { Text = wordAndDescription, TextWrapping = TextWrapping.Wrap };
                    DockPanel panelForOneWord = new DockPanel();
                    panelForOneWord.Children.Add(newWord);
                    StackPanel btnPanel = new StackPanel();
                    Button deleteBtn = new Button
                    {
                        Height = 15,
                        Width = 15,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        FontFamily = new FontFamily("Segoe MDL2 Assets"),
                        Content = "\xE711",
                        Foreground = Brushes.Red,
                        Background = Brushes.White
                    };
                    deleteBtn.Click += DeleteBtn_Click;
                    Button editBtn = new Button
                    {
                        Height = 15,
                        Width = 15,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        FontFamily = new FontFamily("Segoe MDL2 Assets"),
                        Content = "\xE70F",
                        Background = Brushes.White

                    };
                    editBtn.Click += EditBtn_Click;
                    btnPanel.Children.Add(deleteBtn);
                    btnPanel.Children.Add(editBtn);
                    panelForOneWord.Children.Add(btnPanel);
                    Separator separate = new Separator();
                    StackPanelForWords.Children.Add(panelForOneWord);
                    StackPanelForWords.Children.Add(separate);
                }
            }
            else
            {
                TextBlock newWord = new TextBlock {TextWrapping = TextWrapping.Wrap};

                if (_searchMode)
                {
                    newWord.Text =
                        "По Вашему запросу ничего не найдено. Возможно, Вам стоит сначала добавить термин в словарь?";
                }
                else
                {
                    newWord.Text =
                        "Похоже, что в словаре пусто...Возможно, Вам стоит сначала добавить термин в словарь?";
                }
                DockPanel panelForOneWord = new DockPanel();
                panelForOneWord.Children.Add(newWord);
                StackPanelForWords.Children.Add(panelForOneWord);
            }
        }

        private void Search_KeyUp(object sender, KeyEventArgs e)  //чтобы поиск работал не только по кнопке, но и по enter
        {
            if (e.Key == Key.Enter)
            {
                SearchButton_OnClick(null, null);
            }
        }

        private void SearchEmptyButton_Click(object sender, RoutedEventArgs e)
        {
            DictionaryItem_Selected(null, null);
        }

        private List<SimpleTerm> _fillGameList=new List<SimpleTerm>();
        private void FillGameItem_Selected(object sender, RoutedEventArgs e)
        {
            ClearWorkPlace();
            FillGameCountUpDown.Maximum = _fillGameList.Count;
            FillGameEditorPanel.Visibility = Visibility.Visible;
        }

        private void FillGameEditorBTN_Click(object sender, RoutedEventArgs e)
        {
            FillGameEditorPage fillGameEditorPage = new FillGameEditorPage(Serializer.TermList);
            fillGameEditorPage.ShowDialog();
            fillGameEditorPage.DataContext = _fillGameList;
        }

        
        private void FillGameStartBTN_Click(object sender, RoutedEventArgs e)
        {
            bool isCreated = false;
            FillGame fillGame;
            int lvl = 0;
            bool fixedLength = false;
            bool trainingMode = false;
            try
            {
                if (FillGameEasyLvl.IsChecked == true)
                {
                    lvl = 1;
                }
                else if (FillGameNormalLvl.IsChecked == true)
                {
                    lvl = 2;
                }
                else
                {
                    lvl = 3;
                }

                if (FillGameFixedLength.IsChecked == true)
                {
                    fixedLength = true;
                }

                if (FillGameTrainingMode.IsChecked == true)
                {
                    trainingMode = true;
                }

                isCreated = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Не удалось создать игру, попробуйте снова.");
            }

            if (isCreated)
            {
                fillGame = new FillGame(Serializer.TermList, lvl, fixedLength, trainingMode);
                FillGamePage fillGamePage = new FillGamePage(fillGame);
                this.Hide();
                fillGamePage.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Не удалось создать игру, попробуйте снова.");
            }
        }
    }
}
