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
    /// Interaction logic for FrmObuka.xaml
    /// </summary>
    public partial class FrmObuka : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;
        public FrmObuka()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
            txtKategorija.Focus();
        }
        public FrmObuka(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtKategorija.Focus();
            PopuniPadajuceListe();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;
            konekcija = kon.KreirajKonekciju();
        }
        private void PopuniPadajuceListe()
        {
            try
            {
                konekcija.Open();
                string vratiKandidata = @"select kandidatID, imeKandidata, prezimeKandidata from tbl_Kandidat";
                DataTable dtObuka = new DataTable();
                SqlDataAdapter daObuka = new SqlDataAdapter(vratiKandidata, konekcija);


                daObuka.Fill(dtObuka);

                cbKandidat.ItemsSource = dtObuka.DefaultView;
                dtObuka.Dispose();
                dtObuka.Dispose();

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
                DateTime date = (DateTime)dpDatum.SelectedDate;
                string datum = date.ToString("yyyy-MM-dd");

                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                cmd.Parameters.Add("@nazivKategorije", SqlDbType.NVarChar).Value = txtKategorija.Text;
                cmd.Parameters.Add("@datumPocetka", SqlDbType.DateTime).Value = datum;
                cmd.Parameters.Add("@datumZavrsetka", SqlDbType.DateTime).Value = datum;
                cmd.Parameters.Add("@kandidatID", SqlDbType.Int).Value = int.Parse(((DataRowView)cbKandidat.SelectedItem).Row["kandidatID"].ToString());

                if (this.azuriraj)
                {

                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update tbl_Obuka set datumPocetka = @datumPocetka, nazivKategorije = @nazivKategorije, datumZavrsetka = @datumZavrsetka, kandidatID=@kandidatID where obukaID = @id";
                    this.azuriraj = false;
                }
                else
                {
                    cmd.CommandText = "insert into tbl_Obuka(DatumPocetka,nazivKategorije,datumZavrsetka,kandidatID) values(@datumPocetka, @nazivKategorije, @datumZavrsetka, @kandidatID)";
                }

                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();
            }
            catch (SqlException sqlEx)
            {
                MessageBox.Show("Unos odredjenih vrednosti nije validan SQL greska: \n" + sqlEx.Message, "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Odaberite datum", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (FormatException)
            {
                MessageBox.Show("Greska prilikom konverzije podataka!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
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
