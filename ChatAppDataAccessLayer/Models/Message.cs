using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppDataAccessLayer.Models
{
    public class Message
    {
        public int MessageId { get; set; }

        public string Content { get; set; }
        public DateTime Timestamp { get; set; }

        // Foreign keys and navigation
        public int SenderId { get; set; }
        public User Sender { get; set; }

        public int ReceiverId { get; set; }
        public User Receiver { get; set; }
    }

}
