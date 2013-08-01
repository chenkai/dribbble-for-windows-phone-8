using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

using DribbbleClient.EntityModels;

namespace DribbbleClient.UserControls
{
    public partial class ShotCommentTemplate : UserControl
    {
        public ShotCommentTemplate()
        {
            InitializeComponent();
        }

        public event EventHandler GetUserProfileUp;

        private void Ellipse_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Comment currentComment = this.DataContext as Comment;
            if (currentComment == null)
                return;

            if (GetUserProfileUp != null)
                GetUserProfileUp(currentComment, null);
        }

     
    }
}
