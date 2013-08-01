using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MoCommon.EntityModel.Social;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Resources;
using System.Net.Http;
using System.IO.IsolatedStorage;
using RestSharp;
using Newtonsoft.Json;

using System.Windows.Media.Imaging;

namespace MoCommon.Social
{
    /// <summary>
    /// This helper build for social shareable component.
    /// author:chenkai[moji] data:2013-5-31 11:45:19
    /// </summary>
    public class SocialShareHelper : IShareable
    {
        public event EventHandler AsyncAuthorizeComplated;

        public delegate void AsyncHandler(object responseStatus,object exceptionMsg);
        public event AsyncHandler AsyncSendContentComplated;
        public event AsyncHandler AsyncSendPictureComplated;

        /// <summary>
        /// Merge request url add argument value;
        /// </summary>
        /// <param name="basicUrl">Request Basic Url</param>
        /// <param name="argumentDic">Arguemtn Dictionary</param>
        /// <returns>Merge Url String</returns>
        public string MergeRequestArgument(string basicUrl, Dictionary<string, object> argumentDic)
        {
            string mergeUrl = basicUrl;
            if (argumentDic.Count == 0)
                return mergeUrl;
            else
            {
                int count = 0;
                foreach (KeyValuePair<string, object> queryPair in argumentDic)
                {
                    if (count == 0)
                        mergeUrl += "?" + queryPair.Key + "=" + queryPair.Value;
                    else
                        mergeUrl += "&" + queryPair.Key + "=" + queryPair.Value;
                    count++;
                }
            }
            return mergeUrl;
        }




        /// <summary>
        /// Get authorize config by platform type
        /// </summary>
        /// <param name="platformType">Soical PlatformType</param>
        /// <returns>Authorize Config Value</returns>
        public AuthorizeConfig GetAuthorizeConfig(PlatformType platformType)
        {
            AuthorizeConfig authorizeConfig = null;
            StreamResourceInfo configFile = Application.GetResourceStream(new Uri("/MoCommon;component/Social/SocialConfig.xml", UriKind.RelativeOrAbsolute));
            XDocument configDoc = XDocument.Load(configFile.Stream);
            XElement queryElement = null;

            #region read config content from xml file
            switch (platformType)
            {
                case PlatformType.Tencent:
                    queryElement = configDoc.Elements("social").First().Elements("tencent").First();
                    break;
                case PlatformType.Sina:
                    queryElement = configDoc.Elements("social").First().Elements("sina").First();
                    break;
                case PlatformType.WeChat:
                    queryElement = configDoc.Elements("social").First().Elements("wechat").First();
                    break;
            }
            authorizeConfig = new AuthorizeConfig()
            {
                PlatformType = platformType,
                AppKey = queryElement.Attributes().Single(x => x.Name == "appkey").Value,
                AppSecret = queryElement.Attributes().Single(x => x.Name == "appsecret").Value,
                Url = queryElement.Attributes().Single(x => x.Name == "url").Value,
                RedirectUrl = queryElement.Attributes().Single(x => x.Name == "redirecturl").Value,
                ApiUrl = queryElement.Attributes().Single(x => x.Name == "apiurl").Value,
                OauthApiUrl = queryElement.Attributes().Single(x => x.Name == "oauthapiurl").Value
            };
            #endregion
            return authorizeConfig;
        }




        /// <summary>
        /// Get authorize code by the different platfrom type
        /// </summary>
        /// <param name="platformType">Soical Platform</param>
        /// <param name="responseType">Response Type</param>
        /// <returns>Authorize Code</returns>
        public string GetAuthorizeCode(PlatformType platformType, string responseType)
        {
            AuthorizeConfig oauthConfig = GetAuthorizeConfig(platformType);    
            string requestUrl = string.Empty;

            if (platformType == PlatformType.Tencent)
                requestUrl = new TencentSocialHelper().GetAuthorizeCodeUrl(oauthConfig.Url, oauthConfig.AppKey, responseType, oauthConfig.RedirectUrl);
            else if (platformType == PlatformType.Sina)
                requestUrl = new SinaSocialHelper().GetAuthorizeCodeUrl(oauthConfig.Url, oauthConfig.AppKey, oauthConfig.RedirectUrl);
   
            return requestUrl;
        }



        /// <summary>
        /// Get request access Token by the different platform type
        /// </summary>
        /// <param name="platformType">Social Platform</param>
        /// <param name="grantType">Grant Type</param>
        /// <param name="code">Authorize Code</param>
        public void GetRequestAccessToken(PlatformType platformType,string grantType, string code)
        {
            AuthorizeConfig oauthConfig = GetAuthorizeConfig(platformType);
            DataRequestHelper dataReqeustHelper = new DataRequestHelper();

            string requestUrl = string.Empty;

            if (platformType == PlatformType.Tencent)
            {
                #region tencent accesstoken
                TencentSocialHelper tencentSocialHelper=new TencentSocialHelper();
                requestUrl = tencentSocialHelper.GetRequestAccessTokenUrl(oauthConfig.Url, oauthConfig.AppKey, oauthConfig.AppSecret, oauthConfig.RedirectUrl, grantType, code);
                if (tencentSocialHelper.PostArgumentList == null)
                    dataReqeustHelper.ExcuteAsyncRequest(requestUrl, Method.GET);
                else
                    dataReqeustHelper.ExcuteAsyncRequest(requestUrl, Method.GET, tencentSocialHelper.PostArgumentList);

                dataReqeustHelper.AsyncResponseComplated += (content, ex) =>
                {
                    #region get tencent accesstoken and save to local
                    if (!string.IsNullOrEmpty(content.ToString()))
                    {
                        string[] spileResponseArray = content.ToString().Split(new char[] {'=','&'});
                        AccessTokenData tencentToken = new AccessTokenData() {  PlatformType=PlatformType.Tencent};
                        for (int count = 0; count < spileResponseArray.Length; count++)
                        {
                            if (spileResponseArray[count] == "access_token")
                                tencentToken.AccessToken = spileResponseArray[count + 1];
                            else if (spileResponseArray[count] == "expires_in")
                                tencentToken.ExpiresIn = Convert.ToInt32(spileResponseArray[count + 1]);
                            else if (spileResponseArray[count] == "refresh_token")
                                tencentToken.RefreshToken = spileResponseArray[count + 1];
                            else if (spileResponseArray[count] == "openid")
                                tencentToken.OpenId = spileResponseArray[count + 1];
                        }
                        
                       //save to local
                        tencentToken.CreateDate = DateTime.Now;
                        IsolatedStorageHelper.IsolatedStorageSaveObject("tencenttoken", tencentToken);
                        if (AsyncAuthorizeComplated != null)
                            AsyncAuthorizeComplated(tencentToken, null);
                    }
                    #endregion
                };
                #endregion
            }
            else if (platformType == PlatformType.Sina)
            {
                #region sina accesstoken
                SinaSocialHelper sinaSocialHelper = new SinaSocialHelper();
                requestUrl = sinaSocialHelper.GetRequestAccessTokenUrl(oauthConfig.Url, oauthConfig.AppKey, oauthConfig.AppSecret, grantType, code, oauthConfig.RedirectUrl);
                if (sinaSocialHelper.PostArgumentList != null)
                    dataReqeustHelper.ExcuteAsyncRequest(requestUrl, Method.POST, sinaSocialHelper.PostArgumentList);
                else
                    dataReqeustHelper.ExcuteAsyncRequest(requestUrl, Method.POST);

                dataReqeustHelper.AsyncResponseComplated += (content, ex) =>
                {
                    if (!string.IsNullOrEmpty(content.ToString()))
                    {
                        #region spile sina accesstoken data
                        string[] spiltTokenArray = content.ToString().Split(new char[] { ':', ',', '{', '}' });
                        AccessTokenData sinaTokenData = new AccessTokenData() { PlatformType = PlatformType.Sina };
                        for (int count = 0; count < spiltTokenArray.Length; count++)
                        {
                            if (spiltTokenArray[count].Contains("access_token"))
                                sinaTokenData.AccessToken = spiltTokenArray[count + 1].Substring(1,spiltTokenArray[count+1].Length-2);
                            else if (spiltTokenArray[count].Contains("expires_in"))
                                sinaTokenData.ExpiresIn = Convert.ToInt32(spiltTokenArray[count + 1]);
                        }

                        //save to local
                        sinaTokenData.CreateDate = DateTime.Now;
                        IsolatedStorageHelper.IsolatedStorageSaveObject("sinatoken", sinaTokenData);
                        if (AsyncAuthorizeComplated != null)
                            AsyncAuthorizeComplated(sinaTokenData, null);
                        #endregion
                    }
                };
                #endregion
            }           
        }



        /// <summary>
        /// Send Pure text content to social platform 
        /// </summary>
        /// <param name="platformType">platform type</param>
        /// <param name="content">text content</param>
        public void SendPureTextContent(PlatformType platformType,string content)
        {
            AuthorizeConfig authorizeConfig = GetAuthorizeConfig(platformType);
            DataRequestHelper dataRequestHelper = new DataRequestHelper();

            string requestUrl = string.Empty;
            if (platformType == PlatformType.Tencent)
            {
                #region send pure text content to tencent platform
                AccessTokenData tencentToken = IsolatedStorageSettings.ApplicationSettings["tencenttoken"] as AccessTokenData;
                TencentSocialHelper tencentSocialHelper = new TencentSocialHelper();
                requestUrl = tencentSocialHelper.GetPureTextContentUrl(authorizeConfig.ApiUrl, authorizeConfig.AppKey, tencentToken.AccessToken, tencentToken.OpenId, content, FormatType.Json);
                if (tencentSocialHelper.PostArgumentList != null)
                    dataRequestHelper.ExcuteAsyncRequest(requestUrl, Method.POST, tencentSocialHelper.PostArgumentList);
                else
                    dataRequestHelper.ExcuteAsyncRequest(requestUrl, Method.POST);

                dataRequestHelper.AsyncResponseComplated += (responseData, ex) =>{
                    if (!string.IsNullOrEmpty(responseData.ToString()))
                    {
                        TencentTextResponseData pureRepData = JsonConvert.DeserializeObject<TencentTextResponseData>(responseData.ToString());
                        if (pureRepData.Msg.Trim().ToLower() == "ok" && pureRepData.ErrCode == "0")
                        {
                            //send text content success
                            if (AsyncSendContentComplated != null)
                                AsyncSendContentComplated("success", null);
                        }
                        else
                        {
                            //send text content failed
                            if (AsyncSendContentComplated != null)
                                AsyncSendContentComplated("fail",pureRepData.Msg);
                        }
                    }
                };
                #endregion
            }
            else if (platformType == PlatformType.Sina)
            {
                #region send pure text content to sina platform
                AccessTokenData sinaToken = IsolatedStorageSettings.ApplicationSettings["sinatoken"] as AccessTokenData;
                SinaSocialHelper sinaSocialHelper = new SinaSocialHelper();
                requestUrl = sinaSocialHelper.GetPureTextContentUrl(authorizeConfig.ApiUrl, sinaToken.AccessToken, content);
                if (sinaSocialHelper.PostArgumentList != null)
                    dataRequestHelper.ExcuteAsyncRequest(requestUrl, Method.POST, sinaSocialHelper.PostArgumentList);
                else
                    dataRequestHelper.ExcuteAsyncRequest(requestUrl, Method.POST);

                dataRequestHelper.AsyncResponseComplated += (responseData, ex) =>{
                    if (!string.IsNullOrEmpty(responseData.ToString()))
                    {
                        SinaTextErrorData sinaErrorData = JsonConvert.DeserializeObject<SinaTextErrorData>(responseData.ToString());
                        SinaTextResponseData sinaResponseData = JsonConvert.DeserializeObject<SinaTextResponseData>(responseData.ToString());
                        if (sinaErrorData.Error == null && sinaErrorData.Error_Code == null)
                        {
                            //send text content success
                            if (AsyncSendContentComplated != null)
                                AsyncSendContentComplated("success", null);

                        }
                        else
                        {   
                            //send text content failed
                            if (AsyncSendContentComplated != null)
                                AsyncSendContentComplated("fail", sinaErrorData.Error);
                        }
                    }                         
                };
                #endregion
            }
        }




        /// <summary>
        /// Send TExt Content With Picture to social platform 
        /// </summary>
        /// <param name="platformType">platfrom Type</param>
        /// <param name="content">Content TExt</param>
        /// <param name="picFileStream">Upload File Stream</param>
        public void SendTextWithPicContent(PlatformType platformType, string content, Stream picFileStream)
        {
            AuthorizeConfig authorizeConfig = GetAuthorizeConfig(platformType);
            DataRequestHelper dataRequestHelper = new DataRequestHelper();
            string requestUrl = string.Empty;

            //upload picture as text content
            byte[] picBytes = new byte[picFileStream.Length];
            picFileStream.Seek(0, SeekOrigin.Begin);
            picFileStream.Read(picBytes, 0, picBytes.Length);

            //notice : when you add fileparameter rename filename to api convert 
            List<FileParameter> uploadFileList = new List<FileParameter>() { FileParameter.Create("pic", picBytes, DateTime.Now.ToString("yyyyMMddHHmmss")) };

            if (platformType == PlatformType.Tencent)
            {
                #region send text with picture to tencent platform
                AccessTokenData tokenData = IsolatedStorageSettings.ApplicationSettings["tencenttoken"] as AccessTokenData;
                TencentSocialHelper tencentSocialHelper = new TencentSocialHelper();
                requestUrl = tencentSocialHelper.GetTextContentWithPicUrl(authorizeConfig.OauthApiUrl, authorizeConfig.AppKey, tokenData.AccessToken, tokenData.OpenId, content, FormatType.Json);

                if (tencentSocialHelper.PostArgumentList != null)
                    dataRequestHelper.ExcuteAsyncRequest(requestUrl, Method.POST, tencentSocialHelper.PostArgumentList, uploadFileList);
                else
                    dataRequestHelper.ExcuteAsyncRequest(requestUrl, Method.POST, null, uploadFileList);
                dataRequestHelper.AsyncResponseComplated += (responseData, ex) =>
                {
                    if (!string.IsNullOrEmpty(responseData.ToString()))
                    {
                        TencentTextResponseData uploadRepData = JsonConvert.DeserializeObject<TencentTextResponseData>(responseData.ToString());
                        if (uploadRepData.ErrCode == "0" && uploadRepData.Msg.Trim().ToLower() == "ok")
                        {
                            //send text with picture success
                            if (AsyncSendPictureComplated != null)
                                AsyncSendPictureComplated("success", null);
                        }
                        else
                        {
                            //send text with picture failed
                            if (AsyncSendPictureComplated != null)
                                AsyncSendPictureComplated("fail", uploadRepData.Msg);
                        }
                    }                   
                };
                #endregion
            }
            else if (platformType == PlatformType.Sina)
            {
                #region send text with picture to sina platform
                AccessTokenData sinaToken = IsolatedStorageSettings.ApplicationSettings["sinatoken"] as AccessTokenData;
                SinaSocialHelper sinaSocialHelper = new SinaSocialHelper();
                requestUrl = sinaSocialHelper.GetTextContentWithPicUrl(authorizeConfig.OauthApiUrl, sinaToken.AccessToken, content);

                if (sinaSocialHelper.PostArgumentList != null)
                    dataRequestHelper.ExcuteAsyncRequest(requestUrl, Method.POST, sinaSocialHelper.PostArgumentList, uploadFileList);
                else
                    dataRequestHelper.ExcuteAsyncRequest(requestUrl, Method.POST, null, uploadFileList);
                dataRequestHelper.AsyncResponseComplated += (responseData, ex) => 
                {
                    if (!string.IsNullOrEmpty(responseData.ToString()))
                    {
                        SinaTextErrorData sinaErrorData = JsonConvert.DeserializeObject<SinaTextErrorData>(responseData.ToString());
                        SinaTextResponseData sinaRepData = JsonConvert.DeserializeObject<SinaTextResponseData>(responseData.ToString());
                        if (sinaErrorData.Error == null && sinaErrorData.Error_Code == null)
                        {
                            //send text with picture success
                            if (AsyncSendPictureComplated != null)
                                AsyncSendPictureComplated("success", null);
                        }
                        else
                        { 
                            //send text with picture fail
                            if (AsyncSendPictureComplated != null)
                                AsyncSendPictureComplated("fail", sinaErrorData.Error);
                        }
                    }                  
                };
                #endregion
            }
        }



        /// <summary>
        /// Validate Access Token is invalid
        /// but if you local datetime then it was can be changed by user define
        /// </summary>
        /// <param name="platformType">Platform Type</param>
        /// <returns>is in valid</returns>
        public bool ValidateAccessTokenIsValid(PlatformType platformType)
        {
            bool isInValided = false;
            AccessTokenData accessTokenData = null;
            if (platformType == PlatformType.Tencent)
                accessTokenData = IsolatedStorageSettings.ApplicationSettings["tencenttoken"] as AccessTokenData;
            else if (platformType == PlatformType.Sina)
                accessTokenData = IsolatedStorageSettings.ApplicationSettings["sinatoken"] as AccessTokenData;

            DateTime endDatetime =accessTokenData.CreateDate.AddSeconds(accessTokenData.ExpiresIn);
            int compareValue = endDatetime.CompareTo(DateTime.Now);
            if (compareValue < 0)
                isInValided = true;//access token is invalid please refresh new one
            return isInValided;            
        }




        /// <summary>
        /// Cancel Social Platform Account Location Bind
        /// [if account accesstoken is exist and can be work then remove ]
        /// </summary>
        /// <param name="platformType">Platform Type</param>
        public void CancelSocialAccountBind(PlatformType platformType)
        {
            if (platformType==PlatformType.Tencent)
            {
                bool isTencentExist = IsolatedStorageHelper.CheckSaveValueIsExist("tencenttoken");
                if (isTencentExist && !ValidateAccessTokenIsValid(platformType))
                {
                    IsolatedStorageHelper.RemoveLocalSaveObject("tencenttoken");
                }
            }
            else if (platformType == PlatformType.Sina)
            {
                bool isSinaExist = IsolatedStorageHelper.CheckSaveValueIsExist("sinatoken");
                if (isSinaExist && !ValidateAccessTokenIsValid(platformType))
                {
                    IsolatedStorageHelper.RemoveLocalSaveObject("sinatoken");
                }
            }
        }



        /// <summary>
        /// When AccessToken is invalid then refresh new one replace 
        /// </summary>
        /// <param name="platformType">Platform Type</param>
        public void RefreshInValidAccessToken(PlatformType platformType)
        {
            AuthorizeConfig oauthConfig = GetAuthorizeConfig(platformType);
            DataRequestHelper dataRequestHelper = new DataRequestHelper();
            string requestUrl = string.Empty;

            if (platformType == PlatformType.Tencent)
            {
                #region refresh new accesstoke tencent accesstoken
                AccessTokenData tencentToken = IsolatedStorageSettings.ApplicationSettings["tencenttoken"] as AccessTokenData;
                TencentSocialHelper tencentSocialHelper = new TencentSocialHelper();
                requestUrl = tencentSocialHelper.GetRefreshAccessTokenUrl(oauthConfig.Url, oauthConfig.AppKey, tencentToken.RefreshToken);

                if (tencentSocialHelper.PostArgumentList != null)
                    dataRequestHelper.ExcuteAsyncRequest(requestUrl, Method.POST, tencentSocialHelper.PostArgumentList);
                else
                    dataRequestHelper.ExcuteAsyncRequest(requestUrl, Method.POST);
                dataRequestHelper.AsyncResponseComplated += (responseData, ex) =>
                {
                    if (!string.IsNullOrEmpty(responseData.ToString()))
                    {

                    }
                };
                #endregion
            }
            else if (platformType == PlatformType.Sina)
            { }
        }

    }

    public enum PlatformType
    {
        Tencent,
        Sina,
        WeChat
    }

    public enum FormatType
    {
        Json,
        Xml
    }
}
