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
    /// Interaction logic for AddNewSpeciality.xaml
    /// </summary>
    public partial class AddNewSpeciality : Window
    {
        public AddNewSpeciality()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new FacultyEntities())
            {
                var Speciality = new Speciality()
                {
                    specialityCode = Convert.ToInt32(codeText.Text),
                    nameSpec = nameText.Text
                };
                context.Specialities.Add(Speciality);
                context.SaveChanges();
            }
            this.Close();
        }
    }
}
