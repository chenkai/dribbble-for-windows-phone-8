using Microsoft.Phone.Shell;
using System.ComponentModel;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Windows.Media;
using System;
using System.Windows;

namespace MoCompontents.Compotents
{  
    /// <summary>
    /// Source Code from jeff
    /// http://www.jeff.wilcox.name/2011/07/creating-a-global-progressindicator-experience-using-the-windows-phone-7-1-sdk-beta-2/ 
    /// </summary>
    public class GlobalIndicator : INotifyPropertyChanged
    {
        private ProgressIndicator _mangoIndicator;
        private PhoneApplicationPage CurrentPage;

        public GlobalIndicator()
        {
        }

        public void Initialize(PhoneApplicationFrame frame)
        {
            _mangoIndicator = new ProgressIndicator();
            frame.Navigating += new NavigatingCancelEventHandler(frame_Navigating);
            frame.Navigated += OnRootFrameNavigated;
        }

        void frame_Navigating(object sender, NavigatingCancelEventArgs e)
        {
            if (this.IsBusy)
            {
                IsBusy = false;
            }
        }

        private void OnRootFrameNavigated(object sender, NavigationEventArgs e)
        {
            // Use in Mango to share a single progress indicator instance.
            var ee = e.Content;
            var pp = ee as PhoneApplicationPage;
            CurrentPage = pp;
            if (pp != null)
            {
                this._mangoIndicator.IsVisible = true;
                pp.SetValue(SystemTray.ProgressIndicatorProperty, _mangoIndicator);
                pp.SetValue(SystemTray.OpacityProperty, 0.9);
                pp.SetValue(SystemTray.BackgroundColorProperty, Color.FromArgb(255, 0, 0, 0));
                pp.SetValue(SystemTray.ForegroundColorProperty, Color.FromArgb(0, 255, 255, 255));

            }
        }

        private static GlobalIndicator _in;
        public static GlobalIndicator Instance
        {
            get
            {
                if (_in == null)
                {
                    _in = new GlobalIndicator();
                }
                return _in;
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                Deployment.Current.Dispatcher.BeginInvoke(delegate
                {
                    if (CurrentPage != null)
                    {
                        CurrentPage.SetValue(SystemTray.IsVisibleProperty, value);
                    }
                    _mangoIndicator.IsVisible = value;
                    _mangoIndicator.IsIndeterminate = value;

                });
            }
        }

        public string Text
        {
            set
            {
                if (_mangoIndicator == null)
                {
                    _mangoIndicator = new ProgressIndicator();
                }
                Deployment.Current.Dispatcher.BeginInvoke(delegate
                {
                    _mangoIndicator.Text = value;
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

      
    }
}
