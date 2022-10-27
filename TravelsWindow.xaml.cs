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
            ChangeButtons(true);
        }

        private void btnAddTravel_Click(object sender, RoutedEventArgs e)
        {
            AddTravelWindow addTravelWindow = new(travelManager, currentUser);
            addTravelWindow.Show();
        }

        private void btnDetailsTravel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnRemoveTravel_Click(object sender, RoutedEventArgs e)
        {
            ListViewItem currentSelection = new();
            currentSelection = (ListViewItem)lvTravels.SelectedItem;
            Travel currentTravel = (Travel)currentSelection.Tag;
            //ta bort från user och manager
            currentUser.Travels.Remove(currentTravel);
            travelManager.RemoveTravel(currentTravel);
            RefreshTravelList();
            ChangeButtons(false);
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
                newItem.Content = travel.Country;
                newItem.Tag = travel;
                lvTravels.Items.Add(newItem);
            }
        }
        private void ChangeButtons(bool toggle)
        {
            btnDetailsTravel.IsEnabled = toggle;
            btnRemoveTravel.IsEnabled = toggle;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            RefreshTravelList();
        }
    }
}
