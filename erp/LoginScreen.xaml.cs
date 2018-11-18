using System.Diagnostics;
using System.Windows;
using System.Windows.Media;

namespace erp
{
    /// <summary>
    /// Логика взаимодействия для LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        public LoginScreen()
        {
            InitializeComponent();
        }
        private void Confirm_Button(object sender, RoutedEventArgs e)//Click Generall Button
        {
            ClearPassError();
            var data = new DataQ();
            if (DataQ.CheckLog(loginText.Text, passwordBox.Password))
            {
                Debug.WriteLine("My congratulate");
            }
            else
            {
                loginText.BorderBrush = loginText.Foreground =
                passwordBox.BorderBrush = passwordBox.Foreground = 
                    Brushes.IndianRed;
            }
        }

        //-------- Fancy Block
        private void TextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            loginText.BorderBrush = Brushes.Gray;
            loginText.Foreground = Brushes.Black;
        }

        private void GotFocus_Event(object sender, RoutedEventArgs e)
        {
            if (passwordBox.BorderBrush == Brushes.IndianRed)
                passwordBox.Password = "";
            ClearPassError();
        }

        private void ClearPassError()
        {
            passwordBox.BorderBrush = Brushes.Gray;
            passwordBox.Foreground = Brushes.Black;
        }
    }
}
