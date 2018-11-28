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

        private void closeProgram_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void PackIcon_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        //private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        //{
        //    ButtonOpenMenu.Visibility = Visibility.Collapsed;
        //    ButtonCloseMenu.Visibility = Visibility.Visible;
        //}

        //private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        //{
        //    ButtonOpenMenu.Visibility = Visibility.Visible;
        //    ButtonCloseMenu.Visibility = Visibility.Collapsed;

        //}
    }

}