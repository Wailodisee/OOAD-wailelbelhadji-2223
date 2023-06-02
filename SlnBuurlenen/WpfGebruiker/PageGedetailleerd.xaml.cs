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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using MyClassLibrary;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for PageGedetailleerd.xaml
    /// </summary>
    public partial class PageGedetailleerd : Page
    {
        private Gebruiker mijnGebruiker;
        private Voertuig mijnVoertuig;
        public PageGedetailleerd(Gebruiker mijnGebruiker)
        {
            InitializeComponent();
            this.mijnGebruiker = mijnGebruiker;
            Voertuig voertuig = new GetrokkenVoertuig();
            this.mijnVoertuig = voertuig;
        }

        // Creer nieuwe voertuig
        private void GetCreationVoertuig()
        {
            if (mijnVoertuig is GetrokkenVoertuig getrokkenVoertuig)
            {
                getrokkenVoertuig.Gewicht = int.TryParse(txtGewicht.Text, out int gewicht) ? gewicht : 0;

                getrokkenVoertuig.Afmeting = txtAfmetingen.Text;

                getrokkenVoertuig.Maxbelasting = int.TryParse(txtmaxG.Text, out int maxbelasting) ? maxbelasting : 0;

                getrokkenVoertuig.Geremd = rbnJa.IsChecked == true;
            }

            if (mijnVoertuig.Eigenaar_id == null)
            {
                mijnVoertuig.Eigenaar_id = new Gebruiker();
            }
            mijnVoertuig.Eigenaar_id.Id = mijnGebruiker.Id;
            
            mijnVoertuig.Merk = txtMerk.Text;
            mijnVoertuig.Model = txtModel.Text;
            mijnVoertuig.Naam = txtNaam.Text;
            mijnVoertuig.Beschrijving = txtBeschrijving.Text;
            mijnVoertuig.Type = 2;
            
            if (int.TryParse(txtBouwjaar.Text, out int bouwjaar))
            {
                mijnVoertuig.Bouwjaar = bouwjaar;
            }
        }

        // lijst images
        private List<byte[]> listImages = new List<byte[]>();
        private void UploadImages(int voertuigId)
        {
            foreach (var imgData in listImages)
            {
                Foto pictureImage = new Foto();
                pictureImage.Data = imgData;
                pictureImage.VoertuigId = voertuigId;
                pictureImage.GetImagesFromDB();
            }
        }

        // Converter van chatgpt
        public static byte[] ConvertImage(ImageSource imgS)
        {
            var bmpSource = (BitmapSource)imgS;

            var enc = new PngBitmapEncoder();

            enc.Frames.Add(BitmapFrame.Create(bmpSource));

            using (var msS = new MemoryStream())
            {
                enc.Save(msS);
                return msS.ToArray();
            }
        }

        // Button opslaan
        private void btnOpslaan_Click(object sender, RoutedEventArgs e)
        {
            if (CheckingNaamBeschrijving())
            {
              GetCreationVoertuig();
                int newVoertuigId = Voertuig.CreateRecord(mijnVoertuig);
                mijnVoertuig.Id = newVoertuigId;
                UploadImages(mijnVoertuig.Id);
                MessageBox.Show("Voertuig is toegevoegd.");
            }
        }

        // Formchecking
        private bool CheckingNaamBeschrijving()
        {
            if (string.IsNullOrEmpty(txtNaam.Text))
            {
                lblErrorr.Content = "Naam is verplicht."; // error message maar komt in een messagebox
                return false;
            }

            if (string.IsNullOrEmpty(txtBeschrijving.Text))
            {
                lblErrorr.Content = "Beschrijving is verplicht."; // error message maar komt in een messagebox
                return false;
            }

            return true;
        }

        // Button uploaden + chatgpt
        private void btnUploaden_Click_1(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Image files (*.jpg, *.jpeg, *.png) | *.jpg; *.jpeg; *.png";

            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == true)
            {
                string[] fileRoot = openFileDialog.FileNames;

                if (fileRoot.Length > 3)
                {
                    return;
                }

                List<Image> imgcontrol = new List<Image> { img1, img2, img3 };

                for (int ix = 0; ix < fileRoot.Length; ix++)
                {
                    string filePath = fileRoot[ix];
                    BitmapImage bpImg = new BitmapImage(new Uri(filePath));
                    byte[] imgData = ConvertImage(bpImg);

                    listImages.Add(imgData);

                    if (ix < imgcontrol.Count)
                    {
                        imgcontrol[ix].Source = bpImg;
                    }
                }
            }
        }

        // derde button verwijderen
        private void btnVerwijderen3_Click(object sender, RoutedEventArgs e)
        {
            img3.Source = null;
            if (listImages.Count > 0)
            {
                listImages.RemoveAt(0);
            }
            else
            {
                img1 = null;
            }
        }

        // eerste button verwijderen
        private void btnVerwijderen1_Click(object sender, RoutedEventArgs e)
        {
            img1.Source = null;
            if (listImages.Count > 0)
            {
                listImages.RemoveAt(0);
            }
            else
            {
                img1 = null;
            }
        }

        // // tweede button verwijderen
        private void btnVerwijderen2_Click(object sender, RoutedEventArgs e)
        {
            img2.Source = null;
            if (listImages.Count > 0)
            {
                listImages.RemoveAt(0);
            }
            else
            {
                img1 = null;
            }
        }
    }
}
