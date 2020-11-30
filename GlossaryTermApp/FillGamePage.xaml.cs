using FillGameLib;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using TermLib;
using MessageBox = System.Windows.MessageBox;
using TextBox = System.Windows.Controls.TextBox;

namespace GlossaryTermApp
{
    public partial class FillGamePage : Window
    {
        private FillGame game;
        private List<SimpleTerm> list;
        private List<bool> listOfSkippedWords;
        private int numOfSkipWords = 0;
        private List<StringBuilder> listOfBuilders = new List<StringBuilder>();
        public FillGamePage(FillGame game)
        {
            InitializeComponent();
            this.game = game;
            this.list = GetRandomList(game.List, game.Count);
                game.List.FindAll((term => term.ReadyForFillGame)).ToList();

            if (list.Count > 0)
            {
                int count = 1;
                foreach (var gameWord in list)
                {
                    string word = gameWord.Word;                 //вывод самого термина
                    listOfSkippedWords = GetNumOfSkippedWords(game.Lvl,
                        gameWord.DescriptionWordsAndSplittersList
                            .FindAll((descriptionWord => descriptionWord.IsKeyWord)).Count);
                    numOfSkipWords += listOfSkippedWords.FindAll((b => b == true)).Count;
                    TextBlock newWord = new TextBlock { Text = word + " ⸺ ", TextWrapping = TextWrapping.Wrap, FontSize = 20, FontWeight = FontWeights.Bold };
                    WrapPanel panelForOneWord = new WrapPanel();
                    panelForOneWord.VerticalAlignment = VerticalAlignment.Top;
                    panelForOneWord.Children.Add(newWord);

                    StringBuilder termBuilder = new StringBuilder();

                    termBuilder.Append(count.ToString() + ". " + word.ToString() + " - ");
                    count++;
                    foreach (var wordPart in gameWord.DescriptionWordsAndSplittersList) //печать слов из определения
                    {
                        if (wordPart.IsKeyWord && listOfSkippedWords.Count > 0)
                        {
                            if (listOfSkippedWords.First() == true)
                            {
                                TextBox skippedWord = new TextBox()
                                { FontSize = 20, MinWidth = 20, Tag = wordPart.Word };
                                skippedWord.GotKeyboardFocus += SkippedWordOnGotKeyboardFocus;
                                if (game.FixedLength)
                                {
                                    skippedWord.MaxLength = wordPart.Word.Length;
                                }

                                panelForOneWord.Children.Add(skippedWord);

                                for (int i = 0; i < wordPart.Word.Length; i++)
                                {
                                    termBuilder.Append("_");
                                }
                            }
                            else
                            {
                                TextBlock skippedWord = new TextBlock()
                                { Text = wordPart.Word, FontSize = 20, TextWrapping = TextWrapping.Wrap };
                                panelForOneWord.Children.Add(skippedWord);

                                termBuilder.Append(wordPart.Word);
                            }

                            listOfSkippedWords.RemoveAt(0);
                        }
                        else
                        {
                            TextBlock notSkippedWord = new TextBlock()
                            { Text = wordPart.Word, FontSize = 20, TextWrapping = TextWrapping.Wrap };
                            panelForOneWord.Children.Add(notSkippedWord);

                            termBuilder.Append(wordPart.Word);
                        }

                    }
                    termBuilder.Append(".");
                    listOfBuilders.Add(termBuilder);
                    Separator separate = new Separator();
                    panelForOneWord.Margin = new Thickness(0, 7, 0, 7);
                    stackPanelOutput.Children.Add(panelForOneWord);
                    stackPanelOutput.Children.Add(separate);
                }
            }
        }

        private List<SimpleTerm> GetRandomList(List<SimpleTerm> list, int count)
        {
            var readySimpleTerms = game.List.FindAll((term => term.ReadyForFillGame)).ToList();
            Random random=new Random(DateTime.Now.Millisecond);
            List<SimpleTerm> resultList=new List<SimpleTerm>();
            int randomIndex = random.Next() % readySimpleTerms.Count;
            for (int i = 0; i < count; i++)
            {
                randomIndex = random.Next() % readySimpleTerms.Count;
                resultList.Add(readySimpleTerms[randomIndex]);
                readySimpleTerms.RemoveAt(randomIndex);
            }
            return resultList;
        }

        private void SkippedWordOnGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
        {
            var textBox = (TextBox)sender;
            textBox.Background = default;
        }


        private List<bool> GetNumOfSkippedWords(int lvl, int num)
        {
            List<bool> listOfBools = new List<bool>();
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


            Random random = new Random();
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
                        if (listOfBools.Count > num)
                            listOfBools.Remove(false);
                        if (listOfBools.Count < num)
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
                    var wrapPanel = (WrapPanel)child;
                    foreach (var child2 in wrapPanel.Children)
                    {
                        if (child2.GetType() == typeof(TextBox))
                        {
                            var textbox = (TextBox)child2;
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
                GameResult gameResult = new GameResult(numOfErrors, numOfSkipWords);
                gameResult.ShowDialog();
                this.Close();
            }

            numOfErrors = 0;
        }
        public void SaveToPdf(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "pdf files (*.pdf)|*.pdf";
            saveFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.CommonDocuments);
            saveFileDialog.RestoreDirectory = true;
            DialogResult result = saveFileDialog.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                string fileName = saveFileDialog.FileName;
                FileStream fStream = new FileStream(Path.Combine(fileName), FileMode.Create);
                Document document = new Document(PageSize.A4, 40, 40, 50, 50);
                PdfWriter writer = PdfWriter.GetInstance(document, fStream);
                document.Open();
                //шрифт для кириллицы
                BaseFont baseFont = BaseFont.CreateFont("image/arial.ttf", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                Font font = new Font(baseFont, Font.DEFAULTSIZE, Font.NORMAL);

                Phrase task = new Phrase("Вставьте пропущенные слова.", font);
                Paragraph header = new Paragraph(task);
                header.Alignment = Element.ALIGN_CENTER;
                header.SpacingAfter = 30;
                document.Add(header);



                foreach (var term in listOfBuilders)
                {
                    Phrase phrase = new Phrase(term.ToString(), font);
                    Paragraph paragraph = new Paragraph(phrase);
                    document.Add(paragraph);
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

    }
    ////https://github.com/xceedsoftware/wpftoolkit/wiki/IntegerUpDown про UpDown
}
