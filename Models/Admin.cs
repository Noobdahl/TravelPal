using TravelPal.Enums;

namespace TravelPal.Models
{
    public class Admin : IUser
    {
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
    }
}
