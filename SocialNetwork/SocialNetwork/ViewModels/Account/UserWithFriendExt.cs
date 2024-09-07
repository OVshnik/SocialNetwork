using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SocialNetwork.Data.Models;

namespace SocialNetwork.ViewModels
{
    public class UserWithFriendExt:User
    {
        public bool IsFriendWithCurrent {  get; set; }
    }
}
