using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace GlossaryTermApp
{
    /// <summary>
    /// Логика взаимодействия для WelcomeWindow.xaml
    /// </summary>
    public partial class WelcomeWindow : Window
    {
        private List<string> listOfSubjects = new List<string>()
        {
            "Астрономия",
            "Английский язык",
            "Биология",
            "География",
            "Изобразительное искусство",
            "Информатика",
            "История",
            "Литература",
            "Математика",
            "Музыка",
            "МХК",
            "ОБЖ",
            "Обществознание",
            "Право",
            "Русский язык",
            "Технология",
            "Физика",
            "Французский язык",
            "Химия"
        };
        private List<CheckBox> checkBoxes = new List<CheckBox>();
        public List<string> checkedList = new List<string>();
        public WelcomeWindow()
        {
            InitializeComponent();
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
            brush.AlignmentX = AlignmentX.Left;
            brush.Stretch = Stretch.None;
            return brush;
        }

        private void PrepareForm()
        {
            WrapPanel panelForSubject;
            CheckBox checkBox;
            foreach (var subj in listOfSubjects)
            {
                panelForSubject = new WrapPanel();
                checkBox = new CheckBox() { Content = subj, Tag = subj };
                panelForSubject.Children.Add(checkBox);
                SubjStackPanel.Children.Add(panelForSubject);
                checkBoxes.Add(checkBox);
            }

            CreateNewTextBox();
        }

        private void CreateNewTextBox()
        {
            WrapPanel panelForSubject;
            CheckBox checkBox;
            panelForSubject = new WrapPanel();
            TextBox textBox = new TextBox() { MinWidth = 290, Margin = new Thickness(10, 0, 0, 0) };
            textBox.Background = GetBrushFromImage("image/TextBoxBackground1.png");
            checkBox = new CheckBox() { VerticalAlignment = VerticalAlignment.Center, Tag = textBox };
            textBox.Tag = checkBox;
            checkBoxes.Add(checkBox);
            textBox.TextChanged += TextBox_TextChanged;
            panelForSubject.Children.Add(checkBox);
            panelForSubject.Children.Add(textBox);
            NewSubjStackPanel.Children.Add(panelForSubject);
        }

        private List<object> lastTextBox = new List<object>();
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "")
            {
                textBox.Background = GetBrushFromImage("image/TextBoxBackground1.png");
            }
            else
            {
                textBox.Background = null;
                CheckBox checkBox = (CheckBox)textBox.Tag;
                checkBox.IsChecked = true;
                if (!lastTextBox.Contains(sender))
                {
                    CreateNewTextBox();
                    lastTextBox.Add(sender);
                }
            }
           
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            foreach (var checkBox in checkBoxes)
            {
                if (checkBox.IsChecked==true)
                {
                    if (checkBox.Tag is string)
                    {
                        checkedList.Add((string)checkBox.Tag);
                    }
                    else if (checkBox.Tag is TextBox)
                    {
                        var text = ((TextBox) checkBox.Tag).Text;
                        if(text.Length>0)
                            checkedList.Add(text);
                    }
                }
            }
            Close();
        }
    }
}
