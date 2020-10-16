using System.Configuration;
using System.Windows;
using PdfSaver;
using TermLib;
using SerializerLib;
using System.Windows.Controls;
using System.Drawing;

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
            InitializeComponent();
        }


        private void CrosswordItem_OnSelected(object sender, RoutedEventArgs e)
        {
            Export export = new Export();
            export.CaptureScreen();
            
        }

        private void HamburgerMenuItem_Selected(object sender, RoutedEventArgs e)
        {
            ScrollDictionary.Visibility = Visibility.Visible;
            StackPanelForWords.Children.Clear();
            if (Serializer.TermList.Count > 0)
            {
                foreach (var term in Serializer.TermList)
                {
                    string WordAndDiscription = term.ToString();
                    TextBlock newWord = new TextBlock { Text = WordAndDiscription };
                    Button deleteBtn = new Button
                    {
                        Content = "Удалить",
                        Width = 20
                    };
                    StackPanelForWords.Children.Add(newWord);
                    StackPanelForWords.Children.Add(deleteBtn);
                    Separator separate = new Separator();
                    StackPanelForWords.Children.Add(separate);
                    //на форме есть ScrollViewer , а на нем StackPanel(безразмерная, чтобы работала прокрутка Scroll)
                    // и в эту панель добавляем TextBlock  и Button, а потом разделитель.(У Панели ориентация вертикальная)
                    //во второй строке мы очищаем список детей, чтобы очистить панель, а не переписывать в неее снова старый список
                }
            }
        }
    }
}
