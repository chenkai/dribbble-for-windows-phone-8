using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoCommon.EntityModel
{
   public class LocalDeviceInfo
    {
      
       public string ApplicationCurrentMemoryUsage { get; set; }
       public string ApplicationPeakMemoryUsage { get; set; }
       public string ApplicationWorkingSetLimit { get; set; }

       public string DeviceManufacturer { get; set; }
       public string DeviceName { get; set; }
       public string DeviceFirmwareVersion { get; set; }

       public string DeviceHardwareVersion { get; set; }
       public string DeviceTotalMemory { get; set; }
       public byte[]  DeviceUniqueId {get;set;}

       public bool IsApplicationPreinstalled { get; set; }
       public string OriginalMobileOperatorName { get; set; }
       public string DeviceOSVersion { get; set; }


    }
}
