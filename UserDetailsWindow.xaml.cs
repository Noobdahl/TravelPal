using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows;
using TravelPal.Enums;
using TravelPal.Managers;
using TravelPal.Models;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for UserDetailsWindow.xaml
    /// </summary>
    public partial class UserDetailsWindow : Window
    {
        UserManager userManager;
        IUser currentUser;
        public UserDetailsWindow(UserManager uManager, IUser user)
        {
            InitializeComponent();
            userManager = uManager;
            currentUser = user;
            FillComboBox();
            FillInInfo();
        }

        private void FillInInfo()
        {
            tbUsername.Text = currentUser.UserName;
            cbCountry.SelectedIndex = (int)currentUser.Location;
        }

        private void FillComboBox()
        {
            foreach (Countries country in Enum.GetValues(typeof(Countries)))
            {
                string displayName;
                displayName = country.GetType().GetMember(country.ToString()).FirstOrDefault().GetCustomAttribute<DisplayAttribute>()?.GetName();
                if (String.IsNullOrEmpty(displayName))
                {
                    displayName = country.ToString();
                }
                cbCountry.Items.Add(displayName);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ReturnToTravelsWindow();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (tbUsername.Text.Length < 3)
                MessageBox.Show("Username must be atleast 3 characters.");
            else if (pbNewPassword.Password.Length < 5)
                MessageBox.Show("Password must be atleast 5 characters.");
            else if (pbNewPassword.Password != pbConfirmPasssword.Password)
                MessageBox.Show("Passwords did not match.");
            else
            {
                if (userManager.UpdateUsername(currentUser, tbUsername.Text))
                {
                    MessageBox.Show("User update successful!");
                    ReturnToTravelsWindow();
                }
                else
                    MessageBox.Show("This username is already taken.");
            }
            pbNewPassword.Clear();
            pbConfirmPasssword.Clear();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            btnEdit.Visibility = Visibility.Collapsed;
            btnSave.Visibility = Visibility.Visible;
            lblNewPassword.Visibility = Visibility.Visible;
            pbNewPassword.Visibility = Visibility.Visible;
            lblConfirmPassword.Visibility = Visibility.Visible;
            pbConfirmPasssword.Visibility = Visibility.Visible;
            tbUsername.IsEnabled = true;
            cbCountry.IsEnabled = true;
        }
        private void ReturnToTravelsWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType().Name == "TravelsWindow")
                {
                    window.Show();
                }
            }
            ((TravelsWindow)this.Owner).RefreshUser();
            this.Close();
        }
    }
}
