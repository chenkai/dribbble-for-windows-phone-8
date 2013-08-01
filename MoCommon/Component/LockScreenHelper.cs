using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Phone.Shell;

namespace MoCommon.Component
{
    public class LockScreenHelper
    {
        public event EventHandler SetLockScreenExceptionChanged;

        /// <summary>
        /// Update Lock Screen Background Image 
        /// </summary>
        /// <param name="imageUrlPath">Image Url Path</param>
        /// <param name="isAppResource">Is App Resource</param>
        public async void AsyncUpdateLockScreenImage(string imageUrlPath, bool isAppResource)
        {
            try
            {
                var isProvider = Windows.Phone.System.UserProfile.LockScreenManager.IsProvidedByCurrentApplication;
                //tip out messagebox choose 
                /*if (!isProvider)
                {
                    var validatePrompt = await Windows.Phone.System.UserProfile.LockScreenManager.RequestAccessAsync();
                    isProvider = validatePrompt == Windows.Phone.System.UserProfile.LockScreenRequestResult.Granted;
                }*/

                if (isProvider)
                {
                    var schema = isAppResource ? "ms-appx:///" : "ms-appdata:///Local/";
                    var uri = new Uri(schema + imageUrlPath, UriKind.Absolute);
                    Windows.Phone.System.UserProfile.LockScreen.SetImageUri(uri);
                }
            }
            catch (Exception se)
            {
                if (SetLockScreenExceptionChanged != null)
                    SetLockScreenExceptionChanged(se, null);
            }
        }



        /// <summary>
        /// Update Local Screen Weather Info and Start Screen 
        /// </summary>
        /// <param name="wideBackContent">Wide Background Content</param>
        /// <param name="count">Notification Count</param>
        public void UpdateLockScreenAndFirstTitleInfo(string wideBackContent,int count=0)
        {
            ShellTile.ActiveTiles.First().Update(new FlipTileData() 
            {
                Count=count,
                WideBackContent=wideBackContent            
            });
        }




        /// <summary>
        /// Format Background Image File Path 
        /// </summary>
        /// <param name="imageFileName">Image File  Name</param>
        /// <returns>Format String </returns>
        public string FormatBackgroundImagePath(string imageFileName)
        {
            //WideBackgroundImageUrl = @"/Assets/background_wide/" + parseWeatherBg(wid) + ".jpg",
            return @"/Assets/background/" + imageFileName + ".jpg";
        }
    }
}
