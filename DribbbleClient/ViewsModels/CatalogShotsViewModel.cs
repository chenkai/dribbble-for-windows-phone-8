using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using DribbbleClient.EntityModels.ShotCatalog;
using DribbbleClient.EntityModels.ShotDetail;
using DribbbleClient.EntityModels;
using DribbbleClient.Common.Catalog;
using DribbbleClient.Common;
using System.Windows;
using MoCommon;
using MoCompontents.Compotents;

using DribbbleClient.Common.UmengAnalysic;
using DribbbleClient.Common.DynamicLoad;

namespace DribbbleClient.ViewsModels
{
    public class CatalogShotsViewModel : BasicViewModel
    {
        private ObservableCollection<Shot> _popularShotCol = new ObservableCollection<Shot>();
        public ObservableCollection<Shot> PopularShotCol
        {
            get { return this._popularShotCol; }
            set
            {
                this._popularShotCol = value;
                base.BindPropertyNotifyChanged("PopularShotCol");
            }
        }

        private ObservableCollection<Shot> _everyoneShotCol = new ObservableCollection<Shot>();
        public ObservableCollection<Shot> EveryOneShotCol
        {
            get { return _everyoneShotCol; }
            set
            {
                this._everyoneShotCol = value;
                base.BindPropertyNotifyChanged("EveryOneShotCol");
            }
        }

        private ObservableCollection<Shot> _debutsShotCol = new ObservableCollection<Shot>();
        public ObservableCollection<Shot> DebutsShotCol
        {
            get { return this._debutsShotCol; }
            set
            {
                this._debutsShotCol = value;
                base.BindPropertyNotifyChanged("DebutsShotCol");
            }
        }

        private Player _searchUser;
        public Player SearchUser 
        {
            get { return _searchUser; }
            set
            {
                this._searchUser = value;
                BindPropertyNotifyChanged("SearchUser");
                
            }
        }

        private Visibility _displaySearchResult = Visibility.Collapsed;
        public Visibility DisplaySearchResult
        {
            get { return this._displaySearchResult; }
            set
            {
                this._displaySearchResult = value;
                BindPropertyNotifyChanged("DisplaySearchResult");
            }
        }

        public ActionCommand<object> MoreItemCommand { get; private set; }


        public CatalogShotsViewModel()
        {
            if (new LocalDeviceHelper().CheckNetWorkStatus())
                GetCatalogShot(ShotCatalog.Popular, 1, 10);
            else
                base.NetworkIsInvalid();

           MoreItemCommand = new ActionCommand<object>(MoreItem);
        }

        public void GetCatalogShot(Common.ShotCatalog catalogType, int pageIndex, int prePage,bool isDynamicLoad=false)
        {
            //start process bar 
            LoadProcessBarHelper processBarHelper = new LoadProcessBarHelper();
            processBarHelper.StartProcessBar();

            CatalogShotHelper catalogShotHelper = new CatalogShotHelper();
            catalogShotHelper.GetCatalogShots(catalogType, pageIndex, prePage);
            catalogShotHelper.AsyncCatalogShotsComplated += (responseData, ex) =>
            {
                //end process bar
                processBarHelper.EndProcessBar();

                if (ex == null)
                {
                    #region format catalog shot
                    CatalogShots catalogShots = null;
                    if (responseData != null)
                        catalogShots = responseData as CatalogShots;

                    if (catalogShots.Shots.Count > 0)
                    {
                        switch (catalogType)
                        {
                            case ShotCatalog.Popular:
                                if(!isDynamicLoad)
                                   _popularShotCol.Clear();
                                catalogShots.Shots.ForEach(queryEntity => { _popularShotCol.Add(queryEntity); });
                                break;
                            case ShotCatalog.Everyone:
                                if (!isDynamicLoad)
                                    _everyoneShotCol.Clear();
                                catalogShots.Shots.ForEach(queryEntity => { _everyoneShotCol.Add(queryEntity); });
                                break;
                            case ShotCatalog.Debuts:
                                if (!isDynamicLoad)
                                    _debutsShotCol.Clear();
                                catalogShots.Shots.ForEach(queryEntity =>
                                {
                                    #region get shot first author comment detail body
                                    ShotRequestHelper shotHelper = new ShotRequestHelper();
                                    shotHelper.GetShotCommentById(queryEntity.Id, 1, 1);
                                    shotHelper.AsyncRequestComplated += (commentData, comEx) =>
                                    {
                                        if (comEx == null)
                                        {
                                            #region get comment by id
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
                                    _debutsShotCol.Add(queryEntity);
                                });
                                break;
                        }
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

        public void GetPlayerDetailById(string username)
        {
            UserRequestHelper userProfileHelper = new UserRequestHelper();
            userProfileHelper.GetUserProfileByUserName(username);
            userProfileHelper.AsyncUserProfileComplated += (responseData, Ex) =>
            {
                if (Ex == null)
                {
                    #region get user profile
                    Player playerDetail = null;
                    if (responseData != null)
                        playerDetail = responseData as Player;

                    if (playerDetail != null)
                        this.SearchUser = playerDetail;

                    if (this.SearchUser.IsFindUser)
                        this.DisplaySearchResult = Visibility.Visible;
                    else
                        this.DisplaySearchResult = Visibility.Collapsed;
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

        void MoreItem(object parameter)
        {
            if (parameter == null)
                return;

            if (new LocalDeviceHelper().CheckNetWorkStatus())
            {
                switch (parameter.ToString())
                {
                    case "popular":
                        GetCatalogShot(ShotCatalog.Popular, 2, 10, true);
                        break;
                    case "everyone":
                        GetCatalogShot(ShotCatalog.Everyone, 2, 10, true);
                        break;
                    case "debuts":
                        GetCatalogShot(ShotCatalog.Debuts, 2, 10, true);
                        break;
                }              
            }
            else
                base.NetworkIsInvalid();
        }


    }
}
