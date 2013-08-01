using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MoCommon.EntityModel.Social;

namespace MoCommon.Social.WeChat
{
    public class ShareToWeChatFriendsHelper:SocialShareHelper
    {
        public List<KeyValuePair<string, object>> _postArgumentList = null;

        public string GetWeChatOauthUrl(string requestUrl, string appId, ResponseType responseType, string redirectUrl, 
            string scope = "post_timeline", string state="")
        {
            requestUrl += "oauth";      

            Dictionary<string,object> mergeArgumentDic=new Dictionary<string,object>();
            mergeArgumentDic.Add("appid", appId);
            mergeArgumentDic.Add("response_type", responseType.ToString().ToLower());
            mergeArgumentDic.Add("redirect_uri", redirectUrl);

            mergeArgumentDic.Add("scope", scope);
            mergeArgumentDic.Add("state", state);
            return  base.MergeRequestArgument(requestUrl,mergeArgumentDic);
        }


        public string GetWeChatAccessTokenUrl(string requestUrl, GrantType grantType, string code, string refreshToken, string redirectUrl)
        {
            requestUrl += "token.format";

            if (_postArgumentList == null)
                _postArgumentList = new List<KeyValuePair<string, object>>();

            _postArgumentList.Add(new KeyValuePair<string, object>("grant_type", grantType.ToString().ToLower()));

            if (grantType == GrantType.AuthenticationCode)
            {
                //code 
                _postArgumentList.Add(new KeyValuePair<string, object>("code", code));
                _postArgumentList.Add(new KeyValuePair<string, object>("redirect_uri", redirectUrl));
            }
            else
                _postArgumentList.Add(new KeyValuePair<string, object>("refresh_token", refreshToken));
            return requestUrl;
        }


        public string PostWeChatMediaUrl(string requestUrl, string accessToken, string type = "image")
        {
            requestUrl += "media.format";

            if (_postArgumentList == null)
                _postArgumentList = new List<KeyValuePair<string, object>>();
            _postArgumentList.Add(new KeyValuePair<string, object>("type", type));
            _postArgumentList.Add(new KeyValuePair<string, object>("access_token", accessToken));
            return requestUrl;
        }


        public string GetWeChatMediaUrl(string requestUrl, string mediaId)
        {
            requestUrl += "media/:media_id.format";
            if (_postArgumentList == null)
                _postArgumentList = new List<KeyValuePair<string, object>>();
            _postArgumentList.Add(new KeyValuePair<string, object>("media_id", mediaId));
            return requestUrl;
        }


        public string PostWeChatTimeLineUrl(string requestUrl, ContentType contentType, string title, string[] mediaList,
            string mediaUrl, string contentUrl, string coordinates)
        {
            requestUrl += "timeline.format";
            if (_postArgumentList == null)
                _postArgumentList = new List<KeyValuePair<string, object>>();

            _postArgumentList.Add(new KeyValuePair<string, object>("content_type",contentType.ToString().ToLower()));
            _postArgumentList.Add(new KeyValuePair<string, object>("title", title));
            _postArgumentList.Add(new KeyValuePair<string, object>("media_list", mediaList));

            _postArgumentList.Add(new KeyValuePair<string, object>("media_url", mediaUrl));
            _postArgumentList.Add(new KeyValuePair<string, object>("content_url",contentUrl));
            _postArgumentList.Add(new KeyValuePair<string, object>("coordinates", coordinates));

            return requestUrl;
        }
    }

    public enum ResponseType
    {
        Code,
        Token
    }

    public enum GrantType
    {
        AuthenticationCode,
        RefreshToken
    }

    public enum ContentType
    {
        Text,
        Photo,
        Feed,
        Video,
        Music
    }
}
