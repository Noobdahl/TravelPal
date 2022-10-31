using TravelPal.Enums;

namespace TravelPal.Travels
{
    public class Trip : Travel
    {
        public TripTypes Type { get; set; }
        public Trip(string destination, Countries country, int travellers, int travelDays,
            // DateTime startDate, DateTime endDate, 
            TripTypes type
            ) : base(destination, country, travellers, travelDays
                //, startDate, endDate, travelDays
                )
        {
            Type = type;
        }
        public string GetInfo()
        {
            return $"";
        }
        public override void SetTripType(TripTypes type)
        {
            Type = type;
        }
    }
}
