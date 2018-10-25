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
            ClearTextError();
            var data = new DataQ();
            data.tryConnect();
            if (data.Check(loginText.Text, passwordBox.Password))
            {
                Debug.WriteLine("My congratulate");
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            }
            else
            {
                Debug.WriteLine("Not correct login or password"); 
                loginText.BorderBrush = loginText.Foreground =
                passwordBox.BorderBrush = passwordBox.Foreground = 
                    Brushes.IndianRed;
            }
        }

        //-------- Fancy Block
        private void TextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            ClearTextError();
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
        private void ClearTextError()
        {
            loginText.BorderBrush = Brushes.Gray;
            loginText.Foreground = Brushes.Black;
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                Confirm_Button(sender, e);
                Debug.WriteLine("Press");
            }
        }
    }
}
