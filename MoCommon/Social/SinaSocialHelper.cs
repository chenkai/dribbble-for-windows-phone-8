using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoCommon.Social
{
    public class SinaSocialHelper:SocialShareHelper
    {

        public List<KeyValuePair<string, object>> PostArgumentList = null;

        /// <summary>
        /// Get AuthrizeCode Request Url operator by sina platform
        /// </summary>
        /// <param name="requestUrl">Sina Api Basic Url</param>
        /// <param name="appkey">Appkey</param>
        /// <param name="redirectUrl">Redirect Url</param>
        /// <returns>Authrize Code Url</returns>
        public string GetAuthorizeCodeUrl(string requestUrl,string appkey,string redirectUrl)
        {
            requestUrl += "authorize";
            Dictionary<string, object> argumentDic = new Dictionary<string, object>();
            argumentDic.Add("display", "mobile");
            argumentDic.Add("client_id", appkey);
            argumentDic.Add("redirect_uri",redirectUrl);

            return MergeRequestArgument(requestUrl, argumentDic);
        }



        /// <summary>
        ///  Get Request Access Token Url Operator by sina Platform
        /// </summary>
        /// <param name="requestUrl">Request Url basic</param>
        /// <param name="appkey">App Key</param>
        /// <param name="appSecret">App Secret</param>
        /// <param name="grantType">Grant Type</param>
        /// <param name="code">Authorize Code</param>
        /// <param name="redirectUrl">Redirect Url</param>
        /// <returns>Request Accesstoken Url</returns>
        public string GetRequestAccessTokenUrl(string requestUrl, string appkey, string appSecret, string grantType,string code, string redirectUrl)
        {
            requestUrl += "access_token";

            if (PostArgumentList == null)
                PostArgumentList = new List<KeyValuePair<string, object>>();

            PostArgumentList.Add(new KeyValuePair<string, object>("client_id", appkey));
            PostArgumentList.Add(new KeyValuePair<string, object>("client_secret", appSecret));
            PostArgumentList.Add(new KeyValuePair<string, object>("grant_type", grantType));

            PostArgumentList.Add(new KeyValuePair<string, object>("code", code));
            PostArgumentList.Add(new KeyValuePair<string, object>("redirect_uri", redirectUrl));
            return requestUrl;
        }




        /// <summary>
        /// Get Pure Text Content operator sina Platfrom
        /// </summary>
        /// <param name="requestUrl">Request Url</param>
        /// <param name="accessToken">Access Token</param>
        /// <param name="content">TExt Content</param>
        /// <returns>Request Url</returns>
        public string GetPureTextContentUrl(string requestUrl,string accessToken,string content)
        {
            requestUrl += "statuses/update.json";
            if (PostArgumentList == null)
                PostArgumentList = new List<KeyValuePair<string, object>>();
            PostArgumentList.Add(new KeyValuePair<string, object>("access_token",accessToken));
            PostArgumentList.Add(new KeyValuePair<string, object>("status", content));

            return requestUrl;
        }



        /// <summary>
        /// Get TExt Content With Picture Sina Platform
        /// </summary>
        /// <param name="requestUrl">Basic Request Platform</param>
        /// <param name="accessToken">Access Token</param>
        /// <param name="content">Text Content</param>
        /// <returns>Request Url</returns>
        public string GetTextContentWithPicUrl(string requestUrl,string accessToken,string content)
        {
            requestUrl += "statuses/upload.json";
            if (PostArgumentList == null)
                PostArgumentList = new List<KeyValuePair<string, object>>();
            PostArgumentList.Add(new KeyValuePair<string, object>("access_token", accessToken));
            PostArgumentList.Add(new KeyValuePair<string, object>("status", content));
            return requestUrl;
        }
    }
}
