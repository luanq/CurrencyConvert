using System;
using System.Collections.Generic;
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
using System.Data.SqlClient;
using System.Data;

namespace CurrencyConvert
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=D:\\CSharpProject\\CurrencyConvert\\CurrencyConvert\\Database\\CurrencyConvert.mdf;Integrated Security=True";
        SqlConnection conn = null;
        private int CurrencyId = 0;

        //Declare FromAmount with double data type and assign value 0.
        private double FromAmount = 0;

        //Declare ToAmount with double data type and assign value 0.
        private double ToAmount = 0;

        public MainWindow()
        {
            conn = new SqlConnection(connectionString);
            
            InitializeComponent();
            ClearControls();
            BindCurrency();

        }
        public void OpenDB()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            Console.WriteLine("数据库已连接");
        }


        private void BindCurrency()
        {
            OpenDB();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("SELECT Id ,CurrencyName from CurrencyMaster", conn);
            cmd.CommandType= CommandType.Text;

            SqlDataAdapter da= new SqlDataAdapter(cmd);
            da.Fill(dt);

            DataRow newRow = dt.NewRow();
            newRow["Id"] = 0;
            newRow["CurrencyName"] = "--Select--";

            dt.Rows.InsertAt(newRow,0);

            if(dt!=null&&dt.Rows.Count>0)
            {
                cmbFromCurrency.ItemsSource= dt.DefaultView;
                cmbToCurrency.ItemsSource = dt.DefaultView;
            }
            conn.Close();

            cmbFromCurrency.DisplayMemberPath = "CurrencyName";
            cmbFromCurrency.SelectedValuePath = "Id";
            cmbFromCurrency.SelectedValue = 0;
            cmbToCurrency.DisplayMemberPath = "CurrencyName";
            cmbToCurrency.SelectedValuePath = "Id";
            cmbToCurrency.SelectedValue = 0;


        }
        #region 转换按钮事件

        private void btnConvert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                double ConvertedValue;

                if (txtCurrency.Text == null || txtCurrency.Text.Trim() == "")
                {
                    MessageBox.Show("Please enter currency", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    txtCurrency.Focus();
                    return;
                }
                else if (cmbFromCurrency.SelectedValue == null || cmbFromCurrency.SelectedIndex == 0)
                {
                 
                    MessageBox.Show("Please select from currency", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    cmbFromCurrency.Focus();
                    return;
                }
                else if (cmbToCurrency.SelectedValue == null || cmbToCurrency.SelectedIndex == 0)
                {
                    MessageBox.Show("Please select to currency", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    cmbToCurrency.Focus();
                    return;
                }

                if (cmbFromCurrency.SelectedValue == cmbToCurrency.SelectedValue)   
                {
                    ConvertedValue = double.Parse(txtCurrency.Text);

                    //Show the label converted currency name and converted currency amount. The ToString("N3") is used for Placing 000 after the dot(.)
                    lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
                }
                else
                {
                    if (FromAmount != null && FromAmount != 0 && ToAmount != null && ToAmount != 0)
                    {
                        ConvertedValue = FromAmount * double.Parse(txtCurrency.Text) / ToAmount;


                        lblCurrency.Content = cmbToCurrency.Text + " " + ConvertedValue.ToString("N3");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        #endregion

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            //Regular Expression to add regex. Add library using System.Text.RegularExpressions;
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        private void ClearControls()
        {
            try
            {
                txtCurrency.Clear();
                cmbFromCurrency.SelectedIndex = 0;
                cmbToCurrency.SelectedIndex= 0;
                lblCurrency.Content = "";
                txtCurrency.Focus();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

 

        private void txtCurrency_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }


        private void cmbFromCurrency_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void cmbFromCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbToCurrency_PreviewKeyDown(object sender, KeyEventArgs e)
        {

        }

        private void cmbToCurrency_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }


        private void btnClear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
