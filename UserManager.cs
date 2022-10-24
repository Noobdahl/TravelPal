using System.Collections.Generic;
using TravelPal.Models;

namespace TravelPal
{
    public class UserManager
    {
        public List<IUser> Users { get; set; }
        public IUser SignedInUser { get; set; }

        public bool AddUser(IUser user)
        {
            return true;
        }
        public void RemoveUser(IUser user)
        {

        }
        public bool UpdateUsername(IUser user, string newName)
        {
            return true;
        }
        private bool ValidateUsername(string newName)
        {
            return false;
        }
        public bool SignInUser(string userName, string password)
        {
            return true;
        }
    }
}
