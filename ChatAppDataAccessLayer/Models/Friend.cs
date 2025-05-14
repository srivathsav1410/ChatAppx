using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppDataAccessLayer.Models
{
    public class Friend
    {
        public int Id { get; set; }

        public int SentUserId { get; set; }       
        public User SentUser { get; set; }

        public int RequestUserId { get; set; }   
        public  User RequestUser { get; set; }

        public string Status { get; set; }        
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    }

}
