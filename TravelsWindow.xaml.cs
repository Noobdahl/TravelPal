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
            UserDetailsWindow userDetailsWindow = new(userManager, currentUser);
            userDetailsWindow.Owner = this;
            userDetailsWindow.Show();
            this.Hide();
        }

        private void lvTravels_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ChangeButtons(true);
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
            TravelDetailsWindow travelDetailsWindow = new(travelManager, currentUser, GetSelectedTravel());
            travelDetailsWindow.Owner = this;
            travelDetailsWindow.Show();
            this.Hide();
        }
        private Travel GetSelectedTravel()
        {
            return (Travel)((ListViewItem)lvTravels.SelectedItem).Tag;
        }

        private void btnRemoveTravel_Click(object sender, RoutedEventArgs e)
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
            ChangeButtons(false);
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
            ChangeButtons(false);
        }
        private void ChangeButtons(bool toggle)
        {
            btnDetailsTravel.IsEnabled = toggle;
            btnRemoveTravel.IsEnabled = toggle;
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
    }
}
