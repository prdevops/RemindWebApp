using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RemindWebApp.Utlities
{
    public class Utility
    {
        public enum Roles
        {
            Admin,
            Member,
            Guest
        }
        public const string AdminRole = "Admin";
        public const string MemberRole = "Meneger";
        public const string ClientRole = "Guest";
    }
}
