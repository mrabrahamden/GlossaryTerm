using System.Linq;
using SerializerLib;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TermLib;

namespace GlossaryTermApp
{
    public partial class FillGameEditorPage : Window
    {
        private Serializer serializer;
        private MainWindow mainWindow;
        public FillGameEditorPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            this.serializer = mainWindow.Serializer;
            this.Closing += FillGameEditorPage_Closing;
            foreach (var term in serializer.TermList)
            {
                string wordAndDescription = term.Word + " ⸺ ";
                TextBlock newWord = new TextBlock { Text = wordAndDescription, TextWrapping = TextWrapping.Wrap, FontSize = 20 };
                WrapPanel panelForOneWord = new WrapPanel() { Margin = new Thickness(0, 5, 0, 8) };
                CheckBox isKey = new CheckBox() { Tag = term, VerticalContentAlignment = VerticalAlignment.Center};
                if (term.ReadyForFillGame)
                {
                    isKey.IsChecked = true;
                }

                if (!SomeDescriptionWordIsKey(term))
                {
                    DisableCheckBox(isKey, term);
                }
                isKey.Click += IsKey_Click;
                panelForOneWord.Children.Add(isKey);
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
                                Content = word,
                                FontSize = 20,
                                Tag = descriptionWord
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
                            TextBlock split = new TextBlock()
                            {
                                Text = word,
                                FontSize = 20
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

        private void FillGameEditorPage_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            string msg = "Сохранить?";
            MessageBoxResult result =
                MessageBox.Show(
                    msg,
                    "Внимание",
                    MessageBoxButton.OKCancel,
                    MessageBoxImage.Warning);
            if (result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
            else Button_Click(null, null);

        }

        private void IsKey_Click(object sender, RoutedEventArgs e)
        {
            var clickedCheckBox = (CheckBox)sender;
            var term = (SimpleTerm)clickedCheckBox.Tag;
            if (clickedCheckBox.IsChecked == true)
            {
                term.ReadyForFillGame = true;
            }
            else
            {
                term.ReadyForFillGame = false;
            }
        }

        private void ButtonOnClick(object sender, RoutedEventArgs e)
        {
            this.Closing -= FillGameEditorPage_Closing;
            this.Closing += FillGameEditorPage_Closing;
            BtnOk.FontFamily = new FontFamily("Segoe UI");
            BtnOk.Foreground = Brushes.Black;
            BtnOk.FontWeight = FontWeights.Regular;
            BtnOk.Content = "Готово";
            BtnOk.IsEnabled = true;
            Button clickedButton = (Button)sender;
            WrapPanel panel = (WrapPanel) clickedButton.Parent;
            CheckBox isKey = panel.Children.OfType<CheckBox>().First();
            var term = (SimpleTerm) isKey.Tag;
            var descriptionWord = (DescriptionWord)clickedButton.Tag;
            if (clickedButton.Background == Brushes.LightGreen)
            {
                clickedButton.Background = Brushes.LightGray;
                descriptionWord.IsKeyWord = false;
                if (!SomeDescriptionWordIsKey(term))
                {
                    DisableCheckBox(isKey, term);
                }
            }
            else
            {
                clickedButton.Background = Brushes.LightGreen;
                descriptionWord.IsKeyWord = true;
                isKey.IsEnabled = true;
                isKey.IsChecked = true;
                term.ReadyForFillGame = true;
            }
        }

        private static void DisableCheckBox(CheckBox isKey, SimpleTerm term)
        {
            isKey.IsChecked = false;
            isKey.IsEnabled = false;
            term.ReadyForFillGame = false;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Closing -= FillGameEditorPage_Closing;
            BtnOk.FontFamily = new FontFamily("Segoe MDL2 Assets");
            BtnOk.Foreground = Brushes.MediumSeaGreen;
            BtnOk.FontWeight = FontWeights.Bold;
            BtnOk.Content = "\xE73E" + " ";
            BtnOk.IsEnabled = false;
            serializer.Serialize();
            mainWindow.FillGameCountUpDown.Maximum = serializer.TermList.FindAll((term => term.ReadyForFillGame)).Count;
            mainWindow.FillGameCountUpDown.Value = mainWindow.FillGameCountUpDown.Maximum;
        }

        private bool SomeDescriptionWordIsKey(SimpleTerm term)
        {
            foreach (var word in term.DescriptionWordsAndSplittersList)
            {
                if (word.IsKeyWord)
                    return true;
            }
            return false;
        }
    }
}
