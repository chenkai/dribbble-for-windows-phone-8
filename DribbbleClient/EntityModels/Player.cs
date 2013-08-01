using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DribbbleClient.EntityModels
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public int Followers_count { get; set; }

        public int Draftees_count { get; set; }
        public int Likes_count { get; set; }
        public int Likes_received_count { get; set; }
        public int Comments_count { get; set; }

        public int Comments_received_count { get; set; }
        public int Rebounds_count { get; set; }
        public int Rebounds_received_count { get; set; }
        public string Url { get; set; }

        public string Avatar_url { get; set; }
        public string Username { get; set; }
        public string Twitter_screen_name { get; set; }
        public string Website_url { get; set; }

        public string Drafted_by_player_id { get; set; }
        public int Shots_count { get; set; }
        public int Following_count { get; set; }
        public string Created_at { get; set; }

        public bool IsFindUser { get; set; }
        public string Message { get; set; }
        
    }
}
