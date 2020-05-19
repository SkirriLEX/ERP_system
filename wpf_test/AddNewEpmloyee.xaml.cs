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
    /// Interaction logic for AddNewEpmloyee.xaml
    /// </summary>
    public partial class AddNewEpmloyee : Window
    {
        public AddNewEpmloyee()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new FacultyEntities())
            {
                var Employee = new Employee()
                {
                    codeEmployee = Guid.Parse(codeEmpText.Text),
                    firstName = firstNameText.Text,
                    lastName = secondNameText.Text,
                    midName = midNameText.Text,
                    dateofBirth = Convert.ToDateTime(birthdayText.Text),
                    positionCode = Convert.ToInt32(positionText.Text),
                    addrr = addrrLiveText.Text,
                    phoneNum = Convert.ToInt32(phoneText.Text),
                    email = emailText.Text,
                    dateBegin = Convert.ToDateTime(dateBeginText.Text)
                };
                context.Employees.Add(Employee);
                context.SaveChanges();
            }
            this.Close();
        }
    }
}
