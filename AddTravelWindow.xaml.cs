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
    /// Interaction logic for AddTravelWindow.xaml
    /// </summary>
    public partial class AddTravelWindow : Window
    {
        TravelManager travelManager;
        User currentUser;
        string TripReason = "";
        public AddTravelWindow(TravelManager tManager, User cUser)
        {
            InitializeComponent();
            travelManager = tManager;
            currentUser = cUser;
            FillComboBoxes();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string inputDestination = tbDestination.Text;
            string cbCountryTrimmed = cbCountry.Text.Replace(" ", "_");
            int inputTravellers = Convert.ToInt32(tbTravellers.Text);
            Countries inputCountry = (Countries)Enum.Parse(typeof(Countries), cbCountryTrimmed);

            if (TripReason == "Vacation")
            {
                bool isAllInclusive = (bool)chbxAllInclusive.IsChecked;
                Vacation vacation = new(inputDestination, inputCountry, inputTravellers, isAllInclusive);
                AddNClose(vacation);
            }
            else if (TripReason == "Trip")
            {
                TripTypes inputType = (TripTypes)cbTripType.SelectedItem;
                Trip trip = new(inputDestination, inputCountry, inputTravellers, inputType);
                AddNClose(trip);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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
        private void AddNClose(Travel travel)
        {
            currentUser.Travels.Add(travel);
            travelManager.AddTravel(travel);
            this.Close();
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
    }
}
