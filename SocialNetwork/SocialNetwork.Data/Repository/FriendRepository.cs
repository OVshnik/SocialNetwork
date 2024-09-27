using Microsoft.EntityFrameworkCore;
using SocialNetwork.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetwork.Data.Repository
{
    public class FriendRepository : Repository<Friend>
    {
        public FriendRepository(ApplicationDbContext db) : base(db)
        {
            
        }
        public async Task AddFriendAsync(User target, User Friend)
        {
            var friends = Set.AsEnumerable().FirstOrDefault(x => x.UserId == target.Id && x.CurrentFriendId == Friend.Id);

            if (friends == null)
            {
                var item = new Friend()
                {
                    UserId = target.Id,
                    User = target,
                    CurrentFriend = Friend,
                    CurrentFriendId = Friend.Id,
                };
                await Create(item);
            }
        }
        public async Task <List<User>> GetFriendsByUserAsync(User target)
        {
                return await Task.Run(()=> Set.Include(x => x.CurrentFriend).AsEnumerable().Where(x => x?.User?.Id == target.Id).Select(x => x.CurrentFriend).ToList());
        }
        public async Task DeleteFriendAsync(User target, User Friend)
        {
            var friends = Set.AsEnumerable().FirstOrDefault(x => x.UserId == target.Id && x.CurrentFriendId == Friend.Id);
            if (friends != null)
            {
                await Delete(friends);
            }
        }
    }
}
