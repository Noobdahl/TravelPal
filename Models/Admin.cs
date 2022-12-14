using System.Collections.Generic;
using TravelPal.Enums;
using TravelPal.Managers;
using TravelPal.Travels;

namespace TravelPal.Models
{
    public class Admin : IUser
    {
        public TravelManager travelManager { get; set; }
        public UserManager userManager { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Countries Location { get; set; }
        public Admin(TravelManager tManager, UserManager uManager)
        {
            travelManager = tManager;
            userManager = uManager;
        }

        public void IUser(string username, string password, Countries location)
        {
            UserName = username;
            Password = password;
            Location = location;
        }
        //Admin gets the travels of all users, not including self (since admins dont create travels)
        public List<Travel> GetTravels()
        {
            //I use the travelManagers list of travels as a temporary list that gets cleared and filled
            travelManager.Travels.Clear();
            foreach (IUser user in GetUsers())
            {
                //Skips user if its the admin (since admin has no trips)
                if (user.GetType().Name != "Admin")
                {
                    foreach (Travel travel in user.GetTravels())
                    {
                        travelManager.Travels.Add(travel);
                    }
                }
            }
            return travelManager.Travels;
        }

        //Gets all users that is added to the list of users in the usermanager
        public List<IUser> GetUsers()
        {
            return userManager.Users;
        }

        public void RemoveTravelFromUserList(Travel currentTravel)
        {
            foreach (IUser user in GetUsers())
            {
                bool found = false;
                foreach (Travel travel in user.GetTravels())
                {
                    if (travel == currentTravel)
                        found = true;
                }
                if (found)
                    user.GetTravels().Remove(currentTravel);
            }
        }
        public void ReplaceTravelAtUserList(Travel oldTravel, Travel newTravel)
        {
            foreach (IUser user in GetUsers())
            {
                bool found = false;
                foreach (Travel travel in user.GetTravels())
                {
                    if (travel == oldTravel)
                        found = true;
                }
                if (found)
                {
                    user.GetTravels().Remove(oldTravel);
                    user.GetTravels().Add(newTravel);
                }
            }
        }
    }
}
