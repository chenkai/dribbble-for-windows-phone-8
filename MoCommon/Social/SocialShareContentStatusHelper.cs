using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoCommon.Social
{
    public class SocialShareContentStatusHelper
    {
        public event EventHandler SocialContentSharing;

        public delegate void ShareHandler(object shareResult, object shareResultMessage);
        public event ShareHandler SocialContentShared;

        private ShareStatus _shareStatus;
        public ShareStatus ShareStatus
        {
            set
            {
                if (value == ShareStatus.Sharing)
                {
                    if (SocialContentSharing != null)
                        SocialContentSharing("sharing", null);
                }
                else if (value == ShareStatus.Shared)
                {
                    if (SocialContentShared != null)
                        SocialContentShared(ShareResult.ToString(), ShareResultMessage);
                }
            }
            get { return this._shareStatus; }
        }

        public ShareResult ShareResult { get; set; }
        public string ShareResultMessage { get; set; }
    }

    public enum ShareStatus
    {
        Sharing,
        Shared
    }

    public enum ShareResult
    {
        Success,
        Fail
    }
}
