using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MoCommon.EntityModel.Social;
using RestSharp;
namespace MoCommon.Social.WeChat
{
    public class ShareToWeChatHelper:SocialShareHelper
    {
        public string GetAuthorizeCode()
        {
            AuthorizeConfig oauthConfig = GetAuthorizeConfig(PlatformType.WeChat);
            ShareToWeChatFriendsHelper weChatHelper = new ShareToWeChatFriendsHelper();
            string requestUrl = weChatHelper.GetWeChatOauthUrl(oauthConfig.OauthApiUrl, oauthConfig.AppKey, ResponseType.Code, "http://www.mojichina.com/");
            return requestUrl;
        }
    }
}
