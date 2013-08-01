using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MoCommon.Component;
using MoCommon;
using RestSharp;
using Newtonsoft.Json;
using DribbbleClient.EntityModels.ShotDetail;
using DribbbleClient.EntityModels;
using DribbbleClient.EntityModels.User;

namespace DribbbleClient.Common
{
    public class UserRequestHelper:BasicRequestHelper
    {
        public event EventHandler AsyncUserProfileComplated;

        public void GetUserProfileByUserName(string username)
        {
            string requestUrl = GetRequestUrl("players/" + username + "/");
            DataRequestHelper dataRequestHelper = new DataRequestHelper();
            dataRequestHelper.ExcuteAsyncRequest(requestUrl, Method.GET, _postArgumentList);
            dataRequestHelper.AsyncResponseComplated += (responseData, ex) => 
            {
                if (!string.IsNullOrEmpty(responseData.ToString()))
                {
                    if (!base.IsRateLimitedRequest(responseData.ToString()))
                    {
                        Player playerDetail = JsonConvert.DeserializeObject<Player>(responseData.ToString());
                        if (playerDetail != null)
                        {
                            if (!string.IsNullOrEmpty(playerDetail.Message))
                                playerDetail.IsFindUser = false;
                            else
                                playerDetail.IsFindUser = true;
                        }
                        if (AsyncUserProfileComplated != null)
                            AsyncUserProfileComplated(playerDetail, null);
                    }
                    else
                    {
                        if (AsyncUserProfileComplated != null)
                            AsyncUserProfileComplated("Rate limited for a minute.", new EventArgs());
                    }
                }
            };
        }

        public void GetUserMostRecentShots(string username,int pageIndex,int prePage)
        {
            string requestUrl = GetRequestUrl("players/" + username + "/shots", pageIndex, prePage);
            DataRequestHelper requestHelper = new DataRequestHelper();
            requestHelper.ExcuteAsyncRequest(requestUrl, Method.GET, _postArgumentList);
            requestHelper.AsyncResponseComplated += (responseData, ex) => 
            {
                if (!string.IsNullOrEmpty(responseData.ToString()))
                {
                    if (!base.IsRateLimitedRequest(responseData.ToString()))
                    {
                        try
                        {
                            UserRecentShot recentShots = JsonConvert.DeserializeObject<UserRecentShot>(responseData.ToString());
                            if (AsyncUserProfileComplated != null)
                                AsyncUserProfileComplated(recentShots, null);
                        }
                        catch (Exception se)
                        {
                            if (AsyncUserProfileComplated != null)
                                AsyncUserProfileComplated("Json format error.try later", new EventArgs());
                        }
                    }
                    else
                    {
                        if (AsyncUserProfileComplated != null)
                            AsyncUserProfileComplated("Rate limited for a minute.", new EventArgs());
                    }
                }
            };
        }

        public void GetUserFollowingData(string username, int pageIndex, int prePage)
        {
            string requestUrl = GetRequestUrl("players/" + username + "/following", pageIndex, prePage);
            DataRequestHelper requestHelper = new DataRequestHelper();
            requestHelper.ExcuteAsyncRequest(requestUrl, Method.GET, _postArgumentList);
            requestHelper.AsyncResponseComplated += (responseData, Ex) => 
            {
                if (!string.IsNullOrEmpty(responseData.ToString()))
                {
                    if (!base.IsRateLimitedRequest(responseData.ToString()))
                    {
                        try
                        {
                            UserFollowing followingData = JsonConvert.DeserializeObject<UserFollowing>(responseData.ToString());
                            if (AsyncUserProfileComplated != null)
                                AsyncUserProfileComplated(followingData, null);
                        }
                        catch (Exception se)
                        {
                            if (AsyncUserProfileComplated != null)
                                AsyncUserProfileComplated("Json format error.try later", new EventArgs());
                        }
                    }
                    else
                    {
                        if (AsyncUserProfileComplated != null)
                            AsyncUserProfileComplated("Rate limited for a minute.", new EventArgs());
                    }
                }                    
            };
        }

        public void GetUserFollowerData(string username, int pageIndex, int prePage)
        {
            string requestUrl = GetRequestUrl("players/" + username + "/followers", pageIndex, prePage);
            DataRequestHelper requestHelper = new DataRequestHelper();
            requestHelper.ExcuteAsyncRequest(requestUrl, Method.GET, _postArgumentList);
            requestHelper.AsyncResponseComplated += (responseData, Ex) =>
            {
                if (!string.IsNullOrEmpty(responseData.ToString()))
                {
                    if (!base.IsRateLimitedRequest(responseData.ToString()))
                    {
                        try
                        {
                            UserFollower followingData = JsonConvert.DeserializeObject<UserFollower>(responseData.ToString());
                            if (AsyncUserProfileComplated != null)
                                AsyncUserProfileComplated(followingData, null);
                        }
                        catch (Exception se)
                        {
                            if (AsyncUserProfileComplated != null)
                                AsyncUserProfileComplated("Json format error.try later", new EventArgs());
                        }
                    }
                    else
                    {
                        if (AsyncUserProfileComplated != null)
                            AsyncUserProfileComplated("Rate limited for a minute.", new EventArgs());
                    }
                }
            };
        }

    }
}
