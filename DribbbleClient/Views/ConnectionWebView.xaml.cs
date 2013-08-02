using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MoCommon;

namespace DribbbleClient.Views
{
    public partial class ConnectionWebView : PhoneApplicationPage
    {
        public ConnectionWebView()
        {
            InitializeComponent();
            this.Loaded += ConnectionWebView_Loaded;
        }

        void ConnectionWebView_Loaded(object sender, RoutedEventArgs e)
        {
           object connectObj=IsolatedStorageHelper.ReadSaveObjectByKey("connecturl");
           if (connectObj == null)
               return;

          
        }

        string _connectType = string.Empty;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            IDictionary<string, string> argumentDic = this.NavigationContext.QueryString;
            if (argumentDic.Count > 0)
            {
                foreach (KeyValuePair<string, string> argumentPair in argumentDic)
                {
                    if (argumentPair.Key.Equals("type"))
                    {
                        _connectType = argumentPair.Value.ToString();
                        break;
                    }
                }
            }
        }
    }
}