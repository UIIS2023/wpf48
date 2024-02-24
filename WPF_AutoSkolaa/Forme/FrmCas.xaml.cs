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
    /// Interaction logic for FrmCas.xaml
    /// </summary>
    public partial class FrmCas : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;
        public FrmCas()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
            txtVreme.Focus();
        }
        public FrmCas(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtVreme.Focus();
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
                string vratiObuku = @"select obukaID, nazivKategorije from tbl_Obuka";
                DataTable dtObuka = new DataTable();
                SqlDataAdapter daObuka = new SqlDataAdapter(vratiObuku, konekcija);


                daObuka.Fill(dtObuka);

                cbObuka.ItemsSource = dtObuka.DefaultView;
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
                cmd.Parameters.Add("@vreme", SqlDbType.NVarChar).Value = txtVreme.Text;
                cmd.Parameters.Add("@trajanje", SqlDbType.NVarChar).Value = txtTrajanje.Text;
                cmd.Parameters.Add("@datumObuke", SqlDbType.DateTime).Value = datum;
                cmd.Parameters.Add("@obukaID", SqlDbType.Int).Value = int.Parse(((DataRowView)cbObuka.SelectedItem).Row["obukaID"].ToString());

                if (this.azuriraj)
                {

                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update tbl_Cas set Vreme = @vreme, Trajanje = @trajanje, datum = @datumObuke, obukaID=@obukaID where casID = @id";
                    this.azuriraj = false;
                }
                else
                {
                    cmd.CommandText = "insert into tbl_Cas(vreme,trajanje,datum,obukaID) values(@vreme, @trajanje, @datumObuke, @obukaID)";
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
