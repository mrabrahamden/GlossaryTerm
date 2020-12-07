using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CheckBox = System.Windows.Controls.CheckBox;
using MessageBox = System.Windows.MessageBox;
using TextBox = System.Windows.Controls.TextBox;

namespace TeacherryApp
{
    public partial class WelcomeWindow : Window
    {
        private List<string> _listOfSubjects = new List<string>()
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
        private List<CheckBox> _checkBoxes = new List<CheckBox>();
        public List<string> CheckedList = new List<string>();
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
            foreach (var subj in _listOfSubjects)
            {
                var panelForSubject = new WrapPanel();
                var checkBox = new CheckBox() { Content = subj, Tag = subj };
                panelForSubject.Children.Add(checkBox);
                SubjStackPanel.Children.Add(panelForSubject);
                _checkBoxes.Add(checkBox);
            }

            CreateNewTextBox();
        }

        private void CreateNewTextBox()
        {
            var panelForSubject = new WrapPanel();
            TextBox textBox = new TextBox() { MinWidth = 290, Margin = new Thickness(10, 0, 0, 0) };
            textBox.Background = GetBrushFromImage("image/TextBoxBackground1.png");
            var checkBox = new CheckBox() { VerticalAlignment = VerticalAlignment.Center, Tag = textBox };
            textBox.Tag = checkBox;
            _checkBoxes.Add(checkBox);
            textBox.TextChanged += TextBox_TextChanged;
            panelForSubject.Children.Add(checkBox);
            panelForSubject.Children.Add(textBox);
            NewSubjStackPanel.Children.Add(panelForSubject);
        }

        private List<object> _lastTextBox = new List<object>();
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
                if (!_lastTextBox.Contains(sender))
                {
                    CreateNewTextBox();
                    _lastTextBox.Add(sender);
                }
            }

        }

        private bool _hasMistakes = false;

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _hasMistakes = false;
            foreach (var checkBox in _checkBoxes)
            {
                if (checkBox.IsChecked == true)
                {
                    if (checkBox.Tag is string)
                    {
                        CheckedList.Add((string)checkBox.Tag);
                    }
                    else if (checkBox.Tag is TextBox)
                    {
                        var text = ((TextBox)checkBox.Tag).Text;
                        var regex = new Regex(@"([^а-яА-Яa-zA-Z\s])");
                        var hasNonLetterSymbols = regex.IsMatch(text);
                        if (hasNonLetterSymbols)
                        {
                            ((TextBox)checkBox.Tag).Background = new SolidColorBrush(Colors.LightCoral);
                            _hasMistakes = true;
                        }
                        else
                        {
                            var words = text.Split(new char[]{' '}, StringSplitOptions.RemoveEmptyEntries);
                            var subjectSB=new StringBuilder();
                            foreach (var word in words)
                            {
                                subjectSB.Append(word + " ");
                            }

                            string subject;
                            if (subjectSB.Length > 0 && subjectSB[subjectSB.Length - 1] == ' ')
                            {
                                subject = subjectSB.ToString().Substring(0,subjectSB.Length-1);
                            }
                            else
                            {
                                subject = text;
                            }
                            var containsText =
                                 (from t in CheckedList where t.ToLower() == subject.ToLower() select t).ToList();
                            if (subject.Length > 0 && containsText.Count == 0)
                                CheckedList.Add(subject);
                        }
                    }
                }
            }

            if (!_hasMistakes)
            {
                Close();
            }
            else
            {
                CheckedList.Clear();
                MessageBox.Show("Поля могут содержать только буквы и пробелы.");
            }
        }
    }
}
