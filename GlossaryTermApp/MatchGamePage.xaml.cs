using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using MatchGameLib;
using TermLib;
using Color = System.Windows.Media.Color;
using iTextSharp.text;
using iTextSharp.text.pdf;
using DataFormats = System.Windows.DataFormats;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;
using Orientation = System.Windows.Controls.Orientation;

namespace GlossaryTermApp
{
    public partial class MatchGamePage : Window
    {
        public MatchGame MatchGame;
        public MatchGamePage(MatchGame matchGame)
        {
            InitializeComponent();
            MatchGame = matchGame;
            PrepareForm();
        }
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

        private StackPanel _wordsStackPanel;
        private List<TextBlock> _listTextBlocksForCheck;
        private List<Border> _listOfWordBorders;
        private void PrepareForm()
        { 
            _wordsStackPanel = new StackPanel();
            _listTextBlocksForCheck=new List<TextBlock>();
            _listOfWordBorders=new List<Border>();
            _wordsStackPanel.Background = (SolidColorBrush) new BrushConverter().ConvertFromString("#F2F3F4");
            _wordsStackPanel.Margin = new Thickness(0, 0, 0, 10);
            ForStackPanelScrollViewer.Content = _wordsStackPanel;
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
                    FontSize = 20,
                    Tag = term,
                    TextAlignment = TextAlignment.Left,
                    Background = (SolidColorBrush) new BrushConverter().ConvertFromString("#F9F7A8"),
                    TextWrapping = TextWrapping.Wrap,
                    Padding = new Thickness(5, 2, 5, 2),
                    Margin = new Thickness(10, 10, 5, 0)
                };
                TextBlock placeForWordTextBlock = new TextBlock()
                {
                    AllowDrop = true,
                    FontSize = 18,
                    Tag = term,
                    Background = new SolidColorBrush(Color.FromRgb(202, 207, 210)),
                    Margin = new Thickness(15, 10, 10, 0),
                    Width = 200,
                    TextAlignment = TextAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center
                };
                _listTextBlocksForCheck.Add(placeForWordTextBlock);
                placeForWordTextBlock.Drop += PlaceForWordRectangle_Drop;
                placeForWordTextBlock.MouseDown += PlaceForWordTextBlock_MouseDown;
                wrapPanel.Children.Add(descriptionTextBlock);
                TextBlock correctOrNotSign = new TextBlock()
                {
                    Margin = new Thickness(10,10,0,0),
                    Height = 22,
                    Width = 22
                };
                termStackPanel.Children.Add(placeForWordTextBlock);
                dockPanel.Children.Add(correctOrNotSign);
                dockPanel.Children.Add(termStackPanel);
                dockPanel.Children.Add(wrapPanel);
                _wordsStackPanel.Children.Add(dockPanel);

                TextBlock wordTextBlock = new TextBlock()
                {
                    Text = term.Word,
                    FontSize = 18,
                    Tag = term,
                    Background = (SolidColorBrush) new BrushConverter().ConvertFromString("#F08080"),
                    Padding = new Thickness(5, 1, 5, 1)
                };

                Border forTextBlock = new Border()
                {
                    BorderThickness = new Thickness(2),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(202, 207, 210)),
                    Margin = new Thickness(10, 10, 0, 0),
                    Child = wordTextBlock,
                    Tag = wordTextBlock.Tag
                };

                forTextBlock.MouseDown += ForTextBlock_MouseDown;

                _listOfWordBorders.Add(forTextBlock);
            }
            ShuffleList();
            foreach (var border in _listOfWordBorders)
            {
                WordsWrapPanel.Children.Add(border);
            }
        }

        private void PlaceForWordTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var textblock = (TextBlock) sender;
            foreach (var border in _listOfWordBorders)
            {
                var term = (SimpleTerm)border.Tag;
                if (term.Word == textblock.Text)
                    border.Visibility = Visibility.Visible;
            }
            textblock.Text = "";
            textblock.Background = new SolidColorBrush(Color.FromRgb(202, 207, 210));

        }

        private void ShuffleList()
        {
            List<Border> newList=new List<Border>();
            Random random=new Random(DateTime.Now.Millisecond);
            while(_listOfWordBorders.Count>0)
            {
                int count= random.Next() % _listOfWordBorders.Count;
                newList.Add(_listOfWordBorders[count]);
                _listOfWordBorders.RemoveAt(count);
            }
            _listOfWordBorders=new List<Border>(newList);
        }
        private void PlaceForWordRectangle_Drop(object sender, DragEventArgs e)
        {
            var textblock = (TextBlock) sender;
            if (textblock.Text.Length > 0)
            {
                foreach (var border in _listOfWordBorders)
                {
                    var term = (SimpleTerm)border.Tag;
                    if (term.Word == textblock.Text)
                        border.Visibility = Visibility.Visible;
                }
            }
            textblock.Text = (string)e.Data.GetData(DataFormats.Text);
            textblock.Background = (SolidColorBrush) new BrushConverter().ConvertFromString("#F08080");
            foreach (var border in _listOfWordBorders)
            {
                var term = (SimpleTerm) border.Tag;
                if (term.Word == textblock.Text)
                    border.Visibility = Visibility.Hidden;
            }
        }

        private void ForTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var border = (Border) sender;
            var textblock = (TextBlock) border.Child;
            DragDrop.DoDragDrop(border,textblock.Text, DragDropEffects.Copy);
        }

        private void CheckBTN_OnClick(object sender, RoutedEventArgs e)
        {
            var countTrue = 0;
            foreach (var textBlock in _listTextBlocksForCheck)
            {
                var term = (SimpleTerm) textBlock.Tag;
                var smallStackPanel = (StackPanel)textBlock.Parent;
                var dockPanel = (DockPanel) smallStackPanel.Parent;
                var correctOrNotSign = (TextBlock) dockPanel.Children.OfType<TextBlock>().First();
                if (term.Word == textBlock.Text)
                {
                    countTrue++;
                    textBlock.Background = Brushes.LightGreen;
                    correctOrNotSign.Background = GetBrushFromImage("image/true.png");
                }
                else
                {
                    textBlock.Background = Brushes.LightCoral;
                    correctOrNotSign.Background = GetBrushFromImage("image/false.png");
                }
                
            }

            if (!MatchGame.TrainingMode)
            {
                GameResult resultWindow =
                    new GameResult(MatchGame.NumOfTerms - countTrue, MatchGame.NumOfTerms);
                resultWindow.ShowDialog();
                this.Close();
            }
        }

        private void SaveToPdf(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "pdf files (*.pdf)|*.pdf";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            saveFileDialog.RestoreDirectory = true;
            DialogResult result = saveFileDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                List<string> listOfTerms = (from t in MatchGame.TermList select t.Word).ToList();
                List<string> listOfDescr = (from t in MatchGame.TermList select t.Description).ToList();
                string fileName = saveFileDialog.FileName;
                FileStream fStream = new FileStream(Path.Combine(fileName), FileMode.Create);
                Document document = new Document(PageSize.A4, 10, 10, 50, 10);
                PdfWriter writer = PdfWriter.GetInstance(document, fStream);
                document.Open();
                //шрифт для кириллицы
                BaseFont baseFont = BaseFont.CreateFont("image/arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL);
                //создание таблицы
                Phrase task = new Phrase("Соедините термин из левой колонки с его определением.",font);
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
            }
        }

        private void SaveBTN_OnClick(object sender, RoutedEventArgs e)
        {
            SaveToPdf(sender,e);
        }
    }

}
