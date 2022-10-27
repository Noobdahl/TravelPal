using System.Windows;
using System.Windows.Controls;
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
        User currentUser;
        public TravelsWindow(UserManager uManager, TravelManager tManager)
        {
            InitializeComponent();
            userManager = uManager;
            travelManager = tManager;
            currentUser = (User)userManager.SignedInUser;
            lblWelcome.Content = $"Welcome {currentUser.UserName}!";
        }

        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUser_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lvTravels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnAddTravel_Click(object sender, RoutedEventArgs e)
        {
            AddTravelWindow addTravelWindow = new(travelManager, currentUser);
            addTravelWindow.Show();
            this.Hide();
        }

        private void btnDetailsTravel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRemoveTravel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSignOut_Click(object sender, RoutedEventArgs e)
        {
            ((MainWindow)Application.Current.MainWindow).Show();
            this.Close();
        }
        private void RefreshTravelList()
        {
            lvTravels.Items.Clear();
            foreach (Travel travel in currentUser.Travels)
            {
                ListViewItem newItem = new();
                newItem.Content = travel.Destination;
                newItem.Tag = travel;
                lvTravels.Items.Add(newItem);
            }
        }
        private void DisableButtons()
        {
            btnDetailsTravel.IsEnabled = false;
            btnRemoveTravel.IsEnabled = false;
        }
    }
}
