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
using MyClassLibrary;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for PageHome.xaml
    /// </summary>
    public partial class PageHome : Page
    {
        private Gebruiker gebruiker;
        public PageHome(Gebruiker gebruiker)
        {
            InitializeComponent();
            this.gebruiker = gebruiker;
            FetchDataAuto();
        }

        // Filtert voertuiggegevens en geeft ze weer
        private void FetchDataAuto()
        {
            if (WrapPanelVoertuigen != null)
            {
                ShowVoertuigData(FilterAutoCheckboxes());
            }
        }

        // Laadt de nieuwe voertuiggegevens bij verandering van checkbox
        private void HandleCheckBoxChange(object sender, RoutedEventArgs e)
        {
            FetchDataAuto();
        }

        // Converteert byte => ImageSource, wordt gebruikt om een afbeelding weer te geven
        private ImageSource BytesToImageSourceConverter(byte[] foto)
        {
            BitmapImage picturesbyte = new BitmapImage();

            using (MemoryStream memoryS = new MemoryStream(foto))
            {
                picturesbyte.BeginInit();

                picturesbyte.CacheOption = BitmapCacheOption.OnLoad;

                picturesbyte.StreamSource = memoryS;

                picturesbyte.EndInit();
            }
            picturesbyte.Freeze(); 
            return picturesbyte;
        }

        // Geeft voertuiggegevens weer + methode toepassen op elke voertuig 
        private void ShowVoertuigData(List<Voertuig> mijnvoertuig)
        {
            if (mijnvoertuig == null) return;

            ResetData();
            mijnvoertuig.ForEach(voertuigen => ShowSingleVehicle(voertuigen));
        }

        // Wist alle bestaande voertuigen van de panel
        private void ResetData()
        {
            WrapPanelVoertuigen.Children.Clear();
        }

        // Voegt voertuiggegevens aan de panel 
        private void ShowSingleVehicle(Voertuig mijnVoertuig)
        {
            AppendVehicleToWrapPanel(mijnVoertuig, WrapPanelVoertuigen);
        }

        // Voeg elke Card voor elk voertuig
        private void AppendVehicleToWrapPanel(Voertuig mijnVoertuig, WrapPanel mijnPanel)
        {
            Border voertuigCards = GenerateCard();

            voertuigCards.Child = GenerateCardContent(mijnVoertuig);

            mijnPanel.Children.Add(voertuigCards);
        }

        // Maak de border aan 
        private Border GenerateCard()
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

        // Maakt de Grid aan 
        private Grid GenerateCardContent(Voertuig mijnVoertuig)
        {
            Grid cards = new Grid();
            cards.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            cards.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) });
            AddVehicleImage(mijnVoertuig, cards);
            AddVehicleTextInfo(mijnVoertuig, cards);
            return cards;
        }

        // Maakt de grootte van de images etc 
        private void AddVehicleImage(Voertuig mijnVoertuig, Grid mijnCard)
        {
            Foto picture = Foto.GetAllAutoPictures(mijnVoertuig.Id).FirstOrDefault();
            if (picture != null)
            {
                Image pictures = new Image();

                pictures.Source = BytesToImageSourceConverter(picture.Data);

                pictures.Height = 80;

                pictures.Width = 80;

                pictures.Margin = new Thickness(8, 15, 0, 0);

                Grid.SetColumn(pictures, 0);

                mijnCard.Children.Add(pictures);
            }
        }

        // Voeg tekstuele informatie van een voertuig
        private void AddVehicleTextInfo(Voertuig mijnVoertuig, Grid mijnCards)
        {
            StackPanel panelForText = GeneratePanelForText(mijnVoertuig);

            Grid.SetColumn(panelForText, 1);

            mijnCards.Children.Add(panelForText);
        }


        // TextPanel wordt aangemakt 
        private StackPanel GeneratePanelForText(Voertuig mijnVoertuig)
        {
            StackPanel panelForText = new StackPanel { Orientation = Orientation.Vertical };

            panelForText.Children.Add(GenerateVehicleNameTextBlock(mijnVoertuig));

            panelForText.Children.Add(new TextBlock { Height = 15 });

            panelForText.Children.Add(GenerateVehicleMerkTextBlock(mijnVoertuig));

            panelForText.Children.Add(GenerateVehicleModelTextBlock(mijnVoertuig));

            panelForText.Children.Add(GenerateInfoButton(mijnVoertuig));

            return panelForText;
        }

        // Geeft de naam van de voertuig terug
        private TextBlock GenerateVehicleNameTextBlock(Voertuig mijnVoertuig)
        {
            return new TextBlock
            {
                Text = mijnVoertuig.Name,

                FontSize = 20,

                FontWeight = FontWeights.SemiBold,

                Foreground = Brushes.MidnightBlue,

                Margin = new Thickness(-95, 0, 0, 0)
            };
        }

        // Geeft de merk van de voertuig terug
        private TextBlock GenerateVehicleMerkTextBlock(Voertuig mijnVoertuig)
        {
            return new TextBlock
            {
                Text = string.IsNullOrEmpty(mijnVoertuig.Merk) ? " Merk : / " : " Merk : " + mijnVoertuig.Merk,

                FontWeight = FontWeights.Bold,

                FontSize = 14
            };
        }

        // Geeft de model van de voertuig terug
        private TextBlock GenerateVehicleModelTextBlock(Voertuig mijnVoertuig)
        {
            return new TextBlock
            {
                Text = string.IsNullOrEmpty(mijnVoertuig.Model) ? " Model : /" : " Model : " + mijnVoertuig.Model,

                FontWeight = FontWeights.Bold,

                FontSize = 14
            };
        }

        // Maak de button info aan 
        private Button GenerateInfoButton(Voertuig mijnVoertuig)
        {
            Image pictureInformation = new Image
            {
                Source = new BitmapImage(new Uri("img/logoInfo.jpg", UriKind.Relative)),

                Width = 25,
            };

            Button button = new Button
            {
                Content = pictureInformation,
                Tag = mijnVoertuig,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new Thickness(0, 0, 10, 0)
            };
            button.Click += InfoButton_Click;
            return button;
        }

        // Filtert voertuiggegevens op basis van de selecties van de checkboxes
        private List<Voertuig> FilterAutoCheckboxes()
        {
            List<Voertuig> voertuigFilters = new List<Voertuig>();

            if (cbxGemotoriseerd.IsChecked == false && cbxGetrokken.IsChecked == false)
            {
                return voertuigFilters;
            }

            List<Voertuig> mijnVoertuigen = Voertuig.GetAll();

            foreach (var auto in mijnVoertuigen)
            {
                if ((cbxGemotoriseerd?.IsChecked == true && auto.Type == 1) || (cbxGetrokken?.IsChecked == true && auto.Type == 2))
                {
                    voertuigFilters.Add(auto);
                }
            }
            return voertuigFilters;
        }

        private void InfoButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}

