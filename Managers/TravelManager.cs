using System.Collections.Generic;
using TravelPal.Travels;

namespace TravelPal.Managers
{
    public class TravelManager
    {
        public List<Travel> Travels { get; set; } = new();

        public void AddTravel(Travel travel)
        {
            Travels.Add(travel);
        }
        public void RemoveTravel(Travel travel)
        {
            Travels.Remove(travel);
        }
    }
}
