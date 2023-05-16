using System.Windows;
using MyClassLibrary;

namespace WpfGebruiker
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

        public MainWindow(int userId, string voorNaam, string achterNaam)
        {
            InitializeComponent();
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
            Main.NavigationService.Navigate(null);
        }
    }
}
