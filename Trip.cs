using System;
using TravelPal.Enums;

namespace TravelPal
{
    public class Trip : Travel
    {
        public TripTypes Type { get; set; }
        public Trip(string destination, Countries country, int travellers, DateTime startDate, DateTime endDate, int travelDays, TripTypes type) : base(destination, country, travellers, startDate, endDate, travelDays)
        {
            Type = type;
        }
        public string GetInfo()
        {
            return $"";
        }
    }
}
