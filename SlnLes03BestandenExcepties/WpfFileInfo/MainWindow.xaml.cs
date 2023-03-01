using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;


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
                FileInfo information = new FileInfo(KiesDocument);
                lblResultaten.Content = ($@"bestandsnaam: {information.Name}
extensie: {information.Extension}
gemaakt op: {information.CreationTime.ToString()}
mapnaam: {information.Directory.Name}
aantal woorden: {information.Length}
");
            }
        }
    }
}
