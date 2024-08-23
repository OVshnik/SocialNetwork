using SocialNetwork.Data.Models;

namespace SocialNetwork.ViewModels.Account
{
    public class UserViewModel
    {
        public User User { get; set; } 
        public UserViewModel(User user) 
        {
            User=user;
        }
    }
}
