using System;
using TravelPal.Enums;

namespace TravelPal
{
    public class Vacation : Travel
    {
        public bool IsAllInclusive { get; set; }

        public Vacation(string destination, Countries country, int travellers, DateTime startDate, DateTime endDate, int travelDays, bool isAllInclusive) : base(destination, country, travellers, startDate, endDate, travelDays)
        {
            IsAllInclusive = isAllInclusive;
        }
        public string GetInfo()
        {
            return $"";
        }
    }
}
