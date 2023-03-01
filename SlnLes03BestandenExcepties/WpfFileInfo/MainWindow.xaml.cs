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

namespace WpfFileInfo
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

        private void btnKiezen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog document = new OpenFileDialog();
            document.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            document.Filter = "Tekstbestanden|*.TXT;*.TEXT";
            string KiesDocument;
            bool? GekozenDocument = document.ShowDialog();
            if (GekozenDocument == true)
            {
                // user kiest een document 
                KiesDocument = document.FileName;
                FileInfo fi = new FileInfo(KiesDocument);
                lblResultaten.Content = ($@"bestandnaam: {fi.Name}
extensie: {fi.Extension}
gemaakt op: {fi.CreationTime.ToString()}
mapnaam: {fi.Directory.Name}
aantal woorden: {fi.Length}
");
            }
        }
    }
}
