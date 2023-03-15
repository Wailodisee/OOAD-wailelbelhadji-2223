using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfCompare
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            EersteMap();
            TweedeMap();
        }


        private void EersteMap()
        {
            string pathOfFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string folder1 = System.IO.Path.Combine(pathOfFolder, "kader1");

            string[] mijnFiles = Directory.GetFiles(folder1, "*.txt");

            foreach (string fileName in mijnFiles)
            {
                lbx1.Items.Add(System.IO.Path.GetFileName(fileName));
            }
        }

        private void TweedeMap()
        {
            string pathOfFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);

            string folder2 = System.IO.Path.Combine(pathOfFolder, "kader2");

            string[] mijnFiles = Directory.GetFiles(folder2, "*.txt");

            foreach (string fileName in mijnFiles)
            {
                lbx2.Items.Add(System.IO.Path.GetFileName(fileName));
            }
        }

        private void Weergave1(string PathOfFile)
        {
            List<string> Regels = new List<string>();

            using (StreamReader reader = new StreamReader(PathOfFile))
            {
                string lijnen;

                while ((lijnen = reader.ReadLine()) != null)
                {
                    Regels.Add(lijnen);
                }
            }
            lbxbeneden1.ItemsSource = Regels;
        }

        private void Weergave2(string filePath)
        {
            List<string> Regels = new List<string>();

            using (StreamReader reader = new StreamReader(filePath))
            {
                string MijnRegels;

                while ((MijnRegels = reader.ReadLine()) != null)
                {
                    Regels.Add(MijnRegels);
                }
            }
            lbxbeneden2.ItemsSource = Regels;
        }

        private void lbx1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string GekozenFile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "kader1", (string)lbx1.SelectedItem);
            Weergave1(GekozenFile);
        }
        private void lbx2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string GekozenFile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "kader2", (string)lbx2.SelectedItem);
            Weergave2(GekozenFile);

        }

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < lbxbeneden1.Items.Count && i < lbxbeneden2.Items.Count; i++)
            {
                ListBoxItem eersteItem = lbxbeneden1.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;

                ListBoxItem tweedeItem = lbxbeneden2.ItemContainerGenerator.ContainerFromIndex(i) as ListBoxItem;

                if (eersteItem != null && tweedeItem != null)
                {
                    string[] eersteWoord = eersteItem.Content.ToString().Split(' ');

                    string[] tweedeWoord = tweedeItem.Content.ToString().Split(' ');

                    for (int j = 0; j < eersteWoord.Length && j < tweedeWoord.Length; j++)
                    {
                        if (eersteWoord[j] != tweedeWoord[j])
                        {
                            eersteItem.Background = Brushes.Red;

                            tweedeItem.Background = Brushes.Red;
                        }
                    }
                }
            }
        }
    }
}
 