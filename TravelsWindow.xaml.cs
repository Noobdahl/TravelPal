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
        IUser currentUser;
        public TravelsWindow(UserManager uManager, TravelManager tManager)
        {
            InitializeComponent();
            userManager = uManager;
            travelManager = tManager;
            currentUser = userManager.SignedInUser;
            lblWelcome.Content = $"Welcome {currentUser.UserName}!";
            if (IsAdmin())
                btnAddTravel.IsEnabled = false;
            RefreshTravelList();
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
            currentUser.GetTravels().Remove(currentTravel);

            if (currentUser.GetType().Name == "User")
            {
                //travelManager.RemoveTravel(currentTravel);
            }
            else if (currentUser.GetType().Name == "Admin")
            {
                Admin admin = (Admin)currentUser;
                foreach (IUser user in admin.GetUsers())
                {
                    bool found = false;
                    foreach (Travel travel in user.GetTravels())
                    {
                        if (travel == currentTravel)
                            found = true;
                    }
                    if (found)
                        user.GetTravels().Remove(currentTravel);
                }
            }
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
            foreach (Travel travel in currentUser.GetTravels())
            {
                ListViewItem newItem = new();
                if (IsAdmin())
                    newItem.Content = travel.Country;
                else
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
        private bool IsAdmin()
        {
            if (currentUser.GetType().Name == "Admin")
                return true;
            return false;
        }
    }
}
