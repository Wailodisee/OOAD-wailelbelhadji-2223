using System;
using System.Collections.Generic;
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
using MyClassLibrary;

namespace WpfGebruiker
{
    /// <summary>
    /// Interaction logic for PageOntleningen.xaml
    /// </summary>
    public partial class PageOntleningen : Page
    {
       
        private Gebruiker mijnGebruiker;
        public PageOntleningen(Gebruiker mijnGebruiker)
        {
            InitializeComponent();

            this.mijnGebruiker = mijnGebruiker;

            GetmyAanvragen();

            GetmyOntleningen();  
        }

        private List<Ontlening> lijstVanOntleningen = new List<Ontlening>();

        private List<Gebruiker> lijstVanGebruikers = new List<Gebruiker>();

        // Haalt de lijst van Aanvragen + vult de ListBox met elke aanvraag
        private void GetmyAanvragen()
        {
            elkAanvrag = CatchUserOntlening(mijnGebruiker.Id);

            lbxAanvragen.Items.Clear();
            for (int i = 0; i < elkAanvrag.Count; i++)
            {
                Ontlening mijnAanvraag = elkAanvrag[i];
                string naamVoertuig = GetVoertuigNaam(mijnAanvraag.VoertuigId);
                string berichtAanvrag = FormatAanvraagText(naamVoertuig, mijnAanvraag);
                lbxAanvragen.Items.Add(berichtAanvrag);
            }
        }

        // Haalt alle  ontleningen + sorteert ze in het orde + vult de ListBox met voertuignaam en de periode van de ontlening
        private void GetmyOntleningen()
        {
            lijstVanOntleningen = Ontlening.GetAll(mijnGebruiker.Id).OrderByDescending(o => o.Vanaf).ToList();

            lbxOntleningen.Items.Clear();
            for (int i = 0; i < lijstVanOntleningen.Count; i++)
            {
                Ontlening mijnOntlening = lijstVanOntleningen[i];
                string naamVoertuig = Voertuig.CatchIdOfVoertuig(mijnOntlening.VoertuigId)?.Naam ?? "ONBEKEND ";
                string berichtOntlening = $"Status: {mijnOntlening.Status.ToString()} // Voertuig -> {naamVoertuig}, {mijnOntlening.Vanaf.ToString("dd-MM-yyyy")} 00:00 tot {mijnOntlening.Tot.ToString("dd-MM-yyyy")} 00:00";
                lbxOntleningen.Items.Add(berichtOntlening);
            }
        }

        private List<Ontlening> elkAanvrag = new List<Ontlening>();

        // Filter de gebruiker ID + retourneert ze gesorteerd in aflopende volgorde op basis van de datum
        private List<Ontlening> CatchUserOntlening(int gebruikerId)
        {
            return Ontlening.CatchOntleningenByVoertuigId(gebruikerId).OrderByDescending(a => a.Vanaf).ToList();
        }

        // Haalt de naam voertuig of zet ONBEKEND
        private string GetVoertuigNaam(int voertuigId)
        {
            return Voertuig.CatchIdOfVoertuig(voertuigId)?.Naam ?? "ONBEKEND";
        }

        // Returneert de aanvragen in de listbox
        private string FormatAanvraagText(string naamVoertuig, Ontlening mijnAanvraag)
        {
            Gebruiker aanvrager = Gebruiker.GetById(mijnAanvraag.AanvragerId);
            string aanvragerNaam = aanvrager != null ? $"{aanvrager.Voornaam} {aanvrager.Achternaam}" : "ONBEKEND";
            return $" Status : {mijnAanvraag.Status} // Voertuig -> {naamVoertuig}, {mijnAanvraag.Vanaf:dd/MM/yyyy} 00:00, tot {mijnAanvraag.Tot:dd/MM/yyyy} 00:00 door -> {aanvragerNaam}";
        }

        // Annuleert een ontlening
        private void btnAnnuleren_Click(object sender, RoutedEventArgs e)
        {
            Ontlening geselecteerdONT = CatchSelectedOntlening();
            if (geselecteerdONT != null)
            {
                DeleteOntlening(geselecteerdONT);
                Ontlening.Delete(geselecteerdONT.Id); // verwijder uit de database
                GetmyOntleningen();
            }
        }

        // Haalt de geselecteerde ontlening van de lijst
        private Ontlening CatchSelectedOntlening()
        {
            int ixSelect = lbxOntleningen.SelectedIndex;
            return (ixSelect >= 0 && ixSelect < lijstVanOntleningen.Count) ? lijstVanOntleningen[ixSelect] : null;
        }

        // Verwijder een ontlening
        private void DeleteOntlening(Ontlening mijnOntlening)
        {
            Ontlening.Delete(mijnOntlening.Id);
        }

        // Accepteer een ontlening
        private void btnAcceptAanvraag_Click(object sender, RoutedEventArgs e)
        {
            if (lbxAanvragen.SelectedItem != null)
            {
                int ixSelect = lbxAanvragen.SelectedIndex;
                if (ixSelect >= 0 && ixSelect < elkAanvrag.Count)
                {
                    Ontlening geselecteerdONT = elkAanvrag[ixSelect];
                    if (geselecteerdONT != null)
                    {
                        ApproveOntlening(geselecteerdONT);
                        GetmyAanvragen();
                    }
                }
            }
        }
        private void ApproveOntlening(Ontlening mijnOntlening)
        {
            mijnOntlening.Status = OntleningStatus.Goedgekeurd;
            Ontlening.Update(mijnOntlening);
        }

        // Wijst af een ontlening 
        private void btnAfwijzen_Click_1(object sender, RoutedEventArgs e)
        {
            if (lbxAanvragen.SelectedItem != null)
            {
                int ixSelect = lbxAanvragen.SelectedIndex;
                if (ixSelect >= 0 && ixSelect < elkAanvrag.Count)
                {
                    Ontlening geselecteerdONT = elkAanvrag[ixSelect];
                    if (geselecteerdONT != null)
                    {
                        RejectOntlening(geselecteerdONT);
                        Ontlening.Delete(geselecteerdONT.Id); // verwijder uit de database
                        GetmyAanvragen();
                    }
                }
            }
        }
        private void RejectOntlening(Ontlening mijnOntlening)
        {
            mijnOntlening.Status = OntleningStatus.Verworpen;
            Ontlening.Update(mijnOntlening);
        }

        // Roept elke methode om de textboxen in te vullen + disabled de buttons als oude datum
        private void lbxAanvragen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Ontlening geselecteerdONT = CatchSelectedAanvragen();

            if (geselecteerdONT != null)
            {
                btnAcceptAanvraag.IsEnabled = geselecteerdONT.Vanaf > DateTime.Now;
                btnAfwijzen.IsEnabled = geselecteerdONT.Vanaf > DateTime.Now;

                SetVoertuigContent(geselecteerdONT);
                SetPeriodeAanvraagContent(geselecteerdONT);
                SetAanvragerContent(geselecteerdONT);
                SetBerichtAanvraagContent(geselecteerdONT);
            }
            else
            {
                btnAcceptAanvraag.IsEnabled = false;
                btnAfwijzen.IsEnabled = false;
            }
        }

        // Annuleer wordt gedisabled 
        private void lbxOntleningen_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Ontlening geselecteerdONT = CatchSelectedOntlening();

            if (geselecteerdONT != null)
            {
                btnAnnuleren.IsEnabled = geselecteerdONT.Vanaf > DateTime.Now;
            }
            else
            {
                btnAnnuleren.IsEnabled = false;
            }
        }

        // Selecteer de correcte aanvraag
        private Ontlening CatchSelectedAanvragen()
        {
            if (lbxAanvragen.SelectedItem != null && !string.IsNullOrEmpty(lbxAanvragen.SelectedItem.ToString()))
            {
                int selectedIndex = lbxAanvragen.SelectedIndex;
                return elkAanvrag[selectedIndex];
            }

            return null;
        }

        // Vult de voertuig-naam in of zet ONBEKEND
        private void SetVoertuigContent(Ontlening selectedOntlening)
        {
            Voertuig voertuig = Voertuig.CatchIdOfVoertuig(selectedOntlening.VoertuigId);
            if (voertuig != null)
            {
                lblVoertuig.Content = voertuig.Naam;
            }
            else
            {
                lblVoertuig.Content = "ONBEKEND";
            }
        }

        // Vult de periode in of zet ONBEKEND
        private void SetPeriodeAanvraagContent(Ontlening selectedOntlening)
        {
            if (selectedOntlening.Vanaf != null && selectedOntlening.Tot != null)
            {
                string periodeText = $"{selectedOntlening.Vanaf.ToString("dd-MM-yyyy")} tot {selectedOntlening.Tot.ToString("dd-MM-yyyy")}";
                lblPeriodeAanvraag.Content = periodeText;
            }
            else
            {
                lblPeriodeAanvraag.Content = "GONBEKEND";
            }
        }

        // Vult de voornaam en achternaam in of zet ONBEKEND
        private void SetAanvragerContent(Ontlening selectedOntlening)
        {
            Gebruiker aanvrager = Gebruiker.GetById(selectedOntlening.AanvragerId);
            if (aanvrager != null)
            {
                string aanvragerNaam = $"{aanvrager.Voornaam} {aanvrager.Achternaam}";
                lblAanvrager.Content = aanvragerNaam;
            }
            else
            {
                lblAanvrager.Content = "ONBEKEND";
            }
        }

        // Vult de bericht in of zet Geen bericht
        private void SetBerichtAanvraagContent(Ontlening selectedOntlening)
        {
            if (!string.IsNullOrEmpty(selectedOntlening.Bericht))
            {
                lblBerichtAanvraag.Content = selectedOntlening.Bericht;
            }
            else
            {
                lblBerichtAanvraag.Content = "Geen bericht";
            }
        }
    }
}
