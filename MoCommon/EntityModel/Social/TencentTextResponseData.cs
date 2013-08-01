using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoCommon.EntityModel.Social
{
   public class TencentTextResponseData
    {
       public string ErrCode { get; set; }
       public string Msg { get; set; }
       public string Ret { get; set; }
       public TencentResponseTimeStamp Data { get; set; }
       public string Seqid { get; set; }

       public string ImgUrl { get; set; }
    }

   public class TencentResponseTimeStamp
   {
       public string Id { get; set; }
       public string Time { get; set; }
   }
}
