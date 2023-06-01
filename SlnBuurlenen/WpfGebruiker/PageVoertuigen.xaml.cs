﻿using System;
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

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for PageVoertuigen.xaml
    /// </summary>
    public partial class PageVoertuigen : Page
    {
        public PageVoertuigen()
        {
            InitializeComponent();
        }
      
        // Opent window toevoegen
        private void btnToevoegen_Click(object sender, RoutedEventArgs e)
        {
            WindowToevoegen windowToevoegen = new WindowToevoegen(); 
            windowToevoegen.Show();
        }
    }
}
