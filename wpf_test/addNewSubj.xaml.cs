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
    /// Interaction logic for addNewSubj.xaml
    /// </summary>
    public partial class addNewSubj : Window
    {
        public addNewSubj()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new FacultyEntities())
            {
                var Subject = new Subject()
                {
                    codeTeacher = Guid.Parse(teacherText.Text),
                    codeSpec = Guid.Parse(codeSpecText.Text),
                    nameSubj = nameSubjText.Text,
                    codeSubj = Convert.ToInt32(codeSubjText.Text),
                    hoursForSubj = Convert.ToSingle(hoursText.Text)
                };
                context.Subjects.Add(Subject);
                context.SaveChanges();
            }
            this.Close();
        }
    }
}
