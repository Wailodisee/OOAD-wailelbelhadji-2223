using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using MyClassLibrary;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Gebruiker mijnGebruiker;

        public MainWindow(Gebruiker mijnGebruiker)
        {
            InitializeComponent();
            this.mijnGebruiker = mijnGebruiker;
            Main.Content = new PageHome(mijnGebruiker);
            imgNaam.Source = LoadImage(mijnGebruiker.Profielfoto);
        }

        public MainWindow(int userId, string voorNaam, string achterNaam)
        {
            InitializeComponent();
            Main.Content = new PageHome(mijnGebruiker);
        }

        private void btnOntleningen_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new PageOntleningen(mijnGebruiker);
        }

        private void btnVoertuigen_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new PageVoertuigen(mijnGebruiker);
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new PageHome(mijnGebruiker);
        }

        // Methode om fotoprofiel te lezen
        private ImageSource LoadImage(byte[] picturesDatabase)
        {
            if (picturesDatabase == null || picturesDatabase.Length == 0) return null;
            var picture = new BitmapImage();
            using (var memory = new MemoryStream(picturesDatabase))
            {
                memory.Position = 0;
                picture.BeginInit();
                picture.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                picture.CacheOption = BitmapCacheOption.OnLoad;
                picture.UriSource = null;
                picture.StreamSource = memory;
                picture.EndInit();
            }
            picture.Freeze();
            return picture;
        }

        // HandEvent voor buuton uitloggen
        private void btnUitloggen_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow(); 
            loginWindow.Show();  
            this.Close();  
        }
    }
}
