using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfCopy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnKies_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog documenten = new OpenFileDialog();
            documenten.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            documenten.Filter = "Tekstbestanden|*.TXT;*.TEXT";
            string MijnBestand;
            bool? resultaat = documenten.ShowDialog();
            if (resultaat == true)
            {
                MijnBestand = documenten.FileName;
                txtDoc.Text = MijnBestand;
                btnGo.IsEnabled = true;
            }
        }

        private void btnGo_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog MijnFile = new SaveFileDialog();
            MijnFile.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            MijnFile.Filter = "Tekstbestanden|.TXT;.TEXT";
            MijnFile.FileName = "savedfile.txt";
            if (MijnFile.ShowDialog() == true)
            {
                File.WriteAllText(MijnFile.FileName, "Hello World");
                txtDoc.Text = "";
                lblcomDocument.Content = "Bestand is overgezet";
                btnGo.IsEnabled = false;
            }
        }
    }
}
