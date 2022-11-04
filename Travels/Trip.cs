using System;
using System.Collections.Generic;
using TravelPal.Enums;
using TravelPal.Models;
using TravelPal.PackingList;

namespace TravelPal.Travels
{
    public class Trip : Travel
    {
        public TripTypes Type { get; set; }
        public Trip(string destination, Countries country, int travellers, int travelDays,
            DateTime startDate, DateTime endDate, TripTypes type, List<IPackingListItem> packingList, IUser owner)

            : base(destination, country, travellers, travelDays, startDate, endDate, packingList, owner)
        {
            Type = type;
        }
        public override string GetInfo()
        {
            if (base.TravelDays == 1)
                return $"(Trip) {base.Country} for {base.TravelDays} day - ({base.Owner.UserName})";
            else
                return $"(Trip) {base.Country} for {base.TravelDays} days - ({base.Owner.UserName})";
        }

        //Overrided method to be able to set Type when adressing object as a IPackingListItem
        public override void SetTripType(TripTypes type)
        {
            Type = type;
        }
    }
}
