using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DribbbleClient.EntityModels;
using DribbbleClient.Common;
using DribbbleClient.EntityModels.User;
using System.Collections.ObjectModel;
using DribbbleClient.EntityModels.ShotDetail;
using MoCommon;
using MoCompontents.Compotents;
using DribbbleClient.Common.UmengAnalysic;

namespace DribbbleClient.ViewsModels
{
    public class UserProfileViewModel : BasicViewModel
    {
        private Player _playerDetail;
        public Player PlayerDetail
        {
            get { return this._playerDetail; }
            set
            {
                this._playerDetail = value;
                base.BindPropertyNotifyChanged("PlayerDetail");
            }
        }

        private ObservableCollection<Shot> _userRecentShotsCol = new ObservableCollection<Shot>();
        public ObservableCollection<Shot> UserRecentShotCol
        {
            get { return this._userRecentShotsCol; }
            set
            {
                this._userRecentShotsCol = value;
                base.BindPropertyNotifyChanged("UserRecentShotCol");
            }
        }

        private ObservableCollection<Player> _userFollowingCol = new ObservableCollection<Player>();
        public ObservableCollection<Player> UserFollowingCol
        {
            get { return this._userFollowingCol; }
            set
            {
                this._userFollowingCol = value;
                base.BindPropertyNotifyChanged("UserFollowingCol");
            }
        }

        private ObservableCollection<Player> _userFollowersCol = new ObservableCollection<Player>();
        public ObservableCollection<Player> UserFollowersCol
        {
            get { return this._userFollowersCol; }
            set
            {
                this._userFollowersCol = value;
                base.BindPropertyNotifyChanged("UserFollowersCol");
            }
        }

        private LoadProcessBarHelper _processBarHelper = new LoadProcessBarHelper();

        public UserProfileViewModel(string username)
        {
            if (string.IsNullOrEmpty(username))
                return;

            if (new LocalDeviceHelper().CheckNetWorkStatus())
                GetPlayerDetailById(username);
            else
                base.NetworkIsInvalid();
        }

        public void GetPlayerDetailById(string username)
        {
            //start processbar
            _processBarHelper.StartProcessBar();

            UserRequestHelper userProfileHelper = new UserRequestHelper();
            userProfileHelper.GetUserProfileByUserName(username);
            userProfileHelper.AsyncUserProfileComplated += (responseData, Ex) => 
            {
                //end processbar
                _processBarHelper.EndProcessBar();

                if (Ex == null)
                {
                    #region get user profile
                    Player playerDetail = null;
                    if (responseData != null)
                        playerDetail = responseData as Player;

                    if (playerDetail != null)
                        this.PlayerDetail = playerDetail;
                    #endregion
                }
                else
                {
                    string errorMessage = responseData.ToString();
                    if (!string.IsNullOrEmpty(errorMessage))
                        new ToastNotifyHelper().ShowCoding4FunToastNotify(errorMessage, "Tip");
                }
            };
        }

        public void GetPlayerRecentShots(string username,int pageIndex,int prePage)
        {    
            //start processbar
            _processBarHelper.StartProcessBar();

            UserRequestHelper userProfileHelper = new UserRequestHelper();
            userProfileHelper.GetUserMostRecentShots(username, pageIndex, prePage);
            userProfileHelper.AsyncUserProfileComplated += (responseData, ex) => 
            {
                //end processbar
                _processBarHelper.EndProcessBar();

                if (ex == null)
                {
                    #region get use most recent shot
                    UserRecentShot recentShots = null;
                    if (responseData != null)
                        recentShots = responseData as UserRecentShot;

                    if (recentShots != null)
                    {
                        _userRecentShotsCol.Clear();
                        if (recentShots.Shots.Count > 0)
                            recentShots.Shots.ForEach(queryEntity =>
                            {
                                #region get shot first author comment detail body
                                ShotRequestHelper shotHelper = new ShotRequestHelper();
                                shotHelper.GetShotCommentById(queryEntity.Id, 1, 1);
                                shotHelper.AsyncRequestComplated += (commentData, comEx) =>
                                {
                                    if (comEx == null)
                                    {
                                        #region get comment by shot id
                                        ShotComment firstComment = null;
                                        if (commentData != null)
                                            firstComment = commentData as ShotComment;

                                        if (firstComment.Comments.Count > 0)
                                        {
                                            string commentContent = firstComment.Comments[0].Body;
                                            commentContent = commentContent.Replace('\r', ' ').Replace('\n', ' ');

                                            if (commentContent.Length > 150)
                                                queryEntity.Comment = commentContent.Substring(0, 150) + "...";
                                            else
                                                queryEntity.Comment = commentContent;
                                        }

                                        //format datetime
                                        BasicRequestHelper requestHelper = new BasicRequestHelper();
                                        queryEntity.FormatDate = requestHelper.SpiltDateTimeStr(queryEntity.Created_at);
                                        #endregion
                                    }
                                    else
                                    {
                                        string errorMessage = commentData.ToString();
                                        if (!string.IsNullOrEmpty(errorMessage))
                                            new ToastNotifyHelper().ShowCoding4FunToastNotify(errorMessage, "Tip");
                                    }
                                };
                                #endregion
                                this._userRecentShotsCol.Add(queryEntity);
                            });
                    }
                    #endregion
                }
                else
                {
                    string errorMessage = responseData.ToString();
                    if (!string.IsNullOrEmpty(errorMessage))
                        new ToastNotifyHelper().ShowCoding4FunToastNotify(errorMessage, "Tip");
                }
            };
        }

        public void GetPlayerFollowing(string username, int pageIndex, int prePage)
        {
            //start processbar
            _processBarHelper.StartProcessBar();

            UserRequestHelper userProfileHelper = new UserRequestHelper();
            userProfileHelper.GetUserFollowingData(username, pageIndex, prePage);
            userProfileHelper.AsyncUserProfileComplated += (responseData, ex) => 
            {
                //end processbar
                _processBarHelper.EndProcessBar();

                if (ex == null)
                {
                    #region get user follwing data
                    UserFollowing followingData = null;
                    if (responseData != null)
                        followingData = responseData as UserFollowing;

                    if (followingData != null)
                    {
                        _userFollowingCol.Clear();
                        if (followingData.Players.Count > 0)
                            followingData.Players.ForEach(queryEntity => { this._userFollowingCol.Add(queryEntity); });
                    }
                    #endregion
                }
                else
                {
                    string errorMessage = responseData.ToString();
                    if (!string.IsNullOrEmpty(errorMessage))
                        new ToastNotifyHelper().ShowCoding4FunToastNotify(errorMessage, "Tip");
                }
            };
        }

        public void GetPlayerFollowers(string username, int pageIndex, int prePage)
        {
            //start processbar
            _processBarHelper.StartProcessBar();

            UserRequestHelper userProfileHelper = new UserRequestHelper();
            userProfileHelper.GetUserFollowerData(username, pageIndex, prePage);
            userProfileHelper.AsyncUserProfileComplated += (responseData, ex) => 
            {
                //end processbar
                _processBarHelper.EndProcessBar();

                if (ex == null)
                {
                    #region get user follower data
                    UserFollower userFollowerData = null;
                    if (responseData != null)
                        userFollowerData = responseData as UserFollower;

                    if (userFollowerData != null)
                    {
                        _userFollowersCol.Clear();
                        if (userFollowerData.Players.Count > 0)
                            userFollowerData.Players.ForEach(queryEntity => { this._userFollowersCol.Add(queryEntity); });
                    }
                    #endregion
                }
                else
                {
                    string errorMessage = responseData.ToString();
                    if (!string.IsNullOrEmpty(errorMessage))
                        new ToastNotifyHelper().ShowCoding4FunToastNotify(errorMessage, "Tip");
                }
            };
        }

    }
}
