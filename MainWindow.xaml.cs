using System;
using System.Windows;
using System.Windows.Input;
using TravelPal.Enums;
using TravelPal.Managers;
using TravelPal.Models;
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
            Admin admin = new(travelManager, userManager);
            admin.IUser("admin", "password", Countries.Sweden);
            userManager.AddUser(admin);

            //User gandalf = new();
            //gandalf.IUser("Gandalf", "password", Countries.Sweden);
            //userManager.AddUser(gandalf);
            //Vacation gandalfVac = new("Helsinki", Countries.Finland, 1, 1, false);
            //gandalf.GetTravels().Add(gandalfVac);
            //Trip gandalfTrip = new("Ullared", Countries.Sweden, 1, 1, TripTypes.Work);
            //gandalf.GetTravels().Add(gandalfTrip);


            User user2 = new();
            user2.IUser("qwe", "asd", Countries.Sweden);
            userManager.AddUser(user2);

            Vacation standard = new("Bofors", Countries.Sweden, 1, 5, new DateTime(2022, 11, 1), new DateTime(2022, 11, 5), true);
            user2.GetTravels().Add(standard);
            Vacation gandalfVac = new("Helsinki", Countries.Finland, 1, 3, new DateTime(2022, 11, 1), new DateTime(2022, 11, 3), false);
            user2.GetTravels().Add(gandalfVac);
            Trip gandalfTrip = new("Ullared", Countries.Sweden, 1, 2, new DateTime(2022, 11, 1), new DateTime(2022, 11, 2), TripTypes.Work);
            user2.GetTravels().Add(gandalfTrip);
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
