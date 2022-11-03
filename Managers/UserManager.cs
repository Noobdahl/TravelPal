using System.Collections.Generic;
using TravelPal.Models;

namespace TravelPal.Managers
{
    public class UserManager
    {
        public List<IUser> Users { get; set; } = new();
        public IUser SignedInUser { get; set; }

        //AddUser adds user to the list of IUsers if the username is valid
        public bool AddUser(IUser user)
        {
            if (ValidateUsername(user.UserName))
            {
                Users.Add(user);
                return true;
            }
            else
                return false;
        }


        //public void RemoveUser(IUser user)
        //{
        //Do not implement?
        //}


        //Updates username of specified user if the username is valid
        public bool UpdateUsername(IUser user, string newName)
        {
            if (ValidateUsername(newName) || newName == user.UserName)
            {
                user.UserName = newName;
                return true;
            }
            else
            {
                return false;
            }
        }

        //Gets all users and compares recieved name to existing ones, returns true if available
        private bool ValidateUsername(string newName)
        {
            foreach (IUser u in Users)
            {
                if (u.UserName == newName)
                    return false;
            }
            return true;
        }

        //Compares recieved username and password with all users, sets SignedInUser if found
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
