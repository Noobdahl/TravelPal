using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using TravelPal.Enums;
using TravelPal.Managers;
using TravelPal.Models;
using TravelPal.PackingList;
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

            //Filling comboboxes
            FillComboBoxes();

            //Filling in current info
            FillInInformation();

            //Sets up calendar
            cldStart.SelectionMode = CalendarSelectionMode.MultipleRange;
            cldStart.DisplayDateStart = (DateTime.Today);

            //Checks type of current travel, shows info accordingly
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

        //Cancel button - Calls method to return to travelswindow
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ReturnToTravelsWindow();
        }

        //Edit button - Changes UI, shows save button and enables editing in inputboxes
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

        //Save button - TRIES to save new info, catches exception based on some checks:
        //If amount of travellers has letters, destination too few leters, it throws error
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Int32.TryParse(tbTravellers.Text, out int fisk))
                    throw new Exception("Please enter travellers in digits only.");
                else if (tbDestination.Text.Count() <= 0)
                    throw new Exception("Please enter a destination.");

                //This info will always change, no matter what type of travel
                currentTravel.Country = (Countries)Enum.Parse(typeof(Countries), cbCountry.Text.Replace(" ", "_"));
                currentTravel.Travellers = Convert.ToInt32(tbTravellers.Text);
                currentTravel.Destination = tbDestination.Text;
                currentTravel.TravelDays = travelDays;
                currentTravel.StartDate = startDate;
                currentTravel.EndDate = endDate;

                //If travel is same type as before, just change info
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
                //But if a new type of travel is needed, Vacation -> Trip, or Trip -> Vacation
                else
                {
                    //If it was Vacation, now create Trip
                    if (currentTravel.GetType().Name == "Vacation")
                    {
                        Trip newTrip = new(currentTravel.Destination, currentTravel.Country, currentTravel.Travellers, 1, startDate, endDate, (TripTypes)cbTripType.SelectedItem, currentTravel.PackingList);
                        AddToLists(newTrip);
                    }
                    //If it was Trip, now create Vacation
                    else
                    {
                        Vacation newVacation = new(currentTravel.Destination, currentTravel.Country, currentTravel.Travellers, 1, startDate, endDate, (bool)chbxAllInclusive.IsChecked, currentTravel.PackingList);
                        AddToLists(newVacation);
                    }
                    //Removing old travel
                    RemoveCurrentTravelFromLists();

                }
                //Finally, refresh other windows list and return to travel window
                ((TravelsWindow)this.Owner).RefreshTravelList();
                ReturnToTravelsWindow();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went wrong, please check all inputs.\n\n ({ex.Message})");
            }



        }

        //This is called to add the travel to the lists in both user and travelmanager
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

        //This is called to remove the travel from the lists, if Admin also remove from the user
        private void RemoveCurrentTravelFromLists()
        {
            currentUser.GetTravels().Remove(currentTravel);
            if (currentUser.GetType().Name == "Admin")
            {
                Admin admin = (Admin)currentUser;
                admin.RemoveTravelFromUserList(currentTravel);
            }
        }

        //Fills all inputs and sets comboboxes to the information from current selected travel
        private void FillInInformation()
        {
            //Fills the listview with items from packinglist
            foreach (IPackingListItem item in currentTravel.PackingList)
            {
                lvPacklist.Items.Add(item.GetInfo());
            }
            tbDestination.Text = currentTravel.Destination;
            cbCountry.SelectedIndex = (int)currentTravel.Country;

            //Checks type of travel to change UI and fill in the right boxes
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
            travelDays = currentTravel.TravelDays;
            lblStartDate.Content = "Starting date: " + currentTravel.StartDate.ToString("dd/MM/yyyy");
            lblEndDate.Content = "Ending date: " + currentTravel.EndDate.ToString("dd/MM/yyyy");
        }

        //Fills three comboboxes, two of them from enums Countries and TripTypes
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

        //Combobox Tripreason changes - changes UI, shows checkboxes etc accordingly. Also sets "TripReason", which later helps creating the correct travel
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

        //Finds the hidden TravelsWindow and shows it, then closes window
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

        //Calendar change dates - runs when changing dates in calendar to update amount of days selected, and to find first and last day
        private void cldStart_SelectedDatesChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            travelDays = cldStart.SelectedDates.Count();
            startDate = cldStart.SelectedDates[0];
            endDate = cldStart.SelectedDates[cldStart.SelectedDates.Count() - 1];
            //Setting labels after each selection, to show current start and end dates in real time
            SetDateLabels();
            //This is to avoid getting the mouse selection "stuck" in calendar after using it
            Mouse.Capture(null);
        }

        //This changes labels of the start and end date, runs from calendar selected dates method
        private void SetDateLabels()
        {
            lblStartDate.Content = "Starting date: " + startDate.ToString("dd/MM/yyyy");
            lblEndDate.Content = "Ending date: " + endDate.ToString("dd/MM/yyyy");
        }

        //This adds the function of moving the window around by dragging anywhere
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }
    }
}
