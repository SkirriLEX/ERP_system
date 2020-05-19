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
    /// Interaction logic for AddNewSpecialization.xaml
    /// </summary>
    public partial class AddNewSpecialization : Window
    {
        public AddNewSpecialization()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new FacultyEntities())
            {
                var Specialization = new Specialization()
                {
                    specialityCode = Guid.Parse(specialityCodeText.Text),
                    specializationCode = Convert.ToInt32(specializationCodeText.Text),
                    nameSpecialization = nameSpecializationText.Text
                };
                context.Specializations.Add(Specialization);
                context.SaveChanges();
            }
            this.Close();
        }
    }
}
