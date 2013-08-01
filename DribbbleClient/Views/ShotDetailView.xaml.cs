using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using DribbbleClient.ViewsModels;
using DribbbleClient.Common.UmengAnalysic;
using DribbbleClient.EntityModels;

namespace DribbbleClient.Views
{
    public partial class ShotDetailView : PhoneApplicationPage
    {
        public ShotDetailView()
        {
            InitializeComponent();
            this.Loaded += ShotDetailView_Loaded;
        }

        ShotDetailViewModel _shotDetailViewModel = null;
        void ShotDetailView_Loaded(object sender, RoutedEventArgs e)
        {
            if (this._shotDetailViewModel == null)
                _shotDetailViewModel = new ShotDetailViewModel(this._shotId);
            this.DataContext = _shotDetailViewModel;
        }

        public int _shotId;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //append umeng register event
           new UmengRegisterEventHelper().RegisterUmengEventByType(AnalysicEventType.AppViewPageStart, "ShotDetailView");

           IDictionary<string,string> arguemntDic=this.NavigationContext.QueryString;
           if (arguemntDic.Count > 0)
           {
               foreach (KeyValuePair<string, string> argumentPair in arguemntDic)
               {
                   if (argumentPair.Key.Equals("shotid"))
                       this._shotId = Convert.ToInt32(argumentPair.Value);
               }
           }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            new UmengRegisterEventHelper().RegisterUmengEventByType(AnalysicEventType.AppViewPageEnd, "ShotDetailView");
        }

        private void UserProfile_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            string username = this._shotDetailViewModel.ShotDetail.Player.Username;
            if (string.IsNullOrEmpty(username))
                return;

            this.NavigationService.Navigate(new Uri("/Views/UserProfileView.xaml?username="+username, UriKind.RelativeOrAbsolute));
        }

        private void ShotDetail_PT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_shotDetailViewModel == null)
                return;

            PivotItem selectItem = this.ShotDetail_PT.SelectedItem as PivotItem;
            if (selectItem == null)
                return;
           
                switch (selectItem.Header.ToString())
                {
                    case "Detail":
                        _shotDetailViewModel.GetShotDetail(_shotId);
                        break;
                    case "Comment":
                        _shotDetailViewModel.GetShotDetailComments(_shotId, 1, 15);
                        break;
                }           
        }

        private void ShotCommentTemplate_GetUserProfileUp(object sender, EventArgs e)
        {
            Comment currentComment = sender as Comment;
            if (currentComment == null)
                return;

            if(currentComment.Player!=null)
               this.NavigationService.Navigate(new Uri("/Views/UserProfileView.xaml?username=" + currentComment.Player.Username, UriKind.RelativeOrAbsolute));
        }

        private void Image_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Shot detailShot = this._shotDetailViewModel.ShotDetail;
            if (detailShot == null)
                return;
            this.NavigationService.Navigate(new Uri("/Views/ShotImageView.xaml?url="+detailShot.Image_url, UriKind.RelativeOrAbsolute));
        }
    }
}