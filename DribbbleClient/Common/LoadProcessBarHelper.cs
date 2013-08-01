using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MoCompontents.Compotents;

namespace DribbbleClient.Common
{
    public class LoadProcessBarHelper
    {
        public void StartProcessBar(string displayText = "Loading...")
        {
            GlobalIndicator.Instance.Text = displayText;
            GlobalIndicator.Instance.IsBusy = true;
        }

        public void EndProcessBar()
        {
            GlobalIndicator.Instance.Text = string.Empty;
            GlobalIndicator.Instance.IsBusy = false;
        }
    }
}
