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
    public partial class FriendsDetailTemplate : UserControl
    {
        public FriendsDetailTemplate()
        {
            InitializeComponent();
        }

        public event EventHandler GetUserProfileUp;

        private void Ellipse_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Player currentPlayer = this.DataContext as Player;
            if (currentPlayer == null)
                return;

            if (GetUserProfileUp != null)
                GetUserProfileUp(currentPlayer, null);
        }
    }
}
