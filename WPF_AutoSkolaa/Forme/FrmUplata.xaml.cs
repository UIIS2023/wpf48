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
    /// Interaction logic for FrmUplata.xaml
    /// </summary>
    public partial class FrmUplata : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;
        public FrmUplata()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtUplata.Focus();
        }

        public FrmUplata(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            txtUplata.Focus();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;
            konekcija = kon.KreirajKonekciju();
        }

        private void btnSacuvaj_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                konekcija.Open();
                DateTime date = (DateTime)dpDatum.SelectedDate;
                string datum = date.ToString("dd-MM-yyyy");
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                cmd.Parameters.Add("@uplata", SqlDbType.NVarChar).Value = txtUplata.Text;
                cmd.Parameters.Add("@datum", SqlDbType.DateTime).Value = dpDatum;
                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update tbl_Uplata set iznos = @uplata, datumUplate = @datum where uplataID = @id";
                    this.azuriraj = false;
                }
                else
                {
                    cmd.CommandText = "insert into tbl_Uplata(iznos, datumUplate) values(@uplata, @datum)";
                }
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                this.Close();

            }
            catch (SqlException)
            {
                MessageBox.Show("Unos odredjenih vrednosti nije validan", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
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
