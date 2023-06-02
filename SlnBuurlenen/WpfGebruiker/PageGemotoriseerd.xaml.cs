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
    /// Interaction logic for PageGemotoriseerd.xaml
    /// </summary>
    public partial class PageGemotoriseerd : Page
    {
        private Gebruiker mijnGebruiker;
        private Voertuig mijnVoertuig;
        public PageGemotoriseerd(Gebruiker mijnGebruiker)
        {
            InitializeComponent();
            this.mijnGebruiker = mijnGebruiker;
            Voertuig voertuig = new MotorVoertuig();
            this.mijnVoertuig = voertuig;
        }
        private void GetCreationVoertuig()
        {
            if (mijnVoertuig is MotorVoertuig motorVoertuig)
            {
                if (cbxTransmissie.SelectedItem is ComboBoxItem selectedItemTransmissie)
                {
                    switch (selectedItemTransmissie.Content.ToString().ToLower())
                    {
                        case "manueel": motorVoertuig.Transmissie = TransmissieEnum.Manueel;
                            break;
                        case "automatisch": motorVoertuig.Transmissie = TransmissieEnum.Automatisch;
                            break;
                        default: motorVoertuig.Transmissie = null;
                            break;
                    }
                }

                if (cbxBrandstof.SelectedItem is ComboBoxItem selectedItemBrandstof)
                {
                    switch (selectedItemBrandstof.Content.ToString().ToLower())
                    {
                        case "benzine": motorVoertuig.Brandstof = BrandstofEnum.Benzine;
                            break;
                        case "diesel": motorVoertuig.Brandstof = BrandstofEnum.Diesel;
                            break;
                        case "lpg": motorVoertuig.Brandstof = BrandstofEnum.LPG;
                            break;
                        default: motorVoertuig.Brandstof = null;
                            break;
                    }
                }
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
            mijnVoertuig.Type = 1;

            if (int.TryParse(txtBouwjaar.Text, out int bouwjaar))
            {
                mijnVoertuig.Bouwjaar = bouwjaar;
            }
        }

        private void btnOpslaaan_Click(object sender, RoutedEventArgs e)
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
        public static byte[] ConvertImage(ImageSource imgS)
        {
            var bpSource = (BitmapSource)imgS;

            var enc = new PngBitmapEncoder();

            enc.Frames.Add(BitmapFrame.Create(bpSource));

            using (var memS = new MemoryStream())
            {
                enc.Save(memS);
                return memS.ToArray();
            }
        }

        private void btnUploaden_Click(object sender, RoutedEventArgs e)
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

                for (int i = 0; i < fileRoot.Length; i++)
                {
                    string filePath = fileRoot[i];
                    BitmapImage bpImg = new BitmapImage();
                    bpImg.BeginInit();
                    bpImg.CacheOption = BitmapCacheOption.OnLoad;
                    bpImg.UriSource = new Uri(filePath);
                    bpImg.EndInit();

                    byte[] imageData = ConvertImage(bpImg);

                    listImages.Add(imageData);

                    if (i == 0)
                    {
                        img1.Source = bpImg;
                    }
                    else if (i == 1)
                    {
                        img2.Source = bpImg;
                    }
                    else if (i == 2)
                    {
                        img3.Source = bpImg;
                    }
                }
            }
        }

        private void btnAnnuleren_Click(object sender, RoutedEventArgs e)
        {
        }

        private bool CheckingNaamBeschrijving()
        {
            if (string.IsNullOrEmpty(txtNaam.Text))
            {
                MessageBox.Show("Naam is verplicht.");
                return false;
            }

            if (string.IsNullOrEmpty(txtBeschrijving.Text))
            {
                MessageBox.Show("Beschrijving is verplicht.");
                return false;
            }

            return true;
        }

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
    }

}
