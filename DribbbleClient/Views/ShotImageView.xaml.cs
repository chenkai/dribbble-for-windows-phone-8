using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using DribbbleClient.Common.UmengAnalysic;

namespace DribbbleClient.Views
{
    public partial class ShotImageView : PhoneApplicationPage
    {
        public ShotImageView()
        {
            InitializeComponent();
            this.Loaded += ShotImageView_Loaded;
        }

        string _arguemntUrl = string.Empty;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            IDictionary<string, string> argumengDic = this.NavigationContext.QueryString;
            if (argumengDic.Count > 0)
            {
                foreach (KeyValuePair<string, string> argumentPair in argumengDic)
                {
                    if (argumentPair.Key.Equals("url"))
                    {
                        this._arguemntUrl = argumentPair.Value.ToString();
                        break;
                    }
                }
            }
            new UmengRegisterEventHelper().RegisterUmengEventByType(AnalysicEventType.AppViewPageStart, "ShotImageView");
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            new UmengRegisterEventHelper().RegisterUmengEventByType(AnalysicEventType.AppViewPageEnd, "ShotImageView");
        }

        void ShotImageView_Loaded(object sender, RoutedEventArgs e)
        {
            this.ImageView_WB.Navigate(new Uri(_arguemntUrl, UriKind.RelativeOrAbsolute));

        }

        private void SaveToLocal_AP_Click(object sender, EventArgs e)
        {

        }
    }
}