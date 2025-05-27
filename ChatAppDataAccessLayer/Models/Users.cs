using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatAppDataAccessLayer.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string ?Username { get; set; }
        public string ?DisplayName { get; set; }
        public string ?Email { get; set; }
        public string ?Password { get; set; }

        [JsonIgnore]
        public ICollection<Message> MessagesSent { get; set; }
        [JsonIgnore]
        public ICollection<Message> MessagesReceived { get; set; }
        [JsonIgnore]
        public ICollection<Friend> FriendRequestsSent { get; set; }
        [JsonIgnore]
        public ICollection<Friend> FriendRequestsReceived { get; set; } 
    }

}
