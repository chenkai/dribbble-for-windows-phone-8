using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using MoCompontents.Compotents;

namespace DribbbleClient.ViewsModels
{
    public class BasicViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void BindPropertyNotifyChanged(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
                return;

            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public void NetworkIsInvalid()
        {
            new ToastNotifyHelper().ShowCoding4FunToastNotify("Please check your network.", "Tip");
        }


    }
}
