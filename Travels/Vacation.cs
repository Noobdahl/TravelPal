using System;
using System.Collections.Generic;
using System.Diagnostics;
using TravelPal.Enums;
using TravelPal.Models;
using TravelPal.PackingList;

namespace TravelPal.Travels
{
    public class Vacation : Travel
    {
        public bool IsAllInclusive { get; set; }

        public Vacation(string destination, Countries country, int travellers, int travelDays, DateTime startDate,
            DateTime endDate, bool isAllInclusive, List<IPackingListItem> packingList, IUser owner)

            : base(destination, country, travellers, travelDays, startDate, endDate, packingList, owner)
        {
            IsAllInclusive = isAllInclusive;
        }
        public override string GetInfo()
        {
            if (base.TravelDays == 1)
                return $"(Vacation) {base.Country} for {base.TravelDays} day - ({base.Owner.UserName})";
            else    
                return $"(Vacation) {base.Country} for {base.TravelDays} days - ({base.Owner.UserName})";
        }

        //Overrided method to be able to set IsAllInclusive when adressing object as a IPackingListItem
        public override void SetAllInclusive(bool isAllInclusive)
        {
            IsAllInclusive = isAllInclusive;
        }
    }
}
