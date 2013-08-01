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
    public partial class SettingView : PhoneApplicationPage
    {
        public SettingView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            new UmengRegisterEventHelper().RegisterUmengEventByType(AnalysicEventType.AppViewPageStart, "SettingView");
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            new UmengRegisterEventHelper().RegisterUmengEventByType(AnalysicEventType.AppViewPageEnd, "SettingView");
        }
    }
}