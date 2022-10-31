using TravelPal.Enums;

namespace TravelPal.Travels
{
    public class Travel
    {
        public string Destination { get; set; }
        public Countries Country { get; set; }
        public int Travellers { get; set; }
        //public List<PackingListItem> PackingList { get; set; }
        //public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }
        public int TravelDays { get; set; }

        public Travel(string destination, Countries country, int travellers, int travelDays)
        {
            Destination = destination;
            Country = country;
            Travellers = travellers;
            TravelDays = travelDays;
            //PackingList = packingList;
            //StartDate = startDate;
            //EndDate = endDate;
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
