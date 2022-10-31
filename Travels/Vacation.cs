using System;
using TravelPal.Enums;

namespace TravelPal.Travels
{
    public class Vacation : Travel
    {
        public bool IsAllInclusive { get; set; }

        public Vacation(string destination, Countries country, int travellers, int travelDays, DateTime startDate, DateTime endDate, bool isAllInclusive)
            : base(destination, country, travellers, travelDays, startDate, endDate)
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
