using System.Windows;
using PdfSaver;
using SerializerLib;
using System.Windows.Controls;
using System;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Linq;
using System.Threading;
using System.Windows.Interop;
using TermLib;

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

        private void ClearWorkPlace()
        {
            foreach (UIElement workPlaceChild in WorkPlace.Children)
            {
                if (workPlaceChild.Visibility == Visibility.Visible) workPlaceChild.Visibility = Visibility.Hidden;
            }
            //WorkPlace.Children.OfType<Canvas>().ToList().ForEach(x => WorkPlace.Children.Remove(x));
        }


        private ImageBrush getBrushFromImage(string path)
        {
            ImageBrush brush=new ImageBrush();
            System.Drawing.Image image;
            image = System.Drawing.Image.FromFile(path);
            var bitmap = new System.Drawing.Bitmap(image);
            var bitmapSource = Imaging.CreateBitmapSourceFromHBitmap(bitmap.GetHbitmap(),
                IntPtr.Zero,
                Int32Rect.Empty,
                BitmapSizeOptions.FromEmptyOptions()
            );
            bitmap.Dispose();
            brush = new ImageBrush(bitmapSource);
            return brush;
        }

        private void CrosswordItem_OnSelected(object sender, RoutedEventArgs e)
        {
            Export export = new Export();
            export.CaptureScreen();
            
        }

        private void DictionaryItem_Selected(object sender, RoutedEventArgs e)
        {
            ClearWorkPlace();
            ScrollDictionary.Visibility = Visibility.Visible;
            StackPanelForWords.Visibility = Visibility.Visible;
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
                    deleteBtn.Background = getBrushFromImage("image/delete.png");
                    Button editBtn = new Button
                    {
                        Height = 15,
                        Width = 15,
                        HorizontalAlignment = HorizontalAlignment.Right

                    };
                    editBtn.Background = getBrushFromImage("image/edit.png");
                    BtnPanel.Children.Add(deleteBtn);
                    BtnPanel.Children.Add(editBtn);
                    PanelForOneWord.Children.Add(BtnPanel);
                    StackPanelForWords.Children.Add(PanelForOneWord);
                    Separator separate = new Separator();
                    StackPanelForWords.Children.Add(separate);
                }
            }
        }

        private void BtnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void HomeItem_Selected(object sender, RoutedEventArgs e)
        {
            ClearWorkPlace();
            ScrollDictionary.Visibility = Visibility.Visible;
            ScrollDictionary.Height = 1;
            ScrollDictionary.Width = 1;
            Canvas Instructions = new Canvas { Height = 350, Width = 692, Visibility = Visibility.Visible};
            for(int i=0;i<7;i++)
            {
                ImageBrush brush;

                Rectangle photo = new Rectangle { Height = 32, Width = 32 };
                TextBlock text = new TextBlock { FontSize=15,Width=570, TextWrapping = TextWrapping.Wrap };
                if (i == 0)
                {
                    brush=getBrushFromImage("image/home.png");
                    text.Text = "Домашняя страница";
                }
                else if (i == 1) 
                {
                    brush = getBrushFromImage("image/edit.png"); 
                    text.Text = "Редактор определений и добавление терминов";

                }
                else if (i == 2) 
                {
                    brush = getBrushFromImage("image/book.png");
                    text.Text = "Словарь + поиск слов в нём";
                }
                else if (i == 3)
                {
                    brush = getBrushFromImage("image/match.png");
                    text.Text = "Сопоставление слов и определений";
                }
                else if (i == 4)
                {
                    brush = getBrushFromImage("image/fill in.png");
                    text.Text = "Вписать пропущенные слова ";
                }
                else if (i == 5)
                {
                    brush = getBrushFromImage("image/crossword.png"); 
                    text.Text = "Решение кроссворда";

                }
                else  
                { 
                    brush = getBrushFromImage("image/exit.png"); 
                    text.Text = "Вернуться на начальную страницу приложения с выбором предмета и класса";
                }
                photo.Fill = brush;
                Instructions.Children.Add(photo);
                Canvas.SetLeft(photo, 0);
                Canvas.SetTop(photo,i*50);
                Instructions.Children.Add(text);
                Canvas.SetLeft(text, 50);
                Canvas.SetTop(text, i * 50);
            }
            WorkPlace.Children.Add(Instructions);
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
            ClearWorkPlace();
            EditBTN.Content = "Добавить";
            EditStackPanel.Visibility = Visibility.Visible;

        }

        private void EditBTN_Click(object sender, RoutedEventArgs e)
        {
            if (EditBTN.Content == "Добавить")
            {
                if(!(string.IsNullOrEmpty(TermTB.Text)|| string.IsNullOrEmpty(DescriptionTB.Text)))
                {
                    try
                    {
                        Serializer.TermList.Add(new SimpleTerm(TermTB.Text, DescriptionTB.Text));
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

            }
        }

        private void CancelBTN_Click(object sender, RoutedEventArgs e)
        {
            EditItem_Selected(sender,e);
        }
    }
}
