using System;
using System.Collections.Generic;
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
        //Pass is created here for easy access, but not added to list until first interaction with combobox
        TravelDocument pass = new("Passport", true);
        public AddTravelWindow(TravelManager tManager, IUser cUser)
        {
            InitializeComponent();
            travelManager = tManager;
            currentUser = cUser;
            FillComboBoxes();
            cldStart.SelectionMode = CalendarSelectionMode.MultipleRange;
            cldStart.DisplayDateStart = (DateTime.Today);
            cldStart.SelectedDate = (DateTime.Today);
        }

        //Save button - TRIES to create new travel, catches exception based on some checks:
        //If amount of travellers has letters, destination too few leters, it throws error
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!Int32.TryParse(tbTravellers.Text, out int fisk))
                    throw new Exception("Please enter travellers in digits only.");
                else if (tbDestination.Text.Count() <= 0)
                    throw new Exception("Please enter a destination.");
                //Create new list from method, gets a list of IPackingListItems returned from the listview
                List<IPackingListItem> newList = CreateList();

                string inputDestination = tbDestination.Text;
                int inputTravellers = Convert.ToInt32(tbTravellers.Text);
                Countries inputCountry = (Countries)Enum.Parse(typeof(Countries), cbCountry.Text.Replace(" ", "_"));

                //Checks what travel to create
                if (TripReason == "Vacation")
                {
                    Vacation vacation = new(inputDestination, inputCountry, inputTravellers, travelDays, startDate, endDate, (bool)chbxAllInclusive.IsChecked, newList);
                    AddNClose(vacation);
                }
                else if (TripReason == "Trip")
                {
                    Trip trip = new(inputDestination, inputCountry, inputTravellers, travelDays, startDate, endDate, (TripTypes)cbTripType.SelectedItem, newList);
                    AddNClose(trip);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went wrong, please check all inputs.\n\n ({ex.Message})");
            }
        }

        //Called from save method, creates a new list from the items added to the listview and returns it
        private List<IPackingListItem> CreateList()
        {
            List<IPackingListItem> nyLista = new();
            foreach (ListViewItem item in lvInventory.Items)
            {
                nyLista.Add((IPackingListItem)item.Tag);
            }
            return nyLista;
        }

        //Cancel button - Runs method to return to travelswindow
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            ReturnToTravelsWindow();
            this.Close();
        }

        //Combobox Tripreason changes - changes UI, shows checkboxes etc accordingly. Also sets "TripReason", which later helps creating the correct travel
        private void cbTripReason_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbTripReason.SelectedItem != null)
            {
                if (cbTripReason.SelectedItem.ToString() == "Vacation")
                {
                    lblTripType.Content = "All Inclusive:";
                    chbxAllInclusive.Visibility = Visibility.Visible;
                    cbTripType.Visibility = Visibility.Hidden;
                    TripReason = "Vacation";
                }
                else if (cbTripReason.SelectedItem.ToString() == "Trip")
                {
                    lblTripType.Content = "Trip type:";
                    cbTripType.Visibility = Visibility.Visible;
                    chbxAllInclusive.Visibility = Visibility.Hidden;
                    TripReason = "Trip";
                }
            }
        }

        //This is run from save method, adds the travel into the users list and the travelmanagers list.
        //Then it runs a method in the ownerwindow (travelswindow) to refresh the listview there with the new travel added.
        //Finally, closes this window
        private void AddNClose(Travel travel)
        {
            currentUser.GetTravels().Add(travel);
            travelManager.AddTravel(travel);
            ((TravelsWindow)this.Owner).RefreshTravelList();
            ReturnToTravelsWindow();
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

        //Calendar change dates - runs when changing dates in calendar to update amount of days selected, and to find first and last day
        private void cldStart_SelectedDatesChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            travelDays = cldStart.SelectedDates.Count();
            startDate = cldStart.SelectedDates[0];
            endDate = cldStart.SelectedDates[cldStart.SelectedDates.Count() - 1];
            Mouse.Capture(null);
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

        //This adds the function of moving the window around by dragging anywhere
        private void Window_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        //Add packinglist item - TRIES to create new item from user inputs, throws exception if it fails due to wrong input
        private void btnAddItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (tbInput.Text.Count() <= 0)
                    throw new Exception("New Item input is empty.");
                string input = tbInput.Text;
                if ((bool)chbxDocument.IsChecked)
                {
                    //Create traveldocument
                    bool isRequired = (bool)chbxRequired.IsChecked;
                    TravelDocument travelDocument = new(input, isRequired);
                    AddToListView(travelDocument);
                }
                else
                {
                    //Create otheritem
                    if (Int32.TryParse(tbQuantity.Text, out int result))
                    {
                        OtherItem otherItem = new(input, result);
                        AddToListView(otherItem);
                    }
                    else
                        throw new Exception("Please enter Quantity with number");

                }

                //Resets UI
                tbQuantity.Clear();
                tbInput.Clear();
                lblRequired.Visibility = Visibility.Hidden;
                chbxRequired.Visibility = Visibility.Hidden;
                lblQuantity.Visibility = Visibility.Visible;
                tbQuantity.Visibility = Visibility.Visible;
                chbxDocument.IsChecked = false;
                chbxRequired.IsChecked = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong!\n\n" + ex.Message);
            }
        }

        //Checkbox document clicks runs this, changes UI based on if checked or not
        private void chbxDocument_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)chbxDocument.IsChecked)
            {
                lblRequired.Visibility = Visibility.Visible;
                chbxRequired.Visibility = Visibility.Visible;
                lblQuantity.Visibility = Visibility.Hidden;
                tbQuantity.Visibility = Visibility.Hidden;
            }
            else
            {
                lblRequired.Visibility = Visibility.Hidden;
                chbxRequired.Visibility = Visibility.Hidden;
                lblQuantity.Visibility = Visibility.Visible;
                tbQuantity.Visibility = Visibility.Visible;
            }
        }

        //Converts recieved item to a listviewitem and adds it to the packingitem-listview
        private void AddToListView(IPackingListItem newItem)
        {
            ListViewItem newListViewItem = new();
            newListViewItem.Content = newItem.GetInfo();
            newListViewItem.Tag = newItem;

            lvInventory.Items.Add(newListViewItem);
        }

        //Returns true if recieved enum Countries is also in enum EuropeanCountries (if country is in europe)
        private bool IsCountryInEU(Countries checkC)
        {
            foreach (EuropeanCountries c in Enum.GetValues(typeof(EuropeanCountries)))
            {
                if (checkC.ToString() == c.ToString())
                {
                    return true;
                }
            }
            return false;
        }

        //This runs when mouse leaves combobox capture.
        //It TRIES to check if user country is in europe and if destination-country is in europe, and runs RefreshPass-method with true/false
        private void cbCountry_LostMouseCapture(object sender, MouseEventArgs e)
        {
            try
            {
                Countries userCountry = currentUser.Location;
                Countries inputCountry = (Countries)Enum.Parse(typeof(Countries), cbCountry.Text.Replace(" ", "_"));

                if (IsCountryInEU(userCountry))
                {
                    if (IsCountryInEU(inputCountry))
                        RefreshPassStatus(false);
                    else
                        RefreshPassStatus(true);
                }
                else
                    RefreshPassStatus(true);
            }
            catch
            {
            }
        }

        //Finds the passport in the listview
        //If its not found, adds new passport with recieved bool
        //If its found, update the items content with info from updated pass object
        private void RefreshPassStatus(bool status)
        {
            pass.IsRequired = status;
            foreach (ListViewItem item in lvInventory.Items)
            {
                if (item.Tag == pass)
                {
                    item.Content = pass.GetInfo();
                    return;
                }
            }
            AddToListView(pass);
        }
    }
}
