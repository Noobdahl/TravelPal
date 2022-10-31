using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
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
        IUser currentUser;
        string TripReason = "";
        int travelDays = 0;
        DateTime startDate;
        DateTime endDate;
        public AddTravelWindow(TravelManager tManager, IUser cUser)
        {
            InitializeComponent();
            travelManager = tManager;
            currentUser = cUser;
            FillComboBoxes();
            cldStart.SelectionMode = CalendarSelectionMode.MultipleRange;
            cldStart.DisplayDateStart = (DateTime.Today);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            string inputDestination = tbDestination.Text;
            int inputTravellers = Convert.ToInt32(tbTravellers.Text);
            Countries inputCountry = (Countries)Enum.Parse(typeof(Countries), cbCountry.Text.Replace(" ", "_"));

            if (TripReason == "Vacation")
            {
                Vacation vacation = new(inputDestination, inputCountry, inputTravellers, travelDays, startDate, endDate, (bool)chbxAllInclusive.IsChecked);
                AddNClose(vacation);
            }
            else if (TripReason == "Trip")
            {
                Trip trip = new(inputDestination, inputCountry, inputTravellers, travelDays, startDate, endDate, (TripTypes)cbTripType.SelectedItem);
                AddNClose(trip);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ReturnToTravelsWindow();
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
            currentUser.GetTravels().Add(travel);
            travelManager.AddTravel(travel);
            ((TravelsWindow)this.Owner).RefreshTravelList();
            ReturnToTravelsWindow();
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

        private void cldStart_SelectedDatesChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            travelDays = 0;
            foreach (DateTime mySelectedDate in cldStart.SelectedDates)
            {
                travelDays++;
            }
            lblDays.Content = travelDays;
            startDate = cldStart.SelectedDates[0];
            endDate = cldStart.SelectedDates[cldStart.SelectedDates.Count() - 1];
        }
        private void ReturnToTravelsWindow()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window.GetType().Name == "TravelsWindow")
                {
                    window.Show();
                }
            }
            this.Close();
        }
    }
}
