using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
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

namespace WPF_AutoSkolaa.Forme
{
    /// <summary>
    /// Interaction logic for FrmInstruktor.xaml
    /// </summary>
    public partial class FrmInstruktor : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;
        public FrmInstruktor()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
            txtIme.Focus();
        }
        public FrmInstruktor(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
            txtIme.Focus();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;

        }
        private void PopuniPadajuceListe()
        {
            try
            {
                konekcija.Open();
                string vratiKandidata = @"select kandidatID, imeKandidata, prezimeKandidata from tbl_Kandidat";
                DataTable dtKandidat = new DataTable();
                SqlDataAdapter daKandidat = new SqlDataAdapter(vratiKandidata, konekcija);


                daKandidat.Fill(dtKandidat);

                cbKandidat.ItemsSource = dtKandidat.DefaultView;
                dtKandidat.Dispose();

            }
            catch (SqlException)
            {
                MessageBox.Show("Padajuce liste nisu popunjene", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
            }
        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                cmd.Parameters.Add(@"Ime", SqlDbType.NVarChar).Value = txtIme.Text;
                cmd.Parameters.Add(@"Prezime", SqlDbType.NVarChar).Value = txtPrezime.Text;
                cmd.Parameters.Add(@"JMBG", SqlDbType.NVarChar).Value = txtJMBG.Text;
                cmd.Parameters.Add(@"Adresa", SqlDbType.NVarChar).Value = txtAdresa.Text;
                cmd.Parameters.Add(@"Grad", SqlDbType.NVarChar).Value = txtGrad.Text;
                cmd.Parameters.Add(@"Kontakt", SqlDbType.NVarChar).Value = txtKontakt.Text;
                cmd.Parameters.Add("@kandidatID", SqlDbType.Int).Value = int.Parse(((DataRowView)cbKandidat.SelectedItem).Row["KandidatID"].ToString());
                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update tbl_Instruktor set ime = @Ime, Prezime = @Prezime, jmbg = @JMBG,adresa = @Adresa,grad = @Grad,kontakt = @kontakt, kandidatID = @kandidatID where instruktorID = @id";
                    this.azuriraj = false;
                }
                else
                {
                    cmd.CommandText = "insert into tbl_Instruktor(imeInstruktora, prezimeInstruktora, jmbgInstruktora, adresaInstruktora, gradInstruktora, kontaktInstruktora, kandidatID) values(@Ime, @Prezime, @JMBG, @Adresa, @Grad, @Kontakt, @kandidatID)";

                }
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();
            }
            catch (SqlException)
            {
                MessageBox.Show("Unos odredjenih vrednosti nije validan!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
            }
        }

        private void btnOtkazi_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}

