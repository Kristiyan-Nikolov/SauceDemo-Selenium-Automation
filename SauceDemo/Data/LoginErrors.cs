using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SauceDemo.Data
{
    public static class LoginErrors
    {
        public const string ErrorNoSuchUser = "Epic sadface: Username and password do not match any user in this service";
        public const string ErrorUserLockedOut = "Epic sadface: Sorry, this user has been locked out.";
        public const string ErrorUsernameRequired = "Epic sadface: Username is required";
        public const string ErrorPasswordRequired = "Epic sadface: Password is required";
    }
}
