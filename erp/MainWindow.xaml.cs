using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Windows;

namespace erp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var data = new DataQ();
            try
            {
                Debug.Write(@"Connecting to SQL Server ... ");
                using (var connection = new SqlConnection(data.Builder.ConnectionString))
                {
                    connection.Open();
                    Debug.WriteLine("Done.");
                }
            }
            catch (SqlException exception)
            {
                DataQ.DisplaySqlErrors(exception);
                //Debug.WriteLine(exception);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}