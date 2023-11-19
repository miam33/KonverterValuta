using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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


namespace CurrencyConverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection con = new SqlConnection();       //Objekt za konekciju na bazu
        SqlCommand cmd = new SqlCommand();            //Objekt za sql komandu
        SqlDataAdapter da = new SqlDataAdapter();    //SqlAdapter objekt
       
        private int CurrencyId = 0;           //Id valute
        private double FromAmount = 0;        
        private double ToAmount = 0;          


        public MainWindow()
        {
           InitializeComponent();
           BindCurrency();
        }

        public void mycon() 
        {
            String Conn = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString; //Spoj na bazu string
            con = new SqlConnection(Conn);
            con.Open(); //Ostvari konekciju
        }

        /*
            Metoda koja dohvaća valute iz baze i postavlja ih u comboBoxove
        */
        private void BindCurrency() 
        {
            mycon();
            //Stvori objekt DataTable
            DataTable dt = new DataTable();

            //Upit za dohvaćanje podataka iz tablice
            cmd = new SqlCommand("select Id, Amount, CurrencyName from Currency_Master", con);
            cmd.CommandType = CommandType.Text;
            da = new SqlDataAdapter(cmd);
            da.Fill(dt);

            //Postavljanje prvog reda u dropdownu na SELECT
            DataRow newRow = dt.NewRow();

            newRow["Id"] = 0;

            newRow["CurrencyName"] = "--SELECT--";

            //Ubaci red

            dt.Rows.InsertAt(newRow, 0);

            //Ako postoji nesto u dt napuni comboBoxove
            if (dt != null && dt.Rows.Count > 0) 
            {
                //postavljanje podataka u comboBox From
                cmbFromCurrency.ItemsSource = dt.DefaultView;

                //postavljanje podataka u comboBox To
                cmbToCurrency.ItemsSource = dt.DefaultView;
            }
            con.Close();
     
            cmbFromCurrency.DisplayMemberPath = "CurrencyName";
            cmbFromCurrency.SelectedValuePath = "Amount";
            cmbFromCurrency.SelectedIndex = 0;

            cmbToCurrency.DisplayMemberPath = "CurrencyName";
            cmbToCurrency.SelectedValuePath = "Amount";
            cmbToCurrency.SelectedIndex = 0;
        }

        private void Convert_Click(object sender, RoutedEventArgs e)
        {

            double ConvertedValue;

            //Provjeri ako je polje za vrijednost prazno
            if (txtCurrency.Text == null || txtCurrency.Text.Trim() == "")
            {
                //Prikaži messagebox ako je prazno polje
                MessageBox.Show("Molimo vas unesite vrijednost.", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Information);
           
                txtCurrency.Focus();
                return;
            }
            //Ako nije selecktirana valuta iz koje se pretvara
            else if (cmbFromCurrency.SelectedValue == null || cmbFromCurrency.SelectedIndex == 0)
            {
                //prikaži poruku
                MessageBox.Show("Molimo odaberite valutu iz koje pretvarate", "Infromation", MessageBoxButton.OK, MessageBoxImage.Information);

                cmbToCurrency.Focus();
                return;
            }
            //Ako nije selektirana valuta u koju se pretvara
            else if (cmbToCurrency.SelectedValue == null || cmbToCurrency.SelectedIndex == 0) 
            {
                //prikaži poruku
                MessageBox.Show("Molimo odaberite valutu u koju pretvarate", "Information", MessageBoxButton.OK, MessageBoxImage.Information);

                cmbToCurrency.Focus();
                return;
            }

            //Provjeri ako su obje valute iste
            if (cmbFromCurrency.Text == cmbToCurrency.Text)
            {
                //uzmi vrijednost iz 
                ConvertedValue = double.Parse(txtCurrency.Text);

                //Prikaži vrijednost convertedValue
                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
            else 
            {
                //Formula za pretvaranje jedne valute u drugu
                ConvertedValue = (double.Parse(cmbToCurrency.SelectedValue.ToString()) 
                    * double.Parse(txtCurrency.Text)) 
                    / double.Parse(cmbFromCurrency.SelectedValue.ToString());

                //Prikaži rezultat
                lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
            }
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearControls();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^[0-9]+]");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void ClearControls() 
        {
            txtCurrency.Text = string.Empty;
            if (cmbFromCurrency.Items.Count > 0)
                cmbFromCurrency.SelectedIndex = 0;
            if (cmbToCurrency.Items.Count > 0)
                cmbToCurrency.SelectedIndex = 0;
            lblCurrency.Content = "";
            txtCurrency.Focus();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                if (txtAmount.Text == null || txtAmount.Text.Trim() == "")
                {
                    MessageBox.Show("Unesite vrijednost", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtAmount.Focus();
                    return;
                }
                else if (txtCurrencyName.Text == null || txtCurrencyName.Text.Trim() == "")
                {
                    MessageBox.Show("Unesite ime valute", "Upozorenje", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtCurrencyName.Focus();
                    return;
                }
                else 
                {
                    if (CurrencyId > 0) // code for update button. Here check CurrencyId greater than zero than it is go for update
                    {
                        if (MessageBox.Show("Jeste li sigurni da želite ažurirati valutu?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) //Pokaži poruku
                        {
                            mycon();
                            DataTable dt = new DataTable();
                            cmd = new SqlCommand("UPDATE Currency_Master SET Amount = @Amount, CurrencyName = @CurrencyName WHERE Id = @Id", con); //ažuriraj postojeću valutu
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Id", CurrencyId);
                            cmd.Parameters.AddWithValue("@Amount", Math.Round(float.Parse(txtAmount.Text), 3));
                            cmd.Parameters.AddWithValue("@CurrencyName", txtCurrencyName.Text.ToString());
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Uspješno ste ažurirali podatke.", "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    else // save button code
                    {
                        if (MessageBox.Show("Želite li spremiti podatke?", "Potvrda", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes) 
                        {
                            mycon();
                            cmd = new SqlCommand("INSERT INTO Currency_Master(Amount, CurrencyName) VALUES(@Amount, @CurrencyName)", con); //insertaj novu valutu
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.AddWithValue("@Amount", Math.Round(float.Parse(txtAmount.Text), 3));
                            cmd.Parameters.AddWithValue("@CurrencyName", txtCurrencyName.Text);
                            cmd.ExecuteNonQuery();
                            con.Close();

                            MessageBox.Show("Uspješno ste spremili podatke.", "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                    ClearMaster();
                }
            }
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void ClearMaster() //Refreshaj currency master tab
        {
            try 
            {
                txtAmount.Text = string.Empty;
                txtCurrencyName.Text = string.Empty;
                btnSave.Content = "Save";
                GetData();
                CurrencyId = 0;
                BindCurrency();
                txtAmount.Focus();
            } 
            catch(Exception ex) 
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                ClearMaster();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);            
            }
        }

        //Metoda koja napuni tablicu sa svim valutama u drugom tabu
        public void GetData()
        {
            //Postavi vezu sa bazom
            mycon();

            DataTable dt = new DataTable();
            //Upit koji dohvaća sve valute iz baze  
            cmd = new SqlCommand("SELECT * FROM Currency_Master", con);
            //CommandType određuje koju vrstu komande pozivamo   
            cmd.CommandType = CommandType.Text;

            da = new SqlDataAdapter(cmd);
            
            da.Fill(dt); 

            //Provjerimo ako smo nešto dohvatili u dt
            if (dt != null && dt.Rows.Count > 0)
            {
                //Postavi sve valute koje smo dohvatili u tablicu  
                dgvCurrency.ItemsSource = dt.DefaultView;
            }
            else
            {
                dgvCurrency.ItemsSource = null;
            }

            con.Close();
        }

        private void dgvCurrency_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {

            try
            {

                DataGrid grd = (DataGrid)sender;

                //Dohvatimo redak u tablici koji je selektiran
                DataRowView row_selected = grd.CurrentItem as DataRowView;

                //ako je slektiran neki redak udi u if
                if (row_selected != null)
                {

                    //dgvCurrency items count greater than zero
                    if (dgvCurrency.Items.Count > 0)
                    {
                        if (grd.SelectedCells.Count > 0)
                        {

                            //Dohvaćanje ID valute koja je kliknuta
                            CurrencyId = Int32.Parse(row_selected["Id"].ToString());

                            //DisplayIndex od ćelije je 0, to je stupac sa edit gumbom
                            if (grd.SelectedCells[0].Column.DisplayIndex == 0)
                            {
                                //Uzmi vrijednost valute i postavi u polje amount
                                txtAmount.Text = row_selected["Amount"].ToString();

                                //Uzmi ime valute i postavi u polje CurrencyName
                                txtCurrencyName.Text = row_selected["CurrencyName"].ToString();

                                //Postavi text gumba na update jer sada korisnik ažurira vrijednosti
                                btnSave.Content = "Update";
                            }

                            //DisplayIndex ćelije je 1, to je stupac za brisanje               
                            if (grd.SelectedCells[0].Column.DisplayIndex == 1)
                            {
                                //Show confirmation dialogue box
                                if (MessageBox.Show("Želite li obrisati valutu ?", "Upozorenje", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                                {
                                    mycon();
                                    DataTable dt = new DataTable();

                                    //Napravi upit za brisanje valute iz tablice
                                    cmd = new SqlCommand("DELETE FROM Currency_Master WHERE Id = @Id", con);
                                    cmd.CommandType = CommandType.Text;

                                    //CurrencyId je ID valute koju brišemo
                                    cmd.Parameters.AddWithValue("@Id", CurrencyId);
                                    cmd.ExecuteNonQuery();
                                    con.Close();

                                    MessageBox.Show("Valuta je uspješno obrisana", "Uspjeh", MessageBoxButton.OK, MessageBoxImage.Information);
                                    ClearMaster();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //Metoda koja nam pokreće postavljenje prikaza kada je selektiran drugi tab
        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl tabControl)
            {
                // Provjera ako je selektiran drugi tab
                if (tabControl.SelectedIndex == 1) 
                {
                    ClearMaster();
                }
            }
        }
    }
}
