using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using MatchGameLib;
using Color = System.Windows.Media.Color;
using Rectangle = System.Windows.Shapes.Rectangle;

namespace GlossaryTermApp
{
    /// <summary>
    /// Логика взаимодействия для MatchGamePage.xaml
    /// </summary>
    public partial class MatchGamePage : Window
    {
        private bool _captured;
        private Point DragOffset;
        public MatchGame MatchGame;
        public MatchGamePage(MatchGame matchGame)
        {
            InitializeComponent();
            MatchGame = matchGame;
            PrepareForm();
        }

        private void PrepareForm()
        {
            StackPanel WordsCanvas = new StackPanel();
           WordsCanvas.Background = (SolidColorBrush) new BrushConverter().ConvertFromString("#F2F3F4");
           ForStackPanelScrollViewer.Content = WordsCanvas;
            foreach (var term in MatchGame.TermList)
            {
                StackPanel termStackPanel=new StackPanel()
                {
                    Orientation = Orientation.Horizontal
                };
                DockPanel dockPanel = new DockPanel()
                {
                    LastChildFill = true
                };
                WrapPanel wrapPanel = new WrapPanel();
                TextBlock descriptionTextBlock = new TextBlock()
                {
                    Text = term.Description,
                    FontSize = 18,
                    Tag = term,
                    Background = (SolidColorBrush) new BrushConverter().ConvertFromString("#F9F7A8"),
                    TextWrapping = TextWrapping.Wrap,
                    Padding = new Thickness(5, 2, 5, 2),
                    Margin = new Thickness(10, 10, 5, 0)
                };
                Rectangle placeForWordRectangle = new Rectangle()
                {
                    AllowDrop = true,
                    Tag = term,
                    Fill = new SolidColorBrush(Color.FromRgb(202, 207, 210)),
                    Margin = new Thickness(30, 10, 10, 0),
                    Width = 200
                };
                
                wrapPanel.Children.Add(descriptionTextBlock);
                termStackPanel.Children.Add(placeForWordRectangle);
                dockPanel.Children.Add(termStackPanel);
                dockPanel.Children.Add(wrapPanel);
                WordsCanvas.Children.Add(dockPanel);

                TextBlock wordTextBlock = new TextBlock()
                {
                    Text = term.Word,
                    FontSize = 18,
                    Tag = term,
                    Background = (SolidColorBrush) new BrushConverter().ConvertFromString("#F08080"),
                    Padding = new Thickness(5, 1, 5, 1)
                };

                
                Border forTextBlock = new Border()
                {
                    BorderThickness = new Thickness(2),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(202, 207, 210)),
                    Margin = new Thickness(10, 10, 0, 0),
                    Child = wordTextBlock,
                    Tag = wordTextBlock.Tag
                };
                forTextBlock.MouseDown += WordTextBlock_MouseDown;
                forTextBlock.MouseUp += WordTextBlock_MouseUp;
                forTextBlock.MouseMove += WordTextBlock_MouseMove;
                WordsWrapPanel.Children.Add(forTextBlock);
            }
        }

        private Border _senderTextBlock;
        private Rectangle _hitRectangle=new Rectangle();
        private void WordTextBlock_MouseMove(object sender, MouseEventArgs e)
        {
            if (_captured)
            {
                _hitRectangle = new Rectangle();
                _senderTextBlock = (Border) sender;
                Point mousePos = e.GetPosition(this);
                double newX = mousePos.X - DragOffset.X;
                double newY = mousePos.Y - DragOffset.Y;
                _senderTextBlock.RenderTransform = new TranslateTransform(newX, newY);
                _senderTextBlock.RenderTransformOrigin = new Point(newX, newY);

                VisualTreeHelper.HitTest(ForStackPanelScrollViewer, null /*HitFilter*/, HitResult,
                    new PointHitTestParameters(new Point(newX, newY)));
                //HitTestResult result = VisualTreeHelper.HitTest(ForStackPanelScrollViewer, new Point(newY, newY));

                if (_hitRectangle != null)
                {
                    if (_hitRectangle.Tag == _senderTextBlock.Tag)
                    {
                        _senderTextBlock.Tag = "true";
                    }
                }
            }
        }

        //HitTestFilterBehavior HitFilter(DependencyObject o)
        //{
        //    if (!(o is Rectangle)) return HitTestFilterBehavior.ContinueSkipChildren;
        //    return HitTestFilterBehavior.Continue;
        //}
        //docs.microsoft.com/ru-ru/dotnet/api/system.windows.media.visualtreehelper.hittest?redirectedfrom=MSDN&view=netcore-3.1#System_Windows_Media_VisualTreeHelper_HitTest_System_Windows_Media_Visual_System_Windows_Media_HitTestFilterCallback_System_Windows_Media_HitTestResultCallback_System_Windows_Media_HitTestParameters_

        HitTestResultBehavior HitResult(HitTestResult result)
        {
            if (result.VisualHit is Rectangle)
            {
                _hitRectangle = result.VisualHit as Rectangle;
                return HitTestResultBehavior.Stop;
            }

            return HitTestResultBehavior.Continue;
        }

        private void WordTextBlock_MouseUp(object sender, MouseButtonEventArgs e)
        {
            _senderTextBlock = (Border)sender;
            _captured = false;
            _senderTextBlock.ReleaseMouseCapture();
        }

        private void WordTextBlock_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _senderTextBlock = (Border)sender;
            _captured = true;
            DragOffset = e.GetPosition(this);
            _senderTextBlock.CaptureMouse();
        }

        private void CheckBTN_OnClick(object sender, RoutedEventArgs e)
        {
            var countTrue = 0;
            foreach (var child in WordsWrapPanel.Children)
            {
                var termWithBorder = (Border) child;
                if (termWithBorder.Tag == "true")
                {
                    countTrue++;
                }
            }
        }
    }
}
