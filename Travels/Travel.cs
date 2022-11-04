using System;
using System.Collections.Generic;
using TravelPal.Enums;
using TravelPal.PackingList;

namespace TravelPal.Travels
{
    public class Travel
    {
        public string Destination { get; set; }
        public Countries Country { get; set; }
        public int Travellers { get; set; }
        public List<IPackingListItem> PackingList { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TravelDays { get; set; }

        public Travel(string destination, Countries country, int travellers, int travelDays, DateTime startDate, DateTime endDate, List<IPackingListItem> packingList)
        {
            Destination = destination;
            Country = country;
            Travellers = travellers;
            TravelDays = travelDays;
            StartDate = startDate;
            EndDate = endDate;
            PackingList = packingList;
        }

        public virtual string GetInfo()
        {
            return "";
        }
        //Not used method - uses calendar function to measure how many days a travel is
        private int CalculateTravelDays()
        {
            return -1;
        }

        //Two virtual methods that is overrided in both subclasses Trip and Vacation
        public virtual void SetAllInclusive(bool isAllInclusive)
        {
        }
        public virtual void SetTripType(TripTypes type)
        {

        }
    }
}
