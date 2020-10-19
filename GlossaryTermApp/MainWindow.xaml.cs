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
            ClearWorkPlace();
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
            ScrollDictionary.Width = 715;
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
                        HorizontalAlignment = HorizontalAlignment.Right,
                        FontFamily = new FontFamily("Segoe MDL2 Assets"),
                        Content = "\xE711",
                        Foreground = Brushes.Red,
                        Background = Brushes.White

                    };
                    //deleteBtn.Background = getBrushFromImage("image/delete.png");
                    Button editBtn = new Button
                    {
                        Height = 15,
                        Width = 15,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        FontFamily = new FontFamily("Segoe MDL2 Assets"),
                        Content = "\xE70F",
                        Background = Brushes.White

                    };
                    //editBtn.Background = getBrushFromImage("image/edit.png");
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
