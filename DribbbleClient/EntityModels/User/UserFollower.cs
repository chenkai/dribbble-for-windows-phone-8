using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DribbbleClient.EntityModels.User
{
   public class UserFollower
    {
        public int Page { get; set; }
        public int Pages { get; set; }
        public int Per_page { get; set; }
        public int Total { get; set; }
        public List<Player> Players { get; set; }
    }
}
