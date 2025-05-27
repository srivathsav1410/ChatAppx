using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using ChatAppDataAccessLayer.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
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
        public User VerifyUser(string username, string password)
        {

            var user = (from u in context.Users
                        where u.Email == username && u.Password == password
                        select u).FirstOrDefault();

            if (user != null)
            {
                return user;
            }
            else
            {
                return null;
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
                    friendsList.FriendId = c.RequestUserId;
                    friendsList.UserName = await context.Users
                        .Where(u => u.UserId == c.RequestUserId)
                        .Select(u => u.Username)
                        .FirstOrDefaultAsync();
                    friendsList.ImageUrl= getImageUrl(c.RequestUserId);
                }
                else
                {
                    friendsList.FriendId = c.SentUserId;
                    friendsList.UserName = await context.Users
                        .Where(u => u.UserId == c.SentUserId)
                        .Select(u => u.Username)
                       .FirstOrDefaultAsync();
                    friendsList.ImageUrl =  getImageUrl(c.RequestUserId);

                }

                l.Add(friendsList);
            }

            return l;
        }

        public async Task<bool> SendMessagesAsync(Message message)
        {
            try
            {
                context.Messages.Add(message);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<List<Message>> MessagesListAsync(int s, int r)
        {
            try
            {
                return await context.Messages
                    .Where(m => (m.SenderId == s && m.ReceiverId == r) || (m.SenderId == r && m.ReceiverId == s))
                    .ToListAsync();
            }
            catch (Exception)
            {
                // Optional: log exception
                return new List<Message>();
            }
        }
        //public async Task<bool>UploadImage (IFormFile image,string userId)
        //{
        //    bool res = false;
        //    try
        //    {
              

        //        BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
        //        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

        //        containerClient.CreateIfNotExists();

        //        BlobClient blobClient = containerClient.GetBlobClient(blobName);
 

        //        var blobHttpHeaders = new BlobHttpHeaders
        //        {
        //            ContentType = image.ContentType
        //        };

        //        await using var stream = image.OpenReadStream();
        //        await blobClient.UploadAsync(stream, new BlobUploadOptions
        //        {
        //            HttpHeaders = blobHttpHeaders
        //        });
        //        res = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        // Optionally, log the exception: Console.WriteLine(ex.Message);
        //        res = false;
        //    }
        //    return res;
        //}


        //public string getImageUrl(int userId)
        //{
        //    string imageUrl = "";
        //    try
        //    {
               
        //        BlobServiceClient blobServiceClient = new BlobServiceClient(connectionString);
        //        BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);
        //        BlobClient blobClient = containerClient.GetBlobClient(blobName);
        //        if (blobClient.Exists())
        //        {
        //            imageUrl = blobClient.Uri.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        imageUrl = "";
        //        // Optionally, log the exception: Console.WriteLine(ex.Message);
        //    }
        //    return imageUrl;
        //}
    }



}


