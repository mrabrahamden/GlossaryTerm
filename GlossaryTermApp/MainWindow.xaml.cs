using System.Windows;
using PdfSaver;
using SerializerLib;
using System.Windows.Controls;
using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;

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

        private void DictionaryItem_Selected(object sender, RoutedEventArgs e)
        {
            WorkPlace.Children.OfType<Canvas>().ToList().ForEach(x => WorkPlace.Children.Remove(x));// !!!!!!!
            ScrollDictionary.Visibility = Visibility.Visible;
            ScrollDictionary.Height = 350;
            ScrollDictionary.Width = 692;
            StackPanelForWords.Children.Clear();
            if (Serializer.TermList.Count > 0)
            {
                foreach (var term in Serializer.TermList)
                {
                    string WordAndDiscription = term.ToString();
                    TextBlock newWord = new TextBlock { Text = WordAndDiscription, TextWrapping=TextWrapping.Wrap };
                    DockPanel PanelForOneWord = new DockPanel();
                    PanelForOneWord.Children.Add(newWord);
                    StackPanel BtnPanel = new StackPanel();
                    Button deleteBtn = new Button
                    {
                        Height = 15,
                        Width = 15, 
                        HorizontalAlignment = HorizontalAlignment.Right
                    };
                    var brush = new ImageBrush();
                    brush.ImageSource = new BitmapImage(new Uri("delete.png", UriKind.Relative));
                    deleteBtn.Background = brush;
                    Button editBtn = new Button
                    {
                        Height = 15,
                        Width = 15,
                        HorizontalAlignment = HorizontalAlignment.Right
                    };
                    var brushEd = new ImageBrush();
                    brushEd.ImageSource = new BitmapImage(new Uri("edit.png", UriKind.Relative));
                    editBtn.Background = brushEd;
                    BtnPanel.Children.Add(deleteBtn);
                    BtnPanel.Children.Add(editBtn);
                    PanelForOneWord.Children.Add(BtnPanel);
                    StackPanelForWords.Children.Add(PanelForOneWord);
                    Separator separate = new Separator();
                    StackPanelForWords.Children.Add(separate);
                }
            }
        }

        private void HomeItem_Selected(object sender, RoutedEventArgs e)
        {
            ScrollDictionary.Visibility = Visibility.Hidden;
            WorkPlace.Children.OfType<Canvas>().ToList().ForEach(x => WorkPlace.Children.Remove(x));
            ScrollDictionary.Height = 1;
            ScrollDictionary.Width = 1;
            WorkPlace.Visibility = Visibility.Visible;
            Canvas Instructions = new Canvas { Height = 350, Width = 692 };
            for(int i=0;i<8;i++)
            {
                Rectangle photo = new Rectangle { Height = 30, Width = 30 };
                ImageBrush brush = new ImageBrush();
                if (i == 0)
                {
                    brush.ImageSource = new BitmapImage(new Uri(@"home.png", UriKind.RelativeOrAbsolute));
                }
                else if (i == 1) { brush.ImageSource = new BitmapImage(new Uri(@"edit.png", UriKind.RelativeOrAbsolute)); }
                else if (i == 2) { brush.ImageSource = new BitmapImage(new Uri(@"book.png", UriKind.RelativeOrAbsolute)); }
                else if (i == 3) { brush.ImageSource = new BitmapImage(new Uri(@"home.png", UriKind.RelativeOrAbsolute)); }
                else if (i == 4) { brush.ImageSource = new BitmapImage(new Uri(@"match.png", UriKind.RelativeOrAbsolute)); }
                else if (i == 5) { brush.ImageSource = new BitmapImage(new Uri(@"fill in.png", UriKind.RelativeOrAbsolute)); }
                else if (i == 6) { brush.ImageSource = new BitmapImage(new Uri(@"crossword.png", UriKind.RelativeOrAbsolute)); }
                else  { brush.ImageSource = new BitmapImage(new Uri(@"home.png", UriKind.RelativeOrAbsolute)); }
                photo.Fill = brush;
                Instructions.Children.Add(photo);
                Canvas.SetLeft(photo, 0);
                Canvas.SetTop(photo,i*48);
            }
            WorkPlace.Children.Add(Instructions);
        }
    }
}
