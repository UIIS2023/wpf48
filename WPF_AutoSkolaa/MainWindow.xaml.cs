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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_AutoSkolaa.Forme;

namespace WPF_AutoSkolaa
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string ucitanaTabela;
        bool azuriraj;
        Konekcija kon = new Konekcija();
        SqlConnection konekcija = new SqlConnection();

        #region Select upiti
        string selectUslovcVozilo= @"select * from tbl_Vozilo where voziloID=";
        string selectUslovInstruktor = @"select * from tbl_Instruktor where instruktorID=";
        string selectUslovUplata = @"select * from tbl_Uplata where uplataID=";
        string selectUslovKandidat = @"select * from tbl_Kandidat where kandidatID=";
        string selectUslovObuka = @"select * from tbl_Obuka where obukaID=";
        string selectUslovPrisustvo = @"select * from tbl_Prisustvo where prisustvoID=";
        string selectUslovCas = @"select * from tbl_Cas where casID=";
        string selectUslovIspit = @"select * from tbl_Ispit where ispitID=";
        #endregion

        #region Select sa uslovom
        static string casSelect = @"SELECT c.casID as ID, c.datum, c.vreme, c.trajanje, c.obukaID
                                FROM tbl_cas c
                                JOIN tbl_Obuka o ON c.obukaID = o.obukaID;";

        static string instruktorSelect = @"SELECT instruktorID as ID, imeInstruktora, prezimeInstruktora, jmbgInstruktora, adresaInstruktora, gradInstruktora, kontaktInstruktora as Kontakt 
        from tbl_Instruktor join tbl_Kandidat on tbl_Kandidat.kandidatID = tbl_Instruktor.instruktorID;";

        static string ispitSelect = @"SELECT ispitID as ID, datumIspita, vremeIspita, polozen from tbl_Ispit
        join tbl_Obuka on tbl_Obuka.obukaID = tbl_Ispit.ispitID;";

        static string kandidatSelect = @"SELECT kandidatID as ID, imeKandidata, prezimeKandidata, jmbgKandidata, adresaKandidata, adresaKandidata, gradKandidata, kontaktKandidata, 
        datumRodjenja from tbl_Kandidat;";
        
        static string obukaSelect = @"SELECT obukaID as ID, datumPocetka, datumZavrsetka, nazivKategorije from tbl_Obuka 
        join tbl_Kandidat on tbl_Kandidat.kandidatID = tbl_Obuka.obukaID;";

        static string prisustvoSelect = @"SELECT prisustvoID as ID, prisutan, razlogOdsustva from tbl_Prisustvo 
        join tbl_Cas on tbl_Cas.casID = tbl_Prisustvo.prisustvoID;";

        static string uplataSelect = @"SELECT uplataID as ID, iznosUplate, datumUplate from tbl_Uplata;";

        static string voziloSelect = @"SELECT voziloID as ID, marka, model, registracioneOznake from tbl_Vozilo;";

        #endregion
        #region Delete upiti
        string casDelete = @"Delete from tbl_Cas where casID =";
        string instruktorDelete = @"Delete from tbl_Instruktor where instruktorID=";
        string ispitDelete = @"Delete from tbl_Ispit where ispitID=";
        string kandidatDelete = @"Delete from tbl_Kandidat where kandidatID=";
        string obukaDelete = @"Delete from tbl_ Obuka obukaID=";
        string prisustvoDelete = @"Delete from tbl_Prisustvo where prisustvoID=";
        string uplataDelete = @"Delete from tbl_Uplata where uplataID=";
        string voziloDelete = @"Delete from tbl_Vozilo where voziloID=";
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            konekcija = kon.KreirajKonekciju();
            UcitajPodatke(dataGridCentralni, casSelect);
        }

        private void UcitajPodatke(DataGrid grid, string selectUpit)
        {
            try
            {
                konekcija.Open();
                SqlDataAdapter dataAdapter = new SqlDataAdapter(selectUpit, konekcija);
                DataTable dt = new DataTable();
                dataAdapter.Fill(dt);
                if (grid != null)
                {
                    grid.ItemsSource = dt.DefaultView;
                }
                ucitanaTabela = selectUpit;
                dt.Dispose();
                dataAdapter.Dispose();
            }
            catch (SqlException)
            {
                MessageBox.Show("Neuspesno uneti podaci", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
            }
        }

        void PopuniFormu(DataGrid grid, string selectUslov)
        {
            try
            {
                konekcija.Open();
                azuriraj = true;
                DataRowView red = (DataRowView)grid.SelectedItems[0];
                SqlCommand cmd = new SqlCommand
                {
                    Connection = konekcija
                };
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                cmd.CommandText = selectUslov + "@id";
                SqlDataReader citac = cmd.ExecuteReader();
                cmd.Dispose();
                if (citac.Read())
                {
                    if (ucitanaTabela.Equals(casSelect))
                    {
                        FrmCas prozorCas = new FrmCas(azuriraj, red);
                        prozorCas.txtVreme.Text = citac["Vreme"].ToString();
                        prozorCas.txtTrajanje.Text = citac["Trajanje"].ToString();
                        prozorCas.dpDatum.SelectedDate = (DateTime)citac["Datum"];
                        prozorCas.cbObuka.SelectedValue = citac["obukaID"].ToString();
                        prozorCas.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(instruktorSelect))
                    {
                        FrmInstruktor prozorInstruktor = new FrmInstruktor(azuriraj, red);
                        prozorInstruktor.txtIme.Text = citac["Ime"].ToString();
                        prozorInstruktor.txtPrezime.Text = citac["Prezime"].ToString();
                        prozorInstruktor.txtJMBG.Text = citac["JMBG"].ToString();
                        prozorInstruktor.txtAdresa.Text = citac["Adresa"].ToString();
                        prozorInstruktor.txtGrad.Text = citac["Grad"].ToString();
                        prozorInstruktor.txtKontakt.Text = citac["Kontakt"].ToString();
                        prozorInstruktor.cbKandidat.SelectedValue = citac["kandidatID"].ToString();
                        prozorInstruktor.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(ispitSelect))
                    {
                        FrmIspit prozorIspit = new FrmIspit(azuriraj, red);
                        prozorIspit.txtVreme.Text = citac["Vreme"].ToString();
                        prozorIspit.txtPolozen.Text = citac["Polozen"].ToString();
                        prozorIspit.cbObuka.SelectedValue = citac["obukaID"].ToString();
                        prozorIspit.dpDatum.SelectedDate = (DateTime)citac["Datum"];
                        prozorIspit.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(kandidatSelect))
                    {
                        FrmKandidat prozorKandidat = new FrmKandidat(azuriraj, red);
                        prozorKandidat.txtIme.Text = citac["Ime"].ToString();
                        prozorKandidat.txtPrezime.Text = citac["Prezime"].ToString();
                        prozorKandidat.txtJMBG.Text = citac["JMBG"].ToString();
                        prozorKandidat.txtAdresa.Text = citac["Adresa"].ToString();
                        prozorKandidat.txtGrad.Text = citac["Grad"].ToString();
                        prozorKandidat.txtKontakt.Text = citac["Kontakt"].ToString();
                        prozorKandidat.dpDatum.SelectedDate = (DateTime)citac["Datum"];

                        prozorKandidat.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(obukaSelect))
                    {
                        FrmObuka prozorLetovi = new FrmObuka(azuriraj, red);
                        prozorLetovi.dpDatum.SelectedDate = (DateTime)citac["DatumPocetka"];
                        prozorLetovi.dpDatum2.SelectedDate = (DateTime)citac["DatumZavrsetka"];
                        prozorLetovi.txtKategorija.Text = citac["Kategorija"].ToString();
                        prozorLetovi.cbKandidat.SelectedValue = citac["kandidatID"].ToString();
                        prozorLetovi.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(prisustvoSelect))
                    {
                        FrmPrisustvo prozorPrisustvo = new FrmPrisustvo(azuriraj, red);
                        prozorPrisustvo.txtPrisutan.Text = citac["Prisutan"].ToString();
                        prozorPrisustvo.txtRazlog.Text = citac["Razlog"].ToString();
                        prozorPrisustvo.cbCas.SelectedValue = citac["casID"].ToString();
                        prozorPrisustvo.ShowDialog();
                    }
                    else if (ucitanaTabela.Equals(uplataSelect))
                    {
                        FrmUplata prozorUplata = new FrmUplata(azuriraj, red);
                        prozorUplata.dpDatum.SelectedDate = (DateTime)citac["Datum"];
                        prozorUplata.txtUplata.Text = citac["Iznos"].ToString();
                        prozorUplata.ShowDialog();

                    }
                    else if (ucitanaTabela.Equals(voziloSelect))
                    {
                        FrmVozilo prozorVozilo = new FrmVozilo(azuriraj, red);
                        prozorVozilo.txtMarka.Text = citac["Marka"].ToString();
                        prozorVozilo.txtModel.Text = citac["Model"].ToString();
                        prozorVozilo.txtRegistacija.Text = citac["RegistracioneOznake"].ToString();
                        prozorVozilo.ShowDialog();
                    }
                }

            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Niste selektovali red!", "Greska", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                if (konekcija != null)
                    konekcija.Close();
                    azuriraj = false;
            }
        }

        void ObrisiZapis(DataGrid grid, string deleteUpit)
        {
            try
            {
                konekcija.Open();
                DataRowView red = (DataRowView)grid.SelectedItems[0];
                MessageBoxResult rezultat = MessageBox.Show("Da li ste sigurni?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (rezultat == MessageBoxResult.Yes)
                {
                    SqlCommand cmd = new SqlCommand
                    {
                        Connection = konekcija
                    };
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = red["ID"];
                    cmd.CommandText = deleteUpit + "@id";
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Niste selektovali red", "Obavestenje", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SqlException)
            {
                MessageBox.Show("Postoje povezani podaci u nekim drugim tabelama", "Obavestenje", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            finally
            {
                if (konekcija != null)
                {
                    konekcija.Close();
                }
            }
        }

        private void btnCas_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, casSelect);
        }

        private void btnInstruktor_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, instruktorSelect);
        }

        private void btnIspit_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, ispitSelect);
        }


        private void btnKandidat_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, kandidatSelect);
        }

        private void btnObuka_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, obukaSelect);
        }

        private void btnPrisustvo_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, prisustvoSelect);
        }
        private void btnUplata_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, uplataSelect);
        }
        private void btnVozilo_Click(object sender, RoutedEventArgs e)
        {
            UcitajPodatke(dataGridCentralni, voziloSelect);
        }

        private void btnDodaj_Click(object sender, RoutedEventArgs e)
        {
            Window prozor;
            if (ucitanaTabela.Equals(casSelect))
            {
                prozor = new FrmCas();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, casSelect);
            }
            else if (ucitanaTabela.Equals(instruktorSelect))
            {
                prozor = new FrmInstruktor();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, instruktorSelect);
            }
            else if (ucitanaTabela.Equals(ispitSelect))
            {
                prozor = new FrmIspit();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, ispitSelect);
            }
            else if (ucitanaTabela.Equals(kandidatSelect))
            {
                prozor = new FrmKandidat();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, kandidatSelect);
            }
            else if (ucitanaTabela.Equals(obukaSelect))
            {
                prozor = new FrmObuka();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, obukaSelect);
            }
            else if (ucitanaTabela.Equals(prisustvoSelect))
            {
                prozor = new FrmPrisustvo();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, prisustvoSelect);
            }
            else if (ucitanaTabela.Equals(uplataSelect))
            {
                prozor = new FrmUplata();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, uplataSelect);
            }
            else if (ucitanaTabela.Equals(voziloSelect))
            {
                prozor = new FrmVozilo();
                prozor.ShowDialog();
                UcitajPodatke(dataGridCentralni, voziloSelect);
            }
        }

        private void btnIzmeni_Click(object sender, RoutedEventArgs e)
        {
            if (ucitanaTabela.Equals(casSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovCas);
                UcitajPodatke(dataGridCentralni, casSelect);
            }
            else if (ucitanaTabela.Equals(instruktorSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovInstruktor);
                UcitajPodatke(dataGridCentralni, instruktorSelect);
            }
            else if (ucitanaTabela.Equals(ispitSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovIspit);
                UcitajPodatke(dataGridCentralni, ispitSelect);
            }
            else if (ucitanaTabela.Equals(kandidatSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovKandidat);
                UcitajPodatke(dataGridCentralni, kandidatSelect);
            }
            else if (ucitanaTabela.Equals(obukaSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovObuka);
                UcitajPodatke(dataGridCentralni, obukaSelect);
            }
            else if (ucitanaTabela.Equals(prisustvoSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovPrisustvo);
                UcitajPodatke(dataGridCentralni, prisustvoSelect);
            }
            else if (ucitanaTabela.Equals(uplataSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovUplata);
                UcitajPodatke(dataGridCentralni, uplataSelect);
            }
            else if (ucitanaTabela.Equals(voziloSelect))
            {
                PopuniFormu(dataGridCentralni, selectUslovcVozilo);
                UcitajPodatke(dataGridCentralni, voziloSelect);
            }
        }

        private void btnObrisi_Click(object sender, RoutedEventArgs e)
        {
            if (ucitanaTabela.Equals(casSelect))
            {
                ObrisiZapis(dataGridCentralni, casDelete);
                UcitajPodatke(dataGridCentralni, casSelect);
            }
            else if (ucitanaTabela.Equals(instruktorSelect))
            {
                ObrisiZapis(dataGridCentralni, instruktorDelete);
                UcitajPodatke(dataGridCentralni, instruktorSelect);
            }
            else if (ucitanaTabela.Equals(ispitSelect))
            {
                ObrisiZapis(dataGridCentralni, ispitDelete);
                UcitajPodatke(dataGridCentralni, ispitSelect);
            }
            else if (ucitanaTabela.Equals(kandidatSelect))
            {
                ObrisiZapis(dataGridCentralni, kandidatDelete);
                UcitajPodatke(dataGridCentralni, kandidatSelect);
            }
            else if (ucitanaTabela.Equals(obukaSelect))
            {
                ObrisiZapis(dataGridCentralni, obukaDelete);
                UcitajPodatke(dataGridCentralni, obukaSelect);
            }
            else if (ucitanaTabela.Equals(prisustvoSelect))
            {
                ObrisiZapis(dataGridCentralni, prisustvoDelete);
                UcitajPodatke(dataGridCentralni, prisustvoSelect);
            }
            else if (ucitanaTabela.Equals(uplataSelect))
            {
                ObrisiZapis(dataGridCentralni, uplataDelete);
                UcitajPodatke(dataGridCentralni, uplataSelect);
            }
            else if (ucitanaTabela.Equals(voziloSelect))
            {
                ObrisiZapis(dataGridCentralni, voziloDelete);
                UcitajPodatke(dataGridCentralni, voziloSelect);
            }
        }

    }
}
