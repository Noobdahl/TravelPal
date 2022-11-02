using System;
using System.Collections.Generic;
using TravelPal.Enums;
using TravelPal.PackingList;

namespace TravelPal.Travels
{
    public class Trip : Travel
    {
        public TripTypes Type { get; set; }
        public Trip(string destination, Countries country, int travellers, int travelDays,
            DateTime startDate, DateTime endDate, TripTypes type, List<IPackingListItem> packingList)

            : base(destination, country, travellers, travelDays, startDate, endDate, packingList)
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
