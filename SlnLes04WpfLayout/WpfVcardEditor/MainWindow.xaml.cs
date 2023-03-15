using Microsoft.Win32;
using System;
using System.Globalization;
using System.IO;
using System.Windows;
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
            subItemSave.IsEnabled = true;
            try
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
            catch (FileNotFoundException)
            {
                MessageBox.Show($"Het bestand {SaveFilePath}kan niet gevonden worden.", "FOUT", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (UnauthorizedAccessException)
            {
                MessageBox.Show("U hebt geen acces tot dit bestand", "FOUT", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
            try { 
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

            catch (IOException)
            {
                MessageBox.Show("Een fout nam plaats tijdens het opslaan van het bestand.", "FOUT", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Een onbekende fout is opgetreden: " + ex.Message, "FOUT", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void btnselected_Click(object sender, RoutedEventArgs e)
        {
            string base64String = "data:image/jpeg;base64,/9j/2wCEAAgGBgcGBQgHBwcJCQgKDBQNDAsLDBkSEw8UHRofHh0aHBwgJC4nICIsIxwcKDcpLDAxNDQ0Hyc5PTgyPC4zNDIBCQkJDAsMGA0NGDIhHCEyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMjIyMv/AABEIAZABkAMBIgACEQEDEQH/xAGiAAABBQEBAQEBAQAAAAAAAAAAAQIDBAUGBwgJCgsQAAIBAwMCBAMFBQQEAAABfQECAwAEEQUSITFBBhNRYQcicRQygZGhCCNCscEVUtHwJDNicoIJChYXGBkaJSYnKCkqNDU2Nzg5OkNERUZHSElKU1RVVldYWVpjZGVmZ2hpanN0dXZ3eHl6g4SFhoeIiYqSk5SVlpeYmZqio6Slpqeoqaqys7S1tre4ubrCw8TFxsfIycrS09TV1tfY2drh4uPk5ebn6Onq8fLz9PX29/j5+gEAAwEBAQEBAQEBAQAAAAAAAAECAwQFBgcICQoLEQACAQIEBAMEBwUEBAABAncAAQIDEQQFITEGEkFRB2FxEyIygQgUQpGhscEJIzNS8BVictEKFiQ04SXxFxgZGiYnKCkqNTY3ODk6Q0RFRkdISUpTVFVWV1hZWmNkZWZnaGlqc3R1dnd4eXqCg4SFhoeIiYqSk5SVlpeYmZqio6Slpqeoqaqys7S1tre4ubrCw8TFxsfIycrS09TV1tfY2dri4+Tl5ufo6ery8/T19vf4+fr/2gAMAwEAAhEDEQA/AO8zR25NHeiuE7RQBigDFHSlpAIfej+VL9aB0pgFJmlooAQc0uaSigQcUhANLSeuaBXDoKWmmndqAEowKMUvpigAFJ3zR7UtAhc8UcUmaDQAtHVsk0lLQAZpaSigYtA70lLSAXt1pRTaXNAXFo4pBS0DCjFL+NGcUAAoxQKBSGHGaXijvRgYoATvS8fWkxRjigBelLwetJ160p6UAHFA7c0mKXt0pDDH/wCqgilzTe9ACkdqPYUUD60gAcH0oOc9zSn86SgBOlL2ooPHSgCDtSc0uMcUDpzWghRzRQAKKQB3NFHaimAlGT3pRjNJ1oEwJ6UdcUY6UtAhMUdqWj1oAQUd6MdaMUCDFHtRRQAUgp3FFACUDpS4zUM13bWylri4iiUd3cCgCaiucvPHPh6x4bUFlb+7Cpb/AOtWLN8V9FiZhHa3T491FVyS7E8yO9oFeb/8LesCTjTJRj1lH+FPHxcsiM/2dJj/AK6j/CnySHzxPRulGOa4GD4r6RIR5tpcoP8AZKn+orXsvHvh+8YKLvyS3/PYbf16VLhIOZM6igdTVW21C0ulDW91BKD3SQH+Rq3walq25S1DIopMY7UvJ6UAFLRigYoGhO9LS9aPakMM9aSlooAM80v8NJRQAoozxRS96ADtSd6XvSZ44pAHekI560pHejvQAvako7UUDDpRRRjvmgAzQeaMGjgUgIfSigf5zS4zWhIDjrRjFLSGgBMUvFJnNAoAO1ApRk9aTvQK4ZGaOgoxRQAUZ9qB60UCCijHFFACEGk7089K5zxF4y0rw+hSWUS3WDiGMgkH39KEm9hNpHQM6IpZjgDkknAFcnrfxE0bSC0cTG8mX+GFhtH/AAKvKPEPjnVNfbZNKI4M/LDGML+Pr+NctJK54OK3jSVtTN1Ox3+q/FLV7wvHbmO2iPTy1+b8zXHXmr3F07SzSPI7HlnOc1mF2z1pMk1qlbYzbbLBuCedxB9M1EXP/wCs1H17mjPY0xEokwMUb+eaiA6ZpR3oAlDgY71KszKvBIx0xVUY9aUNSGXY7+aFgUc59jz+ddZpPxG1vTl2G582IfwSgN+vWuGzzTg2OOfWlyp7juz3PSPijYXTKl9A1ux/jRty/l1rurW9tb6MS2lxHMh/iRgRXyusmANuA3rWxpHiK/0e5E1pcNFJjkjofY1nKknsWqjW59McijFee+Gvida32y21bbDOxwJVHyH6+lego6yKHRgVPQg5BrCUXHc1jJNaDulJ1pwAo9M1JQlFL1ooGFGKKX2oAO9LikA60uaAEpBxSmkpABo70p6UhZQetAwI5oxzim+ZTSxPU0gJCQMc00yDsKZ1FHagYuSRSE0duvNB496ADuaKKO1aEBSnBpe3NJigGIaKXHNAoFcAMUlLj0oxQAh9KXsaAMfWjGaADHY0lLiigQnamu6opdyFQDJJOMCmzzRWtu880gjijUs7N0AHU14x45+IEmrO9jpzGOxBwWGQZfr7VUY3ZLlY3fGXxMS33WOiOS+WWW4K9PZf8a8juLt7hy8jFmOSSf51FJJuOSTUJNdMYqKMJO45nJ69qaTkE005pRyKoABGeBQM/hSH1oz6UCHHAznmm7sUE8etN96AHdwetOycUzpSnkUALn/69KP0poPGKOfxoAd1PFOPGKjGcehpwbjFAx1AJHSkpRyM0ATRTNGR9a7nwj4/vdDkS3c+dZEkmIjke4NcD0HFOViMYzSaTVmNOzuj6o0rVbXWbCO9spN8L/mpHUGrtfNvhzxTfaDeJNaykKOHjJO1x9K988Pa/Z+ItPF1atyMCRDwVauapTcdUbwnzaGvSE80uOaAKyLExRSnjims6j60DHdqMcZqMyHpjmmsxPegCUkDvTS57VH+NApDFYk9eRRnFIaDQAc0dDSgZoxikMQnvR1oxTselACd6KXFGPWgBKWjNGa0IFpOtGaBQIX6UY5paMc0AIDzSetO74o4oEJjpSiiigAprEKCSQAOpPal7ivOPih4rOn2v9jWjjzrhMzspOUXsPx5/wAmqiruwm7K5zvxF8bnUZm0yxcrZxORI4P+tI/oOa81kk3k0ssu5jkVETx6V0xjZHO3diE5NHc5NNo69KoQueKPXikqxawxTbhJKIzgkE9KAK5ODjFJinhMnOc49KUg46daBkeeaM+1KcDtzTcHsKBDm6cUbunFJn2ox3oAOp6UvQ0gAzS96AFzx1opM8UD8KAHZpaaCB1pM80APzxTucCmdKUMc0gJFY5wOtdJ4W8TXfh7Uku4CWUfK8RPyuD2Irl+cetSJIV4HWiyejHc+pdH1yz1vTY7y0csrD5lI5U9wavebxwMV8/+CPFbaDqaGV9tpKwWdTngf3h7iveIpUliWRGDIwDKR3B71yVIcrOmEuZEpYkmkz7UZpM1mWKPWlpO1ApAKRmlA4pvNOzTGHsRSe9LnJ6UGkAY44paKXFAxCOlKR6UvSigBAM0YpR3NHWmIaRiiloxWhAdKcaT8aO1IB2DSYpy0mKYhPx5oxS4oxSATtSAZp/ak7UAUtVvotK0u5v5jhIIy5Hr6D88V80a9qsurapcXsxHmTuXPtnt+WK9Z+LWtNb2FvpSS7RODJIPUA8D88/lXiUrknpXRSjZXZjUethhOQKSjig8VqZjc0c0YzRnigAz9KcoNNUetOJ9sUDJMAc5GaaWBHWmkgnt9aQ89aQAeuABmkA9TSsc85pB060wEPBoHNO6nNHHagQg60p9qbnj6Uv49aAEFLn6UE4AFJ79KAF96B70vGKQmkAvbilB5po/yaUYz1oAcPfjNKOopBQCM0wJUfDgjr9a9i+GPijz0Oi3cvzqN1uSeo7r+FeMj1zWnpWozaffwXUMmySFw6n6VE48yKjKzPp7vTu1Zug6omtaNbX8eP3qDcB/C3cfnWljNcTVtDrTuGKWgdcUpHNAB9aUc9qPegCgAoxnvS4o6UhhjApR1oHXvS96AADFGMUvFAFABj2pAOaWlA5pgMope/tQRxVkCYpRRR3oAUHP1paTilFMQUUuPSkxQAGgUc0hYKpYnAAzk0hHzz8RNV/tPxVevuzHAfJQZ6BTg/rmuKbkk1pavP5+o3M3eSRm/M1lse1dltDne4vak5HJopQu6gQ3rS8detOxjpSKMH2oGL70wt6U9uM88VJDbNMw2g0AQqGZwMcU4jaeRU2wxuYyOB39KYwAIwdwNAyA0ZqVgAKgJ4xTEx2cijpSdB1peKAEP3qU0UvNACZ3de1PC4FGAB0xRzQAtBXNNJpcndxSAQqccUiip9u+PK9fSogOef1pgAG7vinbe+KQnaaUNhh3/GgAOBt4NSRnHPXFNY5oVsdB70gPWvhPrvlzT6NK5Ik/ewknvj5h+PWvWOtfM/hvUjpWuWV6pI8mQE8/w9/0r6Vt5Y7i3jnibdHIoZT7HpXNWjZ3N6TurEgFLRRWJqL6UopMUo460AIaUUuKBQAuMUmKXHFLigY3vTu9LtooAMcZxRzS84pQDQBHikOcU7OBgUGtCBBSYpQKWkIOKSlxRTAVaXFNzTgeKADtVe9JWwuWHGImP6GrIqK6jM1pNEP442X8xQhM+TbskyknuTVQ9ferl6CJnB6qSKpMe9da1OdjlGSaf0FBUqoJGCaRSe54oAcigk5pWwD6cU4IDyG4prjnBoGCAZzjP1rQgYLI0YOCRlTVD7pzUm7IDcjFILCzHkjv3NR4AXHPrUjsGXio14NMdiI0zYSanUbnAqwsBPTH5Um7DUblHyz6UvknrWsluNvOT9ad9kUr909etTzleyMYoRSA44Na72ePWqUtuVJyKalcmUGiuDSjBB9aNpppyBxVEDhEzHjNSm2kByBUBY44J/OnRyEdM4+tMB4SWJgWUg5p/llm5Xkn0pDKwH3smrEM4kQkqDjt0qWNETWkmMBM4PUUz7NIOQoOPSrQkKkLlhz0wD/9ekk5OQw+lF2FiphuflPvxSsoVAd6k+gNSOzdDyvoec1AoGT2piLEDhTk19G+BLs3ngvTZCcssew8/wB0kV83occbua90+ElyZfDE1uf+WU56e4z/AErKsvduaU37x32O9LilFKK5joExS9aXFHIFIAxRt5pQM0uMdaAEApcUo6UuM9DQAg60Y56U/FGKAExRjBpcUuKYEGKXFL3pDVkBjFGKUUmKAEopxGOtMaSNOGYfSkAtOHWq5ukz8oNRNO7HrgUwLpZV+82KYbpAcKMmqDuccZJNQybljJzg+1ILHzXrsZttXvIGXGyZ1z9GNZXU/wAq6vx3Zm38R3L9RId/4nr+ua5boc11rY52tS065i564FV0TMmPSraHfCPXvUany24AOPWi4x0cJJJ6CpRaxvJy5C+tCtv5J/Ch3JOB+ApNjsSm0gAyjhs9AT0qoyFQQeKcruz8Hp1qcqGG0A49aVyrFUAqBnOKUKZF2jA75qUxZbgkirUNtgZPehySBRbK0NvxkZJq/DAepx+VTwQbR0qzGgI6Vm5XN4wIktwcVYWD1qeNOelTADpis7mvKUHthjIqlcWoY+/rW4yjFV5YA3I601IThc52exBQsuQR1rNkjZeDxXXmDj61RuLBJCA6k+9aRqdzGdG+xzGO1LnArTn05YySM4x61QaBgCfStVJMwcGiPPvUkcpRgSM1FtbPIp4U0yCwJFf/AGT6GgE5OelV2XPrmnqeg7UWGWRKqlQRmq0gw5IPH1peAeM/jUbcHPNAiSM5cDNe0fBuTNlqcecAPGR+TV4sjHO4de3tXtHwewml6kx6vLGPyB/xrOr8JdP4j1HFOAqOORXyAeRUoFcp0iYp2KAOaXHBoATHNKKWigBAOlKF9KUCnd6BCAUYpe9O4pANxS4p2OaMUwK+OaXFVHuyR8q4FQNK7/eJIq7isXnljTq2fpUL3Zz8gAHqaqZ603pSCxM87uTls1Fk0Ck+tIdgyc5ozmgUdelAgPWo5m+U9lA5NOY47mqKz+bK6rglGwV7n3oGjzL4l2Ba5hnjbK4bj05zXmjKdxHoea9t8bGzvNEn5AuVXegY9CP8a8dljDksevtXTBtoxmtRkEm2Nh61A7gSZH41PlI0YdSeKrOCWJHSrRmSRuCpJIFTo4GBgH8aocir9ogkHelLTUqOuhLuXbjv6U5FZ2wKui2HA2irEcCjgc1m5GygyskG3jqasJGcjAI9qsLb89Ksx2wHJzUN3NoxK6o2RU6R4FWY7YE9DmphAPSoNUiBFPFP281KIsGlMZA6UDsQ4pGGRUuz2pNueKQrEGDionQMKtbCRjFNMeOtMVjHuY8jBHOetZc0e3sOldDcQhgc1jXEZXrkH+daRZhOJkOF3ZFNJ7CnSjk4HFRlW44rdHIxQD0NKoxnHWlCs3FSRwOQT1NMBox0NOMa4pTEVPzKfrUqKD8gHJoHYqKPmxivYvBUDaZ4FW8eZ0E9wXUKcZA4/pXkwibzFQDPOMYr2zwnZvqllpto+BYafGuQV/1sh5OPUZzzWVV6F01qdjo/ntpcE03EroGYHr+NaituQEDFNRcKABwOlSIvzNjknmuY2FApw6mjFKBxQCDFLilA4ooAMUuKMU7FADcUu2lAGadxQA3FFO6ikxQBgck0uKTvRmmAUhpaSgBaTpSgGkoAQ8deKaZAo459hTnGVIziqUriKQSBtnbbjqPWgRNI+4YYYxWHrd/HYWzTKoJlwq4HQ+tT6prFtpdgbqZsAdB1yT2xXnPiC+vb61F1OBHGzExxgndt7E+lVFaibMq6vjfXTyu+5EyeT1Fc5c8TYGMMc1achYBjqRzVOfBxgk/UYrdeRmyB2GPTmoyN3Skfg/40qNk4qzMjI5IrZ0iLcm8jp0rLZQTWpBN9mtViT738R9KUti4aO5rBVUn3qRSo54rLS4YqMcf1oa82DCklqz5DX2huQqXcAVswac7AE456VxserXMTfLwfrWhFrtyUGZGB9j0ocGWqqO1tdKRly7c4q4uiRn+LGK5PT9cmjmBdiV9zXTafqqzuN5PPFZSi0dNOpFjpdAGQUfOfaoZfD9yiblXd7VupOrYORz0xVpJioz2HWs7s2smcNLZvESGXBHY9ah8oD1rtb62iulOVALDOcVy+oQC0Yr2PQ01qZyVip5YqORFAyazrrVPJfYBn3rOuNXmZuDgDsBWii2ZOokbEiKwOBms+5g3DrVJdYdSN4Zuak/tGKbhSN3XBqkrGbmmY17bmOU+h/SqhOGx6VrXkivznkHoaznTJLLW0djmklciD46c1PGxb7v3qrrGe1OI8vBYe1Mg0JWV0Ct8rjvTEQZzxweDVZJl3gFR7Zq1H824DnI4pMo1/DVjHfa5arITs8zLgjI45x+mK900W7tYYvLzGGBIbYpGDnp7f4V4z4ZuBYu5ZF3y4UsUB2r3P616todnp0lopLibnAZWyB65HrWE9zSOh2COsigqVOfTmnquAffrWRaxvYyK0bM8DHDoTnYfUeozW0OlZlABS4o6c0ooATFLigUtIAxS9KKUc0AJS4pfaigA7UUdqM8CgZzw60UUDpTAKKKSgVxaKXFFAXK9zMkUe5iAoqjPqMcNuZZJFjTHVqNdmjjsmUgPIxGyPuxrz3ViskQhjHmSPjPUsPbJqkhMrahqq32otcSuSkfyxL6++KztR1Fp4Wt44E3SHlnbLD6elbj2Ftp1jG2olV2DJVBgnPv1NYt34nQ27w2tjBGjDAOORWiRLZydwGhfbIpU9+Kjk+6CDwatXEy3kpZkCknnHeqqnYxQjgHitCCsy05I88nipSF7etADM2FGf6U7isOt4TLKAqkgVqRWErtgROfcDOKvaNpbNCrt/F29a6aG1aFRkAD3Ws3PU2hSujkP7JuJOURlHfNKuiXSsCUOO9dmdqdBz7VGzI3GaXOaexRzB0hymdmCKhfTXT7qtXUF1GR2poaMkZGaXOxeyRz0aMhUkYxwa1rG4ZDH6HjntVxrOKXkdfaoksiJFUZAz1p8yBQaeh1FnOGcNkHPWtBJtzgbcAmsmwi2Y29K3La33/MynaOa52dsG7FW5naMOvHPA9s1z2rybkBJ5OcmuhuoWAkJUnJ4JFc1qCbiyjp2qo6EVOxx1zFunYn14qu8BYnHTqTXQmxMjnsM0p09Ohb8BW3Mjl5Gcw0DuNyjIqrNHsIGT9cV2n2Beg6Gqk2jpIeuPfFPmQnTZyDStwCScetN8zkmuhm0JlJK5x9KoXGkyR5CA+vNaKSZk4NFFYw+eB9KbKhjUE9KlCSRMMjGKdOPMiwBkj09KLisUgFJyDzWjCw8vGQDiqKR7HGRj6irSEpn5eTQxGlbSSxhXiZlccg5rc03xBc28yJcyMm45EncfUdxVHSLB76CZ1DZjAIwM0t9YFIWZNxkQZwRis3Yo9b0DV5ppfst2AxliDxSLkrIB3BrtoP8AUrn0FeTeApZr/TIo2DZt5v3bdduf/r16vA5aJSRzjke9YvctE1KBxSZ9aTcMVIDulLUTSheSQB6mqz6naL1nQn0XmgaRezgUbqxJddAbEUBYerHFU5tVupcjKoP9nrSuUonTM4UZJAHvVaXU7OE4edM+g5/lXLO0kpJd3Y/7RqPHPNFx8pvS+IIwSIYmb3JxVSbW7qThAsY9utZg6g+lO6/Si47Gt3ooHWlqzMTpRRS0gDHFAHNGKO9MDA1+DyrmG+cg26qY5Ac4GehOO3+NYOp2S2+p206BRC2SmMbc88fyru3jSRCkiBlIwQehrjvEOnTWFmRE37hWDxE9Yz3H0xVIR554l1d9Qvm3thU+UBehxXNTSA4C/jVq/Ym4kbvuNUhjJLZJ9BXQtjNjQxGAOpNMmYgg5/CiVthJHUdKaZN45FIkBICvvWlpsPmmNduS5x9aySOTiuo8LQmWZWIzhuKU9EVTV5WOvsrSOC3TamOKdO4HAqWdzGo2/jWLPdg7gxIA6k1gtTteiLL3KjG0bs+nSowJpOVC47VjrLdXErJYupT+JiOB9Ky727u4Z2jWckr1b1NaKm2ZOqkdDcyTwS7JMAnpx1qEXZHUc+ormPtl35qyvMxA6ZrajDtaJOp8xG596HTsSqt2a8F2QeTxWnA4ciuetyJOnXuK19ODlsEHFZNG0TsdPgBCfLngVvRwALxx6cVk6UpManHSt+HBQ+1ZNnZGOhlX0ZK49K5PUgkZbI78V19/nBIH0rjdbDCJzg8c04szqKyuY0tyF4HFVTdjJLPiq8jnbnP41VeQKrOAdoGS3atlG5zSlY1ku4sA+ZU32mNujKfxrlTqyY/1b7exxUqX8chBBOB145FVyE+0R1OQ4wfyqOS1RgfkBrNttQym/eHQnuea07a5SZcjp71OxWjMO9sRvKhSD2rKmtyiHsy+1dvNbrNHyAT64rnb+1aNm3jORxVxkZThY5wH5vmPPqalUkvk1Vd9sjAZ608XJAxk+2a1sYXOj0m+e0RjvdVcYIHA9s1sSDzrJZIAGd/l2DJOf61xEV9LGcJIVJrY0rVriyk3k7gDn6H1qJIaZ6l8ONkEMySSLFJBl3iYYIJ7/wA67pNUtUyPPD4yfl5zXhVtrAnuzJvaOV/+Wi8ZPv613uktdNArTAAkDBz973rGatqaxszsJdfwSIosjsXP9Kpy6xdyniQKp7KKzwCe2T7U8Ie+AKzuaJIleeSX77s31NRgY4HHsKfj2zS4HfigoQHb607OelICo4GSfenFu+AKBCBc0YB6mo2uIUH7yVR+NQNqUCcKrt9KLXFdIuBR2GaUqcYxWY2rMciOMKD681Xe+uXPMpx6DiqUGJyR1wpfxpB0pcUyBaTrRRQIWjFFHtQAnfrVS+t0ubd45OVYEEGrmKr3ztHaSsByF4oA8E1yw+zahLEowu44P41hSIyOe9dfr9zDPIrscckHnqa5abAY4OQa6Y7GTIJQHQEde9VCCD15qVyQeOtRuwZs4qiRUyc9PxrvPCdt5VuHI5PNcKgBxXpOjJ5WnxdvlFY1XZWNqC965YvXOCAa5q/Mk8iqQqqDy2OTXSSZZznoaoXNsW+4vNZRdjqlG6IdIube2jMcox1wQaw9ctR9tkkhDNHIc8c4rei0aSb7kI9ASOlaNv4WuWAbcqAegNbKrFGLoSlscElld3BCrCwA4yRgV0FpY3H2aO2UcqO3NdbbeHFQlpAWPvWvFpqRICEAA9BUSrLoaQw3c5Ww8PNGfMlkDMewFa4tEhUKo5rVKhTgAVWZSz8CsHJs6VBI3NMwYQMdhW1bx4U9Oax9NTaqgmt6LAQ/LmoNlsZOooM49qwru0Ei5aMMo6jHWuiv/m+bFUV5zSvYUlc8z13S5oJi8Mf7gnIUfw/hWJfy7tNMcYII+97ivWb3TRMpI5B6iuI1TQXhkLxDAPbHSumlV0scdWicErbmAIBAHFa+g2nnagehj2/Nn+VSXGmxAjfBsb+8vGams2exjIt4+v3jnk10cyOTldyTVtMjsyJ7YKACCU5xUlhMj7WU/UelV7i4uph8wGM9KdaQFJtwXGeoHQ1lI2idPbkOgzgVjavHhya1LI8AVU1hAELVEdy5rQ89uV2zPznmogM1Pdj9+3uaiiGTjtXUjhe5PAiOdrdauhcAAcVTVgjDAH1FWVl3feP61LQ0Tw7lIIOCf6V6/wCHpxcaTAznMiqA1eS2EQuLoA9MjJr07SrkWtoqImP0rKpsjWmzpN2DwMUm8d6xnv5j0bH4VA08jk7mY/U1jY15jde5ijOGkVfbNVm1OJclVZvc8Vk7ietAIzTsLmZffU5XGECoPzqvJdXEnDysR6ZqHcO1G7PWnYm47607jFRbqN2BycVSAkHApR93rzUQfg8k0m/BouI70Uo60D3xS+1QMKOtL2o6UwEoHXNLRQIMnPtVTUXCWMrnJAGcCnX10tnB5rEAA8knFedeJPGN1vNtZnbI3fGSuf6/QU4xbEzzfVJ2F2xYYBYnB6iqbzxHnbkj261rX1pHCl1LJIXm4XJ/vHGawfLwetdCM2O3A84AzTJQAARSleRSPyKoksWSebcwj1cCvTLePbAigZJArgfD1v52owDHAOeleo2VrgAtXPWetjqw8XqMtdPMmCxwK1otJt8gldx96ktlB4rSTAAB7Vyts74xRHFaonRRxxipBDg1MrdT0pSecDpUmhEUUDpUErfKR2q433c1TuMBSKaEzLmYAk5pbFBJJk9Kq3BZn2J1rRs0Eahc/jVEGxbIPp6Vt2sRZAexrEgYADmteG6Cxbc0i3exFewKrlcfnWKw8qQqOlbE0vmSM7Htis65jDHctJghkZ59c0TWMUycgUyFjkg9e1XYwSPcUilqcrfeF1ky0Zxx0rDufC7IeFx9BXpe0MpDDrUT2qspqlNozlSTPLDo5WQo52n1qVdKaEZzkfSvQ59Kt7hNrpz2IrLm0x7U4BEkR/MVXtCHSscvHF5YzxVbVF32xPFb8+nryV45rM1C2ItHHQgZqoy1MpI8quTmQn3oiwODxS3PEh+p/nUIYluK7zzCfjdjPHrU8UfzfeqoATkj8amjk2sq9c+9Sx3Ne0kWCZNp5+ldnpd1JMGBPAArhI5GR9wxx612WisRDkjkqCaynsaRN/fkZB5oL1AHA5yKQzBT6/SsjVExfFIHzUJk3HOKTLMeDQBYMpHpSGXJ61BsYnmnJH6nFAiQyHtSeYewpCI16tmm+fGDhVZse1OwXHAux4yKXyznJNMM0pX5UVfq2f0FMAuJMDzSvsi0CuemfWl7UdBzR2qRi5pMnNFFMQHrSkHFJmlzSA5zxPcxRJF5xby1BkYDvjoPzIrgdOije3ufEF9LhgxEKg984z+GP0rqviA7rp+5DjDBPz//AFV5nqOptMqWsJwqDb/StYbEso6td/b7tpFj2jPH0Hf6mss8jkVrz2pgts8ZbkkHpWTJkScHOPatE+xDIyeKZwandlkGThT04p7QIIQwySaq4JXN/wAHRbtQDf3RXpcYGBiuB8IIEnkI7gYrvVyFGK5avxHbQVol+AgKKvowKjNZMbn1q9FJnGTWLR0xZdHTil3YbIqJXpxz96oNR7nK5qlPwpzVvr9KgulBiY9hTQmYqpwzn1/SlErA4FRTTCKHeTkEVwuqeLNSiuiLdY44weNy5JreMHIxnLlV2enW142MMfrV2O65wSMdhXn3hjxNJq++C5QJcL0Kj5WH+NdIJ2U5zUyhZ2KjUutDoHuiqFs/rVaK+E8oQHvXJ+JvFC6RZLEg3XUoJQEcAeprndE8X30t4PtKKyE/eQYIo9m2rkuqublPVp4tu1x361PE5KjPasyw1EX0KqepFaKKVIrJo3RdRSQAcD2FIowzLmhWyOvNBOXDEYpDBhxgVA8Yx8wyDVjv1pjNkYNSJmNdWQ3fu/u5rF1OD904xwRiupkx07msTVlPlEHpVxZhNaHheop5dwykY5PH41WUHrir2pgy382B0c4/OqZG0D1r01seVJasmXb39akhi+fhcsTx7VCgPDdhWpppVpFzyc80MEjU0/RjMEeTO32NdLDD5UYVAQKgtplSNdiHp2FWg07D5QqZ9eawk22axsiTy27kD8aNqoDlgB+VRhXkADSMSP7vFWY9NlcZWL8XP+NJQb2QOaW7IfNiHT5j7DNSCY4+VDj34q5FpUn3XcAegqzHpkCnD7m/GtVh5sxliaa6mT5rnjNOWCaQABXat2O3hiGFjX8s1NWscL3ZjLGdkYsem3DD7oX6tVmPSjkGSTj/AGa0RxTq1jh4IxliZvYrJp9uvVSx9SaspGiD5VA+gpQKUetaKKWxm5ye7Ok7UZqAzAdaa9yqjkgfU15B7BZyKaXAPFZj6pbhuGLeyiq8urtkeXHx6saAsbRkA70xrhFGSwA9TXOTX1xIf9YVH+zUB3scksfqaB2NHWTZajZyW0zhgw/h9eorzp/CPlXBmZ8xA57ZrtApHNIQCCOSDTTsOxyOoaY93a/Zoo41ZSG4UD8zXOavobQQJMpDMfvY6Cu2vtLdiHiuXiLN2GaytV0e9ELk3jzccgqB/WtFIhxOAltiOBk+4q3Z2byldxIXPIzWzDo0ptHdgN3X8KeUWBUAGDtGfrTcyoU+praTaG2dHVcKf5V1kQBUVg2EqzWMUijBHyn8K2oHO0c1zyd3c6Y6FgcGrcTZxiqe7jNSq5GMd6ktM0UYZqYNlsZqlG3y89fWpd4LA96lo1TLgHGKzdauDDa7VOC3FXw/HBrC1vLMCOlKO4Sehzc13JGnluCyfyqs2n2l8vzDPfFXJFViciqxKwknOK6VfoYtrqT6dp9rYOgjG0g5z6mtrzVLcHiuf+2w4UE81YSXIypzQ79RadCvq2kw6leiVwWI4qNbK1sgDtC49q3bKS2jbM2Dn1qUWWl3tyQ7vGevFDbsNcty74XIumZ0VtiAYJHWutaMYBFUtMt7aztY47fG0jOfWrwmUkgHIFYS3OhEYOOnWn5zjNRSH+KmCQk1DQyd+OO1QM+OKexG3OaryHjIqSWxjkbtx61jaxMqW7Ox4AJrRmfAHSua8RSM1kyL1bitIRuzGb0PKr0YZpCvJJJOc96oDHNbep2rKjbeQaxwMGvQi7o82cbMfENwOO1X7PAJIzu4qgi8cA1q6XC7uvAHzAfWmydjtdFsjcWcby59h0NdBHYW8YGI8+7c1X06HZAi+WQR1Pqa0lFdMKcbbHDUqyctxFRRwFA+gpwAHalo9a122Mm7h3oAFABzSgYoEFOFJSigAH1p3akApcU0gFFLj0oGaUU7AVpLu4lPMhA/2eKiLbjljk+ppOR1o4xXiHugeeO1IOtGe1JkigY73oyfWm59aQsKAHggnFO3qD1qHfxxxUbN6GgRJKBKhWq+VbKMR5mMYPf3qVOac8SuMbRn3FAGPevDGgxtDH5dv96sW70/fOSGwp6ACtLUdKmkvIJ4SzKp2v8AN2zUskBQ4GeKGa09SHTbf7LaFGOQWyK14D8vtWaisR9Kvw8IpzWZpaxaBqZTVYNT93oaYFwSgKO9OSYE8cVRaUjtUFxc+VbSSE4IFFh81jYe8jjU7nAI7VnXepxbfnKtngDua5S41G5f5EyWPJPpUtlbvjzX3M/qea0UEjN1HIZqVyQX8sYIH61jTySNbbdx3+grpU08yhizdeuRS/2VZJGoLFiOtWrInkkzlY/NbZ8pH07VuWSymEyFTgV0NkdMilVXgTHbiugifRGjPCpxyNvWpcjWNFrqcJKXkXOO1QpPJG4AJyOVOec12tzeaYmVhtkPoSKxxb20k29lx14FJSCVIg0/Vp4JXQSnYwzgnpWnB4gaPjJPsx5FZ6aUhLujZODgVRubeWEYKkn1osmT70TubLW7e6iBLgsTjaeuatebg5HQnpXmtpNIJuUPBHPce9drZXplQBidwHX1rOcLGkKjasbG7sTUcjYFMRt3WmvIMEk/jWSRbZXlcKOeua5zWpkjG+Q4VR+tb02TzkHNcp4iSScrGgOA2WI+lax0MZanOzqk1uxHXBrnbyPymVB0xk10YhkhSTzB8qrnPtXPLFJKXnk3FCe38q6YHNWQWsRckgHAra0oMLgJtOSevpTNFtRcRzscgAcEetdPbacYkVlTGRye9U5GKVy5YXTxzbJZDtIwCx6VsG5EUZdyNoGcisAqB+HXNNwcdTj0pxqSjsTOjGR0Ud7bS8LMpPp0NWV6Z9a5M1JFcTwkFHYfyraOI7o55YXszqaMcViRa1KpxJGr+44NXYtVt3++3ln0ato1YS2Zzyozj0L46UoFMR45AGRww9jTxyeprQyFGaUUduKXFUgFFL3xSCndqAM7p3pM8dKMfnSEe9eKe8Gcim5z0paSgYnOaa1KT78VG0qKDlhQIPxppJzUZuUXoc1GbiRvuRGgRdiHH41P/EKhtwxjUuNrelSnvn0pDK25g+wDPOTmo2SOUlkII6Gn3WRby7fvFSBXnmoaze2t64tJ3iC/KQOjY9Qe9CjzDVTkZ2dzNHbRO7sFVRnJp9lOtzaxyocq4yCK8zvtV1C/UJcTs6enAH6V3PhObzdCgHePKnn3/wDr0Sp8quXGqpOyN4E4FLkimjmgn3qDQd94HNUp13qyMflPrVzPy1TmznGapCM0pBbtjAPPJqY38McRw64qO5tfNGMcetU/7F3j5AfpWiaDlJX1cE4Un/gINQG/YnIR/wAjV620+OJgZFB9sVrQR27AL5afiKehvThc5o6iFwTmnpq20HDkE+tditjYMMvbxnj0qQ6Ro7xMxtBvxxjGD+lGhuqT7nEx6lJK+E3MO+ATVwXF1jIhlbP+ya7S1tdMghAW1G4dTuwP0plx5HVEVR7VLH7FdWcfHqM8DZl8yM/7SkVPNqyTIAzKTW1cRR3KlGQMD6isi58GeZGZYJHJPOwmlzI5qsGnZEdtcQvIOB710tiibBtrlrfRbq2YeYDj1rodOPl/K2c0pakRjY21H5VAx5wOlTkjZ1qqxyeves0OQ1+Fz1rm7kt57kjIJNb97cRWtpLNIwVEUsSelY8TrOoOQQRTaFGxk31ss1mxUH0bBxXOaykFsIoY5C8jfwhs7RXXaxAItDugOZJE2xoOpJNYGheE7iS4W5viYwpz5ZGTW1N2jds56/xWNLw5pTR6cC4+aU7z9K6EIQMBcYqaOJIkCKvAGM049KTlczSKj26P95eT3qpJaMB8nNapQYz3ppQYNO4WMRkZTggg9803mtmSFZF+ZQapS2RAyp/CqTIaKPA7UdaeY2U8jBpvQ80yR8byR8o7KfY1bi1a5i4YrIB/eFUvoaQ8VSnKOzIdOL3Ruw63E+BMhT3HIrQiu7eb/VzIx9M1yPrTs4IPeto4iS3MZYaL2OzGMdaWuastQaEbHlYejZq6dcWJtpAlB/iBxW8a8XuYSoSRKWAGSelRtPGF6g1WYxpkzXCgemaeio6gxwyOD0JXivLPXD7UDwqkn2poe4k+6mPrUEt3OrskcSKBxuzmqjPdv9+4YD0XigC9IrKMzXCxj3OKrG5sl6ytKR2UE1B9iDHLAu3q3JqdbTbwABTAjOpL/wAs9Pc+7sF/xpklzfzcK8cC/wCyu4/matrbBeWNNZV3AL3oEaturC1i3sWYKMk96lz8tInCD6Ype+KkYKnmMFxn2ritU0yCXVLwyxojCbGMEZH9a76yTdeRLyQzAHFUPEelPFqs80UUcsUxy8bHBLY6g9jVwdmTJXPOLrQ7doZnimjiMQyQW+99B1rS8HzbEubUEkIwcHsc/wD6qj1TTJoUMv2CeJSeTL0/PvUfhuN7a+MrSKVlBXYv51cleLIhpI7EHmlOCajyfWk31znWSM2BiqjHMlTs2UqBhz70wLMaIw5AqQBV4AAqrFId2KkOT3ploSRQxOKiIkhOVyRTy2MU0y00y1JomivQMbgQe4q2NRQA9emKyWkGOnNVjMxPWq3NVXaN7+0lxtBOKia6ZycZArJhkJJ9quRZYjFTIXtpM07T52yRn61uxkeWBWJZoy/lWlG5QdeaxYJvcdcBdp+UcVmkAPuAA9qtyzZBB61TblvvfnTREmW0uCUxg/nQG7mqqnaSCafu96Zm2YnjK5Mfh+VEYBpXVMdcjPNcxpmo3dlbBUYyqB/GOn41f8WXDvfwRfMscYzuI+Xcff1qbTrNrm1I8oYPQluvFbRS5dTmlJ8+hRXULi9vIzcvnByoxgCu3jjYc8HI4HpXn8kX2PV0gdgXBxx05r0S2w9rG4IJKjvRNIlO+4FcHkZpCBt5FSFTTD0wagoYQM8dKaRUn400gmmIjOaQ8cU/n0prZ7imBE8cb5yoNVXsAxzG2PY1cPWmkkU7ktGW9vJGcMtMKnHStY5xzz9aryW4zlePaquQ0Z+OKOfrUzxFTyOKZgCmIaORyKSnflSYxTEWEslB4TH1q4HnEfl+cdvTAqQj0xQRx0rA6LEKwqAAeadtQcbRUgXjpSbD14pgMPJ4oKk08jBwOaUKewoAYF4qFFzdIMcZq1sJGKZbqDdcdqQF70HrS96TpTu1AGjo0Ye9jyM/NyK0LyNPthDLtUg84/SqmhA/bFYHoCf0rf8ALDsc0CZwnia0LaeY7dmZW/1gA4U151JDcWVz5sQdZAc7GQjI/wAK92vdLtbiEs1tHvUg7gOa891/S5Wv3hiEah/4znj/AD6VrB9DOS6lS2uPtNsk4XG4Zx6HvUgbisvRRtgnjD71SQgGtHgGspLU6Yu6Jc8VG2SetKSRQozSGKilTnvUqjdnmnxxhhz1qZUCjJx/jTKTKEquOg4qInHvW7FDHOuCRz1pP7GibJEhFS2aJNnOOXc9MVFnGciuyi0C22hmZm/GrK6DYEg+WM+5qlND5GcPAHkYBQx59K27W3ZV+ZSPqK6uLTLeIAKgAHpUrWkOOV/WolMpRS3MOFdqjAqQybT0q7LFHGuAODWe8ZB3biRj7tRuNtEJchyGqPaM+op756jmofMx8uKtGTY5sA56ZpGlEanPQdaY7981m6hcEtHADhpDnOegHNUkRKVkc3LezzyS+dH8zOxww6AngV1nhuKP7GiEiTYuCB2zWcbcFVMi73JBz6it/TIRExwADgdBWknocy3uYet6lp00xsl0pYrqAhTcjHzHPXpmtvSG8ywT/ZJFc54lgEOtOVGN6hq2tCbdbSr6MD+lVO3LoKPxGsymmkZp/OOtMJPpWJoMK5zTSuKl6dsUhweM0xEHzCmlsf8A16mIprKfbmgCLIPamMBnipdg9KYUPQEUwIx70celPximnrimiRjKCMVWktgc7eKtFQaaVxVEtGc0ZXqPypprQZcg5FQPArHg4NO5NjQAyeKcFyKdx9aOlZG43A9TQRS0nWgAyAelG7HQUh60dKAGsWz2ptmhMzMT1FLIcA9KfZA4cn1oAtEYxmj+E0HNAPy0Abvh9Pmkf0WtuLg1l6CuLaZ/oK1ohQQx8ilo2APOK5PxDblbW5miwZfIcnPsK64/dNZl5ErbkPKMpDD69apbiPH/AA+NlnLk5zIea0nNPewGmTzQL03ZznIPA/pimMal7m0VaKE3YpyvzyaiPWjcKdhl+KVc43c0j3CN3/CsW5vlhypPv0quNSaNMsRk/wAqpQZPOkdZbSJyNw9a0FuoywUnrXF2uocOxbOOPTFXBq2xQerYHWpcGaxqI65b5E+Xd2pV1JfMG07vfFcfBqJIcMwyfaoW1aSGXZu+U+1L2bK9qeiQXm8EZ+tD3SxAhiAvY+tcNHrDqxKSZ3ryKfLrLOoRzhwOTUumx+1R08moRsSAeOnNQibeOoIPSuOk1VoJi275DwvFJFrb7tucjrimqbMnVXU6+Vlxlfxqg33i2aqWWoxyptJ2seee+amllCkmlawc1wkfnGeM0y70efULqBY2CbELFvxpsGZLhc+ua6ayX97n0HWi9geqKUOipbWrM7s8oXAOMCktQEfGK2JQFiI/CseHHnAH6VS1MrWMrxVbK15bynumMfSm6G+JmjJxlav+I4t9tDIf4SRWTpjbL6I++M/hVXuhWszpsZHFMIIp/PFJk1BZGWI+tIee3NSZGelIQCM4oEQtSc1IyAYwaYQ1MBpHvTGH5049aD1piI8UmM8nBpxFHB5ppiIiuB6Uh96kx60mOxFNMTRGeB0phUHmpTgHHNMwaZJaGBR2GKUEDgCjJI6VkaiEYFJgUpBpuMd6AAjvSHGDTsZpMCmBFK429antP+PcepNV5ueOKs2y7YVFAExOTR2pMU8AZAoA6fRwV05/9phj8q0oRxVHTk2aag/vnIq9GOKCGS0kkKSrg04c04dMGqRJ5r4oiEGrtGrMRgY3HmsNupre8XMD4gl9QorAJzUPc6Y/CiJj1pqnP1pz1AQw6U0waMrWjsuFwTtxmsx5CRnPHpW9dwrdRlH69jWMYvJwGHI61vFo5pxaZXF0yABTxnJzUsN4zuSx/Kqc6rtyp69RUKkxrnPTvWlkZczRtLegMV3c5796ZcXe6QAHpzWP52ScdaaJizZzRZD5zchuHY4z1qb7awYBzkrWPBebDjrjv6U+S5zyPzqWilIvT3WUwTxn5faqRnOwc5OeartJu6/hSgkKQc+tFhN3NKzvZdygMcg9D3FdVHP5iqcgjriuSsI9zoccA9PWuutId5VR0FZVGkbUk2aunQhn3c5NdBaHDYx1qhZ2/kwbjjPYVftvu++a5k7s6ZK0S1yR0rGcbLpgezGttTgHNY96MXTeh5FaowY3Vxv0iU4yVwa5qzk2XMTdMMM/nXWXCiXTZ1B5MZxXGofmB96cRHaDBAOOoppXuDTbVzJawtnOVFSNn0qSyM5I5xTaecelJx60CIyFzmkORUhAzUbCgQ3Hrio2A5xUhHHHWo8GmAmCPpTafz1pKYiPvQTmnnAqPaMdaYmIBSZ9RTu2KaQRRcksgUuKN3OO1IScVBqL+FNPBwRRz60EZPWgBMCjApOM5Gc0FqYivM3OMVfj+4PYVnOd0gFaSdh7UAHXntT0UswwO9M747VLB8zgH1FAjsI02WNsP9jNTp0pkvCRIDnagH6VLH2pkskHSl9jR2pc8U0SeW+KG8zxBd56qwH6CsfqOtaWvvv12+br+9aszODUPc6o7DTTWXIpx560UhlZ4qqSW4zu/QitIj0qNkHcVSbQnFM5q7sNsnyH5D69qpGBgm5shT37V1ctsrptPSq6WKxqACeK1VUwdC70OVktW52EH6VCUdT0zXXvZpJ95Bx7VBLpsbLhQq56/LVqoiHRZzSI23CqTmpRG5XgH6YrcXS8dGP1xVyCySPkoC3qRSdSIKhI5+KyeTAC5J9KvQ6TKRubHPbrW4IgMcAfQVKqqB71Eqr6GqoJblO109YyD6dK6rS7LChiOTyazrOEyODtJAPpXVWcIii5GG6muepJnTTglsBXC57VPbDKZHrio5OWPpUtpxCw461ENx1di2gyOlZmoj96mP7taScsfpmqGor9wjpyM1sjmYsChrcqf4gQa4hxslYHjDYrtbIkj6GuR1JBDfzKcDLnj8aqO5Leh0mkyF9OiyB8oK/kaucEdD7VlaHI/wBjdc/dfpWmHJGDipe5S2DAJ60141btg0ufakJVhjgUhkbKw6cmmHIHNTDijvTAgJPoKjPXrVg+4FMIAJO0UCISOOKaQalIGelG2mIgZWH/ANamkfLUxye1NOMdKYiHHpR9aft9qQqR34piJcEnoeKXFOz70NjioLG45pCOaXHNB9KAEppwFpxOeg4pjn5T/KgRXT57kema0Rggms+0UfaMnpzV8DA4pgGSPpVmyUvOg9TUAHyirunbEuEaRgqqwJJ6daBXOrnwJAPQYqVKw73xBbJIxhBlIPGOAap/8JROD/x7xY9MmqUWZto60GlJCjcTx3rjJPE98y/L5afRc/zNZ1xrV/OpDXUoBHIU4FWoMXMkc7qM5l1C5c8b5WP6mqueRzmtSSGJ+qKTVSSzXqjFfapdNmka0dmQZBpaY6SQ/eXI9RSBwRwazaa3OhNPYfTSD3p2aQnNAxpGajYDjBqWmGgCIjPejFPKgdKTZnvTCwKpPelEZ9aVFPHXin49qQAqcdakijaR8BaRRzwK2dNs9xHHzH9KluxSVy9p1mAAxGQOma0SOec1PFAsUYUUjAevFYt3NrWKzDipLb/VuB60gSSaTy4kLsfStSx0Z0QmZsM38I7VpShKT0Rz16kYqzZWU4wwGaraiuYFPoa15tNeJMxfMB2PWsu8RjaPn7yjn65rZxcXqc6kpLQp2ZwzDPpXKeLU8nVQwJ+dAa6i04l+o5rJ8YWXmQC+LBRBHyuOozRF6il8IeGXDh1JxuQMK32QYxmuL8K3nnyx7flXlBzXac4wTmlPRjg7xE8sdM/jUflj1qQscYpmeTx+lSWMMXzHFIUI71IzKPWmk5zTAjO5ehzTdxxyKkBFIQuaQERJpCx44qRgtNwMimIiL5PSmnGamZRjjrTNh70xDMD1pMA55pSmM9ab5ZyTk1Qj/9k=\r\n";
            byte[] imageBytes = Convert.FromBase64String(base64String);

            using (MemoryStream ms = new MemoryStream(imageBytes))
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.StreamSource = ms;
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.EndInit();

                // Wijs de BitmapImage toe aan de Image control
                imgPersoonlijk.Source = image;
            }
        }
    }
}

