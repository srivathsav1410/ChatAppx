using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatAppDataAccessLayer.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace ChatAppDataAccessLayer
{
    public class ChatAppRepository
    {
        private ChatAppDbContext context;
        public ChatAppRepository(ChatAppDbContext context)
        {
            this.context = context;
        }
        public List<User> GetAllCategories()
        {
            var categoriesList = (from user in context.Users
                                  select user).ToList();
            return categoriesList;
        }

        public bool AddUser(User user)
        {
            bool res = false;
            try
            {
                context.Users.Add(user);
                context.SaveChanges();
                res = true;
            }
            catch (Exception)
            {

                res = false;
            }
            return res;
        }
        public string VerifyUser(string username, string password)
        {
            var user = (from u in context.Users where u.Username == username && u.Password == password select u).FirstOrDefault();

            if (user.Username == username && user.Password == password)
            {
                return "User Exists";
            }
            else
            {
                return "Incorrect username or Password";
            }

        }
        public async Task<string> SendRequest(int requesterId, int addresseeId)
        {
            var existing = await (from f in context.Friends
                                  where (f.SentUserId == requesterId && f.RequestUserId == addresseeId) ||
                                        (f.SentUserId == addresseeId && f.RequestUserId == requesterId)
                                  select f).FirstOrDefaultAsync();
            if (existing == null)
            {
                var friendRequest = new Friend
                {
                    SentUserId = requesterId,
                    RequestUserId = addresseeId,
                    Status = "Pending",
                    RequestedAt = DateTime.UtcNow
                };
                context.Friends.Add(friendRequest);
                await context.SaveChangesAsync();
                return "Friend request sent.";
            }
            else
            {
                return "Friend request already exists.";


            }
        }
        public async Task<string> AcceptRequest(int requesterId, int addresseeId)
        {
            var existing = await (from f in context.Friends
                                  where (f.SentUserId == requesterId && f.RequestUserId == addresseeId) ||
                                        (f.SentUserId == addresseeId && f.RequestUserId == requesterId)
                                  select f).FirstOrDefaultAsync();

            if (existing == null || existing.Status!= "Pending")
            {
            
                return "No pending request exists";
            }
            else
            {
                existing.Status = "Accepted";
                context.Friends.Update(existing);
                await context.SaveChangesAsync();
                return "Friend request sent.";
            }   
        }
        public async Task<List<FriendsList>> Friends(int Id)
        {
            var existing = await (from f in context.Friends
                                  where (f.SentUserId == Id || f.RequestUserId == Id) && f.Status == "Accepted"
                                  select f).ToListAsync();

            List<FriendsList> l = new List<FriendsList>();
            foreach (var c in existing)
            {
                var friendsList = new FriendsList();

                if (c.SentUserId == Id)
                {
                    friendsList.UserId = c.RequestUserId;
                    friendsList.UserName = await context.Users
                        .Where(u => u.UserId == c.RequestUserId)
                        .Select(u => u.Username)
                        .FirstOrDefaultAsync();
                }
                else
                {
                    friendsList.UserId = c.SentUserId;
                    friendsList.UserName = await context.Users
                        .Where(u => u.UserId == c.SentUserId)
                        .Select(u => u.Username)
                        .FirstOrDefaultAsync();
                }

                l.Add(friendsList);
            }

            return l;
        }


    }

}
