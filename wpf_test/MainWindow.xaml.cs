using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
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
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void closeProgram_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
            ButtonCloseMenu.Visibility = Visibility.Visible;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;

        }

        private void click_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            
        }

        private void ListViewItem_Selected_1(object sender, RoutedEventArgs e)
        {
            Button_addNewNews.Visibility = Visibility.Visible;
            ListView_News.Visibility = Visibility.Visible;
            news ListHello = new news();
            ListHello.getTableNews();
            ListView_News.Items.Add(ListHello.authorID);
            ListView_News.Items.Add(ListHello.theme);
            ListView_News.Items.Add(ListHello.textOfNews);
            ListView_News.Items.Add(ListHello.createdOn);
        }

        private void Button_addNewNews_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddNewNews();
            addWindow.Show();
        }
    }

}