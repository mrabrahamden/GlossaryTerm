using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using SerializerLib;
using TermLib;
using Button = System.Windows.Controls.Button;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;

namespace GlossaryTermApp
{
    /// <summary>
    /// Логика взаимодействия для FullScreenDictionaryPage.xaml
    /// </summary>
    public partial class FullScreenDictionaryPage : Window
    {
        private bool SearchMode = false;
        private MainWindow mainWindow;
        List<DockPanel> listOfPanels=new List<DockPanel>();
        public FullScreenDictionaryPage(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            SearchMode = false;
            PerformDictionaryPrint(mainWindow.Serializer.TermList);
        }

        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchTB.Text))
            {
                var request = SearchTB.Text;
                var result = mainWindow.Serializer.LookForAWord(request);
                SearchMode = true;
                PerformDictionaryPrint(result);
                SearchTB.Text = request;
            }
        }

        private void SearchEmptyButton_Click(object sender, RoutedEventArgs e)
        {
            SearchTB.Clear();
            PerformDictionaryPrint(mainWindow.Serializer.TermList);
        }

        private void PerformDictionaryPrint(List<SimpleTerm> list)
        {
            StackPanelForWords.Children.Clear();
            if (list.Count > 0)
            {
                foreach (var term in list)
                {
                    string wordAndDescription = term.ToString();
                    TextBlock newWord = new TextBlock { Text = wordAndDescription, TextWrapping = TextWrapping.Wrap, FontSize = 16, Padding = new Thickness(0, 0, 0, 0) };
                    DockPanel panelForOneWord = new DockPanel() { Margin = new Thickness(0, 5, 10, 5) ,Tag=term};
                    newWord.Width = Screen.PrimaryScreen.WorkingArea.Width - 100;
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
                        Background = Brushes.White,
                        Tag = term
                    };
                    deleteBtn.Click += DeleteBtn_Click;
                    Button editBtn = new Button
                    {
                        Height = 15,
                        Width = 15,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        FontFamily = new FontFamily("Segoe MDL2 Assets"),
                        Content = "\xE70F",
                        Background = Brushes.White,
                        Tag=term
                    };
                    editBtn.Click += EditBtn_Click;
                    btnPanel.Children.Add(deleteBtn);
                    btnPanel.Children.Add(editBtn);
                    panelForOneWord.Children.Add(btnPanel);
                    Separator separate = new Separator();
                    StackPanelForWords.Children.Add(panelForOneWord);
                    StackPanelForWords.Children.Add(separate);
                    listOfPanels.Add(panelForOneWord);
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

        private void EditBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainWindow.EditBtn_Click(sender,e);
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {

            var button = (Button)sender;
            var term = (SimpleTerm) button.Tag;
            var tag= button.Tag;
            foreach (var dockPanel in listOfPanels)
            {
                if (dockPanel.Tag == tag)
                    dockPanel.Visibility = Visibility.Hidden;
            }
            mainWindow.Serializer.DeleteTermByString(term.ToString());
            InitializeComponent();
            mainWindow.Serializer.Serialize();
            mainWindow.DictionaryItem_Selected(null,null);
            PerformDictionaryPrint(mainWindow.Serializer.TermList);
        }

        private void SearchTB_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SearchButton_OnClick(null, null);
            }
        }
    }
}
