using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
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
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "tekstbestand (*.vcf)|*.vcf";
            if (openFileDialog.ShowDialog() == true)
            {
                string[] vcffile = File.ReadAllLines(openFileDialog.FileName);

                string voornaam = "";
                string achternaam = "";
                string email = "";
                string telefoon = "";
                string jobtitel = "";
                string bedrijf = "";
                string emailwerk = "";

                foreach (string vcfregel in vcffile)
                {


                    if (vcfregel.StartsWith("N;"))
                    {
                        string[] parts = vcfregel.Split(';', ':');
                        voornaam = parts[3];
                        achternaam = parts[2];
                    }
                    else if (vcfregel.StartsWith("EMAIL;CHARSET=UTF-8;type=HOME"))
                    {
                        string[] parts = vcfregel.Split(':', ';');
                        email = parts[3];
                    }
                    else if (vcfregel.StartsWith("TEL;"))
                    {
                        telefoon = vcfregel.Substring(vcfregel.IndexOf(":") + 1);

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
                        string[] parts = vcfregel.Split(':', ';');
                        jobtitel = parts[1];
                    }
                    else if (vcfregel.StartsWith("ORG;"))

                    {
                        string[] parts = vcfregel.Split(':', ';');
                        bedrijf = parts[1];
                    }
                    else if (vcfregel.StartsWith("EMAIL;CHARSET=UTF-8;type=WORK"))

                    {
                        string[] parts = vcfregel.Split(':', ';');
                        emailwerk = parts[3];
                    }
                }

                // Remplir les champs de texte
                txtvoornaam.Text = voornaam;
                txtachternaam.Text = achternaam;
                txtmail.Text = email;
                txttel.Text = telefoon;
                txtjobtitel.Text = jobtitel;
                txtbedrijf.Text = bedrijf;
                txtwerkmail.Text = emailwerk;
            }
        }

        private void subItemSave_Click(object sender, RoutedEventArgs e)
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
        private void subItemSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "tekstbestand (*.vcf)|*.vcf";
            if (saveFileDialog.ShowDialog() == true)
            {

                vCardOpslaan(saveFileDialog.FileName);
                SaveFilePath = saveFileDialog.FileName;
            }
        }

        private void vCardOpslaan(string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {


                writer.WriteLine("BEGIN:VCARD\r\nVERSION:3.0");
                if (txtachternaam.Text != "")
                {
                    writer.Write($"N;CHARSET=UTF-8:{txtachternaam.Text};");
                }
                if (txtvoornaam.Text != "")
                {
                    writer.Write($"{txtvoornaam.Text};");
                }
                writer.WriteLine(";;");

                if (rbnman.IsChecked.Value)
                {
                    writer.WriteLine("GENDER:M");
                }
                if (rbnvrouw.IsChecked.Value)
                {
                    writer.WriteLine("GENDER:F");
                }
                if (rbnonbekend.IsChecked.Value)
                {
                    writer.WriteLine("GENDER:O");
                }
                if (dtgeboorte.SelectedDate != null)
                {
                    writer.WriteLine($"BDAY:{dtgeboorte.SelectedDate.Value.ToString("yyyyMMdd")}");
                }
                if (txtmail.Text != "")
                {
                    writer.WriteLine($"EMAIL;CHARSET=UTF-8;type=HOME,INTERNET:{txtmail.Text}");
                }
                if (txtwerkmail.Text != "")
                {
                    writer.WriteLine($"EMAIL;CHARSET=UTF-8;type=WORK,INTERNET:{txtwerkmail.Text}");
                }
                if (imgPersoonlijk.Source != null)
                {
                    MemoryStream memoryStream = new MemoryStream();
                    JpegBitmapEncoder jpegBitmapEncoder = new JpegBitmapEncoder();
                    BitmapFrame bitmapFrame = BitmapFrame.Create((BitmapImage)imgPersoonlijk.Source);
                    jpegBitmapEncoder.Frames.Add(bitmapFrame);
                    jpegBitmapEncoder.Save(memoryStream);
                    writer.WriteLine($"PHOTO;ENCODING=b;TYPE=JPEG:{Convert.ToBase64String(memoryStream.ToArray())}");
                }
                if (txttel.Text != "")
                {
                    writer.WriteLine($"TEL;TYPE=CELL:{txttel.Text}");
                }
                writer.WriteLine("END:VCARD");
                MessageBox.Show("Document werd goed opgeslagen");
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
            MessageBoxResult respons = MessageBox.Show(" Er zijn onopgelsagen wijzigingen ?","Ben je zeker?",MessageBoxButton.OKCancel);
            if (respons == MessageBoxResult.OK)
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
            else
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
}

