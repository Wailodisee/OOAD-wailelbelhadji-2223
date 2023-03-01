using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

            kader1();
            kader2();
        }
        private void kader1()
        {
            string Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string mijnKader1 = System.IO.Path.Combine(Path, "kader1");

            string[] Mijnfiles = Directory.GetFiles(mijnKader1, "Tekstbestanden|*.TXT;*.TEXT");

            foreach (string fileName in Mijnfiles)
            {
                lbx1.Items.Add(System.IO.Path.GetFileName(fileName));
            }
            Console.ReadLine();
        }
        private void kader2()
        {
            string Path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string mijnKader2 = System.IO.Path.Combine(Path, "kader2");

            string[] mijnnFiles = Directory.GetFiles(mijnKader2, "*.txt");

            foreach (string fileName in mijnnFiles)
            {
                lbx2.Items.Add(System.IO.Path.GetFileName(fileName));
            }
            Console.ReadLine();
        }

        private void Kader1(string filePath)
        {
            List<string> aantalRegels = new List<string>();
            using (StreamReader reader = new StreamReader(filePath))
            {
                string REgels;
                while ((REgels = reader.ReadLine()) != null)
                {
                    aantalRegels.Add(REgels);
                }
            }
            lbx1.ItemsSource = aantalRegels;
            Console.ReadLine();
        }
        private void Kader2(string Path)
        {
            List<string> mijnLijnen = new List<string>();
            using (StreamReader reader = new StreamReader(Path))
            {
                string regels;
                while ((regels = reader.ReadLine()) != null)
                {
                    mijnLijnen.Add(regels);
                }
            }
            lbx2.ItemsSource = mijnLijnen;

            Console.ReadLine();
        }

        private void lbxbeneden1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "kader1", (string)lbx1.SelectedItem);

            kader1(selectedFile);

            Console.ReadLine();
        }

        private void lbxbeneden2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selectedFile = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "kader2", (string)lbx2.SelectedItem);

            kader2(selectedFile);

            Console.ReadLine();
        }
    }
}