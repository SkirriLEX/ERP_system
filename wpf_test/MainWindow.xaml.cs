using System.Data.SqlClient;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Data.Entity.Core.Objects;
using System.Linq;

namespace erp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        FacultyEntities dataEntities = new FacultyEntities();
        public MainWindow()
        {
            InitializeComponent();
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

        private void Button_add_Click(object sender, RoutedEventArgs e)
        {
            var addWindow = new AddNewNews();
            addWindow.Show();

        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            var logInView = new LoginScreen();
            logInView.Show();
        }

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {
            DataGridTable.Visibility = Visibility.Visible;
            Button_Speciality.Visibility = Visibility.Visible;

            var query =
                from Specialities in dataEntities.Specialities
                orderby Specialities.specialityCode ascending 
                select new { Specialities.specialityCode, Specialities.nameSpec};
             DataGridTable.ItemsSource = query.ToList();
            DataGridTable.Columns[0].Header = "Код спеціальності";
            DataGridTable.Columns[1].Header = "Назва спеціальності";
        }

        private void ListViewItem_Selected_2(object sender, RoutedEventArgs e)
        {
            DataGridTable.Visibility = Visibility.Visible;

            var query =
                from Specialization in dataEntities.Specializations
                orderby Specialization.specialityCode ascending
                select new { Specialization.specializationCode, Specialization.nameSpecialization };
            DataGridTable.ItemsSource = query.ToList();
            DataGridTable.Columns[0].Header = "Код спеціалізації";
            DataGridTable.Columns[1].Header = "Назва спеціалізації";
        }

        private void ListViewItem_Selected_3(object sender, RoutedEventArgs e)
        {
            DataGridTable.Visibility = Visibility.Visible;

            var query =
                from Employee in dataEntities.Employees
                orderby Employee.firstName ascending
                select new { Employee.firstName, Employee.midName, Employee.lastName,
                            Employee.dateofBirth, Employee.addrr, Employee.email,
                            Employee.Departments, Employee.Groups, Employee.Subjects};
            DataGridTable.ItemsSource = query.ToList();
            DataGridTable.Columns[0].Header = "Ім'я";
            DataGridTable.Columns[1].Header = "По-батькові";
            DataGridTable.Columns[2].Header = "Призвище";
            DataGridTable.Columns[3].Header = "Дата народження";
            DataGridTable.Columns[4].Header = "Адреса реєстрації";
            DataGridTable.Columns[5].Header = "Ел. пошта";
            DataGridTable.Columns[6].Header = "Назва відділу";
            DataGridTable.Columns[7].Header = "Група";
            DataGridTable.Columns[8].Header = "Предмети";
        }

        private void ListViewItem_Selected_4(object sender, RoutedEventArgs e)
        {
            DataGridTable.Visibility = Visibility.Visible;

            var query =
                from Subject in dataEntities.Subjects
                orderby Subject.nameSubj ascending
                select new { Subject.nameSubj, Subject.hoursForSubj, Subject.Employee, Subject.Specialization};
            DataGridTable.ItemsSource = query.ToList();
            DataGridTable.Columns[0].Header = "Назва предмету";
            DataGridTable.Columns[1].Header = "К-сть годин";
            DataGridTable.Columns[2].Header = "Відповідальний викладач";
            DataGridTable.Columns[3].Header = "Назва спеціалізації";
        }

        private void ListViewItem_Selected_5(object sender, RoutedEventArgs e)
        {
            DataGridTable.Visibility = Visibility.Visible;

            var query =
                from Position in dataEntities.Positions
                orderby Position.namePosition ascending
                select new { Position.codePosition, Position.namePosition };
            DataGridTable.ItemsSource = query.ToList();
            DataGridTable.Columns[0].Header = "Код посади";
            DataGridTable.Columns[1].Header = "Назва посади";
        }

        private void ListViewItem_Selected_6(object sender, RoutedEventArgs e)
        {
            DataGridTable.Visibility = Visibility.Visible;

            var query =
                from Department in dataEntities.Departments
                orderby Department.nameDepartment ascending
                select new {Department.nameDepartment, Department.departmentLeadId, Department.Speciality};
            DataGridTable.ItemsSource = query.ToList();
            DataGridTable.Columns[0].Header = "Назва департаменту";
            DataGridTable.Columns[1].Header = "Завідувач кафедрою";
            DataGridTable.Columns[2].Header = "Спеціальність";
        }

        private void ListViewItem_Selected_7(object sender, RoutedEventArgs e)
        {
            DataGridTable.Visibility = Visibility.Visible;

            var query =
                from Student in dataEntities.Students
                orderby Student.firstName ascending
                select new
                {
                    Student.firstName,
                    Student.midName,
                    Student.lastName,
                    Student.Group,
                    Student.addrr,
                    Student.dateofBirth,
                    Student.email,
                    Student.phoneNum
                };
            DataGridTable.ItemsSource = query.ToList();
            DataGridTable.Columns[0].Header = "Ім'я";
            DataGridTable.Columns[1].Header = "По-батькові";
            DataGridTable.Columns[2].Header = "Прізвище";
            DataGridTable.Columns[3].Header = "Група";
            DataGridTable.Columns[4].Header = "Адреса реєстрації";
            DataGridTable.Columns[5].Header = "Дата народження";
            DataGridTable.Columns[6].Header = "Ел. пошта";
            DataGridTable.Columns[7].Header = "Телефон";
        }

        private void ListViewItem_Selected_8(object sender, RoutedEventArgs e)
        {
            DataGridTable.Visibility = Visibility.Visible;

            var query =
                from Group in dataEntities.Groups
                orderby Group.nameGroup ascending
                select new
                {
                    Group.nameGroup,
                    Group.Specialization,
                    Group.studentLeadId,
                    Group.learningForm,
                    Group.teacherLeadId
                };
            DataGridTable.ItemsSource = query.ToList();
            DataGridTable.Columns[0].Header = "Назва групи";
            DataGridTable.Columns[1].Header = "Спеціалізація";
            DataGridTable.Columns[2].Header = "Староста";
            DataGridTable.Columns[3].Header = "Форма навчання";
            DataGridTable.Columns[4].Header = "Куратор";

        }

        private void ListViewItem_Selected_10(object sender, RoutedEventArgs e)
        {
            DataGridTable.Visibility = Visibility.Visible;

            var query =
                from studyMark in dataEntities.studyMarks
                select new {studyMark.Student, studyMark.Subject, studyMark.markSubj, studyMark.createdOn};

            DataGridTable.ItemsSource = query.ToList();

            DataGridTable.Columns[0].Header = "Студент";
            DataGridTable.Columns[1].Header = "Предмет";
            DataGridTable.Columns[2].Header = "Оцінка";
            DataGridTable.Columns[3].Header = "Дата створення";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var formNewNews = new AddNewNews();
            formNewNews.Show();
        }

        private void ListViewItem_Selected_1(object sender, RoutedEventArgs e)
        {
            Button_news.Visibility = Visibility.Visible;
            DataGridTable.Visibility = Visibility.Visible;

            var query =
                from News in dataEntities.News
                select new {News.createdOn, News.theme, News.textOfNews};

            DataGridTable.ItemsSource = query.ToList();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var formSpeciality = new AddNewSpeciality();
            formSpeciality.Show();
        }
    }

}