using Microsoft.AspNetCore.Identity;
using System.Reflection.Metadata;

namespace SocialNetwork
{
    public class User:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public DateTime BirthDate { get; set; }

    }
}
