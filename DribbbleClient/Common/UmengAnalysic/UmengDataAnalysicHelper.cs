using MoCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Resources;
using System.Xml.Linq;
using UmengSDK;

namespace DribbbleClient.Common.UmengAnalysic
{
   public class UmengDataAnalysicHelper
    {
        private readonly string SAVEUMENGKEY = "UmengKey";
        private string GetUmengAnalysicAppKey()
        {
            string appkey = string.Empty;
            StreamResourceInfo configFile = Application.GetResourceStream(new Uri("/DribbbleClient;component/Common/ClientConfig.xml", UriKind.RelativeOrAbsolute));
            XDocument configDoc = XDocument.Load(configFile.Stream);
            XElement queryElement = configDoc.Elements("client").First().Elements("umenganalysic").First();

            if (queryElement != null)
                appkey = queryElement.Attributes("appkey").First().Value.ToString();
            return appkey;
        }

        public string GetDataAnalysicAppKey()
        {
            string _umengAppKey = string.Empty;
            if (!IsolatedStorageHelper.CheckSaveValueIsExist(SAVEUMENGKEY))
            {
                _umengAppKey = GetUmengAnalysicAppKey();
                IsolatedStorageHelper.IsolatedStorageSaveObject(SAVEUMENGKEY, _umengAppKey);
            }
            else
                _umengAppKey = IsolatedStorageHelper.ReadSaveObjectByKey(SAVEUMENGKEY).ToString();

            return _umengAppKey;
        }

        public void AppLaunchingEvent(bool isDebug = false, params string[] umengChannel)
        {
            UmengSDK.UmengAnalytics.setDebug(isDebug);
            UmengSDK.UmengAnalytics.onLaunching(GetDataAnalysicAppKey(), umengChannel);
        }

        public void AppActivatedEvent(params string[] umengChannel)
        {
            UmengSDK.UmengAnalytics.onActivated(GetDataAnalysicAppKey(), umengChannel);
        }

        public void AppViewPageStart(string pageName)
        {
            if (!string.IsNullOrEmpty(pageName))
                UmengSDK.UmengAnalytics.onPageStart(pageName);
        }

        public void AppViewPageEnd(string pageName)
        {
            if (!string.IsNullOrEmpty(pageName))
                UmengSDK.UmengAnalytics.onPageEnd(pageName);
        }

        public void CustomerDefineEvent(string eventId, Dictionary<string, string> dic = null)
        {
            if (!string.IsNullOrEmpty(eventId))
                UmengSDK.UmengAnalytics.onEvent(eventId, dic);
        }

        public void AppExceptionReport(Exception ex)
        {
            if (ex != null)
                UmengSDK.UmengAnalytics.reportError(ex);
        }
    }
}
