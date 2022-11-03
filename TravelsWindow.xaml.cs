using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TravelPal.Managers;
using TravelPal.Models;
using TravelPal.Travels;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for TravelsWindow.xaml
    /// </summary>
    public partial class TravelsWindow : Window
    {
        UserManager userManager;
        TravelManager travelManager;
        IUser currentUser;
        public TravelsWindow(UserManager uManager, TravelManager tManager)
        {
            InitializeComponent();
            userManager = uManager;
            travelManager = tManager;
            currentUser = userManager.SignedInUser;
            lblWelcome.Content = $"Welcome {currentUser.UserName}!";
            //Uses method that returns true if user is admin, then removes Add Travel button (admins cannot add travels)
            if (IsAdmin())
                btnAddTravel.Visibility = Visibility.Hidden;
            RefreshTravelList();
        }

        //Help button - Shows box with instructions: HowTo
        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("" +
                "Welcome to TravelPal!\n" +
                "Remember all the travels you planned and dreamed of that never happened? " +
                "Now theres a place to collect them, right here in TravelPal! \n" +
                "Enter your worktrips and dream vacations here, and let them be remembered forever " +
                "as a planned travel.\n" +
                "\n" +
                "Functions in TravelPal:\n" +
                "Add Travel - Fill up your account with planned travels.\n\n" +
                "Remove Travel - Changed your mind? Just select travel to delete and *poof*!\n\n" +
                "Details - Lunge yourself deep into the detailed information about any of your planned travels.\n\n" +
                "User - This button shows your information and gives you the option to change username, password and country of origin.\n\n" +
                "Sign Out - Don't forget to sign out before leaving the computer,\n" +
                "your wouldn't want anyone else to read about your travels, right?", "HowTo TravelPal", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //User button - Opens user details window, sets owner to access methods in this window from detailswindow
        private void btnUser_Click(object sender, RoutedEventArgs e)
        {
            UserDetailsWindow userDetailsWindow = new(userManager, currentUser);
            userDetailsWindow.Owner = this;
            userDetailsWindow.Show();
            this.Hide();
        }

        //Add Travel button - Opens add travel window, sets owner to access methods in this window from add travel window
        private void btnAddTravel_Click(object sender, RoutedEventArgs e)
        {
            AddTravelWindow addTravelWindowTest = new(travelManager, currentUser);
            addTravelWindowTest.Owner = this;
            addTravelWindowTest.Show();
            this.Hide();
        }

        //Details button - TRIES to open details window, catches error if no travel is selected, also sets owner to access methods in this window from details window
        private void btnDetailsTravel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TravelDetailsWindow travelDetailsWindow = new(travelManager, currentUser, GetSelectedTravel());
                travelDetailsWindow.Owner = this;
                travelDetailsWindow.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //Returns selected item in list as a travel, throws exception if no selection is made
        private Travel GetSelectedTravel()
        {
            if (lvTravels.SelectedItem == null)
            {
                throw new Exception("Please select a travel in the list.");
            }

            return (Travel)((ListViewItem)lvTravels.SelectedItem).Tag;
        }

        //Remove travel button - TRIES to remove travel, catches error i no travel is selected. 
        private void btnRemoveTravel_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Travel currentTravel = GetSelectedTravel();
                //Users removes from the users list, and admin removes from the list in the travelmanager.
                //(Runs on "same" method, but acts different because they are IUsers)
                currentUser.GetTravels().Remove(GetSelectedTravel());

                //If user is admin, calls method that removes travel from the owner (real user)
                if (currentUser.GetType().Name == "Admin")
                {
                    Admin admin = (Admin)currentUser;
                    admin.RemoveTravelFromUserList(currentTravel);
                }
                RefreshTravelList();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        //Sign Out button - Shows Mainwindow and closes current window
        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Show();
            this.Close();
        }

        //Clears travel-listview and repopulates it with the IUsers GetTravels(), acts different depending on if you are user or admin
        public void RefreshTravelList()
        {
            lvTravels.Items.Clear();
            foreach (Travel travel in currentUser.GetTravels())
            {
                ListViewItem newItem = new();
                if (travel.TravelDays == 1)
                    newItem.Content = $"{travel.Country} for {travel.TravelDays} day";
                else
                    newItem.Content = $"{travel.Country} for {travel.TravelDays} days";
                newItem.Tag = travel;
                lvTravels.Items.Add(newItem);
            }
        }

        //Returns true if current user is Admin
        private bool IsAdmin()
        {
            if (currentUser.GetType().Name == "Admin")
                return true;
            return false;
        }

        //Updates welcome label with updated Username
        public void RefreshUser()
        {
            lblWelcome.Content = $"Welcome {currentUser.UserName}!";
        }

        //This adds the function of moving the window around by dragging anywhere
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
