using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DribbbleClient.EntityModels
{
   public class Comment
    {
       public int Id { get; set; }
       public int Likes_count { get; set; }
       public string Body { get; set; }
       public Player Player { get; set; }
       public string Created_at { get; set; }
       public string FormatDate { get; set; }
    }
}
