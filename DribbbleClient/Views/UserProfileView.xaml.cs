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
    public partial class UserProfileView : PhoneApplicationPage
    {
        public UserProfileView()
        {
            InitializeComponent();
            this.Loaded += UserProfileView_Loaded;
        }

        public UserProfileViewModel _userProfileViewModel = null;
        void UserProfileView_Loaded(object sender, RoutedEventArgs e)
        {
            if (_userProfileViewModel == null)
                this._userProfileViewModel = new UserProfileViewModel(this._username);
            this.DataContext = _userProfileViewModel;
        }

        private string _username;
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //append umeng event register 
            new UmengRegisterEventHelper().RegisterUmengEventByType(AnalysicEventType.AppViewPageStart, "UserProfileView");

            IDictionary<string, string> argumentDic = this.NavigationContext.QueryString;
            if (argumentDic.Count > 0)
            {
                foreach (KeyValuePair<string, string> argumentPair in argumentDic)
                {
                    if (argumentPair.Key.Equals("username"))
                    {
                        this._username = argumentPair.Value.ToString();
                        break;
                    }
                }
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            new UmengRegisterEventHelper().RegisterUmengEventByType(AnalysicEventType.AppViewPageEnd, "UserProfileView");
        }

        private void UserProfile_PT_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_userProfileViewModel == null)
                return;

            PivotItem selectItem = this.UserProfile_PT.SelectedItem as PivotItem;
            if (selectItem == null)
                return;

            switch (selectItem.Header.ToString())
            {
                case "Profile":
                    _userProfileViewModel.GetPlayerDetailById(_username);
                    break;
                case "Shots":
                    _userProfileViewModel.GetPlayerRecentShots(_username, 1, 10);
                    break;
                case "Following":
                    _userProfileViewModel.GetPlayerFollowing(_username, 1, 10);
                    break;
                case "Followers":
                    _userProfileViewModel.GetPlayerFollowers(_username, 1, 10);
                    break;
            }
        }

        private void FriendsDetailTemplate_GetUserProfileUp(object sender, EventArgs e)
        {
            Player currentPlayer = sender as Player;
            if (currentPlayer == null)
                return;

            this.NavigationService.Navigate(new Uri("/Views/UserProfileView.xaml?username=" + currentPlayer.Username, UriKind.RelativeOrAbsolute));
        }

        private void StackPanel_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Shot selectShot = this.UserShots_LP.SelectedItem as Shot;
            if (selectShot == null)
                return;

            this.NavigationService.Navigate(new Uri("/Views/ShotDetailView.xaml?shotid=" + selectShot.Id, UriKind.RelativeOrAbsolute));
        }

    
    }
}
    
