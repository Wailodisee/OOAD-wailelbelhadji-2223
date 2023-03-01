using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfKerstverlichting
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Ellipse> lstellipse = new List<Ellipse>();
        bool buttonOFF = false;
        DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            NewEllipse();
        }
        private void NewEllipse()
        {
            Random rdm = new Random();

            int aantal = 0;

            while (aantal < 40)
            {
                Ellipse ellipse = new Ellipse();

                ellipse.Fill = Brushes.Gray;

                ellipse.Width = 4;

                ellipse.Height = 4;

                double horiz = rdm.Next(0, 299);

                double vertic = rdm.Next(0, 424);

                if (!PixelIsWhite(imgTree, (int)horiz, (int)vertic))
                {
                    cnvTree.Children.Add(ellipse);

                    ellipse.SetValue(Canvas.LeftProperty, (double)horiz);

                    ellipse.SetValue(Canvas.TopProperty, (double)vertic);

                    aantal++;

                    lstellipse.Add(ellipse);
                }
            }
            
        }
        private void GrijsWit(object sender, EventArgs e)
        {
            Random random = new Random();

            foreach (Ellipse element in lstellipse)
            {
                SolidColorBrush brush;

                if (random.Next(2) == 0)
                {
                    brush = Brushes.White;
                }
                else
                {
                    brush = Brushes.Gray;
                }

                element.Fill = brush;
            }
            
        }
        private void btnLicht_Click(object sender, RoutedEventArgs e)
        {
            if (buttonOFF == false)
            {
                timer = new DispatcherTimer();

                timer.Interval = TimeSpan.FromMilliseconds(500);

                timer.Tick += GrijsWit;

                timer.Start();

                btnLicht.Content = "SWITCH OFF";

                buttonOFF = true;
            }
            else
            {
                foreach (Ellipse element in lstellipse)
                {
                    element.Fill = Brushes.Gray;
                }

                timer.Stop();

                btnLicht.Content = "SWITCH ON";

                buttonOFF = false;
            }
        }
        public static bool PixelIsWhite(Image img, int x, int y)
        {
            BitmapSource source = img.Source as BitmapSource;
            Color color = Colors.White;
            CroppedBitmap cb = new CroppedBitmap(source, new Int32Rect(x, y, 1, 1));
            byte[] pixels = new byte[4];
            cb.CopyPixels(pixels, 4, 0);
            color = Color.FromRgb(pixels[2], pixels[1], pixels[0]);
            return color.ToString() == "#FFFFFFFF";
        }
    }
}