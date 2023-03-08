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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfVcardEditor
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
        /* Window voor afsluiten*/
        private void submenu_Exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult meldingtonen = MessageBox.Show("Ben je zeker dat je de applicatie wil afsluiten?",
            "Toepassing sluiten", MessageBoxButton.OKCancel);

            if (meldingtonen == MessageBoxResult.OK)
            {
                this.Close();
            }
            else if (meldingtonen == MessageBoxResult.Cancel)
            {
            }
        }
           /* Window tonen */
        private void submenu_About_Click(object sender, RoutedEventArgs e)
        {
            (new myPopUp()).Show();
        }
    }
}
