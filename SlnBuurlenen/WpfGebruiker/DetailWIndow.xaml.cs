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
        private Voertuig mijnVoertuig;
        private Gebruiker mijnGebruiker;

        public DetailWIndow(Voertuig mijnVoertuig, Gebruiker mijnGebruiker)
        {
            InitializeComponent();

            this.mijnVoertuig = mijnVoertuig;
            this.mijnGebruiker = mijnGebruiker;

            InitialiseerLabels();
            ToonEigenaar();
            LaadAfbeeldingen();
        }

        // Methode die de labels initialiseert met de nodige waarden
        private void InitialiseerLabels()
        {
            lblNaam1.Content = mijnVoertuig.Naam;
            lblBeschrijving.Content = mijnVoertuig.Beschrijving;
            lblMerk.Content = mijnVoertuig.Merk;
            lblModel.Content = mijnVoertuig.Model;
            lblBouwjaar.Content = mijnVoertuig.Bouwjaar.ToString();

            if (mijnVoertuig is MotorVoertuig motorVoertuig)
            {
                lblBrandstof.Content = motorVoertuig.Brandstof?.ToString();
                lblTransmissie.Content = motorVoertuig.Transmissie?.ToString();
            }
        }

        // Eigenaar van voertuig tonen 
        private void ToonEigenaar()
        {
            Gebruiker eigenaar = Gebruiker.GetById(mijnVoertuig.Eigenaar_id.Id);
            if (eigenaar != null)
            {
                lblEigenaar.Content = $"{eigenaar.Voornaam} {eigenaar.Achternaam}";
            }
        }

        // Laad afbeelding in elke image-control
        private void LaadAfbeeldingen()
        {
            List<Foto> fotos = Foto.GetAllAutoPictures(mijnVoertuig.Id);
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

        // Ontleining bevestigen 
        private void btnBevestigen_Click(object sender, RoutedEventArgs e)
        {
            if (DatePickerControl())
            {
                Ontlening mijnOntlening = new Ontlening();
                mijnOntlening.Status = OntleningStatus.InAanvraag;
                mijnOntlening.Vanaf = dtpVan.SelectedDate.Value;
                mijnOntlening.Tot = dtpTot.SelectedDate.Value;
                mijnOntlening.Bericht = txtBericht.Text;
                mijnOntlening.AanvragerId = mijnGebruiker.Id;
                mijnOntlening.VoertuigId = mijnVoertuig.Id;  

                try
                {
                    Ontlening.Insert(mijnOntlening);
                    MessageBox.Show("Uw aanvraag is verstuurd =)");
                    this.Close();
                }
                catch (Exception)
                {
                    MessageBox.Show("Er is een fout opgetreden bij het verzenden van uw aanvraag: ");
                }
            }
        }

        // Controle van de datepickers
        private bool DatePickerControl()
        {
            DateTime? selectedStartDate = dtpVan.SelectedDate;
            DateTime? selectedEndDate = dtpTot.SelectedDate;

            bool isValid = selectedStartDate.HasValue && selectedEndDate.HasValue && selectedEndDate > selectedStartDate;

            lblError.Content = isValid ? null : (selectedStartDate.HasValue && selectedEndDate.HasValue) ? "Gekozen periode is incorrect." : "Kies een datum.";

            return isValid;
        }
    }
}
