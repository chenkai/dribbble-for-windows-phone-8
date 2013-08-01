using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace DribbbleClient.Common
{
    public class BasicRequestHelper
    {
        public string _basicRequestUrl = " http://api.dribbble.com/";
        public List<KeyValuePair<string, object>> _postArgumentList = null;
        public delegate void ResponseHandler(object responseObj, Exception se);

        public string MegerRequestCatalogUrl(ShotCatalog catalogType, int pageIndex, int prePage)
        {
            _basicRequestUrl += "shots/";
            switch (catalogType)
            {
                case ShotCatalog.Everyone:
                    _basicRequestUrl += ShotCatalog.Everyone.ToString().ToLower();
                    break;
                case ShotCatalog.Debuts:
                    _basicRequestUrl += ShotCatalog.Debuts.ToString().ToLower();
                    break;
                case ShotCatalog.Popular:
                    _basicRequestUrl += ShotCatalog.Popular.ToString().ToLower();
                    break;                   
            }

            if (_postArgumentList == null)
                _postArgumentList = new List<KeyValuePair<string, object>>();

            if (pageIndex != 0)
                _postArgumentList.Add(new KeyValuePair<string, object>("page", pageIndex));


            if (prePage > 0 && prePage <= 30)
                _postArgumentList.Add(new KeyValuePair<string, object>("per_page", prePage));

            return _basicRequestUrl;
        }

        public string GetRequestUrl(string appendUrl, int pageIndex=0, int prePage=0)
        {
            _basicRequestUrl += appendUrl;
            if (_postArgumentList == null)
                _postArgumentList = new List<KeyValuePair<string, object>>();

            if (pageIndex != 0)
                _postArgumentList.Add(new KeyValuePair<string, object>("page", pageIndex));


            if (prePage > 0 && prePage <= 30)
                _postArgumentList.Add(new KeyValuePair<string, object>("per_page", prePage));
            return _basicRequestUrl;
        }

        public string SpiltDateTimeStr(string timeSpanStr)
        {
            string formatStr = string.Empty;
            string[] timeArray = timeSpanStr.Split(new char[] { ' ' });
            if (timeArray.Length > 0)
            {
                string[] dateTimeArray = timeArray[0].Split(new char[] { '/' });
                if (dateTimeArray.Length >= 3)
                {
                    DateTime publishTime = new DateTime(Convert.ToInt32(dateTimeArray[0]), Convert.ToInt32(dateTimeArray[1]), Convert.ToInt32(dateTimeArray[2]));
                    formatStr += publishTime.ToString("MMM", CultureInfo.InvariantCulture) + " " + publishTime.Day.ToString() +" "+ publishTime.Year.ToString(); ;
                }
            }
            return formatStr;
        }

        public string SpiltDateTimeWithPM(string timeSpanStr)
        {
            string formatStr = string.Empty;
            string[] timeArray = timeSpanStr.Split(new char[] { ' ' });
            if (timeArray.Length > 0)
            {
                string[] dateTimeArray = timeArray[0].Split(new char[] { '/' });
                if (dateTimeArray.Length >= 3)
                {
                    DateTime publishTime = new DateTime(Convert.ToInt32(dateTimeArray[0]), Convert.ToInt32(dateTimeArray[1]), Convert.ToInt32(dateTimeArray[2]));
                    formatStr += publishTime.ToString("MMM", CultureInfo.InvariantCulture) + " " + publishTime.Day.ToString() + " " + publishTime.Year.ToString(); ;

                    formatStr += " " + timeArray[1];
                }
            }
            return formatStr;
        }

        //Rate limited for a minute.
        //Rate limited for a minute. 
        public bool IsRateLimitedRequest(string responsedata)
        {
            bool isLimited = false;
            if (string.IsNullOrEmpty(responsedata))
                return isLimited;

            if (responsedata.Equals("Rate limited for a minute.") && !IsContainJsonChar(responsedata))
                isLimited = true;
            return isLimited;
        }

        private bool IsContainJsonChar(string responseData)
        {
            bool isContain = false;
            if (string.IsNullOrEmpty(responseData))
                return isContain;

            if (responseData.Contains("{") || responseData.Contains("}") || responseData.Contains("[") || responseData.Contains("]"))
                isContain = true;
            return isContain;
        }
    }

    public enum ShotCatalog
    {
        Debuts,
        Everyone,
        Popular
    }
}
