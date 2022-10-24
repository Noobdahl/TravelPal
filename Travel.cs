using System;
using TravelPal.Enums;

namespace TravelPal
{
    public class Travel
    {
        public string Destination { get; set; }
        public Countries Country { get; set; }
        public int Travellers { get; set; }
        //public List<PackingListItem> PackingList { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int TravelDays { get; set; }

        public Travel(string destination, Countries country, int travellers, DateTime startDate, DateTime endDate, int travelDays)
        {
            Destination = destination;
            Country = country;
            Travellers = travellers;
            //PackingList = packingList;
            StartDate = startDate;
            EndDate = endDate;
            TravelDays = travelDays;
        }
        private int CalculateTravelDays()
        {
            return -1;
        }
    }
}
