using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

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

            PopulateList(lbx1, "kader1");
            PopulateList(lbx2, "kader2");
        }

        private void PopulateList(ListBox listBox, string Namefolders)
        {
            string Path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            string aantalfolders = System.IO.Path.Combine(Path, Namefolders);

            string[] Mijnfiles = Directory.GetFiles(aantalfolders, "*.txt");

            foreach (string NameOfFiles in Mijnfiles)
            {
                listBox.Items.Add(System.IO.Path.GetFileName(NameOfFiles));
            }
        }

        private void DisplayLines(ListBox listBox, string Path)
        {
            List<string> Regels = new List<string>();

            using (StreamReader reader = new StreamReader(Path))
            {
                string MijnLines;

                while ((MijnLines = reader.ReadLine()) != null)
                {
                    Regels.Add(MijnLines);
                }
            }
            listBox.ItemsSource = Regels;
        }
    }
}