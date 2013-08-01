using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Phone.Tasks;
using Microsoft.Phone.Shell;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows.Media.Imaging;
using System.Windows;
using ImageTools;
using ImageTools.Helpers;
using ImageTools.IO;


using System.Windows.Controls;

namespace MoCommon.Component
{
    public class CustomLiveTitleHelper
    {
        public static readonly string SaveBackgroundImagePath ="Shared/ShellContent/titles"+@"/"+"backgroundimage.png";


        public ShellTileData CreateFilpTitleData(string title,string backTitle,string backContent,string wideBackContent,
           string smallBackgroundImage,string backgroundImage,string wideBackgroundImage, string backBackgroundImage="", string wideBackBackgroundImage="", int count=0)
        {
          return new  FlipTileData()
          {
              Title=title,
              BackTitle=backTitle,
              BackContent=backContent,
              WideBackContent=wideBackContent,
              SmallBackgroundImage=new Uri(smallBackgroundImage,UriKind.RelativeOrAbsolute),
              BackgroundImage=new Uri(backgroundImage,UriKind.RelativeOrAbsolute),
              WideBackgroundImage=new Uri(wideBackgroundImage,UriKind.RelativeOrAbsolute),
              BackBackgroundImage=new Uri(backBackgroundImage,UriKind.RelativeOrAbsolute),
              WideBackBackgroundImage=new Uri(wideBackBackgroundImage,UriKind.RelativeOrAbsolute)              
          };
        }


        public bool CheckLiveTitleExistByUrl(string urlStr)
        {
            bool isExist = false;
            ShellTile indexTitle = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(urlStr));
            if (indexTitle != null)
                isExist = true;
            return isExist;
        }


        public void UpdateFilpLiveTitle(string navigationUrl, string title, string backTitle, string backContent, string wideBackContent,
           string smallBackgroundImage, string backgroundImage, string wideBackgroundImage, string backBackgroundImage, string wideBackBackgroundImage)
        {
            if (CheckLiveTitleExistByUrl(navigationUrl))
            {
                //Exist title and update
                ShellTile indexTitle = ShellTile.ActiveTiles.FirstOrDefault(x => x.NavigationUri.ToString().Contains(navigationUrl));
                ShellTileData titleData = CreateFilpTitleData(title, backTitle, backContent, wideBackContent, smallBackgroundImage, backgroundImage, wideBackgroundImage,
                    backBackgroundImage, wideBackBackgroundImage);
                indexTitle.Update(titleData);       
            }
        }


        public void SaveCaptureImageToLocal(FrameworkElement saveElement)
        {

            WriteableBitmap writeBitmap = new WriteableBitmap(saveElement,null); 
            string thirdDicUrl = "Shared/ShellContent/titles";
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!store.DirectoryExists(thirdDicUrl))      
                    store.CreateDirectory(thirdDicUrl);

                using (var writer = new StreamWriter(new IsolatedStorageFileStream(SaveBackgroundImagePath, FileMode.Create, FileAccess.Write, store)))
                {
                    var pngEncoder = new ImageTools.IO.Png.PngEncoder();
                    pngEncoder.Encode(writeBitmap.ToImage(), writer.BaseStream);   
  
                    //MemoryStream controlStream = new MemoryStream();
                    //writeBitmap.SaveJpeg(controlStream,691,336, 0, 100);
                    //writeBitmap.SetSource(controlStream);
                    //writeBitmap.WritePNG(writer.BaseStream);
                }
            }
        }

    }
}
