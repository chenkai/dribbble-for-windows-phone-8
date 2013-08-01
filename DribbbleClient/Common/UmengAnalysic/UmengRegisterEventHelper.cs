using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DribbbleClient.Common.UmengAnalysic
{
    public class UmengRegisterEventHelper : UmengDataAnalysicHelper
    {
        /// <summary>
        /// Register Umeng Event By Catalog Type
        /// </summary>
        /// <param name="eventInfo">EventInfo</param>
        /// <param name="exceptionObj">Exception Error Obj</param>
        /// <param name="pageName">Page Name</param>
        public void RegisterUmengEventByType(AnalysicEventType analysicEventType,string pageName = "",  Exception exceptionObj = null)
        {
            switch (analysicEventType)
            {
                case AnalysicEventType.AppActivated:
                    AppActivatedEvent();
                    break;
                case AnalysicEventType.AppExceptionReport:
                    if (exceptionObj != null)
                        AppExceptionReport(exceptionObj);
                    break;
                case AnalysicEventType.AppLaunching:
                    base.AppLaunchingEvent();
                    break;
                case AnalysicEventType.AppViewPageEnd:
                    if (!string.IsNullOrEmpty(pageName))
                        AppViewPageStart(pageName);
                    break;
                case AnalysicEventType.AppViewPageStart:
                    if (!string.IsNullOrEmpty(pageName))
                        AppViewPageEnd(pageName);
                    break;               
            }
        }
    }


    public enum AnalysicEventType
    {
        AppLaunching,
        AppActivated,
        AppViewPageStart,
        AppViewPageEnd,
        AppExceptionReport,
        CustomerDefineEvent
    }


}
