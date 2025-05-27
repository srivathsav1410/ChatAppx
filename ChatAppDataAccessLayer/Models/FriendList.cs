using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatAppDataAccessLayer.Models
{
    public class FriendsList
    {

        public int FriendId { get; set; }

        public string ?UserName { get; set; }

        public string? ImageUrl { get; set; }
    }
}
