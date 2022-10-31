using System.Collections.Generic;
using TravelPal.Enums;
using TravelPal.Travels;

namespace TravelPal.Models
{
    public class User : IUser
    {
        public List<Travel> Travels { get; set; } = new();
        public string UserName { get; set; }
        public string Password { get; set; }
        public Countries Location { get; set; }

        public void IUser(string username, string password, Countries location)
        {
            UserName = username;
            Password = password;
            Location = location;
            //throw new System.NotImplementedException();
        }
        public List<Travel> GetTravels()
        {
            return Travels;
        }
    }
}


//hur ska admins kunna ändra trip till vacation? de kan inte lägga till resor...