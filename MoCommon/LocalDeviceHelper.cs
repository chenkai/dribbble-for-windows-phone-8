using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;
using System.Net;
using System.Net.Http;
using MoCommon.EntityModel;
using Microsoft.Phone.Info;
using Microsoft.Phone.Net.NetworkInformation;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;

namespace MoCommon
{
   public class LocalDeviceHelper
    {
       public event EventHandler GetNetWorkIpComplated;
       public readonly string _netWorkSaveKey = "network";

       /// <summary>
       /// Get Windows phone network ip address
       /// you can check this out : Getting the local IP address for a socket/network interface 
       /// [ http://stackoverflow.com/questions/6968867/getting-the-local-ip-address-for-a-socket-network-interface]
       /// or visit the http://www.whatismyip.org/ have same result
       /// </summary>
       public void GetNetworkIpAddress()
       {
           string IPPATTERN = "\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}\\.\\d{1,3}";
           WebClient client = new WebClient();
           client.DownloadStringCompleted += (s, e) =>
           {
               if (e.Error == null)
               {
                   string result = e.Result;
                   Regex ipRegex = new Regex(IPPATTERN);
                   var matchesCollection = ipRegex.Matches(result);
                   if (matchesCollection.Count != 0)
                   {
                       string IP = matchesCollection[0].Value;
                       if (GetNetWorkIpComplated != null)
                           GetNetWorkIpComplated(IP, null);
                   }
               }
           };
           client.DownloadStringAsync(new Uri("http://whatismyipaddress.com/", UriKind.Absolute));
       }



       /// <summary>
       /// Get Device Location Information
       /// </summary>
       /// <returns>Local Device Infor Entity</returns>
       public LocalDeviceInfo GetDeviceLocalInfo()
       {
           LocalDeviceInfo deviceInfo = new LocalDeviceInfo();
           deviceInfo.ApplicationCurrentMemoryUsage =DeviceExtendedProperties.GetValue("ApplicationCurrentMemoryUsage").ToString();
           deviceInfo.ApplicationPeakMemoryUsage = DeviceExtendedProperties.GetValue("ApplicationPeakMemoryUsage").ToString();
           deviceInfo.ApplicationWorkingSetLimit = DeviceExtendedProperties.GetValue("ApplicationWorkingSetLimit").ToString();

           deviceInfo.DeviceFirmwareVersion = DeviceExtendedProperties.GetValue("DeviceFirmwareVersion").ToString();
           deviceInfo.DeviceHardwareVersion = DeviceExtendedProperties.GetValue("DeviceHardwareVersion").ToString();
           deviceInfo.DeviceManufacturer = DeviceExtendedProperties.GetValue("DeviceManufacturer").ToString();

           deviceInfo.DeviceName = DeviceExtendedProperties.GetValue("DeviceName").ToString();
           deviceInfo.DeviceTotalMemory = DeviceExtendedProperties.GetValue("DeviceTotalMemory").ToString();

           deviceInfo.DeviceUniqueId =(byte[])DeviceExtendedProperties.GetValue("DeviceUniqueId");
           deviceInfo.DeviceOSVersion = GetDeviceOSVersion();
           return deviceInfo;
       }


       /// <summary>
       /// Get Device Operator System Version
       /// </summary>
       /// <returns>Version Format String</returns>
       public string GetDeviceOSVersion()
       {
           string deviceOsVersion = string.Empty;
           OperatingSystem operatorSystem = Environment.OSVersion;
           Version systemVersion = operatorSystem.Version;
           
           //deviceOsVersion = operatorSystem.Platform.ToString() + ":" + systemVersion.ToString();
           deviceOsVersion = systemVersion.ToString();
           return deviceOsVersion;
       }


       /// <summary>
       /// Get Device Unique Id
       /// windows phone system does't have any api can get imei
       /// </summary>
       /// <returns>Unique Id</returns>
       public string GetDeviceUniqueId()
       {
           object uniqueId;
           var hexString = string.Empty;
           if (DeviceExtendedProperties.TryGetValue("DeviceUniqueId", out uniqueId))
               hexString = BitConverter.ToString((byte[])uniqueId).Replace("-", string.Empty);
           return hexString;
       }



       /// <summary>
       /// Check Current NewWork is Avaliable
       /// </summary>
       /// <returns>Is Connect To NetWork</returns>
       public NetWorkInfo CheckNewWorkIsAvailable()
       {
           bool isConnectNet = Microsoft.Phone.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
           bool isWifi = DeviceNetworkInformation.IsWiFiEnabled;
           NetWorkInfo currentNetWorkInfo = new NetWorkInfo() { IsConnectNetWork = isConnectNet, IsWifi = isWifi };
           IsolatedStorageHelper.IsolatedStorageSaveObject(_netWorkSaveKey, currentNetWorkInfo);

           NetworkChange.NetworkAddressChanged += (sender, ex) => 
           {
               NetWorkInfo changeNetWork = new NetWorkInfo() 
               {
                   IsConnectNetWork=DeviceNetworkInformation.IsNetworkAvailable,
                   IsWifi=DeviceNetworkInformation.IsWiFiEnabled
               };
               IsolatedStorageHelper.IsolatedStorageSaveObject(_netWorkSaveKey, changeNetWork);
           };
           return currentNetWorkInfo;     
       }


       [DllImport("wininet.dll")]
       private extern static bool InternetGetConnectedState(out int Description, int ReservedValue);
       public bool IsConnectedToInternet()
       {
           int Desc;
           return InternetGetConnectedState(out Desc, 0);
       }


       /// <summary>
       /// Check Current Device Have Network 
       /// </summary>
       /// <returns>Is Connect to Internet</returns>
       public bool CheckNetWorkStatus()
       {
           return (Microsoft.Phone.Net.NetworkInformation.NetworkInterface.NetworkInterfaceType !=
                   Microsoft.Phone.Net.NetworkInformation.NetworkInterfaceType.None);
       }

    }

   public enum NetWorkType
   {
        None
   }
}
