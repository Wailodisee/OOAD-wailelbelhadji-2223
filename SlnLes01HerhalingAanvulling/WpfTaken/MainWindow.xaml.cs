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

namespace WpfTaken
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

 

        private void btn1_Click(object sender, RoutedEventArgs e)
        {
           
            string text = "";
            if (rbn1.IsChecked == true)
            {
                text = " door: Adam)";
            }
            else if (rbn2.IsChecked == true)
            {
                text = " door: Bilal)";
            }
            else if (rbn3.IsChecked == true)
            {
                text = " door: Chelsey)";
            }

            ListBoxItem item = new ListBoxItem();
            item.Content = txtTaak.Text + " ( deadline: " + datePicker.SelectedDate.Value.ToString("dd/MM/yyyy ;") + text;
            lbx1.Items.Add(item);
        }
        /*private void ClearListBox()
        {
            lbx1.Items.Clear();
        }*/
    }
}
