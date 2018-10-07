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
            var dataQ = new DataQ();
            try
            {
                Debug.WriteLine(@"Connecting to SQL Server ... ");
                using (var connection = new SqlConnection(dataQ.Builder.ConnectionString))
                {
                    connection.Open();
                    Debug.WriteLine(@"Done.");
                }
            }
            catch (SqlException exception)
            {
                DataQ.DisplaySqlErrors(exception);
                Debug.WriteLine(exception.ToString());
            }
            finally
            {
                using (var connection = new SqlConnection(dataQ.Builder.ConnectionString))
                {
                    connection.Close();
                    Debug.WriteLine(@"Close.");
                }
            }
        }
    }
}