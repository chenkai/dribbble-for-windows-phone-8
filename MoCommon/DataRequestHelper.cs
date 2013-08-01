using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Net;
using System.Net.Http;

//References from Microsoft HTTP Client Libraries 2.1.10
//more detail http://andywigley.com/tag/windows-phone-8/
//download link http://nuget.org/packages/Microsoft.Net.Http
//how to use it http://msdn.microsoft.com/en-us/library/hh193681(v=vs.110)
using System.Net.Http;
using System.Net.Http.Headers;

using RestSharp;
using RestSharp.Serializers;

namespace MoCommon
{
    public class DataRequestHelper
    {
        public event EventHandler AsyncResponseComplated;


        /// <summary>
        /// Excute request async method operator [httpclient version]
        /// </summary>
        /// <param name="requestUrl">Request Url</param>
        /// <param name="requestType">Request Type</param>
        /// <param name="postArguemntList">Post Argument List</param>
        public void ExcuteAsyncRequest(string requestUrl, RequestType requestType,List<KeyValuePair<string,object>> postArguemntList=null)
        {
            HttpClient requestClient = new HttpClient();
            if (requestType == RequestType.GET)
                requestClient.GetAsync(requestUrl).ContinueWith((postback) =>
                {
                    postback.Result.EnsureSuccessStatusCode();
                    if (AsyncResponseComplated != null)
                        AsyncResponseComplated(postback.Result.Content.ReadAsStringAsync().Result, null);

                });
            else if (requestType == RequestType.POST)
            {     
                HttpContent content=null;
                if (postArguemntList != null)
                {
                    List<KeyValuePair<string, string>> argumentList = null;
                    postArguemntList.ForEach(queryArgument => { argumentList.Add(new KeyValuePair<string,string>(queryArgument.Key,queryArgument.Value.ToString())); });
                    content = new FormUrlEncodedContent(argumentList);
                }
                requestClient.PostAsync(requestUrl, content).ContinueWith((postback) =>
                {
                    postback.Result.EnsureSuccessStatusCode();
                    if (AsyncResponseComplated != null)
                        AsyncResponseComplated(postback.Result.Content.ReadAsStringAsync().Result, null);
                });
            }
        }
        


        /// <summary>
        /// Excute Request async method operator [restshare version]
        /// </summary>
        /// <param name="requestUrl">request url</param>
        /// <param name="requestMethod">request method</param>
        /// <param name="postArgumentList">when use post method then argument collection</param>
        /// <param name="postFileList">upload files parameter collection</param>
        public void ExcuteAsyncRequest(string requestUrl, Method requestMethod, List<KeyValuePair<string, object>> postArgumentList = null,List<FileParameter> postFileList=null)
        {
            RestClient restClient = new RestClient(requestUrl);           
            RestRequest restRequest = new RestRequest(requestMethod);

            if (postArgumentList != null)
                postArgumentList.ForEach(queryArgument => {restRequest.AddParameter(queryArgument.Key, queryArgument.Value);});

            if (postFileList != null)
                postFileList.ForEach(queryFile => { restRequest.Files.Add(queryFile); });

            
            restClient.ExecuteAsync(restRequest, (respontData) => {
                if (AsyncResponseComplated != null)
                    AsyncResponseComplated(respontData.Content, null);
            });
        }



    }

    public enum RequestType
    {
        GET,
        POST
    }
}
