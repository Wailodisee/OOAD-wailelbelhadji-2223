using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WpfMatchImages
{
    public partial class MainWindow : Window
    {
        // variabelen declareren

        Button button1;

        DispatcherTimer minsec;

        Stopwatch stopwatch;

        int aantalmatch = 7;
        
        public MainWindow()
        {
            InitializeComponent();

            minsec = new DispatcherTimer();

            stopwatch = new Stopwatch();

            minsec.Interval = TimeSpan.FromMilliseconds(10);

            minsec.Tick += Timer_Tick;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button2 = (Button)sender;

            string images = button2.Tag.ToString();

            if (button1 == null)
            {
                button1 = button2;

                stopwatch.Start();

                minsec.Start();

                button2.Opacity = 0.5;

                button2.IsEnabled = false;

            }

            else if (button1.Tag.ToString() == images)

            {
                if (aantalmatch == 0)
                {
                    lblAntwoord.Content = "Je hebt alles gevonden!";

                    stopwatch.Stop();
                }
                else
                {
                    lblAntwoord.Content = $"Juist voor het moment ! \nnog {Convert.ToString(aantalmatch--)} te vinden";
                }
                button2.IsEnabled = false;

                button2.Opacity = 0.5;

                button1 = null;

            }

            else
            {
                lblAntwoord.Content = "fout begin opnieuw";

                button1.IsEnabled = true;

                button1.Opacity = 1;

                button1 = null;

            }
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            TimeSpan time = stopwatch.Elapsed;

            lblWatch.Content = time.ToString(@"hh\:mm\:ss\.ff");
        }
    }
}
