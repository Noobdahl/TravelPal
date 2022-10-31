using System.Collections.Generic;
using TravelPal.Enums;
using TravelPal.Travels;

namespace TravelPal.Models
{
    public interface IUser
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public Countries Location { get; set; }
        void IUser(string username, string password, Countries location);
        public List<Travel> GetTravels();
    }
}
