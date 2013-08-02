using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Collections.ObjectModel;
using DribbbleClient.EntityModels;
using DribbbleClient.Common;
using DribbbleClient.EntityModels.ShotDetail;
using MoCommon;
using MoCompontents.Compotents;
using DribbbleClient.Common.UmengAnalysic;
using DribbbleClient.Common.DynamicLoad;
using DribbbleClient.EntityModels;

namespace DribbbleClient.ViewsModels
{
    public class ShotDetailViewModel : BasicViewModel
    {
        private ObservableCollection<Comment> _shotCommentsCol = new ObservableCollection<Comment>();
        public ObservableCollection<Comment> ShotCommentsCol
        {
            get { return this._shotCommentsCol; }
            set
            {
                this._shotCommentsCol = value;
                base.BindPropertyNotifyChanged("ShotCommentsCol");
            }
        }

        private Shot _shotDetail;
        public Shot ShotDetail
        {
            get {return this._shotDetail; }
            set
            {
                this._shotDetail = value;
                base.BindPropertyNotifyChanged("ShotDetail");
            }
        }
        public ActionCommand<object> MoreItemCommand { get; private set; }
        private PaginationInfo CurrentPageInfo = null;
        private int ShotId = 0;

        public ShotDetailViewModel(int shotId)
        {
            try
            {
                if (new LocalDeviceHelper().CheckNetWorkStatus())
                    GetShotDetail(shotId);
                else
                    base.NetworkIsInvalid();

                this.ShotId = shotId;
                MoreItemCommand = new ActionCommand<object>(MoreItem);
            }
            catch (Exception se)
            {
                new UmengRegisterEventHelper().RegisterUmengEventByType(AnalysicEventType.AppExceptionReport, "", se);
            }
        }

        public void GetShotDetail(int shotId)
        {
            if (shotId == 0)
                return;

            //start processbar
            LoadProcessBarHelper processBarHelper = new LoadProcessBarHelper();
            processBarHelper.StartProcessBar();

            ShotRequestHelper shotHelper = new ShotRequestHelper();
            shotHelper.GetShotDetailById(shotId);
            shotHelper.AsyncRequestComplated += (responseData, ex) => 
            {
                //end processbar
                processBarHelper.EndProcessBar();

                if (ex == null)
                {
                    #region get shot detail
                    Shot shotDetail = null;
                    if (responseData != null)
                        shotDetail = responseData as Shot;

                    if (shotDetail != null)
                    {
                        BasicRequestHelper requestHelper = new BasicRequestHelper();
                        shotDetail.FormatDate = requestHelper.SpiltDateTimeWithPM(shotDetail.Created_at);
                        this.ShotDetail = shotDetail;
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

        public void GetShotDetailComments(int shotId,int pageIndex=0,int prePage=0,bool isDynamicLoad=false)
        {
            if (shotId == 0)
                return;

            //start processbar
            LoadProcessBarHelper processBarHelper = new LoadProcessBarHelper();
            processBarHelper.StartProcessBar();

            //register pagintion 
            RegisterPaginationInfo(pageIndex, prePage);

            ShotRequestHelper shotHelper = new ShotRequestHelper();
            shotHelper.GetShotCommentById(shotId, pageIndex, prePage);
            shotHelper.AsyncRequestComplated += (responseData, Ex) => 
            {
                //end processbar
                processBarHelper.EndProcessBar();

                if (Ex == null)
                {
                    #region get shot comment by id
                    ShotComment shotComments = null;
                    if (responseData != null)
                        shotComments = responseData as ShotComment;

                    //update total pagecount
                    UpateTotalPage(shotComments.Pages);

                    if (shotComments.Comments != null)
                    {
                        if(!isDynamicLoad)
                           _shotCommentsCol.Clear();

                        if (shotComments.Comments.Count > 0)
                            shotComments.Comments.ForEach(queryEntity =>
                            {
                                BasicRequestHelper requestHelper = new BasicRequestHelper();
                                queryEntity.FormatDate = requestHelper.SpiltDateTimeWithPM(queryEntity.Created_at);
                                _shotCommentsCol.Add(queryEntity);
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

        void MoreItem(object parameter)
        {   
            if (new LocalDeviceHelper().CheckNetWorkStatus())
            {
                if(this.ShotId==0)
                    return;

                int currentIndex=GetCurrentPage();
                if(currentIndex<CurrentPageInfo.TotalPage)
                   GetShotDetailComments(ShotId, currentIndex + 1, 15,true);
            }
            else
                base.NetworkIsInvalid();
        }

        public void RegisterPaginationInfo(int pageIndex,int prePage)
        {
            if (this.CurrentPageInfo == null)
                CurrentPageInfo = new EntityModels.PaginationInfo();

            this.CurrentPageInfo.PrePageCount = prePage;
            this.CurrentPageInfo.CurrentIndex = pageIndex;
        }

        public void UpateTotalPage(int totalCount)
        {
            if (this.CurrentPageInfo == null)
                return;

            this.CurrentPageInfo.TotalPage = totalCount;
        }

        public int GetCurrentPage()
        {
            if (this.CurrentPageInfo == null)
                return 0;
            return this.CurrentPageInfo.CurrentIndex;
        }

    }
}
