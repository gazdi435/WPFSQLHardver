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
using MySql.Data.MySqlClient;

namespace databaseFeladat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private const string kapcsolatLeiro = "datasource=127.0.0.1;port=3306;username=root;password=;database=hardver;charset=utf8;";
        List<Termek> termekek = new List<Termek>();
        MySqlConnection SQLkapcsolat;
        public MainWindow()
        {
            InitializeComponent();
            OpenDatabase();
            KategoriaBetolt();
            GyartokBetoltese();

            TermekBetolteseListaba();

            CloseDatabase();
        }

        private void TermekBetolteseListaba()
        {
            string SQLOsszesTermek = "SELECT * FROM termékek;";
            MySqlCommand SQLparancs = new MySqlCommand(SQLOsszesTermek, SQLkapcsolat);
            MySqlDataReader adatOlvaso = SQLparancs.ExecuteReader();

            while (adatOlvaso.Read())
            {
                Termek uj = new Termek(
                    adatOlvaso.GetString("Kategória"),
                    adatOlvaso.GetString("Gyártó"),
                    adatOlvaso.GetString("Név"),
                    adatOlvaso.GetInt32("Ár"),
                    adatOlvaso.GetInt32("Garidő")
                    );
                termekek.Add(uj);
            }

            adatOlvaso.Close();
            dgTermekek.ItemsSource = termekek;
        }

        private void GyartokBetoltese()
        {
            String SQLGyartokRendezve = "SELECT DISTINCT gyártó FROM termékek ORDER BY gyártó";

            MySqlCommand SQLParancs = new MySqlCommand(SQLGyartokRendezve, SQLkapcsolat);
            MySqlDataReader eredmenyOlvaso = SQLParancs.ExecuteReader();

            cbGyarto.Items.Add("- NINCS MEGADVA -");

            while (eredmenyOlvaso.Read())
            {
                cbGyarto.Items.Add(eredmenyOlvaso.GetString("Gyártó"));
            }

            eredmenyOlvaso.Close();
            cbGyarto.SelectedIndex = 0;
        }

        private void OpenDatabase()
        {
            try
            {
                SQLkapcsolat = new MySqlConnection(kapcsolatLeiro);
                SQLkapcsolat.Open();
            }
            catch (Exception)
            {

                MessageBox.Show("Nem tud kapcsolódni az adatbázishoz!");
                this.Close();
            }
        }

        private void CloseDatabase()
        {
            SQLkapcsolat.Close();
            SQLkapcsolat.Dispose();
        }

        private void KategoriaBetolt()
        {

            string SQLKategoriakRendezv = "SELECT DISTINCT Kategória FROM termékek ORDER BY kategória;";
            MySqlCommand SQLParancs = new MySqlCommand(SQLKategoriakRendezv, SQLkapcsolat);
            MySqlDataReader eredmenyOlvaso = SQLParancs.ExecuteReader();

            cbKategoria.Items.Add("- NINCS MEGADVA -");

            while (eredmenyOlvaso.Read())
            {
                cbKategoria.Items.Add(eredmenyOlvaso.GetString("kategória"));
            }

            eredmenyOlvaso.Close();
            cbKategoria.SelectedIndex = 0;

        }
    }
}
