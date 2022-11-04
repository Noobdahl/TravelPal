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

        //Prefills the disabled inputs with current info
        private void FillInInfo()
        {
            tbUsername.Text = currentUser.UserName;
            cbCountry.SelectedIndex = (int)currentUser.Location;
        }

        //Populates the country combobox with strings made from enum Countries
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

        //Cancel button - Runs method to return to travelswindow
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ReturnToTravelsWindow();
        }

        //Save button - Checks if criteria with username & password are met, then uses usermanager to update and validate info
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (tbUsername.Text.Length < 3)
                MessageBox.Show("Username must be atleast 3 characters.");
            else if (pbNewPassword.Password.Length < 5)
                MessageBox.Show("Password must be atleast 5 characters.");
            else if (pbNewPassword.Password != pbConfirmPassword.Password)
                MessageBox.Show("Passwords did not match.");
            else
            {
                //UpdateUsername returns true if everything is valid and then also changes users location
                if (userManager.UpdateUsername(currentUser, tbUsername.Text))
                {
                    currentUser.Location = (Countries)Enum.Parse(typeof(Countries), cbCountry.Text.Replace(" ", "_"));
                    ReturnToTravelsWindow();
                }
                else
                    MessageBox.Show("This username is already taken.");
            }
            //Clears password boxes - for convenience and privacy
            pbNewPassword.Clear();
            pbConfirmPassword.Clear();
        }

        //Edit button - Changes UI, shows save button and passwordboxes
        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            btnEdit.Visibility = Visibility.Collapsed;
            btnSave.Visibility = Visibility.Visible;
            lblNewPassword.Visibility = Visibility.Visible;
            pbNewPassword.Visibility = Visibility.Visible;
            lblConfirmPassword.Visibility = Visibility.Visible;
            pbConfirmPassword.Visibility = Visibility.Visible;
            tbUsername.IsEnabled = true;
            cbCountry.IsEnabled = true;
            btnEdit.IsDefault = false;
            btnSave.IsDefault = true;
        }

        //Finds the hidden TravelsWindow, shows it and runs a method (RefreshUser) to refresh userdetail in travelswindow
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

        //This adds the function of moving the window around by dragging anywhere
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        //This runs when text changes in textblock username. Gets all users and compare usernames to users input, allows to change to users own name!
        private void tbUsername_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            bool foundUser = false;
            foreach (IUser u in userManager.Users)
            {
                if (u.UserName == tbUsername.Text && u.UserName != currentUser.UserName)
                    foundUser = true;
            }
            if (foundUser)
                lblTaken.Visibility = Visibility.Visible;
            else
                lblTaken.Visibility = Visibility.Collapsed;
        }
    }
}
