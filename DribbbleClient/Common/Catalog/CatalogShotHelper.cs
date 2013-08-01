using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MoCommon;
using MoCommon.Component;
using RestSharp;
using Newtonsoft.Json;
using DribbbleClient.EntityModels.ShotCatalog;

namespace DribbbleClient.Common.Catalog
{
    public class CatalogShotHelper:BasicRequestHelper
    {
        public event ResponseHandler AsyncCatalogShotsComplated;

        public void GetCatalogShots(ShotCatalog shotCatalog, int pageIndex=0, int perPage=0)
        {
            string requestUrl = base.MegerRequestCatalogUrl(shotCatalog, pageIndex, perPage);
            DataRequestHelper requestHelper = new DataRequestHelper();
            requestHelper.ExcuteAsyncRequest(requestUrl, Method.GET, base._postArgumentList);
            requestHelper.AsyncResponseComplated += (responseData, ex) => 
            {
                if (!string.IsNullOrEmpty(responseData.ToString()))
                {
                    if (!base.IsRateLimitedRequest(responseData.ToString()))
                    {
                        try
                        {
                            CatalogShots catalogShots = JsonConvert.DeserializeObject<CatalogShots>(responseData.ToString());
                            if (AsyncCatalogShotsComplated != null)
                                AsyncCatalogShotsComplated(catalogShots, null);
                        }
                        catch (Exception se)
                        {
                            if (AsyncCatalogShotsComplated != null)
                                AsyncCatalogShotsComplated("Json format error.try later", se);
                        }
             
                    }
                    else
                    {
                        if (AsyncCatalogShotsComplated != null)
                            AsyncCatalogShotsComplated("Rate limited for a minute.", new Exception());
                    }
                }
            };
        }
    }
}
