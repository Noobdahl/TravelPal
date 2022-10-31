using System.Collections.Generic;
using TravelPal.Models;

namespace TravelPal.Managers
{
    public class UserManager
    {
        public List<IUser> Users { get; set; } = new();
        public IUser SignedInUser { get; set; }

        public bool AddUser(IUser user)
        {
            if (ValidateUsername(user.UserName))
            {
                Users.Add(user);
                return true;
            }
            else
            {
                return false;
            }
        }
        public void RemoveUser(IUser user)
        {

        }
        public bool UpdateUsername(IUser user, string newName)
        {
            if (ValidateUsername(newName))
            {
                user.UserName = newName;
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool ValidateUsername(string newName)
        {
            foreach (IUser u in Users)
            {
                if (u.UserName == newName)
                {
                    return false;
                }
            }
            return true;
        }
        public bool SignInUser(string userName, string password)
        {
            foreach (IUser u in Users)
            {
                if (userName == u.UserName && password == u.Password)
                {
                    SignedInUser = u;
                    return true;
                }
            }
            return false;
        }
    }
}
