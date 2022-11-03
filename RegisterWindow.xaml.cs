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
            FillComboBox();
        }

        //Fills combobox Country with enums from Countries
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
                cbCountries.Items.Add(displayName);
            }
        }

        //Button Create - TRIES to create a user, throws exception based on problem. Username lengt, password length or mismatch in passwordboxes
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (tbUserName.Text.Length < 3)
                    MessageBox.Show("Username must be atleast 3 characters.");
                else if (pbPassword.Password.Length < 5)
                    MessageBox.Show("Password must be atleast 5 characters.");
                else if (pbPassword.Password != pbConfirmPassword.Password)
                    MessageBox.Show("Passwords did not match.");
                else
                {
                    User user = new();
                    string countryString = cbCountries.Text.Replace(" ", "_");
                    user.IUser(tbUserName.Text, pbPassword.Password, (Countries)Enum.Parse(typeof(Countries), countryString));
                    //Uses usermanagers method to add user, returns true if able to add
                    if (userManager.AddUser(user))
                    {
                        MessageBox.Show("Register completed!");
                        ((MainWindow)Application.Current.MainWindow).Show();
                        this.Close();
                    }
                    //returns false if unable to add
                    else
                    {
                        MessageBox.Show("That username is already taken.");
                        pbPassword.Clear();
                        pbConfirmPassword.Clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong! \n\n" + ex.Message);
            }
            pbPassword.Clear();
            pbConfirmPassword.Clear();
        }

        //Enables create button when changing selection in combobox country
        private void cbCountries_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            btnCreate.IsEnabled = true;
        }

        //This runs when text changes in textblock username. Gets all users and compare usernames to users input
        private void tbUsername_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            bool foundUser = false;
            foreach (IUser u in userManager.Users)
            {
                if (u.UserName == tbUserName.Text)
                    foundUser = true;
            }
            //If username is already found, shows warning text
            if (foundUser)
                lblTaken.Visibility = Visibility.Visible;
            else
                lblTaken.Visibility = Visibility.Collapsed;

        }

        //This adds the function of moving the window around by dragging anywhere
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        //Cancel button - Finds main window and returns to it
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType().Name == "MainWindow")
                {
                    window.Show();
                }
            }
            this.Close();
        }
    }
}
