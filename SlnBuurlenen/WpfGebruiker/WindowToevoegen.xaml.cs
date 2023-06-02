using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Microsoft.Win32;
using MyClassLibrary;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for WindowToevoegen.xaml
    /// </summary>
    public partial class WindowToevoegen : Window
    {
        private Gebruiker mijnGebruiker;
        public WindowToevoegen(Gebruiker mijnGebruiker)
        {
            InitializeComponent();
            this.mijnGebruiker = mijnGebruiker;
        }

        private void btnGem_Click(object sender, RoutedEventArgs e)
        {
            Mainn.Content = new PageGemotoriseerd(mijnGebruiker);
        }

        private void btnGet_Click(object sender, RoutedEventArgs e)
        {
            Mainn.Content = new PageGedetailleerd(mijnGebruiker);
        }
    }
}
