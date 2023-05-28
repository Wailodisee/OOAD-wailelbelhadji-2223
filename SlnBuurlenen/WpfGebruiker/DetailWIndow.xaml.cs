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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using MyClassLibrary;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for DetailWIndow.xaml
    /// </summary>
    public partial class DetailWIndow : Window
    {
        private Voertuig voertuigen;
        private Gebruiker gebruikers;

        public DetailWIndow(Voertuig mijnVoertuig, Gebruiker mijnGebruiker)
        {
            InitializeComponent();

            this.voertuigen = mijnVoertuig;
            this.gebruikers = mijnGebruiker;

            InitialiseerLabels();
            ToonEigenaar();
            LoadImages();
        }

        // Methode die de labels initialiseert met de nodige waarden
        private void InitialiseerLabels()
        {
            lblNaam1.Content = voertuigen.Naam;
            lblBeschrijving.Content = voertuigen.Beschrijving;
            lblMerk.Content = voertuigen.Merk;
            lblModel.Content = voertuigen.Model;
            lblBouwjaar.Content = voertuigen.Bouwjaar.ToString();

            if (voertuigen is MotorVoertuig motorVoertuig)
            {
                lblBrandstof.Content = motorVoertuig.Brandstof?.ToString();
                lblTransmissie.Content = motorVoertuig.Transmissie?.ToString();
            }
        }

        // Eigenaar van voertuig tonen 
        private void ToonEigenaar()
        {
            Gebruiker eigenaar = Gebruiker.GetById(voertuigen.Eigenaar_id.Id);
            if (eigenaar != null)
            {
                lblEigenaar.Content = $"{eigenaar.Voornaam} {eigenaar.Achternaam}";
            }
        }

        // Laad afbeelding in elke image-control
        private void LoadImages()
        {
            List<Foto> fotos = Foto.GetAllAutoPictures(voertuigen.Id);
            Image[] imageControls = { img1, img2, img3 };

            for (int i = 0; i < fotos.Count && i < imageControls.Length; i++)
            {
                imageControls[i].Source = LoadImage(fotos[i].Data);
            }
        }

        // Converteert bytes van de database naar  image met een MS 
        private ImageSource LoadImage(byte[] imageData)
        {
            if (imageData == null || imageData.Length == 0) return null;
            var image = new BitmapImage();
            using (var mem = new MemoryStream(imageData))
            {
                mem.Position = 0;
                image.BeginInit();
                image.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = null;
                image.StreamSource = mem;
                image.EndInit();
            }
            image.Freeze();
            return image;
        }
    }
}
