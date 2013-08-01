using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace MoCommon.Component.ProcessBar
{
    public partial class HighPerformancProcessBar : UserControl
    {
        public HighPerformancProcessBar()
        {
            InitializeComponent();
        }

        private string _displayText;
        public string DisplayText
        {
            get { return this._displayText; }
            set
            {
                _displayText = value;
                this.DisplayText_TB.Text = _displayText;
            }
        }
    }
}
