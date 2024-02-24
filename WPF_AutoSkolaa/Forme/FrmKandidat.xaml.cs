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
    /// Interaction logic for FrmKandidat.xaml
    /// </summary>
    public partial class FrmKandidat : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;
        public FrmKandidat()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtIme.Focus();
        }
        public FrmKandidat(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtIme.Focus();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;
        }

  
        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                DateTime date = (DateTime)dpDatum.SelectedDate;
                string datum = date.ToString("yyyy-MM-dd");
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                cmd.Parameters.Add("@ime", SqlDbType.NVarChar).Value = txtIme.Text;
                cmd.Parameters.Add("@prezime", SqlDbType.NVarChar).Value = txtPrezime.Text;
                cmd.Parameters.Add("@jmbg", SqlDbType.NVarChar).Value = txtJMBG.Text;
                cmd.Parameters.Add("@adresa", SqlDbType.NVarChar).Value = txtAdresa.Text;
                cmd.Parameters.Add("@grad", SqlDbType.NVarChar).Value = txtGrad.Text;
                cmd.Parameters.Add("@kontakt", SqlDbType.NVarChar).Value = txtKontakt.Text;
                cmd.Parameters.Add("@datumRodjenja", SqlDbType.DateTime).Value = datum;

                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update tbl_Kandidat set ime = @ime, prezime = @prezime, jmbg = @jmbg, adresa = @adresa, grad = @grad, kontakt = @kontakt, datumRodjenja = @datumRodjenja where kandidatID = @id";
                    this.azuriraj = false;
                }
                else
                {
                    cmd.CommandText = "insert into tbl_Kandidat(ime, prezime, jmbg, adresa, grad, kontakt, datumRodjenja) values(@ime, @prezime, @jmbg, @adresa, @grad, @kontakt, @datumRodjenja)";

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
