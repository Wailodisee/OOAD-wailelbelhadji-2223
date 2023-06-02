using MyClassLibrary;
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
using System.Windows.Media.Effects;
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
        private Gebruiker mijnGebruiker;
        public PageVoertuigen(Gebruiker mijnGebruiker)
        {
            InitializeComponent();
            this.mijnGebruiker = mijnGebruiker;

            List<Voertuig> gebruikerVoertuigen = Voertuig.CatchId(mijnGebruiker.Id);

            CardVoertuig(gebruikerVoertuigen);
        }

        private void CardVoertuig(List<Voertuig> klassevoertuigen)
        {
            if (klassevoertuigen == null) return;

            WrapPanelVoertuigen.Children.Clear();

            foreach (var mijnVoertuig in klassevoertuigen)
            {
                Border border = CreateBorder();
                Grid card = CreateCardGrid();

                Foto img = Foto.GetAllPictureId(mijnVoertuig.Id).FirstOrDefault();

                if (img != null)
                {
                    Image picture = CreateImage(img.Data);
                    Grid.SetColumn(picture, 0);
                    card.Children.Add(picture);
                }

                StackPanel panelForText = CreateTextPanel(mijnVoertuig);
                Grid.SetColumn(panelForText, 1);
                card.Children.Add(panelForText);

                StackPanel panelforbtn = CreateButtons(mijnVoertuig);
                panelForText.Children.Add(panelforbtn);

                border.Child = card;
                WrapPanelVoertuigen.Children.Add(border);
            }
        }

        private Border CreateBorder()
        {
            return new Border
            {
                Background = Brushes.White,
                BorderBrush = Brushes.DeepSkyBlue,
                BorderThickness = new Thickness(4),
                CornerRadius = new CornerRadius(15),
                Margin = new Thickness(10),
                Padding = new Thickness(5),
                Width = 320,
                Height = 135,
                Effect = new DropShadowEffect
                {
                    Color = Colors.Gray,
                    Direction = -45,
                    ShadowDepth = 2,
                    Opacity = 0.8
                }
            };
        }

        private Grid CreateCardGrid()
        {
            Grid card = new Grid();
            card.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            card.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            return card;
        }

        // maakt image aan
        private Image CreateImage(byte[] imgData)
        {
            Image picture = new Image();
            picture.Source = BytesToImageConverter(imgData);
            picture.Height = 80;
            picture.Width = 80;
            picture.Margin = new Thickness(8, 15, 0, 0);
            return picture;
        }

        // maakt maakt panel aan 
        private StackPanel CreateTextPanel(Voertuig mijnVoertuig)
        {
            StackPanel panelForText = new StackPanel
            {
                Orientation = Orientation.Vertical
            };

            TextBlock naam = new TextBlock
            {
                Text = mijnVoertuig.Naam,

                FontSize = 20,

                FontWeight = FontWeights.SemiBold,

                Foreground = Brushes.MidnightBlue,

                Margin = new Thickness(-95, 0, 0, 0)
            };
            panelForText.Children.Add(naam);

            panelForText.Children.Add(new TextBlock { Height = 24 });

            TextBlock merkTextBlock = new TextBlock();
            merkTextBlock.Text = string.IsNullOrEmpty(mijnVoertuig.Merk) ? "Merk : /" : "Merk : " + mijnVoertuig.Merk;

            // design
            merkTextBlock.FontWeight = FontWeights.Bold;
            merkTextBlock.FontSize = 14;
            panelForText.Children.Add(merkTextBlock);

            TextBlock modelTextBlock = new TextBlock();
            modelTextBlock.Text = string.IsNullOrEmpty(mijnVoertuig.Model) ? "Model : /" : "Model : " + mijnVoertuig.Model;

            // design
            modelTextBlock.FontWeight = FontWeights.Bold;
            modelTextBlock.FontSize = 14;
            panelForText.Children.Add(modelTextBlock);
            return panelForText;
        }

        // Buttons aanmaken
        private StackPanel CreateButtons(Voertuig mijnVoertuig)
        {
            StackPanel btnPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal,

                HorizontalAlignment = HorizontalAlignment.Right,

                Background = Brushes.White
            };

            Button btnUpdate = CreateButton("");

            btnUpdate.Content = "X🗑"; // icoon vuilnisbak komt niet

            btnUpdate.Tag = mijnVoertuig;

            btnPanel.Children.Add(btnUpdate);

            Button btnVerwijder = CreateButton("");

            btnUpdate.Content = "✏";

            btnVerwijder.Tag = mijnVoertuig;

            btnVerwijder.Click += btnVerwijder_Click;

            btnPanel.Children.Add(btnVerwijder);

            Button btnDetail = CreateButton("img/logoInfo.jpg");

            btnDetail.Tag = mijnVoertuig;

            btnDetail.Click += BtnDetails_Click;

            btnPanel.Children.Add(btnDetail);

            return btnPanel;
        }

        // Buttons aanmaken
        private Button CreateButton(string imgRoot)
        {
            Button btn = new Button
            {
                Margin = new Thickness(5, 0, 7, 0),
                Background = Brushes.White
            };

            Image img = new Image
            {
                Source = new BitmapImage(new Uri(imgRoot, UriKind.Relative)),

                Width = 25,

                Height = 25
            };

            btn.Content = img;
            return btn;
        }

        // button toevoegen
        private void btnToevoegen_Click(object sender, RoutedEventArgs e)
        {
            WindowToevoegen toevoegen = new WindowToevoegen(mijnGebruiker);
            toevoegen.Show();
        }

        // button verwijderen
        private void btnVerwijder_Click(object sender, RoutedEventArgs e)
        {
            Voertuig mijnVoertuig = (Voertuig)((Button)sender).Tag;

            Ontlening.DeleteById(mijnVoertuig.Id);

            Foto.DeleteById(mijnVoertuig.Id);

            Voertuig.DeleteRecord(mijnVoertuig.Id);

            List<Voertuig> gebruiker = Voertuig.CatchId(mijnGebruiker.Id);
            CardVoertuig(gebruiker);
        }

        // btn details
        private void BtnDetails_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            Voertuig mijnVoertuig = (Voertuig)button.Tag;

            switch (mijnVoertuig.Type)
            {
                case 1:
                    DetailWIndow dtlWindow = new DetailWIndow(mijnVoertuig, mijnGebruiker);
                    dtlWindow.Show();
                    break;
                case 2:
                    DetailWindow1 dtlWindow1 = new DetailWindow1(mijnVoertuig, mijnGebruiker);
                    dtlWindow1.Show();
                    break;
            }
        }

        // gemaakt met chatgpt
        private ImageSource BytesToImageConverter(byte[] picture)
        {
            BitmapImage imgByte = new BitmapImage();

            using (MemoryStream memoryS = new MemoryStream(picture))
            {
                imgByte.BeginInit();

                imgByte.CacheOption = BitmapCacheOption.OnLoad;

                imgByte.StreamSource = memoryS;

                imgByte.EndInit();
            }
            imgByte.Freeze();
            return imgByte;
        }
    }
}
