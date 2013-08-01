using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DribbbleClient.EntityModels.ShotCatalog
{
   public class CatalogShots
    {
       public string Page { get; set; }
       public int Per_page { get; set; }
       public int Pages { get; set; }
       public int Total { get; set; }
       public List<Shot> Shots { get; set; }
    }
}
