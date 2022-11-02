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
            List<IPackingListItem> fakeList = new();
            OtherItem staff = new("Gandalfs stav", 1);
            fakeList.Add(staff);

            Admin admin = new(travelManager, userManager);
            admin.IUser("admin", "password", Countries.Sweden);
            userManager.AddUser(admin);

            User gandalf = new();
            gandalf.IUser("Gandalf", "password", Countries.Sweden);
            userManager.AddUser(gandalf);
            Vacation gandalfVac = new("Helsinki", Countries.Finland, 1, 1, new DateTime(2022, 11, 1), new DateTime(2022, 11, 5), false, fakeList);
            gandalf.GetTravels().Add(gandalfVac);
            Trip gandalfTrip = new("Ullared", Countries.Sweden, 1, 1, new DateTime(2022, 11, 1), new DateTime(2022, 11, 5), TripTypes.Work, fakeList);
            gandalf.GetTravels().Add(gandalfTrip);


            User qwe = new();
            qwe.IUser("qwe", "asd", Countries.Sweden);
            userManager.AddUser(qwe);
            Vacation standard = new("Bofors", Countries.Sweden, 1, 5, new DateTime(2022, 11, 1), new DateTime(2022, 11, 5), true, fakeList);
            qwe.GetTravels().Add(standard);
            Vacation qwevac = new("Helsinki", Countries.Finland, 1, 3, new DateTime(2022, 11, 1), new DateTime(2022, 11, 3), false, fakeList);
            qwe.GetTravels().Add(qwevac);
            Trip qwetrip = new("Ullared", Countries.Sweden, 1, 2, new DateTime(2022, 11, 1), new DateTime(2022, 11, 2), TripTypes.Work, fakeList);
            qwe.GetTravels().Add(qwetrip);
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow registerWindow = new(userManager);
            registerWindow.Show();
            this.Hide();
        }

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

        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
