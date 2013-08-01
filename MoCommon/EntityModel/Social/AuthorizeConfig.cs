using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoCommon.Social;

namespace MoCommon.EntityModel.Social
{
    public class AuthorizeConfig
    {
        public string AppKey { get; set; }
        public string AppSecret { get; set; }
        public string Url { get; set; }
        public PlatformType PlatformType { get; set; }
        public string RedirectUrl { get; set; }
        public string ApiUrl { get; set; }
        public string OauthApiUrl { get; set; }
    }
}
