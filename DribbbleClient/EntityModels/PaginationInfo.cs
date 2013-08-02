using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DribbbleClient.EntityModels
{
    public class PaginationInfo
    {
        public PagintaionType PageType { get; set; }
        public int CurrentIndex { get; set; }
        public int PrePageCount { get; set; }
        public int TotalPage { get; set; }
    }

    public enum PagintaionType
    {
        CatalogPopular,
        CatalogEveryOne,
        CatalogDebuts
    }
}
