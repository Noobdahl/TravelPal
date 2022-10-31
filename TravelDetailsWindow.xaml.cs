using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows;
using TravelPal.Enums;
using TravelPal.Managers;
using TravelPal.Models;
using TravelPal.Travels;

namespace TravelPal
{
    /// <summary>
    /// Interaction logic for TravelDetailsWindow.xaml
    /// </summary>
    public partial class TravelDetailsWindow : Window
    {
        TravelManager travelManager;
        IUser currentUser;
        Travel travel;
        string TripReason = "";
        public TravelDetailsWindow(TravelManager tManager, IUser user, Travel cTravel)
        {
            InitializeComponent();
            travelManager = tManager;
            currentUser = user;
            travel = cTravel;
            FillComboBoxes();
            FillInInformation();
            if (travel.GetType().Name == "Trip")
            {
                lblTripType.Content = "Trip type";
                cbTripType.Visibility = Visibility.Visible;
                chbxAllInclusive.Visibility = Visibility.Hidden;
            }
            else
            {
                lblTripType.Content = "All inclusive";
                chbxAllInclusive.Visibility = Visibility.Visible;
                cbTripType.Visibility = Visibility.Hidden;
            }

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            tbDestination.IsEnabled = true;
            cbCountry.IsEnabled = true;
            tbTravellers.IsEnabled = true;
            cbTripReason.IsEnabled = true;
            cbTripType.IsEnabled = true;
            btnEdit.Visibility = Visibility.Hidden;
            btnSave.Visibility = Visibility.Visible;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }
        private void FillInInformation()
        {
            tbDestination.Text = travel.Destination;

        }
        private void FillComboBoxes()
        {
            cbTripReason.Items.Add("Vacation");
            cbTripReason.Items.Add("Trip");
            foreach (Countries country in Enum.GetValues(typeof(Countries)))
            {
                string displayName;
                displayName = country.GetType().GetMember(country.ToString()).FirstOrDefault().GetCustomAttribute<DisplayAttribute>()?.GetName();
                if (String.IsNullOrEmpty(displayName))
                {
                    displayName = country.ToString();
                }
                cbCountry.Items.Add(displayName);
            }
            foreach (TripTypes tripType in Enum.GetValues(typeof(TripTypes)))
            {
                cbTripType.Items.Add(tripType);
            }
        }

        private void cbTripReason_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbTripReason.SelectedItem != null)
            {
                if (cbTripReason.SelectedItem.ToString() == "Vacation")
                {
                    lblTripType.Content = "All Inclusive";
                    chbxAllInclusive.Visibility = Visibility.Visible;
                    cbTripType.Visibility = Visibility.Hidden;
                    TripReason = "Vacation";
                }
                else if (cbTripReason.SelectedItem.ToString() == "Trip")
                {
                    lblTripType.Content = "Trip type";
                    cbTripType.Visibility = Visibility.Visible;
                    chbxAllInclusive.Visibility = Visibility.Hidden;
                    TripReason = "Trip";
                }
            }
        }
    }
}
