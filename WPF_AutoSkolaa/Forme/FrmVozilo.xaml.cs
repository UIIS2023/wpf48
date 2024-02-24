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
    /// Interaction logic for FrmVozilo.xaml
    /// </summary>
    public partial class FrmVozilo : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;

        public FrmVozilo()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            txtModel.Focus();
        }
        public FrmVozilo(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            txtModel.Focus();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;
            konekcija = kon.KreirajKonekciju();
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
                cmd.Parameters.Add("@marka", SqlDbType.NVarChar).Value = txtMarka.Text;
                cmd.Parameters.Add("@model", SqlDbType.NVarChar).Value = txtModel.Text;
                cmd.Parameters.Add("@reg", SqlDbType.NVarChar).Value = txtRegistacija.Text;
                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update tbl_Vozilo set marka = @marka, model = @model, registracioneOznake = @reg where voziloID = @id";
                    this.azuriraj = false;
                }
                else
                {
                    cmd.CommandText = "insert into tbl_Vozilo(marka, model, registracioneOznake) values(@marka, @model, @reg)";
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
