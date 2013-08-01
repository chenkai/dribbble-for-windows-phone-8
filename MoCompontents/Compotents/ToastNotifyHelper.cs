using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media;
using Microsoft.Phone.Controls;
using Coding4Fun.Toolkit.Controls;
namespace MoCompontents.Compotents
{
    public class ToastNotifyHelper
    {
        /// <summary>
        /// User Coding4Fun Component build user define toast nofity 
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="title">Title</param>
        /// <param name="timeout">Time Out</param>
        /// <param name="foregroundColor">Forground Color</param>
        /// <param name="backgroundColor">Background Color</param>
        public void ShowCoding4FunToastNotify(string message, string title, int timeout = 2000, double fontSize = 20, SolidColorBrush forgroundColor = null, SolidColorBrush backgroundColor = null)
        {
            ToastPrompt toastPrompt = new ToastPrompt()
            {
                Message = message,
                Title = title,
                IsTimerEnabled = true,
                MillisecondsUntilHidden = timeout,
                IsAppBarVisible = true,
                FontSize = fontSize
            };

            if (forgroundColor == null)
                toastPrompt.Foreground = new SolidColorBrush(Colors.White);
            else
                toastPrompt.Foreground = forgroundColor;

            if (backgroundColor == null)
                toastPrompt.Background = new SolidColorBrush(new RgbConvertToColorHelper().GetColorFromHexString("1BA1E2")); //new SolidColorBrush(Colors.Green);
            else
                toastPrompt.Background = backgroundColor;

            toastPrompt.TextOrientation = System.Windows.Controls.Orientation.Horizontal;
            toastPrompt.Show();
        }
    }
}
