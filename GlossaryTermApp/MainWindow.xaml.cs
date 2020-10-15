using System.Windows;
using PdfSaver;

namespace GlossaryTermApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CrosswordItem_OnSelected(object sender, RoutedEventArgs e)
        {
            Export export = new Export();
            export.CaptureScreen();
            
        }
    }
}
