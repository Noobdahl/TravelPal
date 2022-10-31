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
    /// Interaction logic for TravelDetailsWindow.xaml
    /// </summary>
    public partial class TravelDetailsWindow : Window
    {
        TravelManager travelManager;
        IUser currentUser;
        Travel currentTravel;
        string TripReason = "";
        int travelDays = 0;
        DateTime startDate;
        DateTime endDate;
        public TravelDetailsWindow(TravelManager tManager, IUser user, Travel cTravel)
        {
            InitializeComponent();
            travelManager = tManager;
            currentUser = user;
            currentTravel = cTravel;
            FillComboBoxes();
            FillInInformation();
            cldStart.SelectionMode = CalendarSelectionMode.MultipleRange;
            cldStart.DisplayDateStart = (DateTime.Today);
            if (currentTravel.GetType().Name == "Trip")
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
            ReturnToTravelsWindow();
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
            chbxAllInclusive.IsEnabled = true;
            cldStart.Visibility = Visibility.Visible;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            currentTravel.Destination = tbDestination.Text;
            currentTravel.Country = (Countries)Enum.Parse(typeof(Countries), cbCountry.Text.Replace(" ", "_"));
            currentTravel.Travellers = Convert.ToInt32(tbTravellers.Text);
            currentTravel.TravelDays = travelDays;
            currentTravel.StartDate = startDate;
            currentTravel.EndDate = endDate;

            //If travel is same type, just change it
            if (currentTravel.GetType().Name == cbTripReason.SelectedItem.ToString())
            {
                if (currentTravel.GetType().Name == "Vacation")
                {
                    currentTravel.SetAllInclusive((bool)chbxAllInclusive.IsChecked);
                }
                else
                {
                    currentTravel.SetTripType((TripTypes)cbTripType.SelectedItem);
                }
            }
            else
            {
                //If it was Vacation, now create Trip
                if (currentTravel.GetType().Name == "Vacation")
                {
                    Trip newTrip = new(currentTravel.Destination, currentTravel.Country, currentTravel.Travellers, 1, startDate, endDate, (TripTypes)cbTripType.SelectedItem);
                    AddToLists(newTrip);
                }
                //If it was Trip, now create Vacation
                else
                {
                    Vacation newVacation = new(currentTravel.Destination, currentTravel.Country, currentTravel.Travellers, 1, startDate, endDate, (bool)chbxAllInclusive.IsChecked);
                    AddToLists(newVacation);
                }
                //Removing old travel
                RemoveCurrentTravelFromLists();

            }

            ((TravelsWindow)this.Owner).RefreshTravelList();
            ReturnToTravelsWindow();

        }
        private void AddToLists(Travel travel)
        {
            currentUser.GetTravels().Add(travel);
            travelManager.AddTravel(travel);
            if (currentUser.GetType().Name == "Admin")
            {
                Admin admin = (Admin)currentUser;
                admin.ReplaceTravelAtUserList(currentTravel, travel);
            }
        }
        private void RemoveCurrentTravelFromLists()
        {
            currentUser.GetTravels().Remove(currentTravel);
            if (currentUser.GetType().Name == "Admin")
            {
                Admin admin = (Admin)currentUser;
                admin.RemoveTravelFromUserList(currentTravel);
            }
        }
        private void FillInInformation()
        {
            tbDestination.Text = currentTravel.Destination;
            cbCountry.SelectedIndex = (int)currentTravel.Country;
            if (currentTravel.GetType().Name == "Vacation")
            {
                cbTripReason.SelectedIndex = 0;
                Vacation vac = (Vacation)currentTravel;
                chbxAllInclusive.IsChecked = vac.IsAllInclusive;
            }
            else
            {
                cbTripReason.SelectedIndex = 1;
                Trip trip = (Trip)currentTravel;
                cbTripType.SelectedIndex = (int)trip.Type;
            }
            tbTravellers.Text = currentTravel.Travellers.ToString();
            lblDays.Content = currentTravel.TravelDays;
            travelDays = currentTravel.TravelDays;
            lblStartDate.Content = currentTravel.StartDate;
            lblEndDate.Content = currentTravel.EndDate;
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
    }
}
