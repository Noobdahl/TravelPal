using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using TravelPal.Enums;
using TravelPal.Managers;
using TravelPal.Models;
using TravelPal.PackingList;
using TravelPal.Travels;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        UserManager userManager;
        TravelManager travelManager;
        public MainWindow()
        {
            InitializeComponent();
            userManager = new();
            travelManager = new();

            LoadPremadeAccounts();
        }

        //Create premade accounts - for convenience
        private void LoadPremadeAccounts()
        {
            Admin admin = new(travelManager, userManager);
            admin.IUser("admin", "password", Countries.Sweden);
            userManager.AddUser(admin);
            //Making a temporary list to add the staff to all Gandalfs travels
            List<IPackingListItem> fakeList = new();
            OtherItem staff = new("Gandalfs stav", 1);
            fakeList.Add(staff);
            //Adding Gandalf
            User gandalf = new();
            gandalf.IUser("Gandalf", "password", Countries.Sweden);
            userManager.AddUser(gandalf);
            Vacation gandalfVac = new("Helsinki", Countries.Finland, 1, 5, new DateTime(2022, 11, 1), new DateTime(2022, 11, 5), false, fakeList);
            gandalf.GetTravels().Add(gandalfVac);
            Trip gandalfTrip = new("Ullared", Countries.Sweden, 1, 4, new DateTime(2022, 11, 6), new DateTime(2022, 11, 9), TripTypes.Work, fakeList);
            gandalf.GetTravels().Add(gandalfTrip);
        }

        //Register button - opens register window
        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new(userManager);
            registerWindow.Show();
            this.Hide();
        }

        //Login button - tries to sign in with the usermanager
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (userManager.SignInUser(tbUsername.Text, tbPassword.Password))
            {
                tbPassword.Clear();
                TravelsWindow travelsWindow = new(userManager, travelManager);
                travelsWindow.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Username or password was incorrect.");
                tbPassword.Clear();
            }
        }

        //This adds the function of moving the window around by dragging anywhere
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}