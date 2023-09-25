using BLL_lib.Models.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL_lib.Repository
{
    public class FriendsRepository : Repository<Friend>
    {
        public FriendsRepository(ApplicationDbContext db)
      : base(db)
        {
            
        }
        

        public async Task AddFriend (User target, User Friend)
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

        public async Task<List<User>> GetFriendsByUser(User target)
        {
            var friends = Set.Include(x => x.User).Include(x => x.CurrentFriend).Where(x => x.User.Id == target.Id).Select(x => x.CurrentFriend);

            return await friends.ToListAsync();
        }


        public async Task DeleteFriend(User target, User Friend)
        {
            var friends = Set.FirstOrDefault(x => x.UserId == target.Id && x.CurrentFriendId == Friend.Id);

            if (friends != null)
            {
                await Delete(friends);
            }
        }

    }
}
