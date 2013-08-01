using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MoCommon.Social;

namespace MoCommon.EntityModel.Social
{
   public class AccessTokenData
    {
       public PlatformType PlatformType { get; set; }
       public string AccessToken { get; set; }
       public int ExpiresIn { get; set; }
       public string RefreshToken { get; set; }
       public string OpenId { get; set; }
       public DateTime CreateDate { get; set; }
    }
}

