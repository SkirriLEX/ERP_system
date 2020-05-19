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
using System.Windows.Shapes;

namespace erp
{
    /// <summary>
    /// Interaction logic for AddNewNews.xaml
    /// </summary>
    public partial class AddNewNews : Window
    {
        public AddNewNews()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new FacultyEntities())
            {
                var news1 = new News()
                {
                    theme = themeNewsBox.Text,
                    textOfNews = textNewsBox.Text,
                    createdOn = DateTime.Today,
                    idNews = Guid.NewGuid().ToString()
                };
                context.News.Add(news1);
                context.SaveChanges();
            }
            this.Close();
        }
    }
}
