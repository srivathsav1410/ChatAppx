using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChatAppDataAccessLayer.Models;
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

            if (user.Username==username && user.Password==password)
            {
                return "User Exists";
            }
            else
            {
                return "Incorrect username or Password";
            }

        }
    }

}