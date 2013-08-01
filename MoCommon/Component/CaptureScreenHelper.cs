using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Windows;
using System.Windows.Controls;

using System.Windows.Media.Imaging;

namespace MoCommon.Component
{
   public class CaptureScreenHelper
    {
       public Stream CaptureScrennSaveToStream(FrameworkElement captureElement)
       {
           var writeBitmap = new WriteableBitmap(captureElement, null);

           MemoryStream controlStream = new MemoryStream();        
           writeBitmap.SaveJpeg(controlStream, Convert.ToInt32(captureElement.ActualWidth),Convert.ToInt32(captureElement.ActualHeight), 0, 100);
           controlStream.Seek(0, SeekOrigin.Begin);

              
           return controlStream;
       }
    }
}
