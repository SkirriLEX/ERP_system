using System;
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
            if (true)
            {
                textBox.BorderBrush =
                textBox.Foreground =
                passwordBox.BorderBrush =
                passwordBox.Foreground = Brushes.IndianRed;
            }
            else
            {
                Console.WriteLine("My congratulate");
            }
        }

        //-------- Fancy Block
        private void TextBox_TextChanged(object sender, RoutedEventArgs e)
        {
            textBox.BorderBrush = Brushes.Gray;
            textBox.Foreground = Brushes.Black;
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
