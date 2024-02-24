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
    /// Interaction logic for FrmPrisustvo.xaml
    /// </summary>
    public partial class FrmPrisustvo : Window
    {
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();
        bool azuriraj;
        DataRowView pomocniRed;
        public FrmPrisustvo()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
            txtPrisutan.Focus();
        }
        public FrmPrisustvo(bool azuriraj, DataRowView pomocniRed)
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            PopuniPadajuceListe();
            txtPrisutan.Focus();
            this.azuriraj = azuriraj;
            this.pomocniRed = pomocniRed;

        }
        private void PopuniPadajuceListe()
        {
            try
            {
                konekcija.Open();
                string vratiCas = @"select casID, datum, vreme from tbl_Cas";
                DataTable dtCas = new DataTable();
                SqlDataAdapter daCas = new SqlDataAdapter(vratiCas, konekcija);


                daCas.Fill(dtCas);

                cbCas.ItemsSource = dtCas.DefaultView;
                dtCas.Dispose();

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
                cmd.Parameters.Add(@"prisutan", SqlDbType.NVarChar).Value = txtPrisutan.Text;
                cmd.Parameters.Add(@"razlog", SqlDbType.Int).Value = txtRazlog.Text;
                cmd.Parameters.Add("@casID", SqlDbType.Int).Value = int.Parse(((DataRowView)cbCas.SelectedItem).Row["casID"].ToString());
                if (this.azuriraj)
                {
                    DataRowView red = this.pomocniRed;
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = @"Update tbl_Prisustvo set prisutan = @prisutan, razlogOdsustva = @razlog, casID = @casID where prisustvoID = @id";
                    this.azuriraj = false;
                }
                else
                {
                    cmd.CommandText = "insert into tbl_Prisustvo(prisutan, razlogOdsustva, casID) values(@prisutan, @razlog, @casID)";

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

