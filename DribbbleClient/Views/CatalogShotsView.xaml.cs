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
using DribbbleClient.EntityModels;
using System.Windows.Input;
using DribbbleClient.Common.UmengAnalysic;

namespace DribbbleClient.Views
{
    public partial class CatalogShotsView : PhoneApplicationPage
    {
        public CatalogShotsView()
        {
            InitializeComponent();
            this.Loaded += CatalogShotsView_Loaded;
        }
        CatalogShotsViewModel _catalogShotViewModel = null;

        void CatalogShotsView_Loaded(object sender, RoutedEventArgs e)
        {
            if (_catalogShotViewModel == null)
                _catalogShotViewModel = new CatalogShotsViewModel();
            this.DataContext = _catalogShotViewModel;
        }    

        private void ShotDetail_MouseLeftButtonUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            StackPanel selectorPanel = sender as StackPanel;
            if (selectorPanel == null)
                return;

            LongListSelector selectorList = null;

            switch (selectorPanel.Tag.ToString())
            {
                case "Popular":
                    selectorList = this.PopularLongSelector_LG;
                    break;
                case"EveryOne":
                    selectorList = this.EveryOneLongSelector_LG;
                    break;
                case "Debuts":
                    selectorList = this.DebutsLongSelector_LG;
                    break;
            }

            Shot shotDetail = selectorList.SelectedItem as Shot;
            if (shotDetail == null)
                return;

            this.NavigationService.Navigate(new Uri("/Views/ShotDetailView.xaml?shotid="+shotDetail.Id, UriKind.RelativeOrAbsolute));
        }

        private void SearchDesigner_TB_GotFocus(object sender, RoutedEventArgs e)
        {
            string displayText = this.SearchDesigner_TB.Text;
            if (displayText.Equals("enter username"))
            {
                this.SearchResult_SP.Visibility = Visibility.Collapsed;
                this.SearchDesigner_TB.Text = string.Empty;
            }
        }

        private void SearchDesigner_TB_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            string username = this.SearchDesigner_TB.Text.Trim();
            if (e.Key == Key.Enter && !string.IsNullOrEmpty(username))
            {
                this.SearchResult_SP.Visibility = Visibility.Visible;
                this._catalogShotViewModel.GetPlayerDetailById(username);
            }
        }

        private void Pivot_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_catalogShotViewModel == null)
                return;

            PivotItem selectedItem = this.Catalog_PT.SelectedItem as PivotItem;
            if (selectedItem == null)
                return;

            switch (selectedItem.Header.ToString())
            {
                case "Popular":
                    _catalogShotViewModel.GetCatalogShot(Common.ShotCatalog.Popular, 0, 10);
                    break;
                case "Everyone":
                    _catalogShotViewModel.GetCatalogShot(Common.ShotCatalog.Everyone, 0, 10);
                    break;
                case "Debuts":
                    _catalogShotViewModel.GetCatalogShot(Common.ShotCatalog.Debuts, 0, 10);
                    break;
                case "Designer":
                    break;
            }

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            new UmengRegisterEventHelper().RegisterUmengEventByType(AnalysicEventType.AppViewPageStart, "CatalogShotsView");
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            new UmengRegisterEventHelper().RegisterUmengEventByType(AnalysicEventType.AppViewPageEnd, "CatalogShotsView");
        }
    }
}