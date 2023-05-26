using System.Windows;
using MyClassLibrary;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Gebruiker gebruiker;

        public MainWindow(Gebruiker gebruiker)
        {
            InitializeComponent();
            this.gebruiker = gebruiker;
            Main.Content = new PageHome(gebruiker);
        }

        public MainWindow(int userId, string voorNaam, string achterNaam)
        {
            InitializeComponent();
            Main.Content = new PageHome(gebruiker);
        }

        private void btnOntleningen_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new PageOntleningen();
        }

        private void btnVoertuigen_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new PageVoertuigen();
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new PageHome(gebruiker);
        }
    }
}
