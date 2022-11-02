﻿using System;
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
            if (IsAdmin())
                btnAddTravel.Visibility = Visibility.Hidden;
            RefreshTravelList();
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Clicka på saker", "HowTo TravelPal", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnUser_Click(object sender, RoutedEventArgs e)
        {
            UserDetailsWindow userDetailsWindow = new(userManager, currentUser);
            userDetailsWindow.Owner = this;
            userDetailsWindow.Show();
            this.Hide();
        }

        private void btnAddTravel_Click(object sender, RoutedEventArgs e)
        {
            //AddTravelWindow addTravelWindow = new(travelManager, currentUser);
            //addTravelWindow.Show();

            AddTravelWindow addTravelWindowTest = new(travelManager, currentUser);
            addTravelWindowTest.Owner = this;
            addTravelWindowTest.Show();
            this.Hide();
        }

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

        private Travel GetSelectedTravel()
        {
            if (lvTravels.SelectedItem == null)
            {
                throw new Exception("Please select a travel in the list.");
            }

            return (Travel)((ListViewItem)lvTravels.SelectedItem).Tag;
        }

        private void btnRemoveTravel_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Travel currentTravel = GetSelectedTravel();
                //Users tar bort från egen lista här, och admin tar bort från temporära travelmanagers lista
                currentUser.GetTravels().Remove(GetSelectedTravel());

                //om admin, ta bort från userns lista med
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

        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Show();
            this.Close();
        }
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
        private bool IsAdmin()
        {
            if (currentUser.GetType().Name == "Admin")
                return true;
            return false;
        }
        public void RefreshUser()
        {
            lblWelcome.Content = $"Welcome {currentUser.UserName}!";
        }
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
