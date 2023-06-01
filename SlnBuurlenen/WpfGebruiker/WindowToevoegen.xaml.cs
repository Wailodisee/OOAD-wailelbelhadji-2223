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

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for WindowToevoegen.xaml
    /// </summary>
    public partial class WindowToevoegen : Window
    {
        public WindowToevoegen()
        {
            InitializeComponent();
        }

        private void btnUploaden_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog documenten = new OpenFileDialog();
        }
    }
}
