using System.Windows;
using PdfSaver;
using SerializerLib;
using System.Windows.Controls;
using System;
using System.Drawing;
using System.Windows.Automation;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Interop;
using TermLib;
using Brushes = System.Windows.Media.Brushes;
using FontFamily = System.Windows.Media.FontFamily;
using Image = System.Drawing.Image;
using Rectangle = System.Windows.Shapes.Rectangle;

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
            Export export = new Export();
            export.CaptureScreen();
        }

        private void DictionaryItem_Selected(object sender, RoutedEventArgs e)
        {
            ClearWorkPlace();
            DictionaryStackPanel.Visibility = Visibility.Visible;
            ScrollDictionary.Visibility = Visibility.Visible;
            StackPanelForWords.Visibility = Visibility.Visible;
            ScrollDictionary.Height = 350;
            ScrollDictionary.Width = 692;
            StackPanelForWords.Children.Clear();
            if (Serializer.TermList.Count > 0)
            {
                foreach (var term in Serializer.TermList)
                {
                    string wordAndDescription = term.ToString();
                    TextBlock newWord = new TextBlock { Text = wordAndDescription, TextWrapping=TextWrapping.Wrap };
                    DockPanel panelForOneWord = new DockPanel();
                    panelForOneWord.Children.Add(newWord);
                    StackPanel btnPanel = new StackPanel();
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
                    btnPanel.Children.Add(deleteBtn);
                    btnPanel.Children.Add(editBtn);
                    panelForOneWord.Children.Add(btnPanel);
                    StackPanelForWords.Children.Add(panelForOneWord);
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
            Canvas instructions = new Canvas { Height = 350, Width = 692, Visibility = Visibility.Visible};
            for(int i=0;i<7;i++)
            {
                ImageBrush brush;

                Rectangle photo = new Rectangle { Height = 32, Width = 32 };
                TextBlock text = new TextBlock { FontSize=15,Width=570, TextWrapping = TextWrapping.Wrap };
                switch (i)
                {
                    case 0:
                        brush = getBrushFromImage("image/home.png");
                        text.Text = "Домашняя страница";
                        break;
                    case 1:
                        brush = getBrushFromImage("image/edit.png");
                        text.Text = "Редактор определений и добавление терминов";
                        break;
                    case 2:
                        brush = getBrushFromImage("image/book.png");
                        text.Text = "Словарь + поиск слов в нём";
                        break;
                    case 3:
                        brush = getBrushFromImage("image/match.png");
                        text.Text = "Сопоставление слов и определений";
                        break;
                    case 4:
                        brush = getBrushFromImage("image/fill in.png");
                        text.Text = "Вписать пропущенные слова ";
                        break;
                    case 5:
                        brush = getBrushFromImage("image/crossword.png");
                        text.Text = "Решение кроссворда";
                        break;
                    default:
                        brush = getBrushFromImage("image/exit.png");
                        text.Text = "Вернуться на начальную страницу приложения с выбором предмета и класса";
                        break;
                }
                photo.Fill = brush;
                instructions.Children.Add(photo);
                Canvas.SetLeft(photo, 0);
                Canvas.SetTop(photo,i*50);
                instructions.Children.Add(text);
                Canvas.SetLeft(text, 50);
                Canvas.SetTop(text, i * 50);
            }
            WorkPlace.Children.Add(instructions);
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
            ReadyToAddAWord(sender, e);
            TermTB.Text = null;
            DescriptionTB.Text = null;
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
                        EditBTN.FontFamily = new FontFamily("Segoe MDL2 Assets");
                        EditBTN.Foreground = Brushes.MediumSeaGreen;
                        EditBTN.FontWeight = FontWeights.Bold;
                        EditBTN.Content = "\xE73E" + " ";
                        EditBTN.IsEnabled = false;
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

        private void ClearBTN_Click(object sender, RoutedEventArgs e)
        {
            EditItem_Selected(sender,e);
        }

        private void ReadyToAddAWord(object sender, RoutedEventArgs e)
        {
            EditBTN.FontFamily = new FontFamily("Segoe UI");
            EditBTN.Content = "Добавить";
            EditBTN.IsEnabled = true;
            EditBTN.Foreground = Brushes.Black;
            EditBTN.FontWeight = FontWeights.Regular;
        }

    }
}
