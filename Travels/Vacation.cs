using System;
using System.Collections.Generic;
using TravelPal.Enums;
using TravelPal.PackingList;

namespace TravelPal.Travels
{
    public class Vacation : Travel
    {
        public bool IsAllInclusive { get; set; }

        public Vacation(string destination, Countries country, int travellers, int travelDays, DateTime startDate,
            DateTime endDate, bool isAllInclusive, List<IPackingListItem> packingList)

            : base(destination, country, travellers, travelDays, startDate, endDate, packingList)
        {
            IsAllInclusive = isAllInclusive;
        }
        public string GetInfo()
        {
            return $"";
        }
        public override void SetAllInclusive(bool isAllInclusive)
        {
            IsAllInclusive = isAllInclusive;
        }
    }
}
