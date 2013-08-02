using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.IO.IsolatedStorage;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip;
using System.Windows.Resources;
using System.Windows.Media.Imaging;
using MoCommon.EntityModel;

namespace MoCommon.Component
{
    /// <summary>
    /// Request or post some content then need sharplib to small size 
    /// Author:chenkai date:2013年6月13日14:33:42
    /// about picture use silverlight sharpziplib [ http://slsharpziplib.codeplex.com/ ]
    /// or you control webclient GZip Compression for Windows Phone WebClient [ https://github.com/dotMorten/SharpGIS.GZipWebClient ]
    /// and you change small pix to save [http://www.developer.nokia.com/Community/Wiki/%E5%A6%82%E4%BD%95%E5%9C%A8Windows_Phone%E4%B8%AD%E5%8E%8B%E7%BC%A9%E5%9B%BE%E7%89%87%E5%B9%B6%E4%BF%9D%E5%AD%98]
    /// </summary>
    public class CompressLibHelper
    {

        public void CompressImageBySharpZibLib(Stream compressStream,string compressFileName)
        {
            string saveZipFilePath = DateTime.Now.ToString("yyyyMMddhhmmss")+"zipfile";
            using (IsolatedStorageFile isolateFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var streamOut = new ZipOutputStream(isolateFile.OpenFile(saveZipFilePath,FileMode.Create)))
                {
                    streamOut.SetLevel(9);
                    var streamIn = compressStream;

                    string newName = ZipEntry.CleanName(compressFileName);
                    ZipEntry compressEntity = new ZipEntry(newName);

                    compressEntity.Size = compressStream.Length;
                    compressEntity.DateTime = DateTime.Now;
                    streamOut.PutNextEntry(compressEntity);
                    streamIn.CopyTo(streamOut);
                    streamOut.Flush();
                    streamOut.Finish();

                    var convertValue = ((float)streamOut.Length / (float)streamIn.Length) * 100 + "%";
                }
            }
        }

        public Stream CompressImageToStream(BitmapSource compressSource,int height,int width)
        {
            var writeBmp = new WriteableBitmap(compressSource);
            MemoryStream controlStream = new MemoryStream();
            writeBmp.SaveJpeg(controlStream, width, height, 0, 100);
            return controlStream;
        }
        
    }
}
