using Microsoft.Win32;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;


namespace WpfVcardEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string SaveFilePath = "";
        bool haschanged = false;
        public MainWindow()
        {
            InitializeComponent();
        }




        /* Window afsluiten*/
        private void subItemExit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult meldingtonen = MessageBox.Show("Ben je zeker dat je de applicatie wil afsluiten?",
            "Toepassing sluiten", MessageBoxButton.OKCancel);

            if (meldingtonen == MessageBoxResult.OK)
            {
                Application.Current.MainWindow.Close();
            }
        }

        /* Window tonen */
        private void subItemAbout_Click(object sender, RoutedEventArgs e)
        {
            (new myPopUp()).Show();
        }

        /* Open genereren */
        private void subItemOpen_Click(object sender, RoutedEventArgs e)
        {
            string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = folderPath;
            ofd.Filter = "Fichiers vCard (*.vcf)|*.vcf";

            if (ofd.ShowDialog() == true)
            {
                string filePath = ofd.FileName;

                statusbar1.Text = $"Huidige kaart: {filePath}";

                Vcard card = ReadVcardFromFile(filePath);

                ShowVcard(card);
            }
            subItemSave.IsEnabled = true;

        }

        private Vcard ReadVcardFromFile(string filepath)
        { 
            Vcard card = new Vcard();

            string[] vcffile = File.ReadAllLines(filepath);

            statusbar1.Text = $"Huidige kaart: {filepath}";


            foreach (string vcfregel in vcffile)
            {
                if (vcfregel.StartsWith("N;"))
                {
                    string[] parts = vcfregel.Split(';', ':');
                    card.voornaam = parts[3];
                    card.achternaam = parts[2];
                }
                else if (vcfregel.StartsWith("EMAIL;CHARSET=UTF-8;type=HOME"))
                {
                    string[] parts = vcfregel.Split(':', ';');
                    card.priveEmail = parts[3];
                }
                else if (vcfregel.StartsWith("TEL;"))
                {
                    card.priveTelefoon = vcfregel.Substring(vcfregel.IndexOf(":") + 1);
                }
                else if (vcfregel.StartsWith("BDAY"))
                {
                    string[] parts = vcfregel.Split(':', ';');
                    string geboortedatum = parts[1];
                    DateTime datum;
                    if (DateTime.TryParseExact(geboortedatum, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out datum))
                    {
                        dtgeboorte.SelectedDate = datum;
                    }
                }
                else if (vcfregel.StartsWith("GENDER"))
                {
                    string[] parts = vcfregel.Split(':', ';');
                    string rbngeslacht = parts[1];
                    if (rbngeslacht == "M")
                    {
                        rbnman.IsChecked = true;
                    }
                    else if (rbngeslacht == "O")
                    {
                        rbnonbekend.IsChecked = true;
                    }
                    else if (rbngeslacht == "F")
                    {
                        rbnvrouw.IsChecked = true;
                    }
                }
                else if (vcfregel.StartsWith("TITLE;"))
                {
                    card.JobTitel = vcfregel.Substring(vcfregel.IndexOf(":") + 1);
                }
                else if (vcfregel.StartsWith("ORG;"))
                {
                    card.bedrijf = vcfregel.Substring(vcfregel.IndexOf(":") + 1);
                }
                else if (vcfregel.StartsWith("EMAIL;CHARSET=UTF-8;type=WORK"))
                {
                    string[] parts = vcfregel.Split(':', ';');
                    card.WerkEmail = parts[3];
                }
                   
            }
               return card; 
        }
        private void ShowVcard(Vcard card)
        {
            txtvoornaam.Text = card.voornaam;
            txtachternaam.Text = card.achternaam;
            dtgeboorte.SelectedDate = card.DatumPicker;
            switch (card.gender)
            {
                case 'M':
                    rbnman.IsChecked = true;
                    break;
                case 'O':
                    rbnonbekend.IsChecked = true;
                    break;
                case 'F':
                    rbnvrouw.IsChecked = true;
                    break;
            }
            txtmail.Text = card.priveEmail;
            txttel.Text = card.priveTelefoon;
            txtbedrijf.Text = card.bedrijf;
            txtjobtitel.Text = card.JobTitel;
            txtwerkmail.Text = card.WerkEmail;
            txtwerktel.Text = card.WerkTelefoon;
        }

        private Vcard ToVcard()
        {
            Vcard card = new Vcard();
            card.voornaam = txtvoornaam.Text;
            card.achternaam = txtachternaam.Text;
            card.DatumPicker = dtgeboorte.SelectedDate ?? DateTime.MinValue;

           
            if (rbnman.IsChecked == true)
            {
                card.gender = 'F';
            }
            else if (rbnman.IsChecked == true)
            {
                card.gender = 'M';
            }
           
            else if (rbnonbekend.IsChecked == true)
            {
                card.gender = 'O';
            }
            card.priveEmail = txtmail.Text;
            card.priveTelefoon = txttel.Text;
            card.bedrijf = txtbedrijf.Text;
            card.JobTitel = txtjobtitel.Text;
            card.WerkEmail = txtwerkmail.Text;
            card.WerkTelefoon = txtwerktel.Text;
            return card;
        }


        private void subItemSave_Click(object sender, RoutedEventArgs e)
        {
            try

            {
                if (SaveFilePath == "")
                {
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    saveFileDialog.Filter = "tekstbestand (*.vcf)|*.vcf";
                    if (saveFileDialog.ShowDialog() == true)
                    {

                        vCardOpslaan(saveFileDialog.FileName);
                        SaveFilePath = saveFileDialog.FileName;

                    }
                }
                else
                {
                    vCardOpslaan(SaveFilePath);
                }
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Het bestand bevat geen naam.", "FOUT", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("U kan dit bestand niet oplsaan.", "FOUT", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (IOException)
            {
                MessageBox.Show("Een fout nam plaats tijdens het opslaan van het bestand.", "FOUT", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Een onbekende fout is opgetreden: " + ex.Message, "FOUT", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void subItemSaveAs_Click(object sender, RoutedEventArgs e)
        {
            subItemSave.IsEnabled = true;
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "tekstbestand (*.vcf)|*.vcf";
                if (saveFileDialog.ShowDialog() == true)
                {
                    vCardOpslaan(saveFileDialog.FileName);
                    SaveFilePath = saveFileDialog.FileName;
                }
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("U kan dit bestad niuet opslaan");
            }
            catch (IOException)
            {
                MessageBox.Show("Een fout nam plaats tijdens het opslaan van het bestand.", "FOUT", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Een onbekende fout is opgetreden: " + ex.Message, "FOUT", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void vCardOpslaan(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                Vcard card = ToVcard();
                File.WriteAllText(filePath, card.GenerateVcardCode());
            }
        }

        private void Card_Changed(object sender, EventArgs e) 
        {
            int teller = 0;
            if (txtachternaam.Text != "")
            {
                teller++;
            }
            if (txtvoornaam.Text != "")
            {
                teller++;
            }
            if (rbnman.IsChecked.Value)
            {
                teller++;
            }
            if (rbnvrouw.IsChecked.Value)
            {
                teller++;
            }
            if (rbnonbekend.IsChecked.Value)
            {
                teller++;
            }
            if (dtgeboorte.SelectedDate != null)
            {
                teller++;
            }
            if (txtmail.Text != "")
            {
                teller++;
            }
            if (txtwerkmail.Text != "")
            {
                teller++;
            }
            if (imgPersoonlijk.Source != null)
            {
                teller++;
            }
            if (txttel.Text != "")
            {
                teller++;
            }
            if (teller > 0)
            {
                haschanged = true;
            }
            else
            {
                haschanged = false;
            }
        }

        private void subItemNew_Click(object sender, RoutedEventArgs e)
        {
            if (haschanged)
            { 
            MessageBoxResult respons = MessageBox.Show(" Er zijn onopgeslagen wijzigingen ?","Ben je zeker?",MessageBoxButton.OKCancel);
            if (respons == MessageBoxResult.OK)
            {
               ResetForm();
            }

            }   
            else
            {
                ResetForm();
            }
        }

        private void btnselected_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Image files (*.jpg, *.jpeg, *.png, *.gif) | *.jpg; *.jpeg; *.png; *.gif";
            Nullable<bool> result = dialog.ShowDialog();

            if (result == true)
            {
                string selectedFile = dialog.FileName;
                BitmapImage image = new BitmapImage(new Uri(selectedFile));
                imgPersoonlijk.Source = image;
                lblselectedfoto.Content = selectedFile;
            }
        }
        private void ResetForm()
        {
            txtvoornaam.Text = "";
            txtachternaam.Text = "";
            dtgeboorte.SelectedDate = null;
            rbnman.IsChecked = false;
            rbnvrouw.IsChecked = false;
            rbnonbekend.IsChecked = false;
            txtmail.Text = "";
            txttel.Text = "";
            imgPersoonlijk.Source = null;
        }
    }
}

