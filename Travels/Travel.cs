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
        private int CalculateTravelDays()
        {
            return -1;
        }
        public virtual void SetAllInclusive(bool isAllInclusive)
        {
        }
        public virtual void SetTripType(TripTypes type)
        {

        }
    }
}
