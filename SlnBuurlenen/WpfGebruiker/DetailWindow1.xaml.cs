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
    /// Interaction logic for DetailWindow1.xaml
    /// </summary>
    public partial class DetailWindow1 : Window
    {
        private Voertuig voertuigen;
        private Gebruiker gebruikers;
        public DetailWindow1(Voertuig mijnVoertuig, Gebruiker mijnGebruiker)
        {
            InitializeComponent();


            this.voertuigen = mijnVoertuig;
            this.gebruikers = mijnGebruiker;

            InitialiseerLabels(mijnVoertuig);
            VerwerkVoertuigInfo(mijnVoertuig);
            ToonEigenaar(mijnVoertuig);
            LaadAfbeeldingen(mijnVoertuig);
        }

        // Methode die de labels initialiseert met de nodige waarden
        private void InitialiseerLabels(Voertuig mijnVoertuig)
        {
            lblNaam1.Content = CheckEmptyOrNull(mijnVoertuig.Naam, lblNaam1);
            lblBeschrijving.Content = CheckEmptyOrNull(mijnVoertuig.Beschrijving, lblBeschrijving);
            lblModel.Content = CheckEmptyOrNull(mijnVoertuig.Model, lblModel);
            lblMerk.Content = CheckEmptyOrNull(mijnVoertuig.Merk, lblMerk); 
            lblBouwjaar.Content = mijnVoertuig.Bouwjaar;
        }

        // Methode om de extra labels te initialiseren
        private void VerwerkVoertuigInfo(Voertuig mijnVoertuig)
        {
            if (mijnVoertuig is GetrokkenVoertuig mijnGetrokken)
            {
                lblGewicht.Content = mijnGetrokken.Gewicht?.ToString() + "kg";

                lblMaxb.Content = mijnGetrokken.Maxbelasting?.ToString() + "kg";

                lblGeremd.Content = mijnGetrokken.Geremd?.ToString();

                lblAfmetingen.Content = mijnGetrokken.Afmeting;
            }
        }

        // Eigenaar van voertuig tonen 
        private void ToonEigenaar(Voertuig mijnVoertuig)
        {
            Gebruiker voertuigEigenaar = Gebruiker.GetById(mijnVoertuig.Eigenaar_id.Id);
            if (voertuigEigenaar != null)
            {
                lblEigenaar.Content = $"{voertuigEigenaar.Voornaam} {voertuigEigenaar.Achternaam}";
            }
        }

        // Formchecking als het ingevuld is of niet + zet een oranje / als het leeg is 
        private string CheckEmptyOrNull(string input, Label label)
        {
            if (string.IsNullOrEmpty(input))
            {
                label.Foreground = Brushes.Orange;
                return "/";
            }
            return input;
        }

        // Laad afbeelding in elke image-control
        private void LaadAfbeeldingen(Voertuig mijnVoertuig)
        {
            List<Foto> fotos = Foto.GetAllAutoPictures(mijnVoertuig.Id);
            Image[] images = { img1, img2, img3 };

            for (int i = 0; i < Math.Min(fotos.Count, images.Length); i++)
            {
                images[i].Source = LoadImage(fotos[i].Data);
            }
        }

        // Converteert bytes van de database naar  image met een MS 
        private ImageSource LoadImage(byte[] picturesDatabase)
        {
            if (picturesDatabase == null || picturesDatabase.Length == 0) return null;
            var picture = new BitmapImage();
            using (var memory = new MemoryStream(picturesDatabase))
            {
                memory.Position = 0;
                picture.BeginInit();
                picture.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
                picture.CacheOption = BitmapCacheOption.OnLoad;
                picture.UriSource = null;
                picture.StreamSource = memory;
                picture.EndInit();
            }
            picture.Freeze();
            return picture;
        }

        // Ontleining bevestigen 
        private void btnBevestigen_Click(object sender, RoutedEventArgs e)
        {
            if (DatePickerControl())
            {
                Ontlening newOntlening = new Ontlening();
                newOntlening.Vanaf = dtpVan.SelectedDate.Value;
                newOntlening.Tot = dtpTot.SelectedDate.Value;
                newOntlening.Bericht = txtBericht.Text;
                newOntlening.Status = OntleningStatus.InAanvraag;
                newOntlening.VoertuigId = voertuigen.Id;
                newOntlening.AanvragerId = gebruikers.Id;

                try
                {
                    Ontlening.Insert(newOntlening);
                    MessageBox.Show("Uw aanvraag is verstuurd =)");
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Er is een fout opgetreden bij het verzenden van uw aanvraag: " + ex.Message);
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
