using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Imaging;  
using System.IO;
using MoCommon.EntityModel.Social;

namespace MoCommon.Social
{
   public class TencentSocialHelper:SocialShareHelper
    {
       public List<KeyValuePair<string, object>> PostArgumentList = null;

       /// <summary>
       /// Get Tencent Authroize Code Request Url
       /// </summary>
       /// <param name="appkey">App Key </param>
       /// <param name="responseType">Response Type</param>
       /// <param name="redirectUrl">RedirectUrl</param>
       /// <returns>Merge Url</returns>
       public string GetAuthorizeCodeUrl(string requestUrl, string appkey, string responseType, string redirectUrl)
       {
           requestUrl += "authorize";
           Dictionary<string, object> argumentDic = new Dictionary<string, object>();
           argumentDic.Add("client_id", appkey);
           argumentDic.Add("response_type", responseType);
           argumentDic.Add("redirect_uri", redirectUrl);

           return MergeRequestArgument(requestUrl, argumentDic);
       }



       /// <summary>
       /// Get Tencent Account Token Request Url
       /// </summary>
       /// <param name="requestUrl">Request Url</param>
       /// <param name="appkey">App Key</param>
       /// <param name="appSecret">App Secret</param>
       /// <param name="redirectUrl">Redirect Url</param>
       /// <param name="grantType">Grant Type</param>
       /// <param name="code">Authorize Code</param>
       /// <returns>Access Token Url</returns>
       public string GetRequestAccessTokenUrl(string requestUrl, string appkey, string appSecret, string redirectUrl, string grantType, string code)
       {
           requestUrl += "access_token";
           #region combin the request argument
           if (PostArgumentList == null)
               PostArgumentList = new List<KeyValuePair<string, object>>();

           PostArgumentList.Add(new KeyValuePair<string, object>("client_id", appkey));
           PostArgumentList.Add(new KeyValuePair<string, object>("client_secret", appSecret));
           PostArgumentList.Add(new KeyValuePair<string, object>("redirect_uri", redirectUrl));

           PostArgumentList.Add(new KeyValuePair<string, object>("grant_type", grantType));
           PostArgumentList.Add(new KeyValuePair<string, object>("code", code));
           #endregion
           return requestUrl;
       }




       /// <summary>
       /// GEt Tencent Pure text content request url
       /// </summary>
       /// <param name="requestUrl">request url</param>
       /// <param name="formatType">format type</param>
       /// <param name="content">send content</param>
       /// <param name="clientIp">client ip address</param>
       /// <returns>request url</returns>
       public string GetPureTextContentUrl(string requestUrl, string appkey, string accesstoken, string openid, string content, FormatType format, string clientip="",
           string oauthVersion = "2.a",string scope="all")
       {
           requestUrl += "t/add";
           #region merge all post argument to list
           if (PostArgumentList == null)
               PostArgumentList = new List<KeyValuePair<string, object>>();
           PostArgumentList.Add(new KeyValuePair<string, object>("oauth_consumer_key", appkey));
           PostArgumentList.Add(new KeyValuePair<string, object>("access_token", accesstoken));
           PostArgumentList.Add(new KeyValuePair<string, object>("openid", openid));

           PostArgumentList.Add(new KeyValuePair<string, object>("clientip", clientip));
           PostArgumentList.Add(new KeyValuePair<string, object>("oauth_version", oauthVersion));
           PostArgumentList.Add(new KeyValuePair<string, object>("scope", scope));

           PostArgumentList.Add(new KeyValuePair<string, object>("format", format.ToString().ToLower()));
           PostArgumentList.Add(new KeyValuePair<string, object>("content", content));
           #endregion
           return requestUrl;
       }





       /// <summary>
       /// Get Tencent Text With Picture Request Url
       /// </summary>
       /// <param name="requestUrl">Reqeust Url</param>
       /// <param name="appkey">AppKey</param>
       /// <param name="accesstoken">Access Token</param>
       /// <param name="openid">OpenId</param>
       /// <param name="content">Content</param>
       /// <param name="format">Format Type</param>
       /// <param name="clientip">Client Ip Address</param>
       /// <param name="oauthVersion">Oauth Version</param>
       /// <param name="scope">Scope</param>
       /// <returns>Request Url</returns>
       public string GetTextContentWithPicUrl(string requestUrl, string appkey, string accesstoken, string openid, string content,FormatType format,string clientip="",
           string oauthVersion="2.a",string scope="all")
       {
           requestUrl += "t/add_pic ";
           if (PostArgumentList == null)
               PostArgumentList = new List<KeyValuePair<string, object>>();
           PostArgumentList.Add(new KeyValuePair<string, object>("oauth_consumer_key",appkey));
           PostArgumentList.Add(new KeyValuePair<string, object>("access_token", accesstoken));
           PostArgumentList.Add(new KeyValuePair<string, object>("content", content));

           PostArgumentList.Add(new KeyValuePair<string, object>("openid", openid));
           PostArgumentList.Add(new KeyValuePair<string, object>("format", format.ToString().ToLower()));
           PostArgumentList.Add(new KeyValuePair<string, object>("clientip", clientip));

           PostArgumentList.Add(new KeyValuePair<string, object>("scope", scope));
           PostArgumentList.Add(new KeyValuePair<string, object>("oauth_version", oauthVersion));
        

           return requestUrl;
       }




       /// <summary>
       /// Refresh Access Token Request Url 
       /// </summary>
       /// <param name="requestUrl">Request  Url</param>
       /// <param name="appkey">Application Key</param>
       /// <param name="refreshToken">REfresh Token</param>
       /// <param name="grantType">Grant Type</param>
       /// <returns>Refresh Url</returns>
       public string GetRefreshAccessTokenUrl(string requestUrl, string appkey, string refreshToken,string grantType = "refresh_token")
       {
           requestUrl += "access_token";
           if (PostArgumentList == null)
               PostArgumentList = new List<KeyValuePair<string, object>>();
           PostArgumentList.Add(new KeyValuePair<string, object>("client_id", appkey));
           PostArgumentList.Add(new KeyValuePair<string, object>("grant_type", grantType));
           PostArgumentList.Add(new KeyValuePair<string, object>("refresh_token", refreshToken));

           return requestUrl;
       }
    }
}
