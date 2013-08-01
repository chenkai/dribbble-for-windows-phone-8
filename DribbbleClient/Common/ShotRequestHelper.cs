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

namespace DribbbleClient.Common
{
    public class ShotRequestHelper:BasicRequestHelper
    {
        public event ResponseHandler AsyncRequestComplated;    

        public void GetShotCommentById(int shotId,int pageIndex=0,int prePage=0)
        {
            if (shotId == 0)
                return;
            string requestUrl = GetRequestUrl("shots/" + shotId.ToString() + "/comments",pageIndex,prePage);
            DataRequestHelper dataRequestHelper = new DataRequestHelper();
            dataRequestHelper.ExcuteAsyncRequest(requestUrl, Method.GET, base._postArgumentList);
            dataRequestHelper.AsyncResponseComplated += (responseData, ex) => 
            {
                if (!string.IsNullOrEmpty(responseData.ToString()))
                {
                    if (!base.IsRateLimitedRequest(responseData.ToString()))
                    {
                        try
                        {
                            ShotComment commentDetailCol = JsonConvert.DeserializeObject<ShotComment>(responseData.ToString());
                            if (AsyncRequestComplated != null)
                                AsyncRequestComplated(commentDetailCol, null);
                        }
                        catch (Exception se)
                        {
                            if (AsyncRequestComplated != null)
                                AsyncRequestComplated("Json format error.try later", se); 
                        }
                    }
                    else
                    {
                        if (AsyncRequestComplated != null)
                            AsyncRequestComplated("Rate limited for a minute.", new Exception());
                    }
                }
            };
        }

        public void GetShotDetailById(int shotId)
        {
            string requestUrl = GetRequestUrl("shots/" + shotId);
            DataRequestHelper requestHelper = new DataRequestHelper();
            requestHelper.ExcuteAsyncRequest(requestUrl, Method.GET, base._postArgumentList);
            requestHelper.AsyncResponseComplated += (responseData, ex) => 
            {
                if (!string.IsNullOrEmpty(responseData.ToString()))
                {
                    if (!base.IsRateLimitedRequest(responseData.ToString()))
                    {
                        Shot shotDetail = JsonConvert.DeserializeObject<Shot>(responseData.ToString());
                        if (AsyncRequestComplated != null)
                            AsyncRequestComplated(shotDetail, null);
                    }
                    else
                    {
                        if (AsyncRequestComplated != null)
                            AsyncRequestComplated("Rate limited for a minute.", new Exception());
                    }
                }
            };
        }

    }
}
