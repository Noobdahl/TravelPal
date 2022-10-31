using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using TravelPal.Enums;
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        UserManager userManager;
        public RegisterWindow(UserManager manager)
        {
            InitializeComponent();
            userManager = manager;
            foreach (Countries country in Enum.GetValues(typeof(Countries)))
            {
                string displayName;
                displayName = country.GetType().GetMember(country.ToString()).FirstOrDefault().GetCustomAttribute<DisplayAttribute>()?.GetName();
                if (String.IsNullOrEmpty(displayName))
                {
                    displayName = country.ToString();
                }
                cbCountries.Items.Add(displayName);
            }
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {

            if (tbUsername.Text.Length < 3)
                MessageBox.Show("Username must be atleast 3 characters.");
            else if (pbPassword.Password.Length < 5)
                MessageBox.Show("Password must be atleast 5 characters.");
            else if (pbPassword.Password != pbConfirmPassword.Password)
                MessageBox.Show("Passwords did not match.");
            else
            {
                User user = new();
                string countryString = cbCountries.Text.Replace(" ", "_");
                user.IUser(tbUsername.Text, pbPassword.Password, (Countries)Enum.Parse(typeof(Countries), countryString));
                if (userManager.AddUser(user))
                {
                    MessageBox.Show("Register completed!");
                    ((MainWindow)Application.Current.MainWindow).Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("That username is already taken.");
                    pbPassword.Clear();
                    pbConfirmPassword.Clear();
                }
            }
            pbPassword.Clear();
            pbConfirmPassword.Clear();
        }

        private void cbCountries_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            btnCreate.IsEnabled = true;
        }

        private void tbUsername_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            foreach (IUser u in userManager.Users)
            {
                if (u.UserName == tbUsername.Text)
                {
                    lblTaken.Visibility = Visibility.Visible;
                }
                else
                    lblTaken.Visibility = Visibility.Collapsed;
            }
        }
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
