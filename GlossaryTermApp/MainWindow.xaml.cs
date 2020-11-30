using CrosswordLib;
using FillGameLib;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MatchGameLib;
using SerializerLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using TermLib;
using Brushes = System.Windows.Media.Brushes;
using Button = System.Windows.Controls.Button;
using Color = iTextSharp.text.Color;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;

namespace GlossaryTermApp
{
    public partial class MainWindow : Window
    {

        public Serializer Serializer;

        public MainWindow(Serializer ser)
        {
            Serializer = ser;
            Serializer.DeleteSimilarTerms();
            Serializer.SortList();
            InitializeComponent();
            _editedTerm = new SimpleTerm("", "");
        }


        private void ClearWorkPlace()
        {
            foreach (UIElement workPlaceChild in WorkPlace.Children)
            {
                if (workPlaceChild.Visibility == Visibility.Visible) workPlaceChild.Visibility = Visibility.Hidden;
            }
            SearchTB.Clear();
        }

        public bool SearchMode = false;
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
            CrosswordStackPanel.Visibility = Visibility.Visible;
        }

        public void DictionaryItem_Selected(object sender, RoutedEventArgs e)
        {
            SearchMode = false;
            PerformDictionaryPrint(Serializer.TermList);
        }

        public void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var btnPanel = (StackPanel)button.Parent;
            var panelForOneWord = (DockPanel)btnPanel.Parent;
            var newWord = (TextBlock)panelForOneWord.Children.OfType<TextBlock>().First();
            var term = (SimpleTerm)panelForOneWord.Tag;
            MenuEditBTN.IsSelected = true;
            var wordAndDescr = newWord.Text;
            _editMode = true;
            EditBTN.Content = "Изменить";
            ClearWorkPlace();
            ReadyToAddAWord(sender, e);
            TermTB.Text = null;
            DescriptionTB.Text = null;
            EditStackPanel.Visibility = Visibility.Visible;
            TermTB.Text = term.Word;
            DescriptionTB.Text = term.Description;
            _editedTerm = term;
            _editedTerm.FillingListsForFillGame();
        }

        private bool _editMode = false;
        private SimpleTerm _editedTerm;
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var btnPanel = (StackPanel)button.Parent;
            var panelForOneWord = (DockPanel)btnPanel.Parent;
            var newWord = (TextBlock)panelForOneWord.Children.OfType<TextBlock>().First();
            panelForOneWord.Visibility = Visibility.Hidden;
            var term = (SimpleTerm)panelForOneWord.Tag;
            Serializer.TermList.Remove(term);
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
            Serializer.Serialize();
            this.Hide();
            StartPage startPage = new StartPage();
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
            if ((!(string.IsNullOrEmpty(TermTB.Text) || string.IsNullOrEmpty(DescriptionTB.Text))))
            {
                if (!_editMode)
                {

                    try
                    {
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
                    var termEdited = new SimpleTerm(TermTB.Text, DescriptionTB.Text);
                    var hasSimilar = (from t in Serializer.TermList where t.Word == termEdited.Word && t.Description == termEdited.Description select t).Count();
                    if (hasSimilar!=0)
                    {
                        MessageBox.Show("Данный термин уже внесён в словарь!");
                    }
                    else
                    {
                        _editedTerm.Word = termEdited.Word;
                        _editedTerm.Description = termEdited.Description;
                        editingSuccess = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Нужно заполнить поля термин и описание перед добавлением.");
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
                SearchMode = true;
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
            ScrollDictionary.Width = 720;
            StackPanelForWords.Children.Clear();
            if (list.Count > 0)
            {
                foreach (var term in list)
                {
                    string wordAndDescription = term.ToString();
                    TextBlock newWord = new TextBlock { Text = wordAndDescription, TextWrapping = TextWrapping.Wrap, FontSize = 16, Padding = new Thickness(0, 0, 5, 0), Width = 676 };
                    DockPanel panelForOneWord = new DockPanel() { Margin = new Thickness(0, 5, 0, 5) ,Tag=term};
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
                TextBlock newWord = new TextBlock { TextWrapping = TextWrapping.Wrap, FontSize = 16 };

                if (SearchMode)
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

        private void SearchEmptyButton_Click(object sender, RoutedEventArgs e)
        {
            DictionaryItem_Selected(null, null);
        }
        private void FillGameItem_Selected(object sender, RoutedEventArgs e)
        {
            ClearWorkPlace();
            FillGameCountUpDown.Maximum = Serializer.TermList.FindAll((term => term.ReadyForFillGame)).Count;
            FillGameCountUpDown.Minimum = 1;
            FillGameCountUpDown.Value = FillGameCountUpDown.Maximum;
            FillGameEditorPanel.Visibility = Visibility.Visible;
        }

        private void FillGameEditorBTN_Click(object sender, RoutedEventArgs e)
        {
            if (Serializer.TermList.Count > 0)
            {
                FillGameEditorPage fillGameEditorPage = new FillGameEditorPage(this);
                fillGameEditorPage.ShowDialog();
            }
            else
            {
                MessageBox.Show("Сначала добавьте слов в словарь.");
            }
        }

        private FillGame fillGame = null;
        private bool isPdfSaving = false;
        private void FillGameStartBTN_Click(object sender, RoutedEventArgs e)
        {
            bool isCreated = false;
            int lvl = 0;
            bool fixedLength = false;
            bool trainingMode = false;
            if (FillGameCountUpDown.Value > 0)
            {
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
                    fillGame = new FillGame(Serializer.TermList, lvl, (int)FillGameCountUpDown.Value, fixedLength, trainingMode);
                    if (!isPdfSaving)
                    {
                        FillGamePage fillGamePage = new FillGamePage(fillGame);
                        fillGamePage.ShowDialog();
                    }
                    isPdfSaving = false;
                }
                else
                {
                    MessageBox.Show("Не удалось создать игру, попробуйте снова.");
                }
            }
            else
            {
                MessageBox.Show("В словаре недостаточно терминов для начала. Попробуйте добавить слов и выбрать их в редакторе.");
            }

        }

        private void MatchGameItem_Selected(object sender, RoutedEventArgs e)
        {
            ClearWorkPlace();
            MatchGameCountUpDown.Minimum = 2;
            MatchGameCountUpDown.Maximum = Serializer.TermList.Count;
            MatchGameEditorPanel.Visibility = Visibility.Visible;
        }

        private void MatchGameStartBTN_Click(object sender, RoutedEventArgs e)
        {
            if (Serializer.TermList.Count > 2)
            {
                _matchGame = new MatchGame(Serializer.TermList, (int)MatchGameCountUpDown.Value,
                    (bool)MatchGameTrainingMode.IsChecked);
                _matchGame.IsReady = true;
                if (!isPdfSaving)
                {
                    MatchGamePage matchGamePage = new MatchGamePage(_matchGame);
                    matchGamePage.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("В словаре недостаточно терминов для начала.");
            }
        }

        private CrosswordGame _crosswordGame = null;
        private MatchGame _matchGame = null;

        private void MatchGameSaveBTN_OnClick(object sender, RoutedEventArgs e)
        {
            isPdfSaving = true;
            MatchGameStartBTN_Click(sender, e);
            if (_matchGame.IsReady)
            {
                MatchGameSaveToPdf(_matchGame);
            }
        }

        private void CrosswordStartBTN_Click(object sender, RoutedEventArgs e)
        {
            int lvl = 0;
            if (CrosswordEasyLvl.IsChecked == true)
            {
                lvl = 1;
            }
            else if (CrosswordNormalLvl.IsChecked == true)
            {
                lvl = 2;
            }
            else
            {
                lvl = 3;
            }

            _crosswordGame = new CrosswordGame(Serializer.TermList, lvl);
            if (_crosswordGame.IsReady)
            {
                if (!isPdfSaving)
                {
                    CrosswordGamePage crosswordGamePage = new CrosswordGamePage(_crosswordGame);
                    crosswordGamePage.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Не удалось создать кроссворд, попробуйте добавить больше слов в словарь.");
            }
        }

        private void ButtonFullScreen_OnClick(object sender, RoutedEventArgs e)
        {
            FullScreenDictionaryPage fullScreenDictionaryPage = new FullScreenDictionaryPage(this);
            fullScreenDictionaryPage.ShowDialog();
        }

        private void SearchTB_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchButton_OnClick(null, null);
            }
        }

        private void FillGameSaveBTN_OnClick(object sender, RoutedEventArgs e)
        {
            isPdfSaving = true;
            FillGameStartBTN_Click(null, null);
            new FillGamePage(fillGame).SaveToPdf(sender, e);
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Serializer.Serialize();
        }

        private void BtnDictionarySavePdf_OnClick(object sender, RoutedEventArgs e)
        {
            new FullScreenDictionaryPage(this).BtnSavePdf_Click(sender, e);
        }

        private void CrosswordSaveBTN_OnClick(object sender, RoutedEventArgs e)
        {
            isPdfSaving = true;
            CrosswordStartBTN_Click(sender, e);
            if (_crosswordGame.IsReady)
            {
                CrosswordSaveToPdf(_crosswordGame);
            }

        }

        private void CrosswordSaveToPdf(CrosswordGame game)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "pdf files (*.pdf)|*.pdf";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            saveFileDialog.RestoreDirectory = false;
            DialogResult result = saveFileDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = saveFileDialog.FileName;
                FileStream fStream = new FileStream(Path.Combine(fileName), FileMode.Create);
                Document document = new Document(PageSize.A4, 10, 10, 50, 10);
                PdfWriter writer = PdfWriter.GetInstance(document, fStream);
                document.Open();
                //шрифт для кириллицы
                BaseFont baseFont = BaseFont.CreateFont("image/arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL);
                //создание таблицы
                Phrase task = new Phrase("Решите кроссворд.", font);
                Paragraph header = new Paragraph(task);
                header.Alignment = Element.ALIGN_CENTER;
                header.SpacingAfter = 30;
                document.Add(header);
                int count = 1;

                PdfPTable table = new PdfPTable(game.CrosswordMatrix.GetLength(1));
                table.DefaultCell.Border = Rectangle.NO_BORDER;
                char _nochar = ' ';
                float _cellHeight = (document.PageSize.Width - document.LeftMargin - document.RightMargin) / game.CrosswordMatrix.GetLength(1);
                for (int i = 0; i < game.CrosswordMatrix.GetLength(0); i++)
                {
                    for (int j = 0; j < game.CrosswordMatrix.GetLength(1); j++)
                    {
                        if (game.CrosswordMatrix[i, j] != null)
                        {
                            if (game.CrosswordMatrix[i, j].Letter != _nochar)
                            {
                                if (game.MainWordHorizontalIndex == j)
                                    table.AddCell(new PdfPCell()
                                    { BackgroundColor = Color.LIGHT_GRAY, FixedHeight = _cellHeight });
                                else
                                    table.AddCell(new PdfPCell() { FixedHeight = _cellHeight });
                            }
                            else
                            {
                                table.AddCell(new PdfPCell() { Border = Rectangle.NO_BORDER, FixedHeight = _cellHeight });
                            }
                        }
                        else
                        {
                            table.AddCell(new PdfPCell() { Border = Rectangle.NO_BORDER, FixedHeight = _cellHeight });
                        }
                    }

                }
                table.SpacingAfter = 30;
                document.Add(table);

                task = new Phrase("По вертикали: ", font);
                header = new Paragraph(task);
                header.Alignment = Element.ALIGN_LEFT;
                header.SpacingAfter = 30;
                document.Add(header);

                var mainWord = game.CrossWordTerms[0];
                Phrase phrase = new Phrase(count + ". " + mainWord.Description, font);
                Paragraph paragraph = new Paragraph(phrase);
                paragraph.SpacingAfter = 30;
                document.Add(paragraph);
                count++;

                task = new Phrase("По горизонтали: ", font);
                header = new Paragraph(task);
                header.Alignment = Element.ALIGN_LEFT;
                header.SpacingAfter = 30;
                document.Add(header);

                for (int i = 1; i < game.CrossWordTerms.Length - 1; i++)
                {
                    if (game.CrossWordTerms[i] != null)
                    {
                        phrase = new Phrase(count + ". " + game.CrossWordTerms[i].Description, font);
                        paragraph = new Paragraph(phrase);
                        paragraph.SpacingAfter = 10;
                        document.Add(paragraph);
                        count++;
                    }
                }

                document.Close();
                writer.Close();
                fStream.Close();
                var msgBoxResult = MessageBox.Show("Просмотреть файл?", "PDF", MessageBoxButton.YesNo, MessageBoxImage.Question,
                    MessageBoxResult.Yes);
                if (msgBoxResult == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start(fileName);
                }
            }
        }


        private void MatchGameSaveToPdf(MatchGame game)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "pdf files (*.pdf)|*.pdf";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            saveFileDialog.RestoreDirectory = true;
            DialogResult result = saveFileDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                List<string> listOfTerms = (from t in game.TermList select t.Word).ToList();
                List<string> listOfDescr = (from t in game.TermList select t.Description).ToList();
                string fileName = saveFileDialog.FileName;
                FileStream fStream = new FileStream(Path.Combine(fileName), FileMode.Create);
                Document document = new Document(PageSize.A4, 10, 10, 50, 10);
                PdfWriter writer = PdfWriter.GetInstance(document, fStream);
                document.Open();
                //шрифт для кириллицы
                BaseFont baseFont = BaseFont.CreateFont("image/arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL);
                //создание таблицы
                Phrase task = new Phrase("Соедините термин из левой колонки с его определением.", font);
                Paragraph header = new Paragraph(task);
                header.Alignment = Element.ALIGN_CENTER;
                header.SpacingAfter = 30;
                document.Add(header);
                Random random = new Random(DateTime.Now.Millisecond);
                int count = 0;
                PdfPTable table = new PdfPTable(2);
                table.DefaultCell.Border = Rectangle.NO_BORDER;

                while (listOfTerms.Count > 0)
                {
                    count = random.Next() % listOfTerms.Count;
                    table.AddCell(new Phrase(listOfTerms[count], font));
                    listOfTerms.RemoveAt(count);
                    count = random.Next() % listOfDescr.Count;
                    table.AddCell(new Phrase(listOfDescr[count], font));
                    listOfDescr.RemoveAt(count);
                    //добавим пробел, чтобы ряды не стояли плотно
                    PdfPCell cell = new PdfPCell(new Phrase(""));
                    cell.Colspan = 2;
                    cell.FixedHeight = 8;
                    cell.Border = Rectangle.NO_BORDER;
                    table.AddCell(cell);
                }
                document.Add(table);
                document.Close();
                writer.Close();
                fStream.Close();
                var msgBoxResult = MessageBox.Show("Просмотреть файл?", "PDF", MessageBoxButton.YesNo, MessageBoxImage.Question,
                    MessageBoxResult.Yes);
                if (msgBoxResult == MessageBoxResult.Yes)
                {
                    System.Diagnostics.Process.Start(fileName);
                }
            }
        }

    }
}
