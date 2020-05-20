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
    /// Interaction logic for AddNewStud.xaml
    /// </summary>
    public partial class AddNewStud : Window
    {
        public AddNewStud()
        {
            InitializeComponent();
        }

        private void Button_add_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new FacultyEntities())
            {
                var student = new Student()
                {
                    codePerson = Convert.ToInt32(codeStudentText.Text),
                    firstName = firstNameText.Text,
                    midName = midNameText.Text,
                    lastName = lastNameText.Text,
                    dateofBirth = DateTime.Parse(birthdayText.Text),
                    grupCode = Convert.ToInt32(groupText.Text),
                    addrr = addrrLiveText.Text,
                    phoneNum = phonenumText.Text,
                    email = emailText.Text,
                    parentPhoneNum = parentPhoneText.Text,
                    dateBegin = DateTime.Parse(dateEntryText.Text),
                    isContractor = Convert.ToBoolean(isContractorText.Text)
                };
                context.Students.Add(student);
                context.SaveChanges();
            }
            this.Close();
        }
    }
}
